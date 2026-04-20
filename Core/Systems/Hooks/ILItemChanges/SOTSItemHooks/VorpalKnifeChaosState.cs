using System.Reflection;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.SOTSItemHooks
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public sealed class VorpalKnifeChaosState : ModSystem
    {
        private Hook altFunctionUseHook;
        private Hook teleportPlayerHook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("SOTS", out Mod sots))
                return;

            Type vorpalKnifeType = sots.Code.GetType("SOTS.Items.Invidia.VorpalKnife");
            Type vorpalThrowType = sots.Code.GetType("SOTS.Projectiles.Blades.VorpalThrow");

            if (vorpalKnifeType is null || vorpalThrowType is null)
                return;

            MethodInfo altFunctionUse = vorpalKnifeType.GetMethod("AltFunctionUse", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            MethodInfo teleportPlayer = vorpalThrowType.GetMethod("TeleportPlayer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (altFunctionUse is not null)
                altFunctionUseHook = new Hook(altFunctionUse, BlockChaosTeleportUse);

            if (teleportPlayer is not null)
                teleportPlayerHook = new Hook(teleportPlayer, ApplyChaosStateOnTeleport);
        }

        public override void Unload()
        {
            altFunctionUseHook?.Dispose();
            teleportPlayerHook?.Dispose();

            altFunctionUseHook = null;
            teleportPlayerHook = null;
        }

        private delegate bool Orig_AltFunctionUse(object self, Player player);

        private static bool BlockChaosTeleportUse(Orig_AltFunctionUse orig, object self, Player player)
        {
            if (player.HasBuff(BuffID.ChaosState))
                return false;

            return orig(self, player);
        }

        private delegate void Orig_TeleportPlayer(object self, Player player);

        private static void ApplyChaosStateOnTeleport(Orig_TeleportPlayer orig, object self, Player player)
        {
            if (player.HasBuff(BuffID.ChaosState))
                return;

            orig(self, player);

            player.AddBuff(BuffID.ChaosState, 20 * 60);
        }
    }
}
