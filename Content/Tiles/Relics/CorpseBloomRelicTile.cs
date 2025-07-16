using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Items.Placeables.Relics;
using InfernumMode.Content.Tiles.Relics;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Tiles.Relics
{
    public class CorpseBloomRelicTile : BaseInfernumBossRelic
    {
        public override int DropItemID => ModContent.ItemType<CorpseBloomRelic>();

        public override string RelicTextureName => "InfernalEclipseAPI/Content/Tiles/Relics/CorpseBloomRelicTile";
    }
}
