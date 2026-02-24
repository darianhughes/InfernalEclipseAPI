using System.Reflection;
using MonoMod.RuntimeDetour;
using SOTS;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges
{
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    public class ChallengerRingNerfs : ModSystem
    {
        private static Hook diamandRingEffectHook;

        public override void Load()
        {
            Mod sots = ModLoader.GetMod("SOTS");
            var sotsPlayerType = sots.Code.GetType("SOTS.SOTSPlayer");
            var updateEquips = sotsPlayerType.GetMethod("UpdateEquips", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            diamandRingEffectHook = new Hook(updateEquips, UpdateEquips_Detour);
        }

        public override void Unload()
        {
            diamandRingEffectHook?.Dispose();
            diamandRingEffectHook = null;
        }

        private static void UpdateEquips_Detour(Action<SOTSPlayer> orig, SOTSPlayer self)
        {
            Player player = self.Player;

            int def = player.statDefense;
            if (def > 15)
                def = 15;

            self.previousDefense = def;

            if (self.DiamondRing)
            {
                player.statDefense -= (int)(def * 0.66f);

                player.GetDamage(DamageClass.Generic) += def * 0.01f;
            }

            self.DiamondRing = false;
        }
    }
}
