using Terraria.ModLoader;
using Terraria;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;

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

            if (IsInfernumActive() || GetFargoDifficullty("MasochistMode"))
            {
                if (npc.ModNPC?.Name?.Contains("GraniteEnergyStorm") == true || npc.ModNPC?.Name?.Contains("BuriedChampion") == true)
                {
                    npc.lifeMax += (int)npc.lifeMax;
                }

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

                    npc.lifeMax += (int)(0.25 * npc.lifeMax);
                }
                else if (GetCalDifficulty("death"))
                {
                    if (npc.ModNPC?.Name?.Contains("GraniteEnergyStorm") == true || npc.ModNPC?.Name?.Contains("BuriedChampion") == true)
                    {
                        npc.lifeMax += (int)(0.5 * npc.lifeMax);
                    }

                    npc.lifeMax += (int)(0.2 * npc.lifeMax);
                }
                else if (GetCalDifficulty("revengeance"))
                {
                    if (npc.ModNPC?.Name?.Contains("GraniteEnergyStorm") == true || npc.ModNPC?.Name?.Contains("BuriedChampion") == true)
                    {
                        npc.lifeMax += (int)(0.25 * npc.lifeMax);
                    }

                    npc.lifeMax += (int)(0.1 * npc.lifeMax);
                }
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            //Crossmod intagration for WHummus code (remove the comments)
            //only load if InfernalEclipseAPI isn't active for no overlaps
            //if (ModLoader.TryGetMod("InfernalEclipseAPI", out _)
            //return;

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
                else if (GetCalDifficulty("death"))
                {
                    modifiers.SourceDamage *= 1.2f;
                }
                else if (GetCalDifficulty("revengeance"))
                {
                    modifiers.SourceDamage *= 1.1f;
                }
            }
        }

        public override void PostAI(NPC npc)
        {
            //Crossmod intagration for WHummus code (remove the comments)
            //only load if InfernalEclipseAPI isn't active for no overlaps
            //if (ModLoader.TryGetMod("InfernalEclipseAPI", out _)
            //return;

            //messing with the borean striders speed causes it to phase through the ground.
            if (npc.ModNPC?.Name?.Contains("BoreanStrider") == true)
            {
                return;
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
                else if (GetCalDifficulty("death"))
                {
                    npc.position += npc.velocity * 0.2f;
                }
                else if (GetCalDifficulty("revengeance"))
                {
                    npc.position += npc.velocity * 0.1f;
                }
            }
        }
    }
}