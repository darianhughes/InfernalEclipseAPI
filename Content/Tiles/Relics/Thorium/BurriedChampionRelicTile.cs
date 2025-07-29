using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.Thorium;
using InfernumMode.Content.Tiles.Relics;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Tiles.Relics.Thorium
{
    public class BurriedChampionRelicTile : BaseInfernumBossRelic
    {
        public override int DropItemID => ModContent.ItemType<BurriedChampionRelic>();

        public override string RelicTextureName => "InfernalEclipseAPI/Content/Tiles/Relics/BurriedChampionRelicTile";
    }
}
