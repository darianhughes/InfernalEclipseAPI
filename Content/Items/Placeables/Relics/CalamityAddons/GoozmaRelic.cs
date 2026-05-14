using InfernumMode.Content.Items.Relics;
using Terraria.Localization;
using InfernalEclipseAPI.Content.Tiles.Relics.Thorium;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons
{
    public class GoozmaRelic : BaseRelicItem
    {
        public override int TileID => ModContent.TileType<GoozmaRelicTile>();

        public override Color? PersonalMessageColor => Color.DarkMagenta;

        public override LocalizedText Tooltip => Language.GetOrRegister(InfernalEclipseAPI.Instance.GetLocalizationKey($"Items.{this.Name}.Tooltip")).WithFormatArgs(PersonalMessage);

        public override string PersonalMessage => Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.GoozmaRelic");

        public override void SetStaticDefaults()
        {
            if (ModLoader.HasMod("CalamityHunt"))
                ItemID.Sets.ShimmerTransformToItem[Type] = ModLoader.GetMod("CalamityHunt").Find<ModItem>("GoozmaInfernumRelic").Type;
            base.SetStaticDefaults();
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
    }
}
