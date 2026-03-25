using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public sealed class RemovePinkyPetBossSlowdown : ModSystem
    {
        private ILHook postAIHook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("SOTS", out Mod sots))
                return;

            Type debuffNpcType = sots.Code.GetType("SOTS.Common.GlobalNPCs.DebuffNPC");
            MethodInfo postAIMethod = debuffNpcType?.GetMethod(
                "PostAI",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (postAIMethod is null)
                return;

            postAIHook = new ILHook(postAIMethod, PatchDebuffNPCPostAI);
        }

        public override void Unload()
        {
            postAIHook?.Dispose();
            postAIHook = null;
        }

        private static void PatchDebuffNPCPostAI(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            bool patchedBossPull = PatchPinkyBossPull(c);
            bool patchedBossSlow = PatchPinkyBossSlow(c);

            if (!patchedBossPull || !patchedBossSlow)
            {
                throw new Exception(
                    $"Failed to patch SOTS DebuffNPC.PostAI correctly. " +
                    $"Boss pull patched: {patchedBossPull}, boss slow patched: {patchedBossSlow}");
            }
        }

        private static bool PatchPinkyBossPull(ILCursor c)
        {
            // Find a short window containing:
            // 0.25f, 0.0015f, npc.boss, and 0.01f
            // Then replace that 0.01f with 0f.

            for (int start = 0; start < c.Instrs.Count; start++)
            {
                int end = Math.Min(c.Instrs.Count, start + 35);

                bool found025 = false;
                bool found00015 = false;
                bool foundBossField = false;
                int const001Index = -1;

                for (int i = start; i < end; i++)
                {
                    Instruction ins = c.Instrs[i];

                    if (ins.MatchLdcR4(0.25f))
                        found025 = true;
                    else if (ins.MatchLdcR4(0.0015f))
                        found00015 = true;
                    else if (ins.MatchLdfld<NPC>(nameof(NPC.boss)))
                        foundBossField = true;
                    else if (ins.MatchLdcR4(0.01f))
                        const001Index = i;
                }

                if (found025 && found00015 && foundBossField && const001Index >= 0)
                {
                    c.Instrs[const001Index].Operand = 0f;
                    return true;
                }
            }

            return false;
        }

        private static bool PatchPinkyBossSlow(ILCursor c)
        {
            // Prefer a window after:
            // this.pinkied = false;
            //
            // Then look for:
            // 0.625f, 0.95f, npc.boss
            // and replace 0.95f -> 1f

            int anchor = FindPinkiedResetIndex(c);
            if (anchor >= 0)
            {
                if (TryPatchBossSlowInRange(c, anchor, Math.Min(c.Instrs.Count, anchor + 140)))
                    return true;
            }

            // Fallback: scan the whole method for a window with
            // 0.625f + 0.95f + npc.boss
            for (int start = 0; start < c.Instrs.Count; start++)
            {
                int end = Math.Min(c.Instrs.Count, start + 40);
                if (TryPatchBossSlowInRange(c, start, end))
                    return true;
            }

            return false;
        }

        private static int FindPinkiedResetIndex(ILCursor c)
        {
            for (int i = 0; i < c.Instrs.Count - 2; i++)
            {
                if (c.Instrs[i].MatchLdarg(0) &&
                    c.Instrs[i + 1].MatchLdcI4(0) &&
                    c.Instrs[i + 2].MatchStfld("SOTS.Common.GlobalNPCs.DebuffNPC", "pinkied"))
                {
                    return i;
                }
            }

            return -1;
        }

        private static bool TryPatchBossSlowInRange(ILCursor c, int start, int end)
        {
            bool found0625 = false;
            bool foundBossField = false;
            int const095Index = -1;

            for (int i = start; i < end; i++)
            {
                Instruction ins = c.Instrs[i];

                if (ins.MatchLdcR4(0.625f))
                    found0625 = true;
                else if (ins.MatchLdfld<NPC>(nameof(NPC.boss)))
                    foundBossField = true;
                else if (ins.MatchLdcR4(0.95f))
                    const095Index = i;
            }

            if (found0625 && foundBossField && const095Index >= 0)
            {
                c.Instrs[const095Index].Operand = 1f;
                return true;
            }

            return false;
        }
    }
}
