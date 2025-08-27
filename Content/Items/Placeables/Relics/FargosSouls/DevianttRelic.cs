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
    public class DevianttRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Deviantt Relic";

        public override int TileID => ModContent.TileType<DevianttRelicTile>();

        public override Color? PersonalMessageColor => Color.Pink;

        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.DeviRelic");
    }
}
