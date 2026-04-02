using InfernalEclipseAPI.Core.World;
using InfernumMode;
using InfernumMode.Content.BehaviorOverrides.BossAIs.EmpressOfLight;

namespace InfernalEclipseAPI.Content.DifficultyOverrides.Vanilla.EmpressOverrides
{
    public class EmpressChanges : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        // Synced through Infernum's ExtraAI array.
        private const int ForcedUltimateRainbowIndex = 40;
        private const int HasDoneUltimateRainbowIndex = 41;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.HallowBoss;
        }

        public override bool PreAI(NPC npc)
        {
            if (!InfernalWorld.RagnarokModeEnabled)
                return base.PreAI(npc);

            ref float attackType = ref npc.ai[0];
            ref float forcedUltimateRainbow = ref npc.Infernum().ExtraAI[ForcedUltimateRainbowIndex];
            ref float hasDoneUltimateRainbow = ref npc.Infernum().ExtraAI[HasDoneUltimateRainbowIndex];

            var currentAttack = (EmpressOfLightBehaviorOverride.EmpressOfLightAttackType)(int)attackType;

            if (currentAttack == EmpressOfLightBehaviorOverride.EmpressOfLightAttackType.UltimateRainbow)
                hasDoneUltimateRainbow = 1f;

            return base.PreAI(npc);
        }

        public override bool CheckDead(NPC npc)
        {
            if (!InfernalWorld.RagnarokModeEnabled)
                return base.CheckDead(npc);

            ref float attackType = ref npc.ai[0];
            ref float forcedUltimateRainbow = ref npc.Infernum().ExtraAI[ForcedUltimateRainbowIndex];
            ref float hasDoneUltimateRainbow = ref npc.Infernum().ExtraAI[HasDoneUltimateRainbowIndex];

            var currentAttack = (EmpressOfLightBehaviorOverride.EmpressOfLightAttackType)(int)attackType;

            // Let normal death happen if she's already done the attack, or if she's already in the ending sequence.
            if (hasDoneUltimateRainbow == 1f &&
                (currentAttack == EmpressOfLightBehaviorOverride.EmpressOfLightAttackType.UltimateRainbow ||
                currentAttack == EmpressOfLightBehaviorOverride.EmpressOfLightAttackType.DeathAnimation))
            {
                return base.CheckDead(npc);
            }

            forcedUltimateRainbow = 1f;
            npc.life = 1;
            npc.active = true;

            attackType = (int)EmpressOfLightBehaviorOverride.EmpressOfLightAttackType.UltimateRainbow;
            npc.ai[1] = 0f;
            npc.ai[2] = 0f;
            npc.ai[3] = 0f;

            for (int i = 0; i < npc.localAI.Length; i++)
                npc.localAI[i] = 0f;

            // Keep the synced state flags, clear the rest.
            for (int i = 0; i < npc.Infernum().ExtraAI.Length; i++)
            {
                if (i == ForcedUltimateRainbowIndex || i == HasDoneUltimateRainbowIndex)
                    continue;

                npc.Infernum().ExtraAI[i] = 0f;
            }

            EmpressOfLightBehaviorOverride.ClearAwayEntities();
            npc.netUpdate = true;
            return false;
        }
    }
}
