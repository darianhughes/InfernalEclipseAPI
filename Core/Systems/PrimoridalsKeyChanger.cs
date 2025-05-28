using Terraria.ModLoader;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;
using System.Reflection;
using MonoMod.RuntimeDetour;

public class PrimordialsProgressionChanger : ModSystem
{
    private ILHook ilHook;

    public override void Load()
    {
        if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium) || !ModLoader.TryGetMod("ThoriumRework", out _))
            return;

        var bcSupportType = thorium.Code.GetType("ThoriumMod.ModSupport.ModSupportModules.BossChecklistSupport");
        if (bcSupportType == null)
            return;

        var logBossesMethod = bcSupportType.GetMethod("DoBossChecklistSupport_LogBosses",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (logBossesMethod == null)
            return;

        ilHook = new ILHook(logBossesMethod, IL_EditPrimordialsProgression);
    }

    public override void Unload()
    {
        ilHook?.Dispose();
        ilHook = null;
    }

    private void IL_EditPrimordialsProgression(ILContext il)
    {
        var c = new ILCursor(il);

        // Search for the string "ThePrimordials" as a marker.
        while (c.TryGotoNext(MoveType.After, i => i.MatchLdstr("ThePrimordials")))
        {
            // The very next instruction should be ldc.r4 19.5 (float literal).
            if (c.Next != null && c.Next.MatchLdcR4(19.5f))
            {
                c.Next.Operand = 21.5f; // Change the operand to 22.5f.
            }
        }
    }
}