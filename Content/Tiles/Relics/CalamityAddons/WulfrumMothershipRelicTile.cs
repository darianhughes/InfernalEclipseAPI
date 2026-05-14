using InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons;
using InfernumMode.Content.Tiles.Relics;

namespace InfernalEclipseAPI.Content.Tiles.Relics
{
    public class WulfrumMothershipRelicTile : BaseInfernumBossRelic
    {
        public override int DropItemID => ModContent.ItemType<WulfrumMothershipRelic>();

        public override string RelicTextureName => "InfernalEclipseAPI/Content/Tiles/Relics/CalamityAddons/WulfrumMothershipRelicTile";
    }
}
