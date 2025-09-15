using InfernalEclipseAPI.Content.Items.Placeables.Relics;
using InfernumMode.Content.Tiles.Relics;

namespace InfernalEclipseAPI.Content.Tiles.Relics
{
    public class InfernalTerraBladeRelicTile : BaseInfernumBossRelic
    {
        public override int DropItemID => ModContent.ItemType<InfernalTerraBladeRelic>();

        public override string RelicTextureName => "InfernalEclipseAPI/Content/Tiles/Relics/InfernalTerraBladeRelicTile";
    }
}
