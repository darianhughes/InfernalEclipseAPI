using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems.ILItemChanges
{
    [ExtendsFromMod("ThoriumMod")]
    public class CapeoftheSurvivorNerfSystem : ModSystem
    {
        private ILHook _consumableDodgeIL;
        private ILHook _postUpdateEquipsIL;

        public override void Load()
        {
            var tp = typeof(ThoriumMod.ThoriumPlayer);

            // 1) Disable the “<= 1 damage” Cape dodge.
            var miConsumableDodge = tp.GetMethod("ConsumableDodge", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            _consumableDodgeIL = new ILHook(miConsumableDodge, PatchConsumableDodge);

            // 2) Reduce Cape DR gain so it caps at 0.15 (600 * 0.00025f).
            var miPostUpdateEquips = tp.GetMethod("PostUpdateEquips", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            _postUpdateEquipsIL = new ILHook(miPostUpdateEquips, PatchPostUpdateEquips);
        }

        public override void Unload()
        {
            _consumableDodgeIL?.Dispose();
            _postUpdateEquipsIL?.Dispose();
        }

        private static void PatchConsumableDodge(ILContext il)
        {
            var c = new ILCursor(il);

            // Find the call to CapeoftheSurvivorDodge and back up to the constant "1" in the compare (Damage <= 1).
            if (c.TryGotoNext(MoveType.Before, i => i.MatchCallvirt(typeof(ThoriumMod.ThoriumPlayer).GetMethod("CapeoftheSurvivorDodge",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))))
            {
                if (c.TryGotoPrev(i => i.MatchLdcI4(1)))
                {
                    // Change `<= 1` to `<= 0` (effectively never triggers on normal hits)
                    c.Next.Operand = 0;
                }
            }
        }

        private static void PatchPostUpdateEquips(ILContext il)
        {
            var c = new ILCursor(il);

            // Replace the Cape DR increment coefficient: 0.000334f -> 0.00025f (so 600 ticks => 0.15 DR).
            if (c.TryGotoNext(i => i.MatchLdcR4(0.000334f)))
            {
                c.Next.Operand = 0.00025f;
            }
        }
    }
}
