using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using InfernumMode.Core.GlobalInstances.Systems;
using Terraria.DataStructures;
using System.Reflection;

namespace InfernalEclipseAPI.Core.Players
{
    //Respawn Prevention Code Credit: Fargo's Souls Team
    public class RespawnPlayer : ModPlayer
    {
        public int Respawns;

        public static bool AnyBosses()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && (npc.boss || (npc.type >= NPCID.EaterofWorldsBody && npc.type <= NPCID.EaterofWorldsTail)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool appliedThisDeath;

        public override void ResetEffects()
        {
            if (!AnyBosses() & !Player.dead)
            {
                appliedThisDeath = false;
                Respawns = 0;
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (AnyBosses()) Respawns++;
        }

        private bool GetCalDifficulty(string diff)
        {
            return ModLoader.TryGetMod("CalamityMod", out Mod calamity) &&
                   calamity.Call("GetDifficultyActive", diff) is bool b && b;
        }

        private bool IsInfernumActive()
        {
            return WorldSaveSystem.InfernumModeEnabled;
        }

        private bool GetFargoDifficullty(string diff)
        {
            if (!ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSouls))
            {
                return false;
            }

            return fargoSouls.Call(diff) is bool active && active;
        }
        private bool IsWorldLegendary()
        {
            FieldInfo findInfo = typeof(Main).GetField("_currentGameModeInfo", BindingFlags.Static | BindingFlags.NonPublic);
            GameModeData data = (GameModeData)findInfo.GetValue(null);
            return (Main.getGoodWorld && data.IsMasterMode);
        }

        public bool isMinimumDiffToPreventRespawn()
        {
            Difficulty diff = InfernalConfig.Instance.MinimumDifficultyToPreventRespawns;
            switch (diff)
            {
                case Difficulty.AlwaysOff:
                    return false;
                case Difficulty.AlwaysOn:
                    return true;
                case Difficulty.Expert:
                    return Main.expertMode;
                case Difficulty.Master:
                    return Main.masterMode;
                case Difficulty.Legendary:
                    return IsWorldLegendary();
                case Difficulty.Revengence:
                    return GetCalDifficulty("death") || GetCalDifficulty("revengeance");
                case Difficulty.MasterRevengence:
                    return Main.masterMode && (GetCalDifficulty("death") || GetCalDifficulty("revengeance"));
                case Difficulty.LegendaryRevengence:
                    return IsWorldLegendary() && (GetCalDifficulty("death") || GetCalDifficulty("revengeance"));
                case Difficulty.Death:
                    return GetCalDifficulty("death") || IsInfernumActive();
                case Difficulty.MasterDeath:
                    return Main.masterMode && (GetCalDifficulty("death"));
                case Difficulty.LegendaryDeath:
                    return IsWorldLegendary() && (GetCalDifficulty("death"));
                case Difficulty.Infernum:
                    return IsInfernumActive();
                case Difficulty.MasterInfernum:
                    return Main.masterMode && IsInfernumActive();
                case Difficulty.LegendaryInfernum:
                    return IsWorldLegendary() && IsInfernumActive();
                default:
                    return false;
            }
        }

        public bool PreventRespawn() => isMinimumDiffToPreventRespawn() && AnyBosses() && Respawns > InfernalConfig.Instance.MultiplayerRespawnsAllowed;
        public override void UpdateDead()
        {
            base.UpdateDead();

            ResetEffects();

            if (PreventRespawn()) Player.respawnTimer = 60 * 5;
            else
            {
                int respawnTimerSet = AnyBosses() ? (60 * InfernalConfig.Instance.MultiplayerBossRespawnTimer) : 180;
                if (Player.respawnTimer < respawnTimerSet || Player.respawnTimer > respawnTimerSet)
                {
                    if (!appliedThisDeath)
                    {
                        Player.respawnTimer = respawnTimerSet;
                        appliedThisDeath = true;
                    }
                }
            }
        }

        public override void OnRespawn()
        {
            appliedThisDeath = false;
        }
    }
}
