using InfernalEclipseAPI.Content.Tiles.Relics;
using InfernumMode.Content.Items.Relics;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics
{
    public class InfernalTerraBladeRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Terra Blade Relic";

        public override int TileID => ModContent.TileType<InfernalTerraBladeRelicTile>();
    }
}
