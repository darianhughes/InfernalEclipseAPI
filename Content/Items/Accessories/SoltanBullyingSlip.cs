using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.LoreItems;
using CalamityMod.Rarities;
using InfernalEclipseAPI.Core.Players;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Items.Accessories
{
    public class SoltanBullyingSlip : LoreItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = false;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 32;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<CalamityRed>();
            Item.accessory = true;
            Item.defense = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            InfernalPlayer infernalPlayer = player.GetModPlayer<InfernalPlayer>();
            infernalPlayer.soltanBullying = true;
        }
        public override LocalizedText Tooltip => Language.GetOrRegister("Mods.InfernalEclipseAPI.BullyingTooltip");

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine fullLore = new(Mod, "DiaryLore", Language.GetTextValue("Mods.InfernalEclipseAPI.Lore.DylanSoltan"));
            if (LoreColor.HasValue)
                fullLore.OverrideColor = LoreColor.Value;
            HoldShiftTooltip(tooltips, new TooltipLine[] { fullLore }, true);
        }

        private static void HoldShiftTooltip(List<TooltipLine> tooltips, TooltipLine[] holdShiftTooltips, bool hideNormalTooltip = false)
        {
            // Only perform any changes while holding SHIFT.
            if (!Main.keyState.IsKeyDown(Keys.LeftShift))
                return;

            // Get the first index, last index and total count of standard vanilla tooltip lines.
            // The first index and count are used to delete all vanilla tooltips when holding SHIFT, if requested.
            // The last index is used to insert the "Hold SHIFT" tooltips in the right position.
            int firstTooltipIndex = -1;
            int lastTooltipIndex = -1;
            int standardTooltipCount = 0;
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Name.StartsWith("Tooltip"))
                {
                    if (firstTooltipIndex == -1)
                        firstTooltipIndex = i;
                    lastTooltipIndex = i;
                    standardTooltipCount++;
                }
            }

            if (firstTooltipIndex != -1)
            {
                // If asked to, remove all standard tooltip lines. This moves the last tooltip index.
                if (hideNormalTooltip)
                {
                    tooltips.RemoveRange(firstTooltipIndex, standardTooltipCount);
                    lastTooltipIndex -= standardTooltipCount;
                }

                // Append every "Hold SHIFT" tooltip at the end of standard tooltips.
                tooltips.InsertRange(lastTooltipIndex + 1, holdShiftTooltips);
            }
        }
    }
}
