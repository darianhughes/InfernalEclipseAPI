using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Tiles.Relics.FargosSouls;
using InfernumMode.Content.Items.Relics;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.FargosSouls
{
    public class EarthChampionRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Champion of Earth Relic";

        public override int TileID => ModContent.TileType<EarthChampionRelicTile>();
    }
}
