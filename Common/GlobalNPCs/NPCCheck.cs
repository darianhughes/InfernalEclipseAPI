using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;
using InfernalEclipseAPI.Core.World;
using CalamityMod.Projectiles.Boss;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.AquaticScourge;
using InfernalEclipseAPI.Core.Systems;
using CalamityMod.NPCs.Yharon;
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
