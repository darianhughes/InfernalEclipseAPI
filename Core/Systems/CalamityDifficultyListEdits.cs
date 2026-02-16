using CalamityMod.Systems;
using InfernalEclipseAPI.Content.UI;
using InfernumMode.Content.UI;

namespace InfernalEclipseAPI.Core.Systems
{
    internal sealed class CalamityDifficultyListEdits : ModSystem
    {
        public override void PostSetupContent() => Apply();

        // Calamity calls CalculateDifficultyData() again here, so re-apply after world load.
        public override void PostWorldLoad() => Apply();

        private static void Apply()
        {
            if (DifficultyModeSystem.Difficulties is null)
                return;

            if (!ShouldHideCustomDifficulties())
                return;

            // Remove from the source list.
            DifficultyModeSystem.Difficulties.RemoveAll(d =>
                d is RagnarokDifficulty ||
                d is InfernumDifficulty);

            // Rebuild tiers + MostAlternateDifficulties + _difficultyTier assignments.
            DifficultyModeSystem.CalculateDifficultyData();
        }

        private static bool ShouldHideCustomDifficulties()
        {
            if (InfernalCrossmod.FargosSouls.Loaded)
                return true;

            if (Main.getGoodWorld)
                return true;

            return false;
        }
    }
}
