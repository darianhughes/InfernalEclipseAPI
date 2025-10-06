using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria.Localization;

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

        public void AddTooltip(List<TooltipLine> tooltips, string stealthTooltip, bool InfernalRedActive = false)
        {
            Color InfernalRed = Color.Lerp(
               Color.White,
               new Color(255, 80, 0), // Infernal red/orange
               (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );

            int maxTooltipIndex = -1;
            int maxNumber = -1;

            // Find the TooltipLine with the highest TooltipX name
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Mod == "Terraria" && tooltips[i].Name.StartsWith("Tooltip"))
                {
                    if (int.TryParse(tooltips[i].Name.Substring(7), out int num) && num > maxNumber)
                    {
                        maxNumber = num;
                        maxTooltipIndex = i;
                    }
                }
            }

            // If found, insert a new TooltipLine right after it with the desired color
            if (maxTooltipIndex != -1)
            {
                int insertIndex = maxTooltipIndex + 1;
                TooltipLine customLine = new TooltipLine(Mod, "StealthTooltip", stealthTooltip);
                if (InfernalRedActive)
                    customLine.OverrideColor = InfernalRed;

                tooltips.Insert(insertIndex, customLine);
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!InfernalConfig.Instance.ThoriumBalanceChangess) return;

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod))
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

            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
            {
                if (item.type == ragnarok.Find<ModItem>("MarbleScythe").Type)
                {
                    AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.HolyGlare"));
                }
            }
        }
    }
}
