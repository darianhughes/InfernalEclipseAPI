using InfernalEclipseAPI.Content.Tiles.Relics;
using InfernumMode.Content.Items.Relics;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics
{
    public class MaulerRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Mauler Relic";

        public override int TileID => ModContent.TileType<MaulerRelicTile>();
    }
}
