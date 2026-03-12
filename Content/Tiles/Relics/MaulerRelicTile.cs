using InfernalEclipseAPI.Content.Items.Placeables.Relics;
using InfernumMode.Content.Tiles.Relics;

namespace InfernalEclipseAPI.Content.Tiles.Relics
{
    public class MaulerRelicTile : BaseInfernumBossRelic
    {
        public override int DropItemID => ModContent.ItemType<MaulerRelic>();

        public override string RelicTextureName => "InfernalEclipseAPI/Content/Tiles/Relics/MaulerRelicTile";
    }
}
