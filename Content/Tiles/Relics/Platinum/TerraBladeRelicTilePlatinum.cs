using CalamityMod.Tiles.BaseTiles;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.Platinum;

namespace InfernalEclipseAPI.Content.Tiles.Relics.Platinum
{
    public class TerraBladeRelicTilePlatinum : BaseBossRelic
    {
        public override string RelicTextureName => "InfernalEclipseAPI/Content/Tiles/Relics/Platinum/TerraBladeRelicTilePlatinum";
        public override int AssociatedItem => ModContent.ItemType<TerraBladeRelicPlatinum>();
        public override string Texture => "CatalystMod/Tiles/Furniture/BossRelics";
    }
}
