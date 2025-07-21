using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfernumMode.Content.BehaviorOverrides.BossAIs.MoonLord;
using MonoMod.RuntimeDetour;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.MoonLord
{
    public class MoonLordCoreBehaviorOverrideFixes : ModSystem
    {
        private ILHook selectNextAttackHook;

        public override void Load()
        {
            MethodInfo method = typeof(MoonLordCoreBehaviorOverride).GetMethod("SelectNextAttack", BindingFlags.Public | BindingFlags.Static);
            selectNextAttackHook = new ILHook(method, InjectNetmodeCheck);
        }

        public override void Unload()
        {
            selectNextAttackHook?.Dispose();
        }

        private void InjectNetmodeCheck(ILContext il)
        {
            ILCursor c = new(il);

            c.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.netMode)));
            c.Emit(OpCodes.Ldc_I4_1); // NetmodeID.MultiplayerClient = 1
            c.Emit(OpCodes.Bne_Un_S, c.Next); // Skip return if not MP client
            c.Emit(OpCodes.Ret); // Return from method if MP client
        }
    }
}
