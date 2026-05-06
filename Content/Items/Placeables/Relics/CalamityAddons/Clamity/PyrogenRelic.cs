using InfernumMode.Content.Items.Relics;
using InfernalEclipseAPI.Content.Tiles.Relics.Thorium;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons.Clamity
{
    public class PyrogenRelic : BaseRelicItem
    {
        public override LocalizedText Tooltip => Language.GetOrRegister(InfernalEclipseAPI.Instance.GetLocalizationKey($"Items.{this.Name}.Tooltip")).WithFormatArgs(PersonalMessage);
        public override int TileID => ModContent.TileType<PyrogenRelicTile>();

        public override Color? PersonalMessageColor => Color.OrangeRed;

        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.PyrogenRelic");

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
