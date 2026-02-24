using System.Reflection;
using InfernumMode;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Plantera;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using Terraria.Audio;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILBossChanges
{
    public class PlanteraPhaseTranstitionProtection : ModSystem
    {
        private Hook _hook;

        public override void Load()
        {
            MethodInfo target = typeof(PlanteraBehaviorOverride).GetMethod(
                "DoPhase2Transition",
                BindingFlags.Public | BindingFlags.Static);

            MethodInfo replacement = typeof(PlanteraPhaseTranstitionProtection).GetMethod(
                nameof(DoPhase2Transition_Replacement),
                BindingFlags.NonPublic | BindingFlags.Static);

            if (target != null && replacement != null)
                _hook = new Hook(target, replacement);
        }

        public override void Unload()
        {
            _hook?.Dispose();
            _hook = null;
        }

        private static void DoPhase2Transition_Replacement(NPC npc, Player target, float transitionCountdown)
        {
            // Make Plantera invulnerable during the transition
            npc.dontTakeDamage = true;

            // Keep Infernum’s base visual behavior
            npc.velocity *= 0.95f;
            npc.rotation = npc.AngleTo(target.Center) + MathHelper.PiOver2;

            if (npc.WithinRange(Main.LocalPlayer.Center, 2850f))
            {
                var cam = Main.LocalPlayer.Infernum_Camera();
                cam.ScreenFocusPosition = npc.Center;
                cam.ScreenFocusInterpolant =  Terraria.Utils.GetLerpValue(0f, 15f, transitionCountdown, true);
                cam.ScreenFocusInterpolant *= Terraria.Utils.GetLerpValue(
                    PlanteraBehaviorOverride.Phase2TransitionDuration,
                    PlanteraBehaviorOverride.Phase2TransitionDuration - 8f,
                    transitionCountdown, true);
            }

            // Roar and spawn gore at countdown tick
            if (Main.netMode != NetmodeID.Server &&
                transitionCountdown == PlanteraBehaviorOverride.GoreSpawnCountdownTime)
            {
                Vector2 goreVelocity = (npc.rotation - MathHelper.PiOver2)
                    .ToRotationVector2()
                    .RotatedByRandom(0.54f) * Main.rand.NextFloat(10f, 16f);

                for (int i = 378; i <= 380; i++)
                {
                    Gore.NewGore(npc.GetSource_FromAI(),
                        new Vector2(npc.position.X + Main.rand.Next(npc.width),
                                    npc.position.Y + Main.rand.Next(npc.height)) + goreVelocity * 3f,
                        goreVelocity, i, npc.scale);
                }

                SoundEngine.PlaySound(SoundID.Roar, npc.Center);
            }

            // Disable invulnerability once transition finishes
            if (transitionCountdown <= 1f)
                npc.dontTakeDamage = false;
        }
    }
}
