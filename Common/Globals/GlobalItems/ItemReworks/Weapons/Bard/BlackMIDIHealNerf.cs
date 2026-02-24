using System.Reflection;
using InfernalEclipseAPI.Core.Systems;
using MonoMod.Cil;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ItemReworks.Weapons.Bard
{
    //WardrobeHummus
    [JITWhenModsEnabled(InfernalCrossmod.Thorium.Name)]
    [ExtendsFromMod(InfernalCrossmod.Thorium.Name)]
    public class BlackMIDIHealNerf : ModSystem
    {
        public override void Load()
        {
            // Get the type of the Thorium BlackMIDIPro class
            var blackMidiType = InfernalCrossmod.Thorium.Mod.Code.GetType("ThoriumMod.Projectiles.Bard.BlackMIDIPro");
            if (blackMidiType == null) return;

            // Get the BardOnHitNPC method
            var method = blackMidiType.GetMethod(
                "BardOnHitNPC",
                BindingFlags.Public | BindingFlags.Instance
            );

            if (method == null) return;

            MonoModHooks.Modify(method, IL_BardOnHitNPC);
        }

        private void IL_BardOnHitNPC(ILContext il)
        {
            var c = new ILCursor(il);

            // Look for the constant 0.1f (the heal multiplier)
            if (c.TryGotoNext(i => i.MatchLdcR4(0.1f)))
            {
                // Replace it with 0.01f
                c.Next.OpCode = Mono.Cecil.Cil.OpCodes.Ldc_R4;
                c.Next.Operand = 0.005f;
            }
        }
    }
}
