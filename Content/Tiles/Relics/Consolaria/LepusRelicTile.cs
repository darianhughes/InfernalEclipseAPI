using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.Consolaria;
using InfernumMode.Content.Tiles.Relics;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Tiles.Relics.Thorium
{
    public class LepusRelicTile : BaseInfernumBossRelic
    {
        public override int DropItemID => ModContent.ItemType<LepusRelic>();

        public override string RelicTextureName => "InfernalEclipseAPI/Content/Tiles/Relics/Consolaria/LepusRelicTile";
    }
}
