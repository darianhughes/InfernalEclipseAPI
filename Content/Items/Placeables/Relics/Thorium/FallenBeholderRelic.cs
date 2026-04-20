using InfernalEclipseAPI.Content.Tiles.Relics.Thorium;
using InfernumMode.Content.Items.Relics;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.Thorium
{
    public class FallenBeholderRelic : BaseRelicItem
    {
        public override LocalizedText Tooltip => Language.GetOrRegister(InfernalEclipseAPI.Instance.GetLocalizationKey($"Items.{this.Name}.Tooltip")).WithFormatArgs(PersonalMessage);
        public override int TileID => ModContent.TileType <FallenBeholderRelicTile>();
    }
}
