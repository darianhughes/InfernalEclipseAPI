using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace InfernalEclipseAPI.Core.Systems.Hooks
{
    public class ProjectileHealingHooks : ModSystem
    {
        public override void Load()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out var thorium) || !InfernalConfig.Instance.ThoriumBalanceChangess || InfernalCrossmod.Hummus.Loaded)
                return;

            // --- Patch the BrainCoralPro projectile ---
            var projType = thorium.Code.GetType("ThoriumMod.Projectiles.Healer.BrainCoralPro");
            if (projType != null)
            {
                var aiMethod = projType.GetMethod("AI",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (aiMethod != null)
                    MonoModHooks.Modify(aiMethod, IL_SetBrainCoralHeal);
            }

            var projType2 = thorium.Code.GetType("ThoriumMod.Projectiles.Healer.CelestialWandPro");
            if (projType2 != null)
            {
                var aiMethod = projType2.GetMethod("AI",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (aiMethod != null)
                    MonoModHooks.Modify(aiMethod, IL_SetCelestialWandHeal);
            }

            var projType3 = thorium.Code.GetType("ThoriumMod.Projectiles.Healer.ChiLanternHeal");
            if (projType3 != null)
            {
                var aiMethod = projType3.GetMethod("AI",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (aiMethod != null)
                    MonoModHooks.Modify(aiMethod, IL_SetChiLanternHeal);
            }

            var projType4 = thorium.Code.GetType("ThoriumMod.Projectiles.Healer.EaterOfPainPro");
            if (projType4 != null)
            {
                var aiMethod = projType4.GetMethod("AI",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (aiMethod != null)
                    MonoModHooks.Modify(aiMethod, IL_SetEaterOfPainHeal);
            }

            var gauzeProType = thorium.Code.GetType("ThoriumMod.Projectiles.Healer.GauzePro");
            if (gauzeProType != null)
            {

                var gauzeProjAIMethod = gauzeProType.GetMethod("AI", BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (gauzeProjAIMethod != null)
                    MonoModHooks.Modify(gauzeProjAIMethod, IL_SetGauzeHealToFive);
            }

            var projType5 = thorium.Code.GetType("ThoriumMod.Projectiles.Healer.TheGigaNeedlePro");
            if (projType5 != null)
            {
                var aiMethod = projType5.GetMethod("AI",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (aiMethod != null)
                    MonoModHooks.Modify(aiMethod, IL_SetGigaNeedleHeal);
            }

            /*
            var projType6 = thorium.Code.GetType("CalamityBardHealer.Projectiles.Healer.PurgedSoulPro");
            if (projType6 != null)
            {
                var aiMethod = projType6.GetMethod("AI",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (aiMethod != null)
                    MonoModHooks.Modify(aiMethod, IL_EditHealAmount);
            }
            */

            // Get the RecoveryWandPro type
            var projType7 = thorium.Code.GetType("ThoriumMod.Projectiles.Healer.RecoveryWandPro");
            if (projType7 != null)
            {
                var aiMethod = projType7.GetMethod("AI",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (aiMethod == null)
                    MonoModHooks.Modify(aiMethod, IL_PatchRecoveryWandHeal);
            }

            /*
            if (ModLoader.TryGetMod("ThoriumRework", out var rework))
            {
                var projType8 = rework.Code.GetType("ThoriumRework.Projectiles.BiteyBaby");
                if (projType8 != null)
                {
                    var aiMethod = projType.GetMethod("AI",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (aiMethod != null)
                        MonoModHooks.Modify(aiMethod, IL_FixHealAmount);
                }
            }
            */

            // Get the TheGoodBookPro type
            var projType9 = thorium.Code.GetType("ThoriumMod.Projectiles.Healer.TheGoodBookPro");
            if (projType9 != null)
            {
                var aiMethod = projType.GetMethod("AI",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (aiMethod != null)
                    MonoModHooks.Modify(aiMethod, IL_SetGoodBookHeal);
            }
        }

        #region Hooks
        private void IL_SetGoodBookHeal(ILContext il)
        {
            var c = new ILCursor(il);

            // Find the integer constant '2' used in ThoriumHeal call
            if (c.TryGotoNext(i => i.OpCode == Mono.Cecil.Cil.OpCodes.Ldc_I4_2))
            {
                // Replace base heal 2 - 4
                c.Next.OpCode = Mono.Cecil.Cil.OpCodes.Ldc_I4_4;
            }
        }

        /*
        private void IL_FixHealAmount(ILContext il)
        {
            var c = new ILCursor(il);

            while (c.TryGotoNext(
                i => i.OpCode == OpCodes.Ldfld &&
                     i.Operand is FieldReference fr &&
                     fr.Name == "ai"))
            {
                // BACK UP 1 instruction and check we came from Projectile
                if (c.Prev != null &&
                    c.Prev.OpCode == OpCodes.Ldfld &&
                    c.Prev.Operand.ToString().Contains("Terraria.Projectile"))
                {
                    // We found the correct heal ai[0] load.
                    c.Next.OpCode = OpCodes.Ldc_I4_S;
                    c.Next.Operand = (sbyte)3;
                    return;
                }
            }
        }
        */

        private void IL_PatchRecoveryWandHeal(ILContext il)
        {
            var c = new ILCursor(il);

            // --- Patch base heal: 2 - 5 ---
            if (c.TryGotoNext(i => i.OpCode == Mono.Cecil.Cil.OpCodes.Ldc_I4_2))
            {
                c.Next.OpCode = Mono.Cecil.Cil.OpCodes.Ldc_I4_5;
            }

            // --- Patch out-of-combat bonus: 4 - 10 ---
            c.Index = 0; // Reset cursor to start
            if (c.TryGotoNext(i => i.OpCode == Mono.Cecil.Cil.OpCodes.Ldc_I4_4))
            {
                // If the original delegate adds 4, replace with 10
                c.Next.OpCode = Mono.Cecil.Cil.OpCodes.Ldc_I4_S;
                c.Next.Operand = (sbyte)10;
            }
        }

        private void IL_SetBrainCoralHeal(ILContext il)
        {
            var c = new ILCursor(il);

            // Look for the first Ldc_I4_3 and replace it with 5
            if (c.TryGotoNext(i => i.OpCode == OpCodes.Ldc_I4_3))
            {
                c.Next.OpCode = OpCodes.Ldc_I4_5;
            }
        }

        private void IL_SetCelestialWandHeal(ILContext il)
        {
            var c = new ILCursor(il);

            // Look for the integer 5 pushed before ThoriumHeal
            if (c.TryGotoNext(i => i.MatchLdcI4(5)))
            {
                c.Next.OpCode = OpCodes.Ldc_I4_1;
            }
            else
            {
                ModContent.GetInstance<Mod>().Logger.Warn(
                    "CelestialWandHealingPatch: failed to find heal constant 5.");
            }
        }

        private void IL_SetChiLanternHeal(ILContext il)
        {
            var c = new ILCursor(il);

            // Look for the call to ThoriumHeal
            if (c.TryGotoNext(i =>
                   i.OpCode == OpCodes.Call &&
                   i.Operand is MethodReference m &&
                   m.Name.Contains("ThoriumHeal")))
            {
                // Step back to the first integer pushed (the heal amount)
                int startIndex = c.Index;
                for (int i = startIndex - 1; i >= 0; i--)
                {
                    if (il.Instrs[i].OpCode == OpCodes.Ldc_I4_3) // originally 3
                    {
                        il.Instrs[i].OpCode = OpCodes.Ldc_I4_5; // change to 5
                        break;
                    }
                }
            }
        }

        private void IL_SetEaterOfPainHeal(ILContext il)
        {
            var c = new ILCursor(il);

            // Look for the first constant 2 and replace with 3
            if (c.TryGotoNext(i => i.OpCode == OpCodes.Ldc_I4_2))
            {
                c.Next.OpCode = Mono.Cecil.Cil.OpCodes.Ldc_I4_3;
            }
        }

        private void IL_SetGauzeHealToFive(ILContext il)
        {
            var c = new ILCursor(il);

            // Look for the first integer constant '2' (Ldc_I4_2)
            if (c.TryGotoNext(i => i.OpCode == Mono.Cecil.Cil.OpCodes.Ldc_I4_2))
            {
                // Replace it with 5
                c.Next.OpCode = Mono.Cecil.Cil.OpCodes.Ldc_I4_5;
            }
        }

        private void IL_SetGigaNeedleHeal(ILContext il)
        {
            var c = new ILCursor(il);

            // Look for the 3f healing constant
            if (c.TryGotoNext(i => i.MatchLdcR4(3f)))
            {
                c.Next.Operand = 6f; // Change heal amount 3 - 6
            }
            else
            {
                ModContent.GetInstance<Mod>().Logger.Warn(
                    "GigaNeedleHealingPatch: Could not find 3f heal amount.");
            }
        }

        /*
        private void IL_EditHealAmount(ILContext il)
        {
            var c = new ILCursor(il);

            if (c.TryGotoNext(i => i.MatchLdcI4(12)))
            {
                c.Next.OpCode = OpCodes.Ldc_I4_3; // New heal amount = 3
                c.Next.Operand = null;             // not needed for small ints

                // Success, done.
            }
            else
            {
                ModContent.GetInstance<Mod>().Logger.Warn(
                    "PurgedSoulHealPlayerPatch: Could not find heal constant 12.");
            }
        }
        */
        #endregion
    }
}
