using System.Reflection;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.ILItemChanges.VoidCostChanges
{
    public class GreenJellyfishStaffVoidCostAdjustment : ModSystem
    {
        private static Hook _getVoidHook;

        public override bool IsLoadingEnabled(Mod mod)
        {
            return InfernalConfig.Instance.SOTSBalanceChanges;
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod("SOTS", out var sots))
                return;

            // SOTS assembly holds the type
            var t = sots.Code?.GetType("SOTS.Items.Tide.GreenJellyfishStaff");
            if (t == null) return;

            var mi = t.GetMethod("GetVoid", BindingFlags.Instance | BindingFlags.Public);
            if (mi == null) return;

            // Patch: always return 2
            _getVoidHook = new Hook(mi, (GetVoidDelegate)((self, player) => 2));
        }

        public override void Unload()
        {
            _getVoidHook?.Dispose();
            _getVoidHook = null;
        }

        // Signature must match the target instance method (int GetVoid(Player))
        private delegate int GetVoidDelegate(object self, Player player);
    }
}
