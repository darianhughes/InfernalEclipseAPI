using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Tiles.Relics;
using InfernumMode.Content.Items.Relics;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics
{
    public class QueenJellyfishRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Queen Jellyfish Relic";

        public override int TileID => ModContent.TileType<QueenJellyfishRelicTile>();
    }
}
