using InfernumMode.Content.Items.Relics;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using NoxusBoss.Content.Rarities;
using InfernalEclipseAPI.Content.Tiles.Relics.CalamityAddons.WoTG;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons.WoTG
{
    [ExtendsFromMod("NoxusBoss")]
    public class NamelessDeityRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Nameless Deity Relic";

        public override int TileID => ModContent.TileType<NamelessDeityRelicTile>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ModContent.RarityType<NamelessDeityRarity>();
        }
        public override Color? PersonalMessageColor => Color.Red;
        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.NamelessRelic");
    }
}
