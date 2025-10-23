using CalamityMod.Items.Materials;
using CalamityMod.Items;
using InfernalEclipseAPI.Content.Tiles.MusicBoxes;

namespace InfernalEclipseAPI.Content.Items.Placeables.MusicBoxes
{
    public class BossRushEncoreMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(ModContent.TileType<BossRushEncoreMusicBoxTile>(), 0);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Rock>(1).
                AddIngredient<ShadowspecBar>(3).
                AddIngredient(ItemID.MusicBox).
                AddTile(TileID.HeavyWorkBench).
                Register();
        }

    }
}
