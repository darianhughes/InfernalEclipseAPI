using System.Reflection;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.ILItemChanges.VoidCostChanges
{
    public class GoopwoodSplitVoidCostAdjustment : ModSystem
    {
        private static Hook _voidCostHook;
        public override bool IsLoadingEnabled(Mod mod)
        {
            return InfernalConfig.Instance.SOTSBalanceChanges;
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod("SOTSBardHealer", out var sotsBh))
                return;

            var t = sotsBh.Code?.GetType("SOTSBardHealer.Items.GoopwoodSplit");
            if (t == null) return;

            var getter = t.GetMethod("get_VoidCost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (getter == null) return;

            _voidCostHook = new Hook(getter, (Getter)((self) => 8));
        }

        public override void Unload()
        {
            _voidCostHook?.Dispose();
            _voidCostHook = null;
        }

        private delegate int Getter(object self);
    }
}
