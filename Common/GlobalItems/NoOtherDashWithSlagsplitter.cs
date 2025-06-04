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

namespace InfernalEclipseAPI.Common.GlobalItems
{
    public class NoOtherDashWithSlagsplitter : GlobalItem
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);

            if (!InfernalConfig.Instance.CalamityBalanceChanges) return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);

            int slagsplitter = ModContent.ItemType<SlagsplitterPauldron>();

            if (equippedItem.type == slagsplitter && IsShield(incomingItem)) return false;
            if (incomingItem.type == slagsplitter && IsShield(equippedItem)) return false;
            if (equippedItem.type == slagsplitter && incomingItem.type == slagsplitter) return false;

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }


        bool IsShield(Item item)
        {
            int[] shields =
            {
                ItemID.EoCShield,
                ModContent.ItemType<OrnateShield>(),
                ModContent.ItemType<AsgardsValor>(),
                ModContent.ItemType<ElysianAegis>(),
                ModContent.ItemType<AsgardianAegis>(),
                ModContent.ItemType<DeepDiver>(),
                ModContent.ItemType<ShieldoftheHighRuler>()
            };

            foreach (int shield in shields)
            {
                if (item.type == shield) return true;
            }

            if (ModLoader.TryGetMod("Clamity", out Mod clam))
            {
                if (item.type == clam.Find<ModItem>("SupremeBarrier").Type) return true;
            }

            if (ModLoader.TryGetMod("ShieldsOfCthulhu", out Mod SoC))
            {
                int[] SoCShields =
                {
                    SoC.Find<ModItem>("CobaltShieldOfCthulhu").Type,
                    SoC.Find<ModItem>("ObsidianShieldOfCthulhu").Type,
                    SoC.Find<ModItem>("AnkhShieldOfCthulhu").Type
                };

                foreach (int shield in SoCShields)
                {
                    if (item.type == shield) return true;
                }
            }

            return false;
        }

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
                //int index = tooltips.FindIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("ItemName"));
                //if (index != -1)
                //    tooltips.Insert(index + 1, new TooltipLine(((ModType)this).Mod, "AccessoryWarning", "-Omni Dash-")
                //    {
                //        OverrideColor = new Color?(new Color(102, (int)byte.MaxValue, (int)byte.MaxValue))
                //    });
                //tooltips.Add(new TooltipLine(Mod, "OmniDashInfo", "Cannot be equiped with most other dash accessories")
                //{
                //    OverrideColor = new Color?(InfernalRed)
                //});{
                foreach (TooltipLine tooltip in tooltips)
                {
                    if (tooltip.Text.Contains("You gain 10% damage reduction while dashing"))
                    {
                        tooltip.Text = "You gain 5% damage reduction while dashing";
                    }
                }
                tooltips.Add(new TooltipLine(Mod, "CooldownInfo", "This effect has a 1.5 second cooldown")
                {
                    OverrideColor = new Color?(InfernalRed)
                });
            }
        }
    }
}
