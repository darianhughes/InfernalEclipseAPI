using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Events;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.SupremeCalamitas;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems.YharimMutantChange
{
    public class YharimMutantMusicChange : ModSceneEffect
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            if (!InfernalConfig.Instance.UseAprilFoolsMutant)
            {
                return false;
            }
            return base.IsLoadingEnabled(mod);
        }
        public override bool IsSceneEffectActive(Player player)
        {
            // Check if FargowiltasSouls mod is loaded
            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSouls))
            {
                // Get MutantBoss NPC type
                int mutantBossType = fargoSouls.Find<ModNPC>("MutantBoss").Type;

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
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh + 2;

        public override int Music
        {
            get
            {
                return MusicLoader.GetMusicSlot(Mod, "Assets/Music/TheRealityoftheProphey");
            }
        }
    }
}
