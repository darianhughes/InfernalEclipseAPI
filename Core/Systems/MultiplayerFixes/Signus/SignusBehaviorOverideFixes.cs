using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Signus;
using MonoMod.Cil;
using Terraria.ModLoader;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour;
using Terraria;
using InfernumMode;


namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.Signus
{
    public class SignusBehaviorOverideFixes : ModSystem
    {
        private ILHook kunaiHook;

        public override void Load()
        {
            MethodInfo method = typeof(SignusBehaviorOverride).GetMethod("DoAttack_KunaiDashes", BindingFlags.Public | BindingFlags.Static);
            kunaiHook = new ILHook(method, InjectKunaiFix);
        }

        public override void Unload()
        {
            kunaiHook?.Dispose();
            kunaiHook = null;
        }

        private void InjectKunaiFix(ILContext il)
        {
            var c = new ILCursor(il);

            c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0); // NPC npc
            c.EmitDelegate<Action<NPC>>(npc =>
            {
                if (npc.Infernum().ExtraAI[0] > 2f || npc.Infernum().ExtraAI[0] < 0f)
                    npc.Infernum().ExtraAI[0] = 0f;
            });
        }
    }
}
