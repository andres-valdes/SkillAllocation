using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using System.Linq;

namespace Ratzu.SkillAllocation;

[BepInPlugin(Guid, Name, Version)]
public class App : BaseUnityPlugin
{
    public const string Version = "1.0.0";
    public const string Name = "Skill Allocation";
    public const string Guid = "ratzu.mods.skillallocation";
    public const string Namespace = "Ratzu" + nameof(SkillAllocation);

    private Harmony _harmony;
    public static GameObject DroppedExperienceObj;

    private void Awake()
    {
        var assetBundle = GetAssetBundleFromResources("newbundle");
        DroppedExperienceObj = assetBundle.LoadAsset<GameObject>("Assets/Modding/DroppedExperience.prefab");
        _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), Guid);
    }

    public static AssetBundle GetAssetBundleFromResources(string fileName)
    {
        var execAssembly = Assembly.GetExecutingAssembly();
        var resourceName = execAssembly.GetManifestResourceNames()
            .Single(str => str.EndsWith(fileName));
        using (var stream = execAssembly.GetManifestResourceStream(resourceName))
        {
            return AssetBundle.LoadFromStream(stream);
        }
    }

    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    public static class ZNetScene_Awake_RegisterPrefab
    {
        public static void Prefix(ZNetScene __instance)
        {
            if (__instance.m_prefabs.Find(prefab => prefab.name.GetStableHashCode() == DroppedExperienceObj.name.GetStableHashCode()) == null)
            {
                __instance.m_prefabs.Add(DroppedExperienceObj);
            }
        }
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
}