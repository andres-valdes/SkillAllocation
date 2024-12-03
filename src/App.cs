using BepInEx;
using HarmonyLib;
using System.Reflection;

namespace Ratzu.SkillAllocation;

[BepInPlugin(Guid, Name, Version)]
public class SkillAllocation : BaseUnityPlugin
{
    public const string Version = "1.0.0";
    public const string Name = "Skill Allocation";
    public const string Guid = "ratzu.mods.skillallocation";
    public const string Namespace = "Ratzu" + nameof(SkillAllocation);

    private Harmony _harmony;

    private void Awake()
    {
        _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), Guid);
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
}