using InfernumMode.Content.Items.Relics;
using InfernalEclipseAPI.Content.Tiles.MusicBoxes;

namespace InfernalEclipseAPI.Content.Items.Placeables.MusicBoxes
{
    public class QueenSlimeMusicBox : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.HasMod("InfernumModeMusic");
        }

        public override void SetStaticDefaults()
        {
            if (Main.dedServ)
                return;
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = 576;

            Mod InfernumMusic = ModLoader.GetMod("InfernumModeMusic");
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(InfernumMusic, "Sounds/Music/QueenSlime"), ModContent.ItemType<QueenSlimeMusicBox>(), ModContent.TileType<QueenSlimeMusicBoxTile>(), 0);
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<QueenSlimeMusicBoxTile>();
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 100000;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<QueenSlimeRelic>(1)
                .AddIngredient(ItemID.MusicBox)
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
}
