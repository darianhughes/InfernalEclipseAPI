using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FargowiltasSouls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems.YharimMutantChange
{
    public class AbomMusicFix : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            // Check if FargowiltasSouls mod is loaded
            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSouls))
            {
                // Get MutantBoss NPC type
                int mutantBossType = fargoSouls.Find<ModNPC>("AbomBoss").Type;

                // Check if any instance of MutantBoss is active
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].type == mutantBossType)
                        return true;
                }
            }
            return false;
        }
        public override float GetWeight(Player player)
        {
            return 1f;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override int Music
        {
            get
            {
                bool foundMod = ModLoader.TryGetMod("FargowiltasMusic", out Mod musicMod);
                if (foundMod)
                {
                    if (musicMod.Version >= Version.Parse("0.1.5"))
                        return MusicLoader.GetMusicSlot(musicMod, "Assets/Music/Laevateinn_P1");
                    else
                        return MusicLoader.GetMusicSlot(musicMod, "Assets/Music/Stigma");
                }
                return MusicID.OtherworldlyPlantera;
            }
        }
    }
}