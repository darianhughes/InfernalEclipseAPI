using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalystMod.Items;
using InfernumMode.Content.Items.Relics;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using InfernalEclipseAPI.Content.Tiles.Relics.CalamityAddons;
using InfernalEclipseAPI.Content.Tiles.Relics.Thorium;
using NoxusBoss.Content.Rarities;
using CalamityMod.Rarities;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons
{
    public class GoozmaRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Goozma Relic";

        public override int TileID => ModContent.TileType<GoozmaRelicTile>();

        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.GoozmaRelic");
    }
}
