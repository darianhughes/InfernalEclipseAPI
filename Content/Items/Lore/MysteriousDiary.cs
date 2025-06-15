using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.Items.LoreItems;
using CalamityMod.Rarities;
using InfernumMode.Content.Items.Placeables;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Lore
{
    public class MysteriousDiary : LoreItem
    {

        public override LocalizedText Tooltip => Language.GetOrRegister("Mods.InfernalEclipseAPI.DiaryTooltip");

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.consumable = false;
            Item.Calamity().devItem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DemonicChaliceOfInfernum>())
                .AddTile(TileID.Bookcases)
                .Register();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine fullLore = new(Mod, "DiaryLore", "Turning to page one, you see nothing except a few words.\n\n\"Who the hell am I, that I still get to live a life?\"\n\nThe rest of the pages are blank...\n[c/E3AF40:The book feels otherworldly... perhaps there is something greater than Yharim or even the deity out there.]\n-Stay tunned for the future of Infernal Eclipse of Ragnarok!-");
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
