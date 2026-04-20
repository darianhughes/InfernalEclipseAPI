using InfernalEclipseAPI.Content.Items.Placeables.Relics.Thorium;
using InfernumMode.Content.Tiles.Relics;

namespace InfernalEclipseAPI.Content.Tiles.Relics.Thorium
{
    public class IllusionistRelicTile : BaseInfernumBossRelic
    {
        public override int DropItemID => ModContent.ItemType<IllusionistRelic>();

        public override string RelicTextureName => "InfernalEclipseAPI/Content/Tiles/Relics/Thorium/IllusionistRelicTile";
    }
}
