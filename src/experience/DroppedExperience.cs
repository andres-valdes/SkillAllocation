using System.Collections.Generic;
using UnityEngine;

namespace Ratzu.SkillAllocation;

public class DroppedExperience : MonoBehaviour, Interactable, Hoverable
{
    private const string ZDO_OWNER = "owner";
    private const string ZDO_EXPERIENCE = "experience";
    private const string ZDO_HAS_BEEN_CLAIMED = "hasBeenClaimed";
    private const string ZDO_OWNER_HAS_DIED = "ownerHasDied";

    private ZNetView m_nview;
    private static Dictionary<long, DroppedExperience> drops = new Dictionary<long, DroppedExperience>();

    void Awake()
    {
        m_nview = GetComponent<ZNetView>();
        long ownerID = GetOwnerID();
        if (ownerID == 0L) {
            return;
        }
        drops.Add(ownerID, this);
    }

    void Update()
    {
        if (!m_nview.IsValid()) 
        {
            return;
        }
        long ownerID = GetOwnerID();
        if (ownerID == 0L) {
            return;
        }
        if (!drops.ContainsKey(ownerID)) {
            drops.Add(ownerID, this);
        }
        if (HasOwnerDied() || HasBeenClaimed()) {
            Cleanup();
        }
    }

    public void SetOwnerHasDied(bool hasDied) {
        m_nview.GetZDO().Set(ZDO_OWNER_HAS_DIED, hasDied);
    }

    public bool HasOwnerDied() {
        return m_nview.GetZDO().GetBool(ZDO_OWNER_HAS_DIED);
    }

    public bool HasBeenClaimed() {
        return m_nview.GetZDO().GetBool(ZDO_HAS_BEEN_CLAIMED);
    }

    public void Cleanup() {
        long ownerID = GetOwnerID();
        if (ownerID == 0L) {
            return;
        }
        drops.Remove(ownerID);
        ZNetScene.instance.Destroy(gameObject);
    }

    public void Setup(double experience, Player owner)
    {
        SetExperience(experience);
        SetOwner(owner);
    }

    public long GetOwnerID() {
        return m_nview.GetZDO().GetLong(ZDO_OWNER, 0);
    }

    public void SetOwner(Player owner) {
        m_nview.GetZDO().Set(ZDO_OWNER, owner.GetPlayerID());
    }

    public void SetExperience(double experience) {
        m_nview.GetZDO().Set(ZDO_EXPERIENCE, (int)experience);
    }

    public double GetExperience() {
        return m_nview.GetZDO().GetInt(ZDO_EXPERIENCE);
    }

    public bool Interact(Humanoid humanoid, bool hold, bool alt)
    {
        if (humanoid is Player player) {
            if (GetOwnerID() == player.GetPlayerID()) {
                player.GetComponent<ExperiencePool>().AddExperience(GetExperience());
                m_nview.GetZDO().Set(ZDO_HAS_BEEN_CLAIMED, true);
                player.Message(MessageHud.MessageType.TopLeft, "Recovered " + GetExperience() + " XP", 0, null);
                Cleanup();
                return true;
            }
        }
        return false;
    }

    public bool UseItem(Humanoid user, ItemDrop.ItemData item)
    {
        return false;
    }

    public string GetHoverText()
    {
        if (GetOwnerID() == Player.m_localPlayer.GetPlayerID()) {
            return "Recover XP";
        } else {
            Player owner = Player.GetPlayer(GetOwnerID());
            return string.Format("{0}'s XP", owner.GetPlayerName());
        }
    }

    public string GetHoverName()
    {
        return "Dropped XP";
    }

    public static DroppedExperience GetDroppedExperience(long ownerID)
    {
        drops.TryGetValue(ownerID, out DroppedExperience drop);
        return drop;
    }
}
