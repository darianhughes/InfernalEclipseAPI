using InfernumMode.Content.Items.Relics;
using Terraria.Localization;
using InfernalEclipseAPI.Content.Tiles.Relics.Thorium;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons
{
    public class NoxusRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Noxus Relic";

        public override int TileID => ModContent.TileType<NoxusRelicTile>();

        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.NoxusRelic");
    }
}
