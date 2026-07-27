using InfernumMode.Content.BehaviorOverrides.BossAIs.GreatSandShark;

namespace InfernalEclipseAPI.Content.DifficultyOverrides.Calamity.Infernum.BereftVassalOverrides
{
    public class VassalChanges : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == ModContent.NPCType<BereftVassal>();
        }

        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            //npc.lifeMax = (int)(npc.lifeMax * 0.8f * balance * bossAdjustment);
        }
    }
}
