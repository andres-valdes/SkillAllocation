using System;
using HarmonyLib;
using UnityEngine;

namespace Ratzu.SkillAllocation;

public class Player_Patch
{
    [HarmonyPatch(typeof(Player), nameof(Player.Awake))]
    public static class Player_Awake_AddExperiencePoolComponent
    {
        public static void Postfix(Player __instance)
        {
            __instance.gameObject.AddComponent<ExperiencePool>();
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.RaiseSkill))]
    public static class Player_RaiseSkill_AddExperienceToPlayerExperiencePool
    {
        public static bool Prefix(Player __instance, ref Skills.SkillType skill, ref float value)
        {
            __instance.GetComponent<ExperiencePool>().AddExperience(Math.Ceiling(value));
            return false;
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.Load))]
    public static class Player_Load_LoadExperiencePoolFromSaveData
    {
        public static void Postfix(Player __instance, ref ZPackage pkg)
        {
            ExperiencePool experiencePool = __instance.GetComponent<ExperiencePool>();
            if (experiencePool == null)
            {
                return;
            }
            experiencePool.Load(pkg);
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
    public static class Player_OnDeath_MarkDroppedExperienceAsDeadAndCreateNewDrop
    {
        public static void Postfix(Player __instance)
        {
            DroppedExperience.GetDroppedExperience(__instance.GetPlayerID())?.SetOwnerHasDied(true);
            if (__instance.GetComponent<ExperiencePool>()?.GetExperience() > 0)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(ZNetScene.instance.GetPrefab(App.DroppedExperienceObj.name.GetStableHashCode()), new Vector3(__instance.transform.position.x, __instance.transform.position.y + 2, __instance.transform.position.z), Quaternion.identity);
                gameObject.AddComponent<DroppedExperience>().Setup(__instance.GetComponent<ExperiencePool>().GetExperience(), __instance);
            }
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.Save))]
    public static class Player_Save_SaveExperiencePoolToSaveData
    {
        public static void Postfix(Player __instance, ref ZPackage pkg)
        {
            ExperiencePool experiencePool = __instance.GetComponent<ExperiencePool>();
            if (experiencePool == null)
            {
                return;
            }
            experiencePool.Save(pkg);
        }
    }
}
