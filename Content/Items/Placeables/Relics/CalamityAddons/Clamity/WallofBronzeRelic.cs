using InfernumMode.Content.Items.Relics;
using InfernalEclipseAPI.Content.Tiles.Relics.Thorium;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons.Clamity
{
    public class WallofBronzeRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Wall of Bronze Relic";

        public override int TileID => ModContent.TileType<WallofBronzeRelicTile>();
    }
}
