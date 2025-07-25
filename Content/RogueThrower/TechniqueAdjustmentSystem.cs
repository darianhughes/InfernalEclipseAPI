using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.RogueThrower
{
    public class TechniqueAdjustmentSystem : ModSystem
    {
        private ILHook hook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            var helperType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueTypeHelper");
            var enumType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueType");
            var getCostMethod = helperType.GetMethod("GetCost", new Type[] { enumType });

            if (getCostMethod == null)
                return;

            hook = new ILHook(getCostMethod, PatchShadowDanceCost);
        }

        public override void Unload()
        {
            hook?.Dispose();
            hook = null;
        }

        private void PatchShadowDanceCost(ILContext il)
        {
            var c = new ILCursor(il);
            int costTwoCount = 0;

            while (c.Next != null)
            {
                if (c.Next is Instruction instruction)
                {
                    if (instruction.OpCode == OpCodes.Ldc_I4_2)
                    {
                        if (costTwoCount == 5) // 0-based index; 6th occurrence is ShadowDance
                        {
                            instruction.OpCode = OpCodes.Ldc_I4_5; // Replace with 'ldc.i4.4'
                            instruction.Operand = null; // Always null for short-form
                            return;
                        }
                        costTwoCount++;
                    }
                }
                c.Next = c.Next.Next;
            }
        }
    }
}
