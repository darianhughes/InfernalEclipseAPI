using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILTileChanges
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class SancturaryPotNoMeteorite : ModSystem
    {
        private static ILHook evostonePotDropsHook;

        public override void OnModLoad()
        {
            if (!ModLoader.TryGetMod("SOTS", out Mod sots) || !InfernalConfig.Instance.BossKillCheckOnOres)
                return;

            var evostonePotsType = sots.Code.GetType("SOTS.Items.Invidia.EvostonePots");
            if (evostonePotsType == null)
                return;

            MethodInfo potDropsMethod = evostonePotsType.GetMethod("PotDrops", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (potDropsMethod == null)
                return;

            // Create ILHook and keep a reference so it doesn't get GC’d
            evostonePotDropsHook = new ILHook(potDropsMethod, Patch_PotDrops);
        }

        public override void OnModUnload()
        {
            evostonePotDropsHook?.Dispose();
            evostonePotDropsHook = null;
        }

        private static void Patch_PotDrops(ILContext il)
        {
            var c = new ILCursor(il);

            // Look for the ldc.i4 117 used in:
            // Item.NewItem(..., 117, Main.rand.Next(1, 4), ...)
            if (!c.TryGotoNext(i => i.MatchLdcI4(117)))
            {
                InfernalEclipseAPI.Instance.Logger.Warn("[IEoR:SancturaryPotNoMeteorite] Could not replace Meteorite bars.");
                return;
            }

            // Replace "ldc.i4 117" with "call int GetEvostoneBarItemId()"
            c.Next.OpCode = OpCodes.Call;
            c.Next.Operand = typeof(SancturaryPotNoMeteorite).GetMethod(
                nameof(GetEvilBarItemId),
                BindingFlags.Static | BindingFlags.NonPublic
            );
        }

        private static int GetEvilBarItemId()
        {
            return WorldGen.crimson ? ItemID.CrimtaneBar : ItemID.DemoniteBar;
        }
    }
}
