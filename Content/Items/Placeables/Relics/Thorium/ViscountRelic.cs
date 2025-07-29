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
    public class ViscountRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Viscount Relic";

        public override int TileID => ModContent.TileType <ViscountRelicTile>();
    }
}
