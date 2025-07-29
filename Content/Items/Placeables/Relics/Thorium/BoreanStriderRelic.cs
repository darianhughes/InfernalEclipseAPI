using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Tiles.Relics.Thorium;
using InfernumMode.Content.Items.Relics;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.Thorium
{
    public class BoreanStriderRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Borean Strider Relic";

        public override int TileID => ModContent.TileType<BoreanStriderRelicTile>();
    }
}
