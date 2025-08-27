using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems.ILItemChanges
{
    //WH
    public class MoltenThresherPostDrawIL : ModSystem
    {
        private ILHook thresherPostDrawIL;

        public override void Load()
        {
            // Only run if Thorium is loaded
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium) || ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                return;

            // Get Thorium's MoltenThresherPro type
            var projType = thorium.Code?.GetType("ThoriumMod.Projectiles.Scythe.MoltenThresherPro");
            if (projType == null)
                return;

            // Get the PostDraw method (instance, any visibility)
            var method = projType.GetMethod("PostDraw", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method == null)
                return;

            // Hook: insert an immediate 'ret' at the start so PostDraw does nothing
            thresherPostDrawIL = new ILHook(method, (ILContext il) =>
            {
                var c = new ILCursor(il);
                c.Goto(0);           // beginning of the method
                c.Emit(OpCodes.Ret); // return; (void method, safe to do)
            });
        }

        public override void Unload()
        {
            thresherPostDrawIL?.Dispose();
            thresherPostDrawIL = null;
        }
    }
}
