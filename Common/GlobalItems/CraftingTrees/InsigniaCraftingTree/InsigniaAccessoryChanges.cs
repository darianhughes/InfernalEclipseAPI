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
using Terraria.Localization;
using Terraria.DataStructures;

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
                player.wingTimeMax = 350;
                player.wingAccRunSpeed = 11f;
                player.wingRunAccelerationMult = 2f;
            }

            if (sots != null & calFargo != null)
            {
                if (item.type == fargosSouls.Find<ModItem>("FlightMasterySoul").Type)
                {
                    SOTSPlayer sotsPlayer = SOTSPlayer.ModPlayer(player);
                    VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
                    voidPlayer.bonusVoidGain += 3f;
                    voidPlayer.voidRegenSpeed += 0.25f;
                    sotsPlayer.SpiritSymphony = true;
                    MachinaBoosterPlayer modPlayer = player.GetModPlayer<MachinaBoosterPlayer>();
                    modPlayer.canCreativeFlight = true;
                }
            }
        }

        public void AddTooltip(List<TooltipLine> tooltips, string stealthTooltip, bool InfernalRedActive = false, bool CalNerf = false)
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
                if (CalNerf)
                    customLine.OverrideColor = Color.Red;

                tooltips.Insert(insertIndex, customLine);
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
            string spirtInfo1 = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.SpiritInsignia");
            string accendInfo1 = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.AscendantInsignia");

            string gildedInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Gilded");

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
                AddTooltip(tooltips, spirtInfo1, true);
            }

            if (item.type == sots.Find<ModItem>("GildedBladeWings").Type)
            {
                AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.GildedVoid"), true);
                AddTooltip(tooltips, accendInfo1, true);
                AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.CalSoaringNerf"), CalNerf: true);
            }

            if (calFargo != null)
            { 
                if (item.type == fargosSouls.Find<ModItem>("FlightMasterySoul").Type)
                {
                    AddTooltip(tooltips, gildedInfo, true);
                }
            }
        }
    }
}
