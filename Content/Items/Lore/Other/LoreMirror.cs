using CalamityMod.Items.LoreItems;
using CalamityMod.Rarities;

namespace InfernalEclipseAPI.Content.Items.Lore.Other
{
    public class LoreMirror : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 26;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            if (ModLoader.TryGetMod("YouBoss", out Mod you))
            {
                CreateRecipe()
                    .AddIngredient(ItemID.Book)
                    .AddIngredient(you.Find<ModItem>("FirstFractal").Type)
                    .AddTile(TileID.Bookcases)
                    .Register();
            }
        }
    }
}
