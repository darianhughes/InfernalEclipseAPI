using CalamityMod.Items.LoreItems;
using CalamityMod.Rarities;
using CalamityMod.Items.Materials;

namespace InfernalEclipseAPI.Content.Items.Lore.InfernalEclipse
{
    public class LoreMegalomaniac : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 26;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            return;

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LoreCynosure>())
                .AddIngredient(ModContent.ItemType<ShadowspecBar>())
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}
