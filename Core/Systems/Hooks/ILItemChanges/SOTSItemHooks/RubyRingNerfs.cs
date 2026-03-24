using System.Reflection;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.SOTSItemHooks
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public sealed class RubyRingNerfs : ModSystem
    {
        private Hook onPickupHook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("SOTS", out Mod sots))
                return;

            Type sotsItemType = sots.Code.GetType("SOTS.SOTSItem");
            if (sotsItemType is null)
                return;

            MethodInfo onPickupMethod = sotsItemType.GetMethod(
                "OnPickup",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(Item), typeof(Player) },
                null
            );

            if (onPickupMethod is null)
                return;

            onPickupHook = new Hook(onPickupMethod, SOTSOnPickup_Override);
        }

        public override void Unload()
        {
            onPickupHook?.Dispose();
            onPickupHook = null;
        }

        private delegate bool Orig_OnPickup(object self, Item item, Player player);

        private static bool SOTSOnPickup_Override(Orig_OnPickup orig, object self, Item item, Player player)
        {
            // Do nothing.
            // GlobalItem.OnPickup's default behavior is effectively "allow pickup",
            // so just return true instead of calling the original SOTS logic.
            return true;
        }
    }
}
