using InfernalEclipseAPI.Content.Tiles.MusicBoxes;
using InfernumMode.Content.Items.Relics;

namespace InfernalEclipseAPI.Content.Items.Placeables.MusicBoxes
{
    public class LastBattleMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (Main.dedServ)
                return;
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = 576;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot("InfernalEclipseAPI/Assets/Music/LastBattle"), ModContent.ItemType<LastBattleMusicBox>(), ModContent.TileType<LastBattleMusicBoxTile>(), 0);
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<LastBattleMusicBoxTile>();
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Terraria.Item.sellPrice(silver: 20);
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MusicBox)
                .AddIngredient<DevourerOfGodsRelic>()
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
}
