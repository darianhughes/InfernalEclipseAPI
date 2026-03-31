using CalamityMod.Events;
using SOTS.NPCs.Boss;

namespace InfernalEclipseAPI.Core.Systems.BossChanges
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class SubspaceLimboTheme : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh + 67;
        public override int Music
        {
            get => MusicLoader.GetMusicSlot(Mod, "Assets/Music/Isolation");
        }

        public override bool IsSceneEffectActive(Player player)
        {
            return !BossRushEvent.BossRushActive && NPC.AnyNPCs(ModContent.NPCType<SubspaceSerpentHead>());
        }
    }
}
