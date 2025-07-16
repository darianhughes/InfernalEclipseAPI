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
    public class CorpseBloomRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Corpse Bloom Relic";

        public override int TileID => ModContent.TileType<CorpseBloomRelicTile>();
    }
}
