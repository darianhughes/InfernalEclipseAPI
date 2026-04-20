using InfernumMode.Content.Items.Relics;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using NoxusBoss.Content.Rarities;
using InfernalEclipseAPI.Content.Tiles.Relics.CalamityAddons.WoTG;
using System.Collections.Generic;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons.WoTG
{
    [ExtendsFromMod("NoxusBoss")]
    public class AvatarOfEmptinessRelic : BaseRelicItem
    {
        public override int TileID => ModContent.TileType<AvatarOfEmptinessRelicTile>();

        public override Color? PersonalMessageColor => Color.Red;

        public override LocalizedText Tooltip => Language.GetOrRegister(InfernalEclipseAPI.Instance.GetLocalizationKey($"Items.{this.Name}.Tooltip")).WithFormatArgs(PersonalMessage);

        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.AvatarRelic");

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ModContent.RarityType<AvatarRarity>();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name.StartsWith("Tooltip"))
                {
                    if (PersonalMessageColor is null)
                    {
                        float colorInterpolant = (float)(Math.Sin(Math.PI * Main.GlobalTimeWrappedHourly + 1f) * 0.5f) + 0.5f;
                        Color c = LumUtils.MulticolorLerp(
                            colorInterpolant,
                            new Color(170, 0, 0, 255),
                            Color.OrangeRed,
                            new Color(255, 200, 0, 255)
                        );

                        line.OverrideColor = c;
                    }
                    else
                    {
                        line.OverrideColor = PersonalMessageColor;
                    }
                }
            }
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
