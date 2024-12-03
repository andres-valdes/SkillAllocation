using HarmonyLib;

namespace Ratzu.SkillAllocation;

public class Skills_Patch
{
    [HarmonyPatch(typeof(Skills), nameof(Skills.LowerAllSkills))]
    public static class Skills_LowerAllSkills_DoNotLowerSkills
    {
        public static bool Prefix(Skills __instance)
        {
            return false;
        }
    }
}
