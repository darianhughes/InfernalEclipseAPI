using CalamityMod.Events;
using InfernalEclipseAPI.Core.Configs;

namespace InfernalEclipseAPI.Core.ModSceneEffects.RagnarokMusic
{
    internal class WulfrumMothershipRagnarok : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;
        public override int Music
        {
            get
            {
                switch (InfernalConfig.Instance.MothershipMusic)
                {
                    case MothershipSong.Vanilla:
                    case MothershipSong.WarMachine:
                        return MusicID.Boss1;

                    case MothershipSong.Thorium:
                        if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                            return MusicLoader.GetMusicSlot(thorium, "Sounds/Music/Stronger_Foe");
                        return MusicID.Boss1;

                    case MothershipSong.Infernum:
                        if (ModLoader.TryGetMod("InfernumModeMusic", out Mod musicMod))
                            return MusicLoader.GetMusicSlot(musicMod, "Sounds/Music/Minibosses");
                        return MusicID.Boss1;

                    default:
                        return MusicID.Boss1;
                }
            }
        }

        public override bool IsSceneEffectActive(Player player)
        {
            if (BossRushEvent.BossRushActive)
                return false;

            if (!ModLoader.TryGetMod("CalamityAddon", out Mod warMachine))
                return false;

            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.active && npc.type == warMachine.Find<ModNPC>("WulfrumMothership").Type && InfernalConfig.Instance.MothershipMusic != MothershipSong.WarMachine)
                    return true;
            }

            return false;
        }
    }
}
