using InfernalEclipseAPI.Content.Items.Placeables.Relics;
using InfernumMode.Content.Tiles.Relics;

namespace InfernalEclipseAPI.Content.Tiles.Relics
{
    public class CragmawMireRelicTile : BaseInfernumBossRelic
    {
        public override int DropItemID => ModContent.ItemType<CragmawMireRelic>();

        public override string RelicTextureName => "InfernalEclipseAPI/Content/Tiles/Relics/CragmawMireRelicTile";
    }
}
