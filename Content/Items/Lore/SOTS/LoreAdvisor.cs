using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.LoreItems;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Items.Lore.SOTS
{
    public class LoreAdvisor : LoreItem
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.TryGetMod("SOTS", out _);

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 26;
            Item.rare = ItemRarityID.Green;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            ModLoader.TryGetMod("SOTS", out Mod sots);

            CreateRecipe()
                .AddIngredient(sots.Find<ModItem>("AdvisorTrophy").Type)
                .AddTile(TileID.Bookcases)
                .Register();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine fullLore = new(Mod, "AdvisorLore", Language.GetTextValue("Mods.InfernalEclipseAPI.Lore.Advisor"));
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
