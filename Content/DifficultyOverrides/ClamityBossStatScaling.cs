using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Reflection;
using Terraria.DataStructures;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;
using Terraria.WorldBuilding;

namespace InfernalEclipseAPI.Content.DifficultyOverrides
{
    public class ClamityBossStatScaling : GlobalNPC
    {
        private bool GetCalDifficulty(string diff)
        {
            return ModLoader.TryGetMod("CalamityMod", out Mod calamity) &&
                   calamity.Call("GetDifficultyActive", diff) is bool b && b;
        }

        private bool IsInfernumActive()
        {
            return InfernumSaveSystem.InfernumModeEnabled;
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
        public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
        {
            return ((ModType)npc.ModNPC)?.Mod.Name == "Clamity";
        }

        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            if (npc.boss)
            {
                npc.lifeMax += npc.lifeMax;

                if (IsWorldLegendary())
                {
                    npc.lifeMax += (int)(0.1 * npc.lifeMax);
                }
                if (IsInfernumActive() || GetFargoDifficullty("MasochistMode"))
                {
                    npc.lifeMax += (int)(((double).35) * (double)npc.lifeMax);
                }
                else
                {
                    if (GetFargoDifficullty("EternityMode"))
                    {
                        npc.lifeMax += (int)(0.25 * npc.lifeMax);
                    }
                }
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SourceDamage *= 2.0f;

            if (IsWorldLegendary())
            {
                npc.lifeMax += (int)(0.1 * npc.lifeMax);
            }
            if (IsInfernumActive() || GetFargoDifficullty("MasochistMode"))
            {
                modifiers.SourceDamage *= 1.35f;
            }
            else
            {
                if (GetFargoDifficullty("EternityMode"))
                {
                    modifiers.SourceDamage *= 1.25f;
                }
            }
        }

        public override void PostAI(NPC npc)
        {
            ModNPC modNPC14 = npc.ModNPC;
            if (!((modNPC14 != null ? (((ModType)modNPC14).Name.Contains("ClamitasBoss") ? 1 : 0) : 0) != 0))
            {
                npc.position += npc.velocity * 0.1f;

                if (IsWorldLegendary())
                {
                    npc.lifeMax += (int)(0.1 * npc.lifeMax);
                }
                if (IsInfernumActive() || GetFargoDifficullty("MasochistMode"))
                {
                    npc.position += npc.velocity * 0.35f;
                }
                else
                {
                    if (GetFargoDifficullty("EternityMode"))
                    {
                        npc.position += npc.velocity * 0.25f;
                    }
                }
            }
        }
    }
}