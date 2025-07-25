using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalystMod.Items;
using InfernalEclipseAPI.Content.Tiles.Relics;
using InfernumMode.Content.Items.Relics;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics
{
    [ExtendsFromMod("CatalystMod")]
    public class AstrageldonRelic : BaseRelicItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return !ModLoader.TryGetMod("CnI", out _);
        }
        public override string DisplayNameToUse => "Infernal Astrageldon Relic";

        public override int TileID => ModContent.TileType<AstrageldonRelicTile>();

        public override string PersonalMessage => null;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = CatalystItem.RarityMasterSuperboss;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string[] tooltipArray = new string[3]
            {
                Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.AstrageldonRelic.Line1"),
                Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.AstrageldonRelic.Line2"),
                Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.AstrageldonRelic.Line3"),
            };

            for (int i = 0; i < tooltipArray.Length; ++i)
            {
                Mod mod = Mod;
                string tooltip1;
                if (i != tooltipArray.Length - 1) tooltip1 = $"Tooltip{i}";
                else tooltip1 = "TooltipLast";
                string tooltip2 = tooltipArray[i];
                TooltipLine tooltipLine = new TooltipLine(mod, tooltip1, tooltip2);
                if (tooltipLine.Name != "TooltipLast") tooltipLine.OverrideColor = new Color?(new Color(200, 100, (int)byte.MaxValue));
                tooltips.Add(tooltipLine);
            }
        }
    }

    [ExtendsFromMod("CatalystMod")]
    public class AstrageldonRelicGlobal : GlobalItem
    {
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (item.ModItem is AstrageldonRelic && (line.Name == "ItemName"))
            {
                SuperbossRarity.Draw(item, line);
                return false;
            }
            if (!(item.ModItem is AstrageldonRelic) || !(line.Name == "TooltipLast"))
                return true;
            SuperbossRarity.Draw(item, line);
            return false;
        }
    }
}
