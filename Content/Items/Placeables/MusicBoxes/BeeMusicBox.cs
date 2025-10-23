using InfernumMode.Content.Items.Relics;
using InfernalEclipseAPI.Content.Tiles.MusicBoxes;

namespace InfernalEclipseAPI.Content.Items.Placeables.MusicBoxes
{
    public class BeeMusicBox : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("InfernumModeMusic", out _);
        }

        public override string Texture => "InfernalEclipseAPI/Assets/Textures/Items/PlaceholderInfernumMusicBox";

        public override void SetStaticDefaults()
        {
            if (Main.dedServ)
                return;
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = 576;
            ModLoader.TryGetMod("InfernumModeMusic", out Mod InfernumMusic);
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(InfernumMusic, "Sounds/Music/QueenBee"), ModContent.ItemType<BeeMusicBox>(), ModContent.TileType<BeeMusicBoxTile>(), 0);
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<BeeMusicBoxTile>();
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 100000;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<QueenBeeRelic>(1)
                .AddIngredient(ItemID.MusicBox)
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
}
