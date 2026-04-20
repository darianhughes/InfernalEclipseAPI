using InfernumMode.Content.Items.Relics;
using InfernalEclipseAPI.Content.Tiles.Relics.Thorium;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons.Clamity
{
    public class WallofBronzeRelic : BaseRelicItem
    {
        public override LocalizedText Tooltip => Language.GetOrRegister(InfernalEclipseAPI.Instance.GetLocalizationKey($"Items.{this.Name}.Tooltip")).WithFormatArgs(PersonalMessage);
        public override int TileID => ModContent.TileType<WallofBronzeRelicTile>();
    }
}
