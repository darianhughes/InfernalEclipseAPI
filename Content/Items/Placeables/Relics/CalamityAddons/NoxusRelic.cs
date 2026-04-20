using InfernumMode.Content.Items.Relics;
using Terraria.Localization;
using InfernalEclipseAPI.Content.Tiles.Relics.Thorium;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons
{
    public class NoxusRelic : BaseRelicItem
    {
        public override int TileID => ModContent.TileType<NoxusRelicTile>();

        public override LocalizedText Tooltip => Language.GetOrRegister(InfernalEclipseAPI.Instance.GetLocalizationKey($"Items.{this.Name}.Tooltip")).WithFormatArgs(PersonalMessage);

        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.NoxusRelic");
    }
}
