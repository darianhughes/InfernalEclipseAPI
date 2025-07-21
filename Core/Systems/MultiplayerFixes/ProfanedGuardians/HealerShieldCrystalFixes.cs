using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfernumMode.Content.BehaviorOverrides.BossAIs.ProfanedGuardians;
using MonoMod.RuntimeDetour;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour.HookGen;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.ProfanedGuardians
{
    public class HealerShieldCrystalFixes : ModSystem
    {
        private ILHook sitStillILHook;

        public override void Load()
        {
            MethodInfo method = typeof(HealerShieldCrystal).GetMethod(
                "DoBehavior_SitStill",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
            sitStillILHook = new ILHook(method, InjectNetUpdateCheck);
        }

        public override void Unload()
        {
            sitStillILHook?.Dispose();
            sitStillILHook = null;
        }

        private void InjectNetUpdateCheck(ILContext il)
        {
            var c = new ILCursor(il);

            // At the very beginning of the method
            c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0); // Load 'this' (HealerShieldCrystal)
            c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_1); // Load argument (Player)
            c.EmitDelegate<Action<HealerShieldCrystal, Player>>((self, target) =>
            {
                if (Main.netMode == NetmodeID.Server && self.ShatteringTimer % 150 == 0)
                    self.NPC.netUpdate = true;
            });
        }
    }
}
