using CalamityMod;
using YouBoss.Content.NPCs.Bosses.TerraBlade;
using YouBoss.Core;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;

namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs
{
    [ExtendsFromMod("YouBoss")]
    public class TerraBladeDebuff : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == ModContent.NPCType<TerraBladeBoss>();
        }

        public override void PostAI(NPC npc)
        {
            if (!npc.active || !InfernumSaveSystem.InfernumModeEnabled)
                return;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && npc.Distance(player.Center) < 8000f)
                {
                    player.ClearBuff(ModContent.BuffType<GracedWings>());
                }
            }
        }

        public override void OnKill(NPC npc)
        {
            WorldSaveSystem.HasDefeatedYourself = true;
            CalamityNetcode.SyncWorld();
        }
    }
}
