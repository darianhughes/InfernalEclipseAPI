using CalamityMod.Events;
using CalamityMod.NPCs;
using InfernalEclipseAPI.Core.World;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Providence;
using InfernumMode.Core.TrackedMusic;

namespace InfernalEclipseAPI.Content.DifficultyOverrides.Calamity.Infernum.ProvidenceOverrides
{
    public class ProvidenceMusicSceneRagnarok : ProvidenceMusicSceneInfernum
    {
        public override SceneEffectPriority Priority => BossRushEvent.BossRushActive ? SceneEffectPriority.None : (SceneEffectPriority)26;

        public override int? MusicModMusic
        {
            get
            {
                if (CalamityGlobalNPC.holyBoss == -1 || !InfernalWorld.RagnarokModeEnabled || !ProvidenceIsInPhase2 || !ProvidenceBehaviorOverride.IsEnraged)
                    return base.MusicModMusic;

                if (Main.npc[CalamityGlobalNPC.holyBoss].ai[0] == (float)ProvidenceBehaviorOverride.ProvidenceAttackType.CrystalForm)
                    return 0;

                return MusicLoader.GetMusicSlot(InfernalEclipseAPI.ProvidenceNightPath);
            }
        }
    }

    public class ProvidenceNightTrackedMusic : ProvidenceTrackedMusic
    {
        public override string MusicPath => InfernalEclipseAPI.ProvidenceNightPath;
    }
}
