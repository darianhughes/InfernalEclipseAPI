using System.Reflection;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.ILItemChanges.VoidCostChanges
{
    public class TurboSlicerVoidCostAdjustment : ModSystem
    {
        private static Hook _voidCostHook;

        public override bool IsLoadingEnabled(Mod mod)
        {
            return InfernalConfig.Instance.SOTSBalanceChanges;
        }
        public override void Load()
        {
            if (!ModLoader.TryGetMod("SOTSBardHealer", out var sotsBH))
                return;

            var t = sotsBH.Code?.GetType("SOTSBardHealer.Items.TurboSlicer");
            if (t == null) return;

            var getter = t.GetMethod("get_VoidCost", BindingFlags.Instance | BindingFlags.Public);
            if (getter == null) return;

            _voidCostHook = new Hook(getter, (Getter)((self) => 2));
        }

        public override void Unload()
        {
            _voidCostHook?.Dispose();
            _voidCostHook = null;
        }

        private delegate int Getter(object self);
    }
}
