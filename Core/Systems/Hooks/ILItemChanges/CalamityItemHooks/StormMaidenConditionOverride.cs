using System.Reflection;
using CalamityHunt.Common.Systems;
using InfernumMode.Core.GlobalInstances.Systems;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.CalamityItemHooks
{
    [JITWhenModsEnabled("CalamityHunt")]
    [ExtendsFromMod("CalamityHunt")]
    public class StormMaidenConditionOverride : ModSystem
    {
        private static Hook spearCanBeSummonedHook;

        public override void Load()
        {
            // Get the getter for the SpearCanBeSummoned property
            var getter = typeof(StormMaidensRetributionSpawnSystem)
                .GetProperty(nameof(StormMaidensRetributionSpawnSystem.SpearCanBeSummoned),
                             BindingFlags.Public | BindingFlags.Static)
                ?.GetMethod;

            if (getter != null)
                spearCanBeSummonedHook = new Hook(getter, SpearCanBeSummonedDetour);
        }

        public override void Unload()
        {
            spearCanBeSummonedHook?.Dispose();
            spearCanBeSummonedHook = null;
        }
        private static bool SpearCanBeSummonedDetour(Func<bool> orig)
        {
            if (!ExtraSpearCondition())
                return false;

            return orig();
        }

        private static bool ExtraSpearCondition()
        {
            if (!BossDownedSystem.Instance.GoozmaDowned)
                return false;

            return true;
        }
    }
}
