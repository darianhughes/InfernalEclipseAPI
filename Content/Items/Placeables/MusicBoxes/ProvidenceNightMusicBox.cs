using InfernalEclipseAPI.Content.Tiles.MusicBoxes;
using InfernalEclipseAPI.Content.Items.Placeables.Paintings;

namespace InfernalEclipseAPI.Content.Items.Placeables.MusicBoxes
{
    public class ProvidenceNightMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (Main.dedServ)
                return;
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = 576;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot("InfernalEclipseAPI/Assets/Music/ProvidenceNight"), ModContent.ItemType<ProvidenceNightMusicBox>(), ModContent.TileType<ProvidenceNightMusicBoxTile>(), 0);
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ProvidenceNightMusicBoxTile>();
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Terraria.Item.sellPrice(silver: 20);
            Item.accessory = true;
        }
    }
}
