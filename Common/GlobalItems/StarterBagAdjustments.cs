using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.LoreItems;
using InfernalEclipseAPI.Content.Items.Lore;
using CalamityMod.Items.TreasureBags.MiscGrabBags;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    public class StarterBagAdjustments : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ModContent.ItemType<StarterBag>())
            {
                itemLoot.Add(ItemDropRule.ByCondition(new ProviPlayerCondition(), ModContent.ItemType<LoreProvi>()));
                itemLoot.Add(ItemDropRule.ByCondition(new ProviPlayerCondition(), ModContent.ItemType<MysteriousDiary>()));
            }

            if (!ModLoader.TryGetMod("ThoriumMod", out var thoriumMod) ||
                ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                return;

            if (item.type == ModContent.ItemType<StarterBag>())
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

    public class ProviPlayerCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            // Loop through all players in the world
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && player.name == "Galactica")
                    return true;
            }
            return false;
        }

        public bool CanShowItemDropInUI() => false;
        public string GetConditionDescription() => "A certain person must be present...";
    }
}
