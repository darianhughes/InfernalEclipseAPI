using CalamityMod.Events;
using InfernalEclipseAPI.Core.World;
using InfernumMode.Content.BehaviorOverrides.BossAIs.GreatSandShark;

namespace InfernalEclipseAPI.Core.ModSceneEffects.RagnarokMusic
{
    public class BereftVassalRagnarok : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh + 1;

        public override int Music
        {
            get
            {
                if (InfernalConfig.Instance.BereftVassalRagnarok && InfernalWorld.RagnarokModeEnabled)
                    return MusicLoader.GetMusicSlot("InfernalEclipseAPI/Assets/Music/BereftVassal");

                if (ModLoader.TryGetMod("InfernumModeMusic", out Mod musicMod))
                    return MusicLoader.GetMusicSlot(musicMod, "Sounds/Music/BereftVassal");

                return MusicID.OldOnesArmy;
            }
        }

        public override bool IsSceneEffectActive(Player player)
        {
            if (BossRushEvent.BossRushActive)
                return false;

            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (!npc.active || npc.type != ModContent.NPCType<BereftVassal>())
                    continue;

                if (npc.ModNPC is not BereftVassal vassal)
                    continue;

                if (vassal.CurrentAttack != BereftVassal.BereftVassalAttackType.IdleState)
                    return true;
            }

            return false;
        }
    }
}
