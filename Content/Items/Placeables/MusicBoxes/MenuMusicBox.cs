using InfernalEclipseAPI.Content.Tiles.MusicBoxes;
using InfernalEclipseAPI.Content.Items.Placeables.Paintings;

namespace InfernalEclipseAPI.Content.Items.Placeables.MusicBoxes
{
    public class MenuMusicBox : ModItem
    {
        public override string Texture => "InfernalEclipseAPI/Assets/Textures/Items/PlaceholderInfernumMusicBox";

        public override void SetStaticDefaults()
        {
            if (Main.dedServ)
                return;
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = 576;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot("InfernalEclipseAPI/Assets/Music/TeardropsofDragonfire"), ModContent.ItemType<MenuMusicBox>(), ModContent.TileType<MenuMusicBoxTile>(), 0);
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<MenuMusicBoxTile>();
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 100000;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MusicBox)
                .AddIngredient<InfernalTwilight>()
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
}
