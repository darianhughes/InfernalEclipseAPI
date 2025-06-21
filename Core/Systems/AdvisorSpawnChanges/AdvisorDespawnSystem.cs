using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems.AdvisorSpawnChanges
{
    [ExtendsFromMod("SOTS")]
    public class AdvisorDespawnSystem : ModSystem
    {
        private int despawnTimer = 0;
        private const int despawnDelay = 300; // 5 seconds

        public override void PostUpdateWorld()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) // Only on server or singleplayer
                return;

            bool anyPlayerInBiome = false;
            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player = Main.player[p];
                if (player.active && player.InModBiome(ModContent.GetInstance<SOTS.Biomes.PlanetariumBiome>()))
                {
                    anyPlayerInBiome = true;
                    break;
                }
            }

            if (!anyPlayerInBiome)
            {
                bool advisorActive = NPC.AnyNPCs(ModContent.NPCType<SOTS.NPCs.Boss.Advisor.TheAdvisorHead>());
                bool constructsActive = NPC.AnyNPCs(ModContent.NPCType<SOTS.NPCs.Constructs.OtherworldlyConstructHead2>());

                if (advisorActive && constructsActive)
                {
                    despawnTimer++;
                    if (despawnTimer > despawnDelay)
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc = Main.npc[i];
                            if (npc.active && (
                                npc.type == ModContent.NPCType<SOTS.NPCs.Boss.Advisor.TheAdvisorHead>() ||
                                npc.type == ModContent.NPCType<SOTS.NPCs.Constructs.OtherworldlyConstructHead2>()))
                            {
                                npc.active = false;
                                npc.netUpdate = true;
                            }
                        }
                        despawnTimer = 0;
                    }
                }
                else
                {
                    despawnTimer = 0;
                }
            }
            else
            {
                despawnTimer = 0;
            }
        }
    }
}
