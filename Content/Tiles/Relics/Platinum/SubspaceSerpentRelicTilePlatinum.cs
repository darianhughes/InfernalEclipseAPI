using CalamityMod.Tiles.BaseTiles;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.Platinum;

namespace InfernalEclipseAPI.Content.Tiles.Relics.Platinum
{
    public class SubspaceSerpentRelicTilePlatinum : BaseBossRelic
    {
        public override string RelicTextureName => "InfernalEclipseAPI/Content/Tiles/Relics/Platinum/SubspaceSerpentRelicTilePlatinum";
        public override int AssociatedItem => ModContent.ItemType<SubspaceSerpentRelicPlatinum>();
        public override string Texture => "InfernalEclipseAPI/Content/Tiles/Relics/Platinum/PlatinumRelicBase";
    }
}
