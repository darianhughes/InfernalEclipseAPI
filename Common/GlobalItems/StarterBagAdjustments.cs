using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    public class StarterBagAdjustments : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            // Only run if both mods are loaded
            if (!ModLoader.TryGetMod("CalamityMod", out var calamityMod) ||
                !ModLoader.TryGetMod("ThoriumMod", out var thoriumMod) ||
                ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                return;

            // Safely get the StarterBag item
            if (!calamityMod.TryFind("StarterBag", out ModItem starterBagItem))
                return;

            if (item.type == starterBagItem.Type)
            {
                if (thoriumMod.TryFind("Tambourine", out ModItem tambourineItem))
                {
                    itemLoot.Add(ItemDropRule.Common(tambourineItem.Type));
                }

                if (thoriumMod.TryFind("Pill", out ModItem pillsItem))
                {
                    itemLoot.Add(ItemDropRule.Common(pillsItem.Type, 1, 200, 200));
                }
            }
        }
    }
}
