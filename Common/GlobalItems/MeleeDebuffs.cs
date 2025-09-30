using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Buffs.StatDebuffs;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    // Wardrobe Hummus
    public class MeleeDebuffs : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return !ModLoader.TryGetMod("WHummusMultiModBalancing", out _);
        }

        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                var thunderTalon = thoriumMod.Find<ModItem>("ThunderTalon");
                if (thunderTalon != null && item.type == thunderTalon.Type)
                {
                    target.AddBuff(144, 180); // Electrified
                }

                var lifeQuartz = thoriumMod.Find<ModItem>("LifeQuartzClaymore");
                if (lifeQuartz != null && item.type == lifeQuartz.Type)
                {
                    HealPlayer(player, 2);
                    return;
                }

                var hereticBreaker = thoriumMod.Find<ModItem>("HereticBreaker");
                if (hereticBreaker != null && item.type == hereticBreaker.Type)
                {
                    HealPlayer(player, 3);
                }
            }

            if (ModLoader.TryGetMod("SOTS", out Mod sots) && InfernalConfig.Instance.SOTSBalanceChanges)
            {
                var blazingClub = sots.Find<ModItem>("BlazingClub");
                if (blazingClub != null && item.type == blazingClub.Type)
                {
                    target.AddBuff(BuffID.OnFire, 180);
                }

                if (item.type == sots.Find<ModItem>("IrradiatedChainReactor").Type)
                {
                    target.AddBuff(ModContent.BuffType<Irradiated>(), 60 * 3);
                }
            }

            if (item.type == ItemID.LucyTheAxe && InfernalConfig.Instance.VanillaBalanceChanges)
            {
                target.AddBuff(ModContent.BuffType<Crumbling>(), 180);
            }
        }

        private void HealPlayer(Player player, int healAmount)
        {
            player.statLife += healAmount;
            player.HealEffect(healAmount, true); // true = play heal effect number popup
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                var lifeQuartz = thoriumMod.Find<ModItem>("LifeQuartzClaymore");
                if (lifeQuartz != null && item.type == lifeQuartz.Type)
                {
                    // Remove the existing tooltip
                    tooltips.RemoveAll(t => t.Text.Contains("Steals 1 life"));

                    // Add your custom tooltip
                    tooltips.Add(new TooltipLine(Mod, "CustomTooltip", "Steals 3 life"));
                }
            }
        }
    }
}
