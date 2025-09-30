using InfernumMode.Content.Items.Relics;
using InfernalEclipseAPI.Content.Tiles.Relics.Thorium;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons.Clamity
{
    public class PyrogenRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Pyrogen Relic";

        public override int TileID => ModContent.TileType<PyrogenRelicTile>();
    }
}
