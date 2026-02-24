using CalamityMod.Items.Materials;
using InfernalEclipseAPI.Content.Tiles.MusicBoxes;
using CalamityMod.Items.Placeables;

namespace InfernalEclipseAPI.Content.Items.Placeables.MusicBoxes
{
    public class BossRushTier6MusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(ModContent.TileType<BossRushTier6MusicBoxTile>(), 0);
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
