using System.Collections.Generic;
using System.Linq;
using InfernalEclipseAPI.Core.Systems;
using Microsoft.Xna.Framework;
using SOTS.Items.CritBonus;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Items.Accessories.ExoSights
{
    [ExtendsFromMod("SOTS")]
    public class ExoSightIngredientTooltips : GlobalItem
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            int[] bonusCritDamageItems =
            {
                ModContent.ItemType<PutridCoin>(),
                ModContent.ItemType<BloodstainedCoin>(),
                ModContent.ItemType<PolishedCoin>(),
                ModContent.ItemType<FocusCrystal>(),
                ModContent.ItemType<FocusReticle>(),
                ModContent.ItemType<ExoSights>(),
            };

            if (bonusCritDamageItems.Contains(equippedItem.type) && bonusCritDamageItems.Contains(incomingItem.type) && InfernalConfig.Instance.SOTSBalanceChanges)
            {
                return false;
            }

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public void AddTooltip(List<TooltipLine> tooltips, string stealthTooltip, bool InfernalRedActive = false, bool NoSOTSPinkActive = false)
        {
            Color InfernalRed = Color.Lerp(
               Color.White,
               new Color(255, 80, 0), // Infernal red/orange
               (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );
            Color NoSOTSPink = Color.Lerp(
                Color.White,
                new Color(251, 198, 207),
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
            if (!InfernalConfig.Instance.SOTSBalanceChanges)
                return;

            if (item.type == ModContent.ItemType<CloverCharm>())
            {
                string nerf = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.CloverNerf2");
                AddTooltip(tooltips, nerf, true);
            }

            if (InfernalCrossmod.SOTSBardHealer.Loaded)
            {
                if (item.type == InfernalCrossmod.SOTSBardHealer.Mod.Find<ModItem>("RingofRest").Type)
                {
                    string nerf = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.RingofRestNerf");
                    AddTooltip(tooltips, nerf, true);
                }
            }

            if (item.type == ModContent.ItemType<SoulCharm>() || item.type == ModContent.ItemType<BagOfCharms>())
            {
                string nerf;
                if (InfernalCrossmod.SOTSBardHealer.Loaded)
                    nerf = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.CloverNerf");
                else
                    nerf = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.CloverNerf2");

                AddTooltip(tooltips, nerf, true);
            }
        }
    }
}
