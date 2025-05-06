using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;

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
            if (ModLoader.TryGetMod("CalamityModMusic", out Mod musicMod))
            {
                int[] musicBox =
                {
                    musicMod.Find<ModItem>("BossRushTier5MusicBox").Type
                };

                foreach (int num in musicBox)
                {
                    foreach (TooltipLine tooltip in tooltips)
                        tooltip.Text = tooltip.Text.Replace("This music box is unfinished an does not work", "Plays 'Omiscience Of Gods' by TheTrester");
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
