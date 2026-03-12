using CalamityMod.Events;
using Microsoft.Xna.Framework;
using SOTS.NPCs.Boss.Glowmoth;

namespace InfernalEclipseAPI.Content.DifficultyOverrides.SecretsOfTheShadows.GlowmothOverrides
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    internal class GlowmothChanges : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private const int DespawnDelay = 600;

        private const float PlayerCheckRange = 3200f;
        private const float PlayerCheckRangeSQ = PlayerCheckRange * PlayerCheckRange;

        private int ticksWithoutValidPlayer;
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == ModContent.NPCType<Glowmoth>() && !BossRushEvent.BossRushActive;
        public override bool PreAI(NPC npc)
        {
            if (Main.gameMenu || !npc.active)
                return base.PreAI(npc);

            bool validPlayerFound = false;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];

                if (player is null || !player.active || player.dead)
                    continue;

                if (Vector2.DistanceSquared(player.Center, npc.Center) > PlayerCheckRangeSQ)
                    continue;

                if (player.ZoneGlowshroom)
                {
                    validPlayerFound = true;
                    break;
                }
            }

            if (validPlayerFound)
            {
                ticksWithoutValidPlayer = 0;
                return base.PreAI(npc);
            }

            ticksWithoutValidPlayer++;

            if (ticksWithoutValidPlayer >= DespawnDelay)
                ForceDespawn(npc);

            return base.PreAI(npc);
        }

        private static void ForceDespawn(NPC npc)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            npc.active = false;
            npc.netUpdate = true;

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
        }
    }
}