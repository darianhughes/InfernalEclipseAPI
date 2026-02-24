using CalamityMod.NPCs.SlimeGod;
using InfernumMode.Core.GlobalInstances.Systems;

namespace InfernalEclipseAPI.Common.Globals.GlobalNPCs
{
    public class SlimeGodCorePhaseChangeProtection : GlobalNPC
    {
        private const int PhaseTransitionAnimationState = 2;

        public override bool InstancePerEntity => false;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
            => entity.type == ModContent.NPCType<SlimeGodCore>();

        private static bool IsInPhaseTransition(NPC npc)
        {
            return (int)npc.ai[0] == PhaseTransitionAnimationState && WorldSaveSystem.InfernumModeEnabled;
        }

        public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        {
            if (IsInPhaseTransition(npc))
                return false;

            return null;
        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            if (IsInPhaseTransition(npc))
                return false;

            return null;
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (!IsInPhaseTransition(npc))
                return;

            modifiers.FinalDamage *= 0f;
        }
    }
}
