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
using NoxusBoss.Content.Rarities;
using InfernalEclipseAPI.Content.Tiles.Relics.CalamityAddons.WoTG;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons.WoTG
{
    [ExtendsFromMod("NoxusBoss")]
    public class NamelessDeityRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Nameless Deity Relic";

        public override int TileID => ModContent.TileType<NamelessDeityRelicTile>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ModContent.RarityType<NamelessDeityRarity>();
        }
        public override Color? PersonalMessageColor => Color.Red;
        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.NamelessRelic");
    }
}
