using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOTS.Void;
using SOTS;
using Terraria;
using Terraria.ModLoader;
using SOTS.Items.Wings;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Accessories;
using CalamityMod;
using InfernumMode.Core.OverridingSystem;
using Steamworks;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.InsigniaCraftingTree
{
    [ExtendsFromMod("SOTS")]
    public class InsigniaAccessoryChanges : GlobalItem
    {
        private Mod calamity
        {
            get
            {
                ModLoader.TryGetMod("CalamityMod", out Mod cal);
                return cal;
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

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "CalamityMod" &&
                item.ModItem.Name == "AscendantInsignia" &&
                sots != null)
            {
                SOTSPlayer sotsPlayer = SOTSPlayer.ModPlayer(player);
                VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
                ++voidPlayer.bonusVoidGain;
                voidPlayer.voidRegenSpeed += 0.25f;
                sotsPlayer.SpiritSymphony = true;
            }

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "SOTS" &&
                item.ModItem.Name == "GildedBladeWings" &&
                sots != null)
            {
                var CalPlayer = player.GetModPlayer<CalamityPlayer>();
                CalPlayer.ascendantInsignia = true;
                MachinaBoosterPlayer modPlayer = player.GetModPlayer<MachinaBoosterPlayer>();
                modPlayer.CreativeFlightTier2 = false;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees || sots == null)
                return;

            Color InfernalRed = Color.Lerp(
               Color.White,
               new Color(255, 80, 0), // Infernal red/orange
               (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );
            string spirtInfo1 = "Increases void gain by 1 and void regeneration speed by 25%";
            string spirtInfo2 = "Doubles damage done to non-Lux spirit enemies";
            string accendInfo1 = "Increases movement and jump speed by 10% and acceleration by 1.1x";
            string accendInfo2 = "Pressing the Ascendant Insignia keybind will give you the power of ascension, providing infinite flight time for 4 seconds";
            string accendInfo3 = "Ascension has a 40 second cooldown";

            if (item.type == sots.Find<ModItem>("SpiritInsignia").Type)
            {
                foreach (TooltipLine tooltip in tooltips)
                {
                    if (tooltip.Text.Contains("Grants infinite wing and rocket boot flight"))
                    {
                        tooltip.Text = "Increases wing flight time by 25%";
                    }
                }
            }

            if (item.type == ModContent.ItemType<AscendantInsignia>())
            {
                tooltips.Add(new TooltipLine(Mod, "SpiritInfo", spirtInfo1)
                {
                    OverrideColor = new Color?(InfernalRed)
                });
                tooltips.Add(new TooltipLine(Mod, "SpiritInfo2", spirtInfo2)
                {
                    OverrideColor = new Color?(InfernalRed)
                });
            }

            if (item.type == sots.Find<ModItem>("GildedBladeWings").Type)
            {
                tooltips.Add(new TooltipLine(Mod, "VoidDrain", "Increases void drain by 3 while active")
                {
                    OverrideColor = new Color?(InfernalRed)
                });
                tooltips.Add(new TooltipLine(Mod, "AInfo", accendInfo1)
                {
                    OverrideColor = new Color?(InfernalRed)
                });
                tooltips.Add(new TooltipLine(Mod, "AInfo2", accendInfo2)
                {
                    OverrideColor = new Color?(InfernalRed)
                });
                tooltips.Add(new TooltipLine(Mod, "AInfo3", accendInfo3)
                {
                    OverrideColor = new Color?(InfernalRed)
                });
                tooltips.Add(new TooltipLine(Mod, "CalamityNerf", "[Calamity]: No longer allows infinite flight")
                {
                    OverrideColor = Color.Red
                });
            }
        }
    }
}
