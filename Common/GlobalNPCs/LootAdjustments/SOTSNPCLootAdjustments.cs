using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.TreasureBags;
using CalamityMod.NPCs.Cryogen;
using InfernalEclipseAPI.Content.Items.Lore.SOTS;
using SOTS.Items.Fragments;
using SOTS.NPCs.Boss;
using SOTS.NPCs.Boss.Advisor;
using SOTS.NPCs.Boss.Curse;
using SOTS.NPCs.Boss.Glowmoth;
using SOTS.NPCs.Boss.Lux;
using SOTS.NPCs.Boss.Polaris;
using SOTS.NPCs.Boss.Polaris.NewPolaris;
using SOTS.NPCs.TreasureSlimes;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalNPCs.LootAdjustments
{
    [ExtendsFromMod("SOTS")]
    public class SOTSNPCLootAdjustments : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            //Lore Items
            if (npc.type == ModContent.NPCType<TheAdvisorHead>())
            {
                bool firstAdvisorKill() => !SOTS.SOTSWorld.downedAdvisor;
                npcLoot.AddConditionalPerPlayer(firstAdvisorKill, ModContent.ItemType<LoreAdvisor>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<Glowmoth>())
            {
                bool firstGlowmothKill() => !SOTS.SOTSWorld.downedGlowmoth;
                npcLoot.AddConditionalPerPlayer(firstGlowmothKill, ModContent.ItemType<LoreGlowmoth>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<Lux>())
            {
                bool firstLuxKill() => !SOTS.SOTSWorld.downedLux;
                npcLoot.AddConditionalPerPlayer(firstLuxKill, ModContent.ItemType<LoreLux>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<PharaohsCurse>())
            {
                bool firstCurseKill() => !SOTS.SOTSWorld.downedCurse;
                npcLoot.AddConditionalPerPlayer(firstCurseKill, ModContent.ItemType<LorePharaoh>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<Polaris>() || npc.type == ModContent.NPCType<NewPolaris>())
            {
                bool firstPolarisKill() => !SOTS.SOTSWorld.downedAmalgamation;
                npcLoot.AddConditionalPerPlayer(firstPolarisKill, ModContent.ItemType<LorePolaris>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<PutridPinkyPhase2>())
            {
                bool firstPutridKill() => !SOTS.SOTSWorld.downedPinky;
                npcLoot.AddConditionalPerPlayer(firstPutridKill, ModContent.ItemType<LorePutrid>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<SubspaceSerpentHead>())
            {
                bool firstSupspaceKill() => !SOTS.SOTSWorld.downedSubspace;
                npcLoot.AddConditionalPerPlayer(firstSupspaceKill, ModContent.ItemType<LoreSerpent>(), desc: DropHelper.FirstKillText);
            }

            //Other
            if (npc.type == ModContent.NPCType<CrimsonTreasureSlime>() || npc.type == ModContent.NPCType<CorruptionTreasureSlime>())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BlightedGel>(), 1, 7, 13));
            }

            if (npc.type == ModContent.NPCType<Cryogen>())
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<FragmentOfPermafrost>(), 1, 12, 18));
            }
        }
    }

    [ExtendsFromMod("SOTS")]
    public class SOTSBossBagChanges : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ModContent.ItemType<CryogenBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfPermafrost>(), 1, 15, 21));
            }
        }
    }
}
