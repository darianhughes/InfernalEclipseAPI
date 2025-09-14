using Terraria.ModLoader;
using Terraria;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;

namespace InfernalEclipseAPI.Content.DifficultyOverrides
{
    public class ConsolariaBossStatScaling : GlobalNPC
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
        public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
        {
            return npc.boss && ((ModType)npc.ModNPC)?.Mod.Name == "Consolaria";
        }

        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            Mod mod;
            bool flag = false;
            int num1 = 0, num2 = 0;

            if (ModLoader.TryGetMod("CalamityMod", out mod))
            {
                object result = mod.Call("GetDifficultyActive", "BossRush");
                if (result is bool b)
                {
                    flag = b;
                    num1 = 1;
                }
            }
            num2 = flag ? 1 : 0;
            num2 = flag ? 1 : 0;

            if (IsInfernumActive() || GetFargoDifficullty("MasochistMode"))
            {
                //Boss Rush Boost
                if ((num1 & num2) != 0)
                {
                    npc.lifeMax += (int)(((double).25 * (double)npc.lifeMax));
                }

                npc.lifeMax += (int)(0.30 * npc.lifeMax);
            }
            else
            {
                if (GetFargoDifficullty("EternityMode"))
                {
                    //Boss Rush Boost
                    if ((num1 & num2) != 0)
                    {
                        npc.lifeMax += (int)(((double).2 * (double)npc.lifeMax));
                    }

                    npc.lifeMax += (int)(0.20 * npc.lifeMax);
                }
                else if (GetCalDifficulty("death"))
                {
                    //Boss Rush Boost
                    if ((num1 & num2) != 0)
                    {
                        npc.lifeMax += (int)(((double).15 * (double)npc.lifeMax));
                    }

                    npc.lifeMax += (int)(0.15 * npc.lifeMax);
                }
                else if (GetCalDifficulty("revengeance"))
                {
                    //Boss Rush Boost
                    if ((num1 & num2) != 0)
                    {
                        npc.lifeMax += (int)(((double).1 * (double)npc.lifeMax));
                    }

                    npc.lifeMax += (int)(0.05 * npc.lifeMax);
                }
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (IsInfernumActive() || GetFargoDifficullty("MasochistMode"))
            {
                modifiers.SourceDamage *= 1.25f;
            }
            else
            {
                if (GetFargoDifficullty("EternityMode"))
                {
                    modifiers.SourceDamage *= 1.20f;
                }
                else if (GetCalDifficulty("death"))
                {
                    modifiers.SourceDamage *= 1.15f;
                }
                else if (GetCalDifficulty("revengeance"))
                {
                    modifiers.SourceDamage *= 1.05f;
                }
            }
        }

        public override void PostAI(NPC npc)
        {
            ModNPC modNPC14 = npc.ModNPC;
            if (modNPC14.Name.Contains("Lepus") || modNPC14.Name.Contains("Turkor"))
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