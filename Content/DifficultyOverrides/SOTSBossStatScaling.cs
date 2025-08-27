using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;
using Terraria.ID;
using CalamityMod.Events;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;

namespace InfernalEclipseAPI.Content.DifficultyOverrides
{
    public class SOTSBossStatScaling : GlobalNPC
    {
        public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
        {
            return npc.boss && ((ModType)npc.ModNPC)?.Mod.Name == "SOTS";
        }

        public override void SetDefaults(NPC entity)
        {
            if (entity.ModNPC.Name.Contains("Excavator"))
            {
                entity.defense += 5;
            }
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

            if (InfernumActive.InfernumActive)
            {
                //Boss Rush Boost
                if ((num1 & num2) != 0)
                {
                    string name = npc.ModNPC?.Name ?? "";
                    if (name.Contains("Excavator"))
                        npc.lifeMax *= 105;

                    npc.lifeMax += (int)(((double).25 * (double)npc.lifeMax));
                }

                if (npc.ModNPC?.Name?.Contains("TheAdvisorHead") == true || npc.ModNPC.Name.Contains("Excavator"))
                {
                    npc.lifeMax += (int)(0.25 * npc.lifeMax);
                }

                npc.lifeMax += (int)(((double).35) * (double)npc.lifeMax);
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (InfernumActive.InfernumActive)
            {
                modifiers.SourceDamage *= 1.35f;

                if (npc.ModNPC.Name.Contains("Excavator"))
                {
                    modifiers.SourceDamage *= 0.25f;
                }
            }
        }

        public override void PostAI(NPC npc)
        {
            if (InfernumActive.InfernumActive)
            {
                npc.position += npc.velocity * 0.35f;
            }
        }

        public class AdvisorDefenseReset : GlobalNPC
        {
            public override bool InstancePerEntity => true;
            // This issue has been fixed in main Revengence+ - ..nevermind
            private bool scaledBossRushHP = false;
            public override void PostAI(NPC npc)
            {
                // 1) Only target SOTS's TheAdvisorHead
                if (npc.ModNPC is not ModNPC modNpc
                    || modNpc.Mod.Name != "SOTS"
                    || modNpc.Name != "TheAdvisorHead")
                {
                    return;
                }

                // 2) If Calamity Boss Rush is active, do nothing
                if (BossRushEvent.BossRushActive && !scaledBossRushHP)
                {
                    npc.lifeMax += (int)(((double).25) * (double)npc.lifeMax);
                    npc.life = npc.lifeMax;
                    scaledBossRushHP = true;
                    return;
                }

                // 3) Default to base SOTS defense
                int targetDefense = 24;

                // 4) If Calamity is loaded, ask it which alternate difficulty is active
                if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
                {
                    // These calls must match Calamity's internal names exactly:
                    object isDeath = calamity.Call("GetDifficultyActive", "Death");
                    object isRevenge = calamity.Call("GetDifficultyActive", "Revengeance");

                    if (isDeath is bool bDeath && bDeath)
                    {
                        // Death Mode
                        npc.position += npc.velocity * 0.35f;
                        targetDefense = 39;
                    }
                    else if (isRevenge is bool bRev && bRev)
                    {
                        // Revengeance Mode
                        npc.position += npc.velocity * 0.25f;
                        targetDefense = 32;
                    }
                }

                // 5) Clamp it on the NPC
                npc.defense = targetDefense;
            }

            public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
            {
                if (npc.ModNPC is not ModNPC modNpc
                                   || modNpc.Mod.Name != "SOTS"
                                   || modNpc.Name != "TheAdvisorHead")
                {
                    return;
                }

                if (InfernumSaveSystem.InfernumModeEnabled)
                    modifiers.SourceDamage *= 1.35f;
            }
        }
    }
}