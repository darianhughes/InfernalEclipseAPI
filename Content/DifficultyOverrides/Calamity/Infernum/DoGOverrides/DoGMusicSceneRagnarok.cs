using CalamityMod.Events;
using CalamityMod.NPCs;
using InfernalEclipseAPI.Core.World;
using InfernumMode;
using InfernumMode.Content.BehaviorOverrides.BossAIs.DoG;

namespace InfernalEclipseAPI.Content.DifficultyOverrides.Calamity.Infernum.DoGOverrides
{
    public class DoGMusicSceneRagnarok : DoGMusicSceneInfernum
    {
        public override SceneEffectPriority Priority => BossRushEvent.BossRushActive ? SceneEffectPriority.None : (SceneEffectPriority)11;

        public override int? MusicModMusic
        {
            get
            {
                if (!InfernalWorld.RagnarokModeEnabled || InfernalConfig.Instance.DoGRagnarok == DoGSong.Off) return base.MusicModMusic;

                NPC dog = CalamityGlobalNPC.DoGHead >= 0 && Main.npc.IndexInRange(CalamityGlobalNPC.DoGHead) ? Main.npc[CalamityGlobalNPC.DoGHead] : null;

                if (dog != null && dog.active)
                {
                    ref float hasEnteredFinalPhaseFlag =  ref dog.Infernum().ExtraAI[DoGPhase1HeadBehaviorOverride.HasEnteredFinalPhaseFlagIndex];

                    if (DoGChanges.DesperationHasTriggered || DoGChanges.DesperationCanDie)
                        return MusicLoader.GetMusicSlot(Mod, "Assets/Music/LastBattleDesperationCut");

                    if (hasEnteredFinalPhaseFlag == 1f && InfernalConfig.Instance.DoGRagnarok == DoGSong.On)
                        return MusicLoader.GetMusicSlot(Mod, "Assets/Music/LastBattle");
                }

                return base.MusicModMusic;
            }
        }
    }
}
