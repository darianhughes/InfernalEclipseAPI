using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;
using System.Reflection;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Content.DifficultyOverrides
{
    public class ThoriumBossStatScaling : GlobalNPC
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

        public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
        {
            return npc.boss && npc.ModNPC?.Mod?.Name == "ThoriumMod";
        }

        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            //Crossmod intagration for WHummus code (remove the comments)
            //only load if InfernalEclipseAPI isn't active for no overlaps
            //if (ModLoader.TryGetMod("InfernalEclipseAPI", out _)
            //return;

            //Boss Rush, 
            if (GetCalDifficulty("bossrush"))
            {
                string name = npc.ModNPC?.Name ?? "";

                //do this 
                if (name.Contains("BoreanStrider"))
                    npc.lifeMax *= 65;

                //ignore the rest if Thorium Bosses Reworked is active as this is already done in that mod.
                if (!ModLoader.TryGetMod("ThoriumRework", out _))
                {
                    if (name.Contains("TheGrandThunderBird"))
                        npc.lifeMax *= 125;
                    else if (name.Contains("QueenJellyfish"))
                        npc.lifeMax *= 115;
                    else if (name.Contains("Viscount"))
                        npc.lifeMax *= 110;
                    else if (name.Contains("StarScouter"))
                        npc.lifeMax *= 105;
                    else if (name.Contains("BuriedChampion") || name.Contains("GraniteEnergyStorm"))
                        npc.lifeMax *= 75;
                    else if (name.Contains("FallenBeholder"))
                        npc.lifeMax *= 65;
                    else if (name.Contains("Lich"))
                        npc.lifeMax *= 30;
                    else if (name.Contains("ForgottenOne"))
                        npc.lifeMax *= 15;
                    else if (name.Contains("SlagFury") || name.Contains("Aquaius") || name.Contains("Omnicide") || name.Contains("DreamEater"))
                        npc.lifeMax *= 2;
                }
            }

            if (IsWorldLegendary())
            {
                npc.lifeMax += (int)(0.1 * npc.lifeMax);
            }
            if (IsInfernumActive() || GetFargoDifficullty("MasochistMode"))
            {
                if (npc.ModNPC?.Name?.Contains("GraniteEnergyStorm") == true || npc.ModNPC?.Name?.Contains("BuriedChampion") == true)
                {
                    npc.lifeMax += (int)npc.lifeMax;
                }
                if (npc.ModNPC?.Name?.Contains("StarScouter") == true)
                {
                    npc.lifeMax += (int)(0.75 * npc.lifeMax);
                }
                string name = npc.ModNPC?.Name ?? "";
                if (name.Contains("SlagFury") || name.Contains("Aquaius") || name.Contains("Omnicide") || name.Contains("DreamEater"))
                    npc.lifeMax += (int)(0.15 * npc.lifeMax);

                npc.lifeMax += (int)(0.35 * npc.lifeMax);
            }
            else
            {
                if (GetFargoDifficullty("EternityMode"))
                {
                    if (npc.ModNPC?.Name?.Contains("GraniteEnergyStorm") == true || npc.ModNPC?.Name?.Contains("BuriedChampion") == true)
                    {
                        npc.lifeMax += (int)(0.75 * npc.lifeMax);
                    }
                    if (npc.ModNPC?.Name?.Contains("StarScouter") == true)
                    {
                        npc.lifeMax += (int)(0.5 * npc.lifeMax);
                    }

                    npc.lifeMax += (int)(0.25 * npc.lifeMax);
                }
                else if (GetCalDifficulty("death"))
                {
                    if (npc.ModNPC?.Name?.Contains("GraniteEnergyStorm") == true || npc.ModNPC?.Name?.Contains("BuriedChampion") == true)
                    {
                        npc.lifeMax += (int)(0.5 * npc.lifeMax);
                    }
                    if (npc.ModNPC?.Name?.Contains("StarScouter") == true)
                    {
                        npc.lifeMax += (int)(0.375 * npc.lifeMax);
                    }

                    npc.lifeMax += (int)(0.2 * npc.lifeMax);
                }
                else if (GetCalDifficulty("revengeance"))
                {
                    if (npc.ModNPC?.Name?.Contains("GraniteEnergyStorm") == true || npc.ModNPC?.Name?.Contains("BuriedChampion") == true)
                    {
                        npc.lifeMax += (int)(0.25 * npc.lifeMax);
                    }
                    if (npc.ModNPC?.Name?.Contains("StarScouter") == true)
                    {
                        npc.lifeMax += (int)(0.1875 * npc.lifeMax);
                    }

                    npc.lifeMax += (int)(0.1 * npc.lifeMax);
                }
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            string name = npc.ModNPC?.Name ?? "";
            float damageMod = 0;

            if (name.Contains("SlagFury") || name.Contains("Aquaius") || name.Contains("Omnicide") || name.Contains("DreamEater"))
                damageMod += 0.40f;

            if (IsWorldLegendary())
            {
                damageMod += 1.05f;
            }
            if (IsInfernumActive() || GetFargoDifficullty("MasochistMode"))
            {
                damageMod += 1.275f;
            }
            else
            {
                if (GetFargoDifficullty("EternityMode"))
                {
                    damageMod += 1.2f;
                }
                else if (GetCalDifficulty("death"))
                {
                    damageMod += 1.1f;
                }
            }

            modifiers.SourceDamage *= damageMod;
        }

        public override void PostAI(NPC npc)
        {
            //messing with the borean striders speed causes it to phase through the ground.
            if (npc.ModNPC?.Name?.Contains("BoreanStrider") == true || npc.ModNPC?.Name?.Contains("FallenBeholder") == true)
            {
                return;
            }

            if (IsWorldLegendary())
            {
                npc.position += npc.velocity * 0.5f;
            }
            if (IsInfernumActive() || GetFargoDifficullty("MasochistMode"))
            {
                npc.position += npc.velocity * 0.20f;
            }
            else
            {
                if (GetFargoDifficullty("EternityMode"))
                {
                    npc.position += npc.velocity * 0.10f;
                }
                else if (GetCalDifficulty("death"))
                {
                    npc.position += npc.velocity * 0.05f;
                }
            }
        }
    }
}