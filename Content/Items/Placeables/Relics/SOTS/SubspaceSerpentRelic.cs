using System.Collections.Generic;
using InfernalEclipseAPI.Content.Tiles.Relics.Thorium;
using InfernumMode.Content.Items.Relics;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.SOTS
{
    public class SubspaceSerpentRelic : BaseRelicItem
    {
        public override LocalizedText Tooltip => Language.GetOrRegister(InfernalEclipseAPI.Instance.GetLocalizationKey($"Items.{this.Name}.Tooltip")).WithFormatArgs(PersonalMessage);
        public override int TileID => ModContent.TileType<SubspaceSerpentRelicTile>();
        public override Color? PersonalMessageColor => Color.DarkOliveGreen;

        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.SubspaceRelic");

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
    }
}
