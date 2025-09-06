using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Buffs;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using InfernalEclipseAPI.Core.Players.ThoriumMulticlassNerf;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    public class AntiHealerMulticlassCheck : GlobalItem
    {
        public override bool? UseItem(Item item, Player player)
        {
            if (!InfernalConfig.Instance.NerfThoriumMulticlass) return base.UseItem(item, player);
            if (item is null || item.type == ItemID.None) return base.UseItem(item, player);

            var anti = player.GetModPlayer<ThoriumMulticlassPlayerNerfs>();
            bool healerUse = IsHealerWeaponOrTool(item);

            // Using a healer weapon/tool during penalty window => apply debuff (keeps -10 healing active)
            if (healerUse && anti.switchToHealerPenaltyTimer > 0)
                player.AddBuff(ModContent.BuffType<BrokenOath>(), anti.switchToHealerPenaltyTimer);

            // Using a non-healer combat item refreshes the window even if no hit connects
            if (!healerUse && IsCombatWeapon(item))
                anti.switchToHealerPenaltyTimer = ThoriumMulticlassPlayerNerfs.PenaltyDuration;

            return base.UseItem(item, player);
        }

        // === Helpers ===
        private static bool IsCombatWeapon(Item item)
            => item.damage > 0
               && item.useStyle != ItemUseStyleID.None
               && !item.accessory
               && item.ammo == AmmoID.None
               && item.pick <= 0 && item.axe <= 0 && item.hammer <= 0;

        private static bool IsHealerWeaponOrTool(Item item)
        {
            if (item is null) return false;

            // Requires Thorium reference; if you can't reference it directly, replace with your own detection.
            if (item.CountsAsClass<HealerDamage>() || item.CountsAsClass<HealerToolDamageHybrid>() || item.CountsAsClass<HealerTool>()) return true;

            return false;
        }
    }
}
