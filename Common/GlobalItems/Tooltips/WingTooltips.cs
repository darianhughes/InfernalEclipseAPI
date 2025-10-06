using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace InfernalEclipseAPI.Common.GlobalItems.Tooltips
{
    //Provided by Wardrobe Hummus
    public class WingTooltips : GlobalItem
    {
        public void AddTooltip(List<TooltipLine> tooltips, string stealthTooltip, bool InfernalRedActive = false)
        {
            Color InfernalRed = Color.Lerp(
               Color.White,
               new Color(255, 80, 0), // Infernal red/orange
               (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );

            int maxTooltipIndex = -1;
            int maxNumber = -1;

            // Find the TooltipLine with the highest TooltipX name
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Mod == "Terraria" && tooltips[i].Name.StartsWith("Tooltip"))
                {
                    if (int.TryParse(tooltips[i].Name.Substring(7), out int num) && num > maxNumber)
                    {
                        maxNumber = num;
                        maxTooltipIndex = i;
                    }
                }
            }

            // If found, insert a new TooltipLine right after it with the desired color
            if (maxTooltipIndex != -1)
            {
                int insertIndex = maxTooltipIndex + 1;
                TooltipLine customLine = new TooltipLine(Mod, "StealthTooltip", stealthTooltip);
                if (InfernalRedActive)
                    customLine.OverrideColor = InfernalRed;

                tooltips.Insert(insertIndex, customLine);
            }
        }

        public void AddTooltipBeforeSplash(List<TooltipLine> tooltips, string stealthTooltip)
        {
            int maxTooltipIndex = -1;
            int maxNumber = -1;

            // Find the TooltipLine with the highest TooltipX name
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Mod == "Terraria" && tooltips[i].Name.StartsWith("Tooltip"))
                {
                    if (int.TryParse(tooltips[i].Name.Substring(7), out int num) && num > maxNumber)
                    {
                        maxNumber = num;
                        maxTooltipIndex = i;
                    }
                }
            }

            // If found, insert a new TooltipLine right after it with the desired color
            if (maxTooltipIndex != -1)
            {
                int insertIndex = maxTooltipIndex;
                TooltipLine customLine = new TooltipLine(Mod, "StealthTooltip", stealthTooltip);
                tooltips.Insert(insertIndex, customLine);
            }
        }

        public void FullTooltipOveride(List<TooltipLine> tooltips, string stealthTooltip)
        {
            for (int index = 0; index < tooltips.Count; ++index)
            {
                if (tooltips[index].Mod == "Terraria")
                {
                    if (tooltips[index].Name == "Tooltip0")
                    {
                        TooltipLine tooltip = tooltips[index];
                        tooltip.Text = $"{stealthTooltip}";
                    }
                    else if (tooltips[index].Name.Contains("Tooltip"))
                    {
                        tooltips[index].Hide();
                    }
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModLoader.TryGetMod("SOTS", out Mod sots) && InfernalConfig.Instance.SOTSBalanceChanges)
            {
                if (item.type == sots.Find<ModItem>("GelWings").Type)
                {
                    //AddTooltipBeforeSplash(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.WingInfo.GelDash"));
                }
            }

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) && InfernalConfig.Instance.ThoriumBalanceChangess && !ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
            {
                if (!thoriumMod.TryFind("SubspaceWings", out ModItem subspaceWings) ||
                    !thoriumMod.TryFind("TerrariumWings", out ModItem terrariumWings) ||
                    !thoriumMod.TryFind<ModItem>("WhiteDwarfThrusters", out var whiteDwarfItem))
                    return;

                if (item.type == subspaceWings.Type)
                {
                    AddTooltipBeforeSplash(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.WingInfo.5"));
                }
                else if (item.type == terrariumWings.Type)
                {
                    AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.WingInfo.10"));
                }

                if (item.type == whiteDwarfItem.Type)
                {
                    AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.WingInfo.FasterIvory"));
                }

                if (item.type == thoriumMod.Find<ModItem>("ChampionWing").Type)
                {
                    FullTooltipOveride(tooltips, Language.GetTextValue("Mods.ThoriumMod.Items.ChampionWing.Tooltip"));
                }
            }
        }
    }
}
