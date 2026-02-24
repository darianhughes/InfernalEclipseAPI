using System.Reflection;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.ThoriumItemHooks
{
    [JITWhenModsEnabled(InfernalCrossmod.Thorium.Name)]
    [ExtendsFromMod(InfernalCrossmod.Thorium.Name)]
    public class SheathNerfHooks : ModSystem
    {
        private Hook leatherSheathHook;
        private Hook titanSheathHook;

        public override void Load()
        {
            Mod thorium = InfernalCrossmod.Thorium.Mod;

            var leatherSheathType = thorium.Code.GetType("ThoriumMod.Core.Sheaths.LeatherSheathData");
            if (leatherSheathType != null)
            {
                var getter = leatherSheathType.GetProperty(
                    "DamageMultiplier",
                    BindingFlags.Instance | BindingFlags.Public)
                    ?.GetGetMethod();

                if (getter != null)
                    leatherSheathHook = new Hook(getter, LeatherSheathDamageMult);
            }

            var titanSheathType = thorium.Code.GetType("ThoriumMod.Core.Sheaths.TitanSlayerSheathData");
            if (titanSheathType != null)
            {

                var getter = titanSheathType.GetProperty(
                    "DamageMultiplier",
                    BindingFlags.Instance | BindingFlags.Public)
                    ?.GetGetMethod();

                if (getter != null)
                    titanSheathHook = new Hook(getter, TitanSheathDamageMult);
            }
        }

        private float LeatherSheathDamageMult(object self)
        {
            return 8f;
        }

        private float TitanSheathDamageMult(object self)
        {
            return 15f;
        }

        public override void Unload()
        {
            leatherSheathHook?.Dispose();
            leatherSheathHook = null;

            titanSheathHook?.Dispose();
            titanSheathHook = null;
        }
    }
}
