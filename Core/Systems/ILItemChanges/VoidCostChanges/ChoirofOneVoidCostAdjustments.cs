using System.Reflection;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.ILItemChanges.VoidCostChanges
{
    public class ChoirofOneVoidCostAdjustments : ModSystem
    {
        private static Hook _voidCostHook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("SOTSBardHealer", out var sotsBH))
                return;

            var t = sotsBH.Code?.GetType("SOTSBardHealer.Items.ChoirofOne");
            if (t == null) return;

            var getter = t.GetMethod("get_VoidCost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (getter == null) return;

            _voidCostHook = new Hook(getter, (Getter)((self) =>
            {
                // Read this.NetInfo (BitsByte or byte)
                var netInfoProp = self.GetType().GetProperty("NetInfo", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var netInfoVal = netInfoProp?.GetValue(self);

                int level = 1; // fallback

                if (netInfoVal is byte b)
                {
                    level = b;
                }
                else if (netInfoVal != null)
                {
                    // Use implicit conversion BitsByte -> byte if present
                    var tt = netInfoVal.GetType();
                    var opImplicit = tt.GetMethod("op_Implicit", BindingFlags.Public | BindingFlags.Static, null, new[] { tt }, null);
                    if (opImplicit?.Invoke(null, new[] { netInfoVal }) is byte bb)
                        level = bb;
                }

                // Clamp to [1,3], like original
                level = Math.Clamp(level, 1, 3);

                int baseCost = 10;
                return baseCost * (4 - level);
            }));
        }

        public override void Unload()
        {
            _voidCostHook?.Dispose();
            _voidCostHook = null;
        }

        private delegate int Getter(object self);
    }
}
