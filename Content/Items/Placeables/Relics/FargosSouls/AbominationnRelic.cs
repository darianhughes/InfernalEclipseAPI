using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernumMode.Content.Items.Relics;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using InfernalEclipseAPI.Content.Tiles.Relics.FargosSouls;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.FargosSouls
{
    public class AbominationnRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Abominationn Relic";

        public override int TileID => ModContent.TileType<AbominationnRelicTile>();

        public override Color? PersonalMessageColor => Color.Orange;

        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.AbomRelic");
    }
}
