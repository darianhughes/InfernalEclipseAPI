using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.FargosSouls;
using InfernumMode.Content.Tiles.Relics;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Tiles.Relics.FargosSouls
{
    public class WillChampionRelicTile : BaseInfernumBossRelic
    {
        public override int DropItemID => ModContent.ItemType<WillChampionRelic>();

        public override string RelicTextureName => "InfernalEclipseAPI/Content/Tiles/Relics/FargosSouls/WillChampionRelicTile";
    }
}
