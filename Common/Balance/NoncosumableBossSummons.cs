using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using InfernumMode.Content.Items.SummonItems;

namespace InfernalEclipseAPI.Common.Balance
{
    public class NoncosumableBossSummons : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            //Consolaria
            if (ModLoader.TryGetMod("Consolaria", out Mod console))
            {
                //Suspicious Looking Egg
                if (item.type == console.Find<ModItem>("SuspiciousLookingEgg").Type)
                {
                    item.consumable = false;
                }

                //Cursed Stuffing
                if (item.type == console.Find<ModItem>("CursedStuffing").Type)
                {
                    item.consumable = false;
                }

                //Suspicious Looking Skull
                if (item.type == console.Find<ModItem>("SuspiciousLookingSkull").Type)
                {
                    item.consumable = false;
                }
            }

            //Secrets of the Shadows
            if (ModLoader.TryGetMod("SOTS", out Mod sots))
            {
                //Suspicious Looking Candle
                if (item.type == sots.Find<ModItem>("SuspiciousLookingCandle").Type)
                {
                    item.consumable = false;
                }

                if (item.type == sots.Find<ModItem>("JarOfPeanuts").Type)
                {
                    item.consumable = false;
                }

                if (item.type == sots.Find<ModItem>("CatalystBomb").Type)
                {
                    item.consumable = false;
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {

            foreach (TooltipLine tooltip in tooltips)
            {
                if (tooltip.Text.Contains("This music box is unfinished and does not work"))
                {
                    tooltip.Text = "Plays 'Omniscience Of Gods' by TheTrester";
                }

                if (tooltip.Text.Contains("It becomes nighttime if this item is used during daytime") && item.type == ModContent.ItemType<DungeonsCurse>())
                {
                    tooltip.Text = "Can only be used at night";
                }
            }

            if (ModLoader.TryGetMod("Consolaria", out Mod console)) {
                int[] consoleArray =
                {
                    console.Find<ModItem>("SuspiciousLookingEgg").Type,
                    console.Find<ModItem>("CursedStuffing").Type,
                    console.Find<ModItem>("SuspiciousLookingSkull").Type
                };

                foreach (int num in consoleArray)
                {
                    foreach (TooltipLine tooltip in tooltips)
                        if (num == item.type) 
                        {
                            //tooltip.Text = tooltip.Text.Replace("Consumable", "Not consumable");
                            if (!tooltips[^1].Text.Contains("Not consumable"))
                            {
                                tooltips[^1].Text += "\nNot consumable";
                            }
                        }

                }
            }

            if (ModLoader.TryGetMod("SOTS", out Mod sots))
            {
                int[] sotsArray =
                {
                    sots.Find<ModItem>("SuspiciousLookingCandle").Type,
                    sots.Find<ModItem>("JarOfPeanuts").Type,
                    sots.Find<ModItem>("CatalystBomb").Type
                };

                foreach (int num in sotsArray)
                {
                    foreach (TooltipLine tooltip in tooltips)
                        if (num == item.type)
                            if (!tooltips[^1].Text.Contains("Not consumable"))
                            {
                                tooltips[^1].Text += "\nNot consumable";
                            }
                }
            }
        }
    }
}
