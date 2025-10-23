using Terraria.GameContent.ItemDropRules;

namespace InfernalEclipseAPI.Content.Items.Materials
{
    public class AbandonedWorkshop : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.rare = ItemRarityID.Blue;
            Item.value = 0;
        }
    }

    public class AbandonedWorkshopDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            int[] cthulhuBosses =
            {
                NPCID.EyeofCthulhu,
                NPCID.EaterofWorldsHead,
                NPCID.BrainofCthulhu,
                NPCID.SkeletronHead,
                NPCID.WallofFlesh
            };

            foreach (int bossID in cthulhuBosses)
            {
                if (npc.type == bossID)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AbandonedWorkshop>()));
                }
            }
        }
    }
}
