using CalamityMod.Items.LoreItems;
using CalamityMod.Rarities;
using CalamityMod;

namespace InfernalEclipseAPI.Content.Items.Lore.InfernalEclipse
{
    public class LoreDylan : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.consumable = false;
            Item.Calamity().devItem = true;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Book, 1)
                .AddIngredient(ItemID.TeamBlockYellow, 5)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}
