using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.ILItemChanges
{
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    public class ElementalAmuletNoAlchemy : ModSystem
    {
        ILHook resetEffectsILHook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("SOTS", out var sots) || sots?.Code == null) return;

            var sotsPlayerType = sots.Code.GetType("SOTS.SOTSPlayer");
            if (sotsPlayerType == null) return;

            var mi = sotsPlayerType.GetMethod("ResetEffects", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (mi == null) return;

            resetEffectsILHook = new ILHook(mi, Patch_ResetEffects);
        }

        public override void Unload()
        {
            resetEffectsILHook?.Dispose();
            resetEffectsILHook = null;
        }

        private void Patch_ResetEffects(ILContext il)
        {
            var c = new ILCursor(il);

            // Match exact sequence:  ... ldfld Player::adjTile ; ldc.i4 355 ; ldc.i4 1 ; stelem.i1
            while (c.TryGotoNext(
                i => i.MatchLdfld(typeof(Player).GetField("adjTile")),
                i => i.MatchLdcI4(TileID.AlchemyTable), // 355
                i => i.MatchLdcI4(1),                  // true
                i => i.MatchStelemI1()))
            {
                // Cursor is at the first matched instr (ldfld). Move to the true-const and flip it.
                c.Index += 2;                         // now on ldc.i4.1
                c.Next.OpCode = OpCodes.Ldc_I4_0;     // set false
                c.Index += 2;                         // advance past stelem.i1 to continue scanning
            }

            c.Index = 0;
            var alchemyField = typeof(Player).GetField("alchemyTable", BindingFlags.Instance | BindingFlags.Public);
            if (alchemyField != null)
            {
                while (c.TryGotoNext(i => i.MatchStfld(alchemyField)))
                {
                    // Ensure the value being stored is 'true' and flip it.
                    var prev = c.Prev;
                    if (prev != null && prev.OpCode == OpCodes.Ldc_I4_1)
                        prev.OpCode = OpCodes.Ldc_I4_0;

                    c.Index++; // continue searching
                }
            }
        }
    }
}
