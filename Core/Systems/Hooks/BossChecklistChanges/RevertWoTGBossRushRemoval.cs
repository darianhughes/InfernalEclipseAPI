using System.Reflection;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks.BossChecklistChanges
{
    [JITWhenModsEnabled(InfernalCrossmod.NoxusBoss.Name)]
    [ExtendsFromMod(InfernalCrossmod.NoxusBoss.Name)]
    public class RevertWoTGBossRushRemoval : ModSystem
    {
        private Hook removeBossRushHook;

        public override void Load()
        {
            // Make sure NoxusBoss is loaded
            if (!ModLoader.TryGetMod("NoxusBoss", out Mod noxusBoss))
                return;

            // Type that contains RemoveBossRushFromChecklist
            Type compatType = noxusBoss.Code?.GetType("NoxusBoss.Core.CrossCompatibility.Inbound.BossChecklist.BossChecklistCompatibilitySystem");

            if (compatType == null)
                return;

            // private static void RemoveBossRushFromChecklist()
            MethodInfo removeBossRushMethod = compatType.GetMethod("RemoveBossRushFromChecklist", BindingFlags.NonPublic | BindingFlags.Static);

            if (removeBossRushMethod == null)
                return;

            // Detour it to a no-op
            removeBossRushHook = new Hook(removeBossRushMethod, RemoveBossRushDetour);
        }

        public override void Unload()
        {
            removeBossRushHook?.Dispose();
            removeBossRushHook = null;
        }

        private static void RemoveBossRushDetour(Action orig)
        {
        }
    }
}
