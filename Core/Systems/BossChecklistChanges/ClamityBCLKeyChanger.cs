using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems.BossChecklistChanges
{
    public class ClamityBCLKeyChanger : ModSystem 
    {
        private ILHook ilHook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("Clamity", out Mod clamity))
                return;

            var wrType = clamity.Code.GetType("Clamity.Commons.SetupWeakReferences");
            if (wrType == null)
                return;

            var method = wrType.GetMethod("SetupBossChecklist", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (method == null)
                return;

            ilHook = new ILHook(method, EditPyrogenKey);
        }

        public override void Unload()
        {
            ilHook?.Dispose();
            ilHook = null;
        }

        private void EditPyrogenKey(ILContext il)
        {
            var c = new ILCursor(il);

            // Find "Pyrogen" string
            while (c.TryGotoNext(MoveType.After, instr => instr.MatchLdstr("Pyrogen")))
            {
                // The next instruction should be ldc.r4 8.5 (the float value)
                if (c.Next != null && c.Next.MatchLdcR4(8.5f))
                {
                    c.Next.Operand = 8.51f; // Change to your desired value
                }
            }
        }
    }
}
