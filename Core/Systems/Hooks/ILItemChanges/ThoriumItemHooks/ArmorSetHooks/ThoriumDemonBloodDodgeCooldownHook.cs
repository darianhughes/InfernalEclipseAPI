using System.Reflection;
using InfernalEclipseAPI.Content.RogueThrower;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.ThoriumItemHooks.ArmorSetHooks
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class ThoriumDemonBloodDodgeCooldownHook : ModSystem
    {
        private Hook dodgeHook;
        private static MethodInfo dodgeMethod;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            var thoriumPlayerType = thorium.Code.GetType("ThoriumMod.ThoriumPlayer");
            if (thoriumPlayerType is null)
                return;

            dodgeMethod = thoriumPlayerType.GetMethod(
                "DemonBloodBreastplateDodge",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (dodgeMethod != null)
                dodgeHook = new Hook(dodgeMethod, DodgeHook);
        }

        public override void Unload()
        {
            dodgeHook?.Dispose();
            dodgeHook = null;
            dodgeMethod = null;
        }

        private delegate void Orig_Dodge(object self, bool broadcast);

        private void DodgeHook(Orig_Dodge orig, object self, bool broadcast)
        {
            if (self is not ModPlayer mp)
            {
                orig(self, broadcast);
                return;
            }

            Player player = mp.Player;
            var cooldowns = player.GetModPlayer<RogueThrowerPlayer>();

            // Block if on cooldown
            if (cooldowns.demonBloodDodgeCooldown > 0)
                return;

            // Run original effect
            orig(self, broadcast);

            // Start cooldown
            cooldowns.demonBloodDodgeCooldown = 60 * 15;
        }
    }
}
