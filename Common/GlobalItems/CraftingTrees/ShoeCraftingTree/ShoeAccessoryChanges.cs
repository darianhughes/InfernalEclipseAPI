using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories.Wings;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.ShoeCraftingTree
{
    public class ShoeAccessoryChanges : GlobalItem
    {
        private Mod calamity
        {
            get
            {
                ModLoader.TryGetMod("CalamityMod", out Mod cal);
                return cal;
            }
        }

        private Mod thorium
        {
            get
            {
                ModLoader.TryGetMod("ThoriumMod", out Mod thor);
                return thor;
            }
        }

        private Mod sots
        {
            get
            {
                ModLoader.TryGetMod("SOTS", out Mod sots);
                return sots;
            }
        }

        public Mod fargosSouls
        {
            get
            {
                ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls);
                return fargosSouls;
            }
        }

        public Mod calFargo
        {
            get
            {
                ModLoader.TryGetMod("FargowiltasCrossmod", out Mod fargoCrossmod);
                return fargoCrossmod;
            }
        }

        private Mod SOTSBardHealer
        {
            get
            {
                ModLoader.TryGetMod("SOTSBardHealer", out Mod sotsbh);
                return sotsbh;
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;

            if (thorium != null)
            {
                if (item.type == thorium.Find<ModItem>("TerrariumParticleSprinters").Type)
                {
                    CalamityPlayer modPlayer = player.Calamity();
                    modPlayer.angelTreads = true;

                    if (calFargo != null)
                    {
                        player.fairyBoots = true;
                        player.flowerBoots = true;
                        player.GetJumpState(ExtraJump.FartInAJar).Enable();
                        player.desertBoots = true;
                        player.jumpBoost = true;
                        player.noFallDmg = true;
                    }
                }
            }

            if (sots != null)
            {
                if (item.type == sots.Find<ModItem>("FlashsparkBoots").Type)
                {
                    CalamityPlayer modPlayer = player.Calamity();
                    modPlayer.hellfireTreads = true;
                }

                if (item.type == sots.Find<ModItem>("SubspaceBoosters").Type)
                {
                    CalamityPlayer modPlayer = player.Calamity();
                    modPlayer.angelTreads = true;
                    modPlayer.hellfireTreads = true;

                    if (calFargo != null)
                    {
                        player.fairyBoots = true;
                        player.flowerBoots = true;
                        player.GetJumpState(ExtraJump.FartInAJar).Enable();
                        player.desertBoots = true;
                        player.jumpBoost = true;
                        player.noFallDmg = true;
                    }

                    if (thorium != null)
                    {
                        ModItem terrariumShoe = thorium.Find<ModItem>("TerrariumParticleSprinters");

                        terrariumShoe.UpdateAccessory(player, hideVisual);
                    }
                }
            }

            if (item.type == ModContent.ItemType<TracersCelestial>() || item.type == ModContent.ItemType<TracersElysian>() || item.type == ModContent.ItemType<TracersSeraph>())
            {
                CalamityPlayer modPlayer = player.Calamity();
                modPlayer.hellfireTreads = true;
            }
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

        public void FullTooltipOveride(List<TooltipLine> tooltips, string stealthTooltip)
        {
            for (int index = 0; index < tooltips.Count; ++index)
            {
                if (tooltips[index].Mod == "Terraria")
                {
                    if (tooltips[index].Name == "Tooltip0")
                    {
                        TooltipLine tooltip = tooltips[index];
                        tooltip.Text = $"{stealthTooltip}";
                    }
                    else if (tooltips[index].Name.Contains("Tooltip"))
                    {
                        tooltips[index].Hide();
                    }
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees || calamity == null || thorium == null)
                return;

            if (SOTSBardHealer != null)
            {
                if (item.type == thorium.Find<ModItem>("TerrariumParticleSprinters").Type)
                {
                    FullTooltipOveride(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.TPS"));
                }
            }

            if (sots != null)
            {
                if (item.type == ModContent.ItemType<TracersCelestial>() || item.type == ModContent.ItemType<TracersElysian>() || item.type == ModContent.ItemType<TracersSeraph>() || item.type == sots.Find<ModItem>("SubspaceBoosters").Type || item.type == sots.Find<ModItem>("FlashsparkBoots").Type)
                {
                    AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Hellfire"), false);
                }
            }
        }
    }
}
