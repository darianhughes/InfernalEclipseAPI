using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Weapons.Ranged;
using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;

namespace InfernalEclipseAPI.Content.DifficultyOverrides
{
    public class ThoriumBossStatScaling : GlobalNPC
    {
        public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
        {
            return npc.boss && ((ModType)npc.ModNPC)?.Mod.Name == "ThoriumMod";
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
            if ((num1 & num2) != 0)
            {
                if (!ModLoader.TryGetMod("ThoriumRework", out Mod rework))
                {
                    ModNPC modNpc1 = npc.ModNPC;
                    if ((modNpc1 != null ? (((ModType)modNpc1).Name.Contains("TheGrandThunderBird") ? 1 : 0) : 0) != 0)
                    {
                        npc.lifeMax *= 125;
                    }
                    else
                    {
                        ModNPC modNpc2 = npc.ModNPC;
                        if ((modNpc2 != null ? (((ModType)modNpc2).Name.Contains("QueenJellyfish") ? 1 : 0) : 0) != 0)
                        {
                            npc.lifeMax *= 115;
                        }
                        else
                        {
                            ModNPC modNpc3 = npc.ModNPC;
                            if ((modNpc3 != null ? (((ModType)modNpc3).Name.Contains("Viscount") ? 1 : 0) : 0) != 0)
                            {
                                npc.lifeMax *= 110;
                            }
                            else
                            {
                                ModNPC modNpc4 = npc.ModNPC;
                                if ((modNpc4 != null ? (((ModType)modNpc4).Name.Contains("StarScouter") ? 1 : 0) : 0) != 0)
                                {
                                    npc.lifeMax *= 105;
                                }
                                else
                                {
                                    ModNPC modNpc5 = npc.ModNPC;
                                    if ((modNpc5 != null ? (((ModType)modNpc5).Name.Contains("BuriedChampion") ? 1 : 0) : 0) != 0)
                                    {
                                        npc.lifeMax *= 75;
                                    }
                                    else
                                    {
                                        ModNPC modNpc6 = npc.ModNPC;
                                        if ((modNpc6 != null ? (((ModType)modNpc6).Name.Contains("GraniteEnergyStorm") ? 1 : 0) : 0) != 0)
                                        {
                                            npc.lifeMax *= 75;
                                        }
                                        else
                                        {
                                            ModNPC modNpc7 = npc.ModNPC;
                                            if ((modNpc7 != null ? (((ModType)modNpc7).Name.Contains("FallenBeholder") ? 1 : 0) : 0) != 0)
                                            {
                                                npc.lifeMax *= 65;
                                            }
                                            else
                                            {
                                                ModNPC modNpc8 = npc.ModNPC;
                                                if ((modNpc8 != null ? (((ModType)modNpc8).Name.Contains("Lich") ? 1 : 0) : 0) != 0)
                                                {
                                                    npc.lifeMax *= 30;
                                                }
                                                else
                                                {
                                                    ModNPC modNpc9 = npc.ModNPC;
                                                    if ((modNpc9 != null ? (((ModType)modNpc9).Name.Contains("ForgottenOne") ? 1 : 0) : 0) != 0)
                                                    {
                                                        npc.lifeMax *= 15;
                                                    }
                                                    else
                                                    {
                                                        ModNPC modNpc10 = npc.ModNPC;
                                                        if ((modNpc10 != null ? (((ModType)modNpc10).Name.Contains("SlagFury") ? 1 : 0) : 0) != 0)
                                                        {
                                                            npc.lifeMax *= 2;
                                                        }
                                                        else
                                                        {
                                                            ModNPC modNpc11 = npc.ModNPC;
                                                            if ((modNpc11 != null ? (((ModType)modNpc11).Name.Contains("Aquaius") ? 1 : 0) : 0) != 0)
                                                            {
                                                                npc.lifeMax *= 2;
                                                            }
                                                            else
                                                            {
                                                                ModNPC modNpc12 = npc.ModNPC;
                                                                if ((modNpc12 != null ? (((ModType)modNpc12).Name.Contains("Omnicide") ? 1 : 0) : 0) != 0)
                                                                {
                                                                    npc.lifeMax *= 2;
                                                                }
                                                                else
                                                                {
                                                                    ModNPC modNpc13 = npc.ModNPC;
                                                                    if ((modNpc13 != null ? (((ModType)modNpc13).Name.Contains("DreamEater") ? 1 : 0) : 0) != 0)
                                                                    {
                                                                        npc.lifeMax *= 2;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ModNPC modNPC14 = npc.ModNPC;
                if ((modNPC14 != null ? (((ModType)modNPC14).Name.Contains("BoreanStrider") ? 1 : 0) : 0) != 0)
                {
                    npc.lifeMax *= 65;
                }
            }

            if (InfernumActive.InfernumActive)
            {
                npc.lifeMax += (int)(((double).35) * (double)npc.lifeMax);
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (InfernumActive.InfernumActive)
            {
                modifiers.SourceDamage *= 1.35f;
            }
        }

        public override void PostAI(NPC npc)
        {
            ModNPC modNPC14 = npc.ModNPC;
            if (InfernumActive.InfernumActive && !(((ModType)modNPC14).Name.Contains("BoreanStrider")))
            {
                npc.position += npc.velocity * 0.25f;
            }
        }
    }
}