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
    public class MutantRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Mutant Relic";

        public override int TileID => ModContent.TileType<MutantRelicTile>();

        public override Color? PersonalMessageColor => Color.Cyan;

        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MutantRelic");
    }
}
