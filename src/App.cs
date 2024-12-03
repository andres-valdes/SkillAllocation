using BepInEx;
using HarmonyLib;
using System.Reflection;

namespace Ratzu.AutoShield;

[BepInPlugin(Guid, Name, Version)]
public class ReviveAllies : BaseUnityPlugin
{
    public const string Version = "1.0.0";
    public const string Name = "AutoShield";
    public const string Guid = "ratzu.mods.autoshield";
    public const string Namespace = "Ratzu" + nameof(AutoShield);

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