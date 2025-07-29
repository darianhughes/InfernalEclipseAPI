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

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons
{
    [ExtendsFromMod("NoxusBoss")]
    public class AvatarOfEmptinessRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Avatar of Emptiness Relic";

        public override int TileID => ModContent.TileType<AvatarOfEmptinessRelicTile>();

        public override Color? PersonalMessageColor => Color.Red;

        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.AvatarRelic");

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ModContent.RarityType<AvatarRarity>();
        }

        //public override void ModifyTooltips(List<TooltipLine> tooltips)
        //{
        //    string[] tooltipArray = 
        //    {
        //        Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.AvatarRelic")
        //    };

        //    for (int i = 0; i < tooltipArray.Length; ++i)
        //    {
        //        Mod mod = Mod;
        //        string tooltip1;
        //        if (i != tooltipArray.Length - 1) tooltip1 = $"Tooltip{i}";
        //        else tooltip1 = "TooltipLast";
        //        string tooltip2 = tooltipArray[i];
        //        TooltipLine tooltipLine = new TooltipLine(mod, tooltip1, tooltip2);
        //        if (tooltipLine.Name != "TooltipLast") tooltipLine.OverrideColor = new Color?(new Color(200, 100, byte.MaxValue));
        //        tooltips.Add(tooltipLine);
        //    }
        //}
    }
}
