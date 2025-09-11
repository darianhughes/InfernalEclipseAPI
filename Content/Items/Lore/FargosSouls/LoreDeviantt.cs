using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.LoreItems;
using Microsoft.Xna.Framework.Graphics;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Content.Items.Lore.FargosSouls
{
    public class LoreDeviantt : LoreItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            if (!ModLoader.TryGetMod("FargowiltasSouls", out _)) return false;
            bool hasCSE = ModLoader.TryGetMod("ssm", out Mod cse) && cse.Version > Version.Parse("1.1.4.2");
            return !hasCSE;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 26;
            Item.rare = ItemRarityID.Purple;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            ModLoader.TryGetMod("FargowiltasSouls", out Mod souls);

            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(souls.Find<ModItem>("DeviTrophy").Type)
                .AddTile(TileID.Bookcases)
                .Register();
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if ((line.Mod == "Terraria" && line.Name == "ItemName") || line.Name == "FlavorText")
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
                ManagedShader shader = ShaderManager.GetShader("FargowiltasSouls.Text");
                shader.TrySetParameter("mainColor", new Color(42, 66, 99));
                shader.TrySetParameter("secondaryColor", Color.Pink);
                shader.Apply("PulseUpwards");
                Utils.DrawBorderString(Main.spriteBatch, line.Text, new Vector2(line.X, line.Y), Color.White, 1);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                return false;
            }
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine fullLore = new(Mod, "DeviLore", Language.GetTextValue("Mods.InfernalEclipseAPI.Lore.Devi"));
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
