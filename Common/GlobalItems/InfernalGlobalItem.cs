using Terraria.GameContent.ItemDropRules;
using CalamityMod.Items.TreasureBags.MiscGrabBags;
using CalamityMod.Items.SummonItems;
using InfernalEclipseAPI.Content.Items.Lore.InfernalEclipse;
using InfernalEclipseAPI.Core.Players;
using InfernalEclipseAPI.Core.World;
using Terraria.DataStructures;
using InfernalEclipseAPI.Content.Items.Placeables.Paintings;
using InfernalEclipseAPI.Content.Items.Placeables.MusicBoxes;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    public class InfernalGlobalItem : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ModContent.ItemType<StarterBag>())
            {
                if (ModLoader.TryGetMod("ThoriumMod", out var thoriumMod) && !ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
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

                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MenuMusicBox>()));

                itemLoot.Add(ItemDropRule.ByCondition(new ProviPlayerCondition(), ModContent.ItemType<LoreProvi>()));
                itemLoot.Add(ItemDropRule.ByCondition(new ProviPlayerCondition(), ModContent.ItemType<MysteriousDiary>()));

                itemLoot.Add(ItemDropRule.ByCondition(new SoltanPlayerCondition(), ModContent.ItemType<LoreDylan>()));

                itemLoot.Add(ItemDropRule.ByCondition(new CheesePlayerCondition(), ModContent.ItemType<DeathWhistle>()));

                itemLoot.Add(ItemDropRule.ByCondition(new devListPlayerCondition(), ModContent.ItemType<InfernalTwilight>()));
            }
        }

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (item.type == ItemID.TinkerersWorkshop)
            {
                InfernalWorld.craftedWorkshop = true;
            }

            base.OnCreated(item, context);
        }

        public override bool OnPickup(Item item, Player player)
        {
            if (item.type == ItemID.TinkerersWorkshop)
            {
                player.GetModPlayer<InfernalPlayer>().workshopHasBeenOwned = true;
                InfernalWorld.craftedWorkshop = true;
            }

            return base.OnPickup(item, player);
        }
    }

    public class devListPlayerCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            // Loop through all players in the world
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                foreach (string name in InfernalTwilight.devList)
                {
                    if (player.active && player.name.ToLower().Contains(name))
                        return true;
                }
                if (player.active && (player.name.ToLower().Contains("nuggets") || player.name.ToLower().Contains("hummus")))
                    return true;
            }
            return false;
        }

        public bool CanShowItemDropInUI() => false;
        public string GetConditionDescription() => "A certain person must be present...";
    }

    public class ProviPlayerCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            // Loop through all players in the world
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && (player.name == "Galactica" || player.name.ToLower().Contains("radiant")))
                    return true;
            }
            return false;
        }

        public bool CanShowItemDropInUI() => false;
        public string GetConditionDescription() => "A certain person must be present...";
    }

    public class SoltanPlayerCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            // Loop through all players in the world
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && player.name == "Bloxxer")
                    return true;
            }
            return false;
        }

        public bool CanShowItemDropInUI() => false;
        public string GetConditionDescription() => "A certain person must be present...";
    }


    public class CheesePlayerCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            // Loop through all players in the world
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && player.name == "lifenuggets")
                    return true;
            }
            return false;
        }

        public bool CanShowItemDropInUI() => false;
        public string GetConditionDescription() => "A certain person must be present...";
    }
}
