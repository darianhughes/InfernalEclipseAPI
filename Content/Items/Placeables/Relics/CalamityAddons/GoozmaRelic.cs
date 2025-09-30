using InfernumMode.Content.Items.Relics;
using Terraria.Localization;
using InfernalEclipseAPI.Content.Tiles.Relics.Thorium;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons
{
    public class GoozmaRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Goozma Relic";

        public override int TileID => ModContent.TileType<GoozmaRelicTile>();

        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.GoozmaRelic");
    }
}
