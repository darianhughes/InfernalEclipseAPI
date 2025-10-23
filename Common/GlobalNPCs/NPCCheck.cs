using InfernalEclipseAPI.Core.World;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.Yharon;
using InfernalEclipseAPI.Content.Items.Placeables.Paintings;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using Terraria.GameContent.ItemDropRules;
namespace InfernalEclipseAPI.Common.GlobalNPCs
{
    public class NPCCheck : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.TheDestroyer)
            {
                InfernalWorld.dreadonDestroyerDialoguePlayed = false;
                InfernalWorld.dreadonDestroyer2DialoguePlayed = false;
            }
            if (npc.type == NPCID.Plantera)
            {
                InfernalWorld.jungleSubshockPlanteraDialoguePlayed = false;
                InfernalWorld.jungleSlagspitterPlateraDiaglougePlayer = false;
            }
            if (npc.type == ModContent.NPCType<BrimstoneElemental>() || npc.type == ModContent.NPCType<AquaticScourgeHead>())
            {
                InfernalWorld.sulfurScourgeDialoguePlayed = false;
                InfernalWorld.brimstoneDialoguePlayed = false;
            }
            if (npc.type == ModContent.NPCType<Yharon>())
            {
                InfernalWorld.yharonDischarge = false;
                InfernalWorld.yharonSmasher = false;
            }

            if (npc.type == NPCID.BloodNautilus)
            {
                InfernalDownedBossSystem.downedDreadNautilus = true;

                if (Main.netMode == NetmodeID.Server)
                {
                    // Sync to all clients
                    ModPacket packet = ModContent.GetInstance<InfernalEclipseAPI>().GetPacket();
                    packet.Write((byte)InfernalEclipseMessageType.SyncDownedBosses);
                    packet.Send();
                }
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.boss) //anything that is considered a boss will have a 1/100 chance to drop our dev painting directly
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<InfernalTwilight>(), ThankYouPainting.DropInt));
            }
        }

        public override bool CheckDead(NPC npc)
        {
            if (npc.type == NPCID.TheDestroyer)
            {
                InfernalWorld.dreadonDestroyerDialoguePlayed = false;
                InfernalWorld.dreadonDestroyer2DialoguePlayed = false;
            }
            if (npc.type == NPCID.Plantera)
            {
                InfernalWorld.jungleSubshockPlanteraDialoguePlayed = false;
                InfernalWorld.jungleSlagspitterPlateraDiaglougePlayer = false;
            }
            if (npc.type == ModContent.NPCType<BrimstoneElemental>() || npc.type == ModContent.NPCType<AquaticScourgeHead>())
            {
                InfernalWorld.sulfurScourgeDialoguePlayed = false;
                InfernalWorld.brimstoneDialoguePlayed = false;
            }

            return base.CheckDead(npc);
        }
        public override bool InstancePerEntity => true;
    }
}
