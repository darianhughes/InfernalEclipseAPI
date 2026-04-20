using System.Collections.Generic;
using InfernalEclipseAPI.Core.Utils;
using Terraria;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ModSpecific
{
    public class CalamityAmmoGlobalItem : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return InfernalConfig.Instance.CalamityBalanceChanges;
        }

        public override void SetDefaults(Item item)
        {
            if (ModLoader.TryGetMod("CalamityAmmo", out Mod calAmmo))
            {
                if (item.type == calAmmo.Find<ModItem>("HydrothermicArrow").Type || item.type == calAmmo.Find<ModItem>("HydrothermicBullet").Type || item.type == calAmmo.Find<ModItem>("DivineArrow").Type || item.type == calAmmo.Find<ModItem>("DivineBullet").Type)
                {
                    item.ammo = AmmoID.None;
                }
            }
        }

        public override void UpdateEquip(Item item, Player player)
        {
            if (!ModLoader.TryGetMod("CalamityAmmo", out Mod calamityAmmo))
                return;

            if (item.type == calamityAmmo.Find<ModItem>("WulfrumCoil").Type)
            {
                player.GetDamage(DamageClass.Ranged) -= 0.02f;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModLoader.TryGetMod("CalamityAmmo", out Mod calAmmo))
            {
                if (item.type == calAmmo.Find<ModItem>("HydrothermicArrow").Type || item.type == calAmmo.Find<ModItem>("HydrothermicBullet").Type || item.type == calAmmo.Find<ModItem>("DivineArrow").Type || item.type == calAmmo.Find<ModItem>("DivineBullet").Type)
                {
                    InfernalUtilities.AddDisabledItemTag(tooltips);
                }
            }
        }
    }
}
