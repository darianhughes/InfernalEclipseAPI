using System.Collections.Generic;
using SOTS.Void;
using SOTS;
using SOTS.Items.Wings;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Accessories;
using Terraria.Localization;
using InfernalEclipseAPI.Core.Utils;
using InfernalEclipseAPI.Core.Players.SOTSPlayerOverrides;
using Terraria.GameInput;
using InfernalEclipseAPI.Core.Configs;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.InsigniaCraftingTree
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class InsigniaAccessoryChanges : GlobalItem
    {
        private static Mod sots
        {
            get
            {
                ModLoader.TryGetMod("SOTS", out Mod sots);
                return sots;
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "CalamityMod" &&
                item.ModItem.Name == "AscendantInsignia" &&
                sots != null &&
                InfernalConfig.Instance.MergeCraftingTrees)
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
                if (InfernalConfig.Instance.MergeCraftingTrees)
                {
                    var CalPlayer = player.GetModPlayer<CalamityPlayer>();
                    CalPlayer.ascendantInsignia = true;
                }
                MachinaBoosterPlayer modPlayer = player.GetModPlayer<MachinaBoosterPlayer>();
                modPlayer.CreativeFlightTier2 = false;
                player.wingTimeMax = 350;
                player.wingAccRunSpeed = 11f;
                player.wingRunAccelerationMult = 2f;
                player.GetModPlayer<SOTSPlayerAdjustments>().bladeWings = true;
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
                TooltipLine customLine = new(Mod, "StealthTooltip", stealthTooltip);
                if (InfernalRedActive)
                    customLine.OverrideColor = InfernalRed;
                if (CalNerf)
                    customLine.OverrideColor = Color.Red;

                tooltips.Insert(insertIndex, customLine);
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string spirtInfo1 = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.SpiritInsignia.Effect");

            if (item.type == sots.Find<ModItem>("SpiritInsignia").Type && InfernalConfig.Instance.MergeCraftingTrees)
            {
                InfernalUtilities.FullTooltipOveride(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.SpiritInsignia.Replace"));
            }

            if (item.type == ModContent.ItemType<AscendantInsignia>() && InfernalConfig.Instance.MergeCraftingTrees)
            {
                AddTooltip(tooltips, spirtInfo1, true);
            }

            string str1 = Language.GetTextValue("Mods.SOTS.Common.Unbound");
            string str2 = str1;
            using (List<string>.Enumerator enumerator = SOTS.SOTS.MachinaBoosterHotKey.GetAssignedKeys((InputMode)0).GetEnumerator())
            {
                if (enumerator.MoveNext())
                    str1 = enumerator.Current;
            }
            using (List<string>.Enumerator enumerator = SOTS.SOTS.SlowFlightHotKey.GetAssignedKeys((InputMode)0).GetEnumerator())
            {
                if (enumerator.MoveNext())
                    str2 = enumerator.Current;
            }

            if (item.type == ModContent.ItemType<MachinaBooster>())
            {
                InfernalUtilities.ReplaceTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.MachinaBooster.Orig"), Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.MachinaBooster.Replace", str1));
            }

            if (item.type == sots.Find<ModItem>("GildedBladeWings").Type)
            {
                if (InfernalConfig.Instance.MergeCraftingTrees)
                {
                    InfernalUtilities.ReplaceTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.GildedBladeWings.Orig"), Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.GildedBladeWings.Replace", str1, str2));
                }
                AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.AscendantInsignia"), true);
                AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.GildedVoid"), true);
            }
        }
    }
}
