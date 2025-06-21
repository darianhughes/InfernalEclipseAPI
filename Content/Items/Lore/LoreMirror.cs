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

namespace InfernalEclipseAPI.Content.Items.Lore
{
    public class LoreMirror : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 26;
            Item.rare = ItemRarityID.Purple;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            ModLoader.TryGetMod("YouBoss", out Mod you);

            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(you.Find<ModItem>("FirstFractal").Type)
                .AddTile(TileID.Bookcases)
                .Register();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine fullLore = new(Mod, "MirrorLore", "I stared into it for a long while, but like any other mirror, I only saw myself.\nBut even then, something felt... different. It felt wrong, almost... twisted.\nI disregarded it. But even now, I can feel the piercing gaze my reflection had... shooting directly through my skull.\nOnce you come so far, the hardest part of your journey is facing yourself.");
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
