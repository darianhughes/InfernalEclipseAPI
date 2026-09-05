using CatalystMod.Items;
using CatalystMod.Tiles.Furniture;
using InfernalEclipseAPI.Content.Tiles.Relics.Platinum;
using Terraria.GameContent.Creative;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.Platinum
{

	public class TerraBladeRelicPlatinum : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<TerraBladeRelicTilePlatinum>(), 0);
			Item.width = 30;
			Item.height = 40;
			Item.maxStack = 9999;
			Item.rare = CatalystItem.RarityMasterSuperboss;
			Item.master = true;
			Item.value = Item.buyPrice(0, 5, 0, 0);
		}
	}
}