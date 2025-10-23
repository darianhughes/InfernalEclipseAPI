using System.Collections.Generic;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.MiscCraftingTrees
{
    public class MiscItemChanges : GlobalItem
    {
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

        public override bool IsLoadingEnabled(Mod mod)
        {
            return InfernalConfig.Instance.MergeCraftingTrees;
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ModContent.ItemType<TheAbsorber>())
            {
                if (thorium != null)
                    thorium.Find<ModItem>("MonsterCharm").UpdateAccessory(player, hideVisual);
            }

            if (thorium != null)
            {
                if (item.type == thorium.Find<ModItem>("GreedyGoblet").Type)
                {
                    if (sots != null)
                    {
                        sots.Find<ModItem>("GreedierRing").UpdateAccessory(player, hideVisual);
                    }
                }
            }
        }

        public void AddTooltip(List<TooltipLine> tooltips, string stealthTooltip, bool InfernalRedActive = true)
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
            if (item.type == ModContent.ItemType<TheAbsorber>())
            {
                if (thorium != null)
                    AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.MonsterCharm"));
            }

            if (thorium != null)
            {
                if (item.type == thorium.Find<ModItem>("GreedyGoblet").Type)
                {
                    if (sots != null)
                    {
                        AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.GreedierRing"));
                    }
                }
            }
        }
    }
}
