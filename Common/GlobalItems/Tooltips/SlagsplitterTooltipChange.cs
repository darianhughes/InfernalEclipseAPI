using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using InfernumMode.Content.Items.SummonItems;
using Terraria.Localization;

namespace InfernalEclipseAPI.Common.GlobalItems.Tooltips
{
    public class SlagsplitterTooltipChange : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!InfernalConfig.Instance.CalamityBalanceChanges) return;

            Color InfernalRed = Color.Lerp(
                Color.White,
                new Color(255, 80, 0), // Infernal red/orange
                (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );

            if (item.type == ModContent.ItemType<SlagsplitterPauldron>())
            {
                foreach (TooltipLine tooltip in tooltips)
                {
                    if (tooltip.Text.Contains("You gain 10% damage reduction while dashing"))
                    {
                        tooltip.Text = "You gain 5% damage reduction while dashing";
                    }
                }
                tooltips.Add(new TooltipLine(Mod, "CooldownInfo", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.SlagsplitterCooldown"))
                {
                    OverrideColor = new Color?(InfernalRed)
                });
            }
        }
    }
}
