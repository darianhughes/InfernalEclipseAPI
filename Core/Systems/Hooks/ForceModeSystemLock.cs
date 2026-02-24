using CalamityMod.UI.ModeIndicator;
using System.Reflection;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using CalamityMod.Events;
using Mono.Cecil.Cil;
using InfernalEclipseAPI.Core.World;

namespace InfernalEclipseAPI.Core.Systems.Hooks
{
    public class ForceDifficultyMenuLockSystem : ModSystem
    {
        private static ILHook _hook;

        /// <summary>
        /// Assign this from anywhere in your mod if you want to control the lock condition dynamically.
        /// If null, no forced lock is applied.
        /// </summary>
        public static Func<bool> ForcedLockCondition;

        public override void Load()
        {
            // CalamityMod.UI.ModeIndicator.ModeIndicatorUI.GetLockStatus(out LocalizedText text, out bool locked)
            MethodInfo m = typeof(ModeIndicatorUI).GetMethod(
                "GetLockStatus",
                BindingFlags.Public | BindingFlags.Static
            );

            if (m is null)
                throw new MissingMethodException("ModeIndicatorUI.GetLockStatus not found.");

            _hook = new ILHook(m, Patch_GetLockStatus);
        }

        public override void Unload()
        {
            _hook?.Dispose();
            _hook = null;
            ForcedLockCondition = null;
        }

        private static bool ShouldForceDifficultyLock()
        {
            return ModContent.GetInstance<InfernalConfig>().InfernumModeForced;

            return ForcedLockCondition?.Invoke() ?? false;
        }

        private static void Patch_GetLockStatus(ILContext il)
        {
            var c = new ILCursor(il);

            /*
             * We inject into this Calamity code path:
             * if (CalamityPlayer.areThereAnyDamnBosses || BossRushEvent.BossRushActive)
             *     locked = true;
             *
             * IL shape (typical):
             *   ldsfld bool CalamityPlayer::areThereAnyDamnBosses
             *   brtrue.s  LOCK_LABEL
             *   ldsfld bool BossRushEvent::BossRushActive
             *   brfalse.s CONTINUE
             * LOCK_LABEL:
             *   ...
             *
             * We patch after loading BossRushActive:
             *   ldsfld BossRushActive
             *   call ShouldForceDifficultyLock
             *   or
             *   brfalse.s CONTINUE
             */
            bool patched = c.TryGotoNext(
                MoveType.After,
                i => i.MatchLdsfld(typeof(BossRushEvent).GetField("BossRushActive", BindingFlags.Public | BindingFlags.Static))
            );

            if (!patched)
            {
                // Fallback: search by name if field binding differs (rare).
                c.Index = 0;
                patched = c.TryGotoNext(
                    MoveType.After,
                    i => i.OpCode == OpCodes.Ldsfld && i.Operand is FieldInfo fi && fi.Name.Contains("BossRushActive", StringComparison.Ordinal)
                );
            }

            if (!patched)
            {
                ModContent.GetInstance<ForceDifficultyLockHookLogger>()
                    .Warn("Failed to patch ModeIndicatorUI.GetLockStatus: BossRushActive load not found.");
                return;
            }

            // Stack currently: [ BossRushActive(bool) ]
            c.EmitDelegate(ShouldForceDifficultyLock); // push bool
            c.Emit(OpCodes.Or);                                     // OR them -> [combined bool]
        }

        /// <summary>
        /// Small helper ModType for logging without requiring your Mod instance in this file.
        /// </summary>
        private sealed class ForceDifficultyLockHookLogger : ModSystem
        {
            public void Warn(string msg) => Mod.Logger.Warn(msg);
        }
    }
}