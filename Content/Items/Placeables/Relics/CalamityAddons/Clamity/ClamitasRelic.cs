using InfernumMode.Content.Items.Relics;
using InfernalEclipseAPI.Content.Tiles.Relics.Thorium;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons.Clamity
{
    public class ClamitasRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Clamitas Relic";

        public override int TileID => ModContent.TileType<ClamitasRelicTile>();
    }
}
