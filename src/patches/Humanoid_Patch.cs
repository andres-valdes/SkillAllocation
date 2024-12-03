using HarmonyLib;

namespace Ratzu.AutoShield;

public class Humanoid_Patch
{
    [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.EquipItem))]
    public static class Humanoid_EquipItem_EquipShieldIfNewlyEquippedItemIsOneHanded
    {
        public static void Postfix(Humanoid __instance, ref bool __result, ItemDrop.ItemData item)
        {
            if (!__result || __instance.GetLeftItem() != null)
            {
                return;
            }
            if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.OneHandedWeapon)
            {
                ItemDrop.ItemData shield = __instance.m_inventory
                    .GetAllItems()
                    .Find(i => i.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shield);
                if (shield == null)
                {
                    return;
                }
                __instance.EquipItem(shield, false);
            }
        }
    }
}
