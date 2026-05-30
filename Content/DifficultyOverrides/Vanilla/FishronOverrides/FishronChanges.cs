using InfernalEclipseAPI.Core.World;
using InfernumMode;
using InfernumMode.Content.BehaviorOverrides.BossAIs.DukeFishron;
using MonoMod.RuntimeDetour;
using static InfernumMode.Content.BehaviorOverrides.BossAIs.DukeFishron.DukeFishronBehaviorOverride;
using Terraria.Audio;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Content.DifficultyOverrides.Vanilla.FishronOverrides
{
    public class FishronChanges : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.DukeFishron;
        }

        private bool hasHealedForPhase4;

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            hasHealedForPhase4 = false;
        }

        public override bool PreAI(NPC npc)
        {
            ref float attackState = ref npc.Infernum().ExtraAI[5];
            ref float aiStateIndex = ref npc.ai[1];
            ref float attackTimer = ref npc.Infernum().ExtraAI[6];
            ref float frameDrawType = ref npc.ai[3];
            ref float phaseTransitionTime = ref npc.Infernum().ExtraAI[8];
            ref float hasEyes01Flag = ref npc.Infernum().ExtraAI[9];
            ref float attackDelay = ref npc.Infernum().ExtraAI[10];
            ref float hasEnteredPhase4 = ref npc.Infernum().ExtraAI[12];

            if (hasEnteredPhase4 != 1f || hasHealedForPhase4 || !InfernalWorld.RagnarokModeEnabled)
                return true;

            Vector2 mouthPosition = (npc.rotation + (npc.spriteDirection == 1).ToInt() * Pi).ToRotationVector2() * (npc.Size + Vector2.UnitY * 55f) * 0.6f + npc.Center;
            mouthPosition.Y += 24f;

            if (attackDelay < 60f)
            {
                npc.damage = 0;
                if (attackDelay == 1f)
                    npc.velocity = Vector2.UnitY * -4.4f;
                else
                    npc.velocity.Y *= 0.95f;

                // Roar in the middle of animation.
                if (attackDelay == 30f)
                    SoundEngine.PlaySound(SoundID.Zombie20, npc.Center);

                if (attackDelay >= 30f)
                    frameDrawType = (int)DukeFrameDrawingType.OpenMouth;

                attackDelay++;
                attackState = (int)DukeAttackType.ChargeWait;
                return false;
            }

            frameDrawType = (int)DukeFrameDrawingType.FinFlapping;

            npc.damage = 0;
            npc.dontTakeDamage = true;
            npc.rotation = npc.rotation.AngleLerp(0f, 0.2f);
            npc.velocity *= 0.96f;
            npc.velocity.Y = Lerp(npc.velocity.Y, 0f, 0.04f);

            if (phaseTransitionTime == 0f)
            {
                for (int i = 0; i < npc.buffImmune.Length; i++)
                    npc.buffImmune[i] = true;
                while (npc.buffTime[0] != 0)
                    npc.DelBuff(0);
            }

            if (phaseTransitionTime <= 120f && npc.life < npc.lifeMax * 0.4f)
            {
                int max = (int)(npc.lifeMax * 0.4f);
                int heal = max / 120;
                npc.life += heal;

                CombatText.NewText(npc.Hitbox, CombatText.HealLife, heal);
                npc.netUpdate = true;
            }

            if (phaseTransitionTime == 75f)
            {
                hasEyes01Flag = 1f;
                SoundEngine.PlaySound(SoundID.Zombie20, npc.Center);
            }

            phaseTransitionTime++;

            if (phaseTransitionTime >= 75f)
                frameDrawType = (int)DukeFrameDrawingType.OpenMouth;

            if (phaseTransitionTime >= 120f)
            {
                hasHealedForPhase4 = true;
                aiStateIndex = -1f;
                phaseTransitionTime = 0f;

                //Ensure we didn't heal over 40%
                if (npc.life > npc.lifeMax * 0.4f)
                    npc.life = (int)(npc.lifeMax * 0.4f);
            }
            return false;
        }
    }

    internal class DukeFishronPhase4RegenSystem : ModSystem
    {
        private delegate void SelectNextAttackDelegate(NPC npc);

        private Hook selectNextAttackHook;

        public override void Load()
        {
            selectNextAttackHook = new Hook(typeof(DukeFishronBehaviorOverride).GetMethod(nameof(SelectNextAttack)), SelectNextAttackDetour);
        }

        public override void Unload()
        {
            selectNextAttackHook?.Dispose();
            selectNextAttackHook = null;
        }

        private static void SelectNextAttackDetour(SelectNextAttackDelegate orig, NPC npc)
        {
            if (npc.type != NPCID.DukeFishron)
            {
                orig(npc);
                return;
            }

            // ExtraAI[12] is Infernum's hasEnteredPhase4 flag.
            bool forcePhase4 = npc.Infernum().ExtraAI[12] == 1f;

            if (!forcePhase4)
            {
                orig(npc);
                return;
            }

            npc.ai[1]++;

            DukeAttackType[] pattern = Subphase4Pattern;

            DukeAttackType nextAttack =  pattern[(int)(npc.ai[1] % pattern.Length)];

            npc.Infernum().ExtraAI[5] = (int)nextAttack;
            npc.Infernum().ExtraAI[6] = 0f;

            for (int i = 0; i < 5; i++)
                npc.Infernum().ExtraAI[i] = 0f;
        }
    }
}
