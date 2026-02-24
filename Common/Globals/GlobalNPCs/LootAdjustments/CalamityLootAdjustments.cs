using Terraria.GameContent.ItemDropRules;
using RagnarokMod.Items.HealerItems.Other;
using CalamityMod.NPCs.AcidRain;

namespace InfernalEclipseAPI.Common.GlobalNPCs.LootAdjustments
{
    [ExtendsFromMod("RagnarokMod")]
    public class CalamityLootAdjustments : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            foreach (IItemDropRule rule in npcLoot.Get())
            {
                if (rule is CommonDrop drop && drop.itemId == ModContent.ItemType<Virusprayer>() && ModLoader.TryGetMod("InfernalEclipseWeaponsDLC", out _))
                {
                    npcLoot.Remove(drop);
                }
            }

            if (npc.type == ModContent.NPCType<Mauler>())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Virusprayer>(), 2, 1, 1));
            }
        }
    }

    [ExtendsFromMod("RagnarokMod")]
    public class CalamityLootBagAdjustments : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            foreach (IItemDropRule rule in itemLoot.Get())
            {
                if (rule is CommonDrop drop && drop.itemId == ModContent.ItemType<Virusprayer>() && ModLoader.TryGetMod("InfernalEclipseWeaponsDLC", out _))
                {
                    itemLoot.Remove(drop);
                }
            }
        }
    }
}
