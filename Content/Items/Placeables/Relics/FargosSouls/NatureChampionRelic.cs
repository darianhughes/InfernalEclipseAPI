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
    public class NatureChampionRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Champion of Nature Relic";

        public override int TileID => ModContent.TileType<NatureChampionRelicTile>();
    }
}
