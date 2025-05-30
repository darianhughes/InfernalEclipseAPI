using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;
using Terraria.ID;
using Humanizer;
using ThoriumMod.Utilities;
using MonoMod.RuntimeDetour;
using System.Reflection;

public class SoulBleedPatch : ModSystem
{
    private ILHook ilHook;

    public override void Load()
    {
        if (!ModLoader.TryGetMod("ThoriumRework", out Mod thoriumRework))
            return;

        // Adjust this type string if your namespace is different!
        var type = thoriumRework.Code.GetType("ThoriumRework.Projectiles.SoulBleed");
        if (type == null)
            return;

        var aiMethod = type.GetMethod("AI", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (aiMethod == null)
            return;

        ilHook = new ILHook(aiMethod, IL_NerfSoulBleedHeal);
    }

    public override void Unload()
    {
        ilHook?.Dispose();
        ilHook = null;
    }

    private void IL_NerfSoulBleedHeal(ILContext il)
    {
        var c = new ILCursor(il);

        // Find ldc.r8 or ldc.r4 for the float value 0.02, and change it to 0.005
        while (c.TryGotoNext(MoveType.After, i =>
            (i.OpCode == OpCodes.Ldc_R8 && (double)i.Operand == 0.02) ||
            (i.OpCode == OpCodes.Ldc_R4 && (float)i.Operand == 0.02f)))
        {
            if (c.Prev.OpCode == OpCodes.Ldc_R8)
            {
                c.Prev.Operand = 0.005;
            }
            else if (c.Prev.OpCode == OpCodes.Ldc_R4)
            {
                c.Prev.Operand = 0.005f;
            }
        }
    }
}