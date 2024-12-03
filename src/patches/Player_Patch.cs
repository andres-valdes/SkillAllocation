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
            __instance.GetComponent<ExperiencePool>().AddExperience((int)Math.Floor(value));
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
            if (__instance.GetComponent<ExperiencePool>().GetExperience() > 0)
            {
                // TODO: Create prefab
                GameObject gameObject = UnityEngine.Object.Instantiate(ZNetScene.instance.GetPrefab("DroppedExperience"), __instance.transform.position, Quaternion.identity);
                gameObject.GetComponent<DroppedExperience>().SetExperience(__instance.GetComponent<ExperiencePool>().GetExperience());
                gameObject.GetComponent<DroppedExperience>().SetOwner(__instance);
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
