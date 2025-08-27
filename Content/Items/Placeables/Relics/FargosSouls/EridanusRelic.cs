using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Tiles.Relics.FargosSouls;
using InfernumMode.Content.Items.Relics;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.FargosSouls
{
    public class EridanusRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Eridanus, Champion of Cosmos Relic";

        public override int TileID => ModContent.TileType<EridanusRelicTile>();

        public override Color? PersonalMessageColor => Color.Plum;

        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.EridanusRelic");
    }
}
