using SOTS.NPCs.Boss;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Content.DifficultyOverrides.SecretsOfTheShadows.SubspaceOverrides
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class SubspaceChanges : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int spawnTimer;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == ModContent.NPCType<SubspaceSerpentBody>() || entity.type == ModContent.NPCType<SubspaceSerpentHead>() || entity.type == ModContent.NPCType<SubspaceSerpentTail>();
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            spawnTimer = 60 * 5;
        }

        public override bool PreAI(NPC npc)
        {
            if (spawnTimer > 0)
            {
                spawnTimer--;

                if (spawnTimer <= 0)
                    npc.dontTakeDamage = false;
                else
                    npc.dontTakeDamage = true;
            }

            return base.PreAI(npc);
        }
    }
}
