using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;
using CalamityMod.Events;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;
using SOTS.NPCs.Boss;
using SOTS.NPCs.Boss.Advisor;
using CalamityMod.UI;
using InfernalEclipseAPI.Core.Systems;
using SOTS.NPCs.Boss.Glowmoth;
using System.Linq;
using SOTS.Projectiles.Earth.Glowmoth;
using InfernalEclipseAPI.Core.Utils;
using CalamityMod;
using InfernalEclipseAPI.Common.Globals.GlobalNPCs;
using SOTS.Projectiles.Chaos;
using SOTS.NPCs.Boss.Lux;

namespace InfernalEclipseAPI.Content.DifficultyOverrides
{
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod("SOTS")]
    public class SOTSBossStatScaling : GlobalNPC
    {
        public static readonly int[] sotsMinionTypes =
        {
            ModContent.NPCType<GlowmothMinion>(),
            ModContent.NPCType<PutridPinky1>(),
            ModContent.NPCType<PutridHook>(),
            ModContent.NPCType<FakeLux>(),
        };

        public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
        {
            return (npc.boss || sotsMinionTypes.Contains(npc.type)) && npc.ModNPC?.Mod.Name == "SOTS";
        }

        public override void SetDefaults(NPC entity)
        {
            if (entity.ModNPC.Name.Contains("Excavator") && InfernumActive.InfernumActive)
            {
                entity.defense += 5;
            }

            if (entity.type == ModContent.NPCType<GlowmothMinion>())
            {
                entity.Calamity().canBreakPlayerDefense = true;
            }

            if (entity.type == ModContent.NPCType<Glowmoth>() || entity.type == ModContent.NPCType<GlowmothMinion>())
            {
                entity.GetGlobalNPC<SOTSGlobalNPC>().canDoVoidDamage = true;
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
                        npc.lifeMax *= 7; //less because holy fuck its tanky in boss rush

                    npc.lifeMax += (int)(((double).25 * npc.lifeMax));
                }

                if (npc.ModNPC?.Name?.Contains("TheAdvisorHead") == true || npc.ModNPC.Name.Contains("Excavator") || npc.ModNPC.Name.Contains("Lux"))
                {
                    npc.lifeMax += (int)(0.25 * npc.lifeMax);
                }

                if (npc.type == ModContent.NPCType<PutridPinkyPhase2>())
                {
                    npc.lifeMax += (int)(((double).15) * npc.lifeMax);
                }
                else if (npc.type == ModContent.NPCType<PutridPinky1>())
                {
                    npc.lifeMax += 3 * npc.lifeMax;
                }
                else if (npc.type == ModContent.NPCType<PutridHook>())
                {
                    npc.lifeMax -= (int)(npc.lifeMax * 0.3);
                }
                else if (npc.ModNPC.Name.Contains("SubspaceSerpent"))
                {
                    npc.lifeMax += (int)(0.25f * npc.lifeMax);
                }
                else
                    npc.lifeMax += (int)(((double).35) * npc.lifeMax);
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (InfernumActive.InfernumActive)
            {
                if (npc.type == ModContent.NPCType<PutridPinkyPhase2>())
                {
                    modifiers.SourceDamage *= 1.20f;
                }
                else if (npc.type == ModContent.NPCType<PutridPinky1>())
                {
                    modifiers.SourceDamage *= 2.5f;
                }
                else modifiers.SourceDamage *= 1.35f;

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
                /*
                if (npc.type == ModContent.NPCType<PutridPinkyPhase2>())
                {
                    npc.position += npc.velocity * 0.3f;
                }
                else 
                */
                if (npc.type == ModContent.NPCType<PutridPinky1>())
                    return;

                if (npc.type == ModContent.NPCType<SubspaceSerpentHead>())
                    npc.position += npc.velocity * 0.25f;
                else
                    npc.position += npc.velocity * 0.35f;
            }
        }
    }

    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod("SOTS")]
    public class AdvisorDefenseReset : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        // This issue has been fixed in main Revengence+ - ..nevermind
        private bool scaledBossRushHP = false;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == ModContent.NPCType<TheAdvisorHead>();
        }

        public override bool PreAI(NPC npc)
        {
            if (!npc.boss && !BossHealthBarManager.BossExclusionList.Contains(npc.type))
                BossHealthBarManager.BossExclusionList.Add(npc.type);
            else if (npc.boss && BossHealthBarManager.BossExclusionList.Contains(npc.type))
                BossHealthBarManager.BossExclusionList.Remove(npc.type);
            return base.PreAI(npc);
        }

        public override void PostAI(NPC npc)
        {
            if (BossRushEvent.BossRushActive && !scaledBossRushHP)
            {
                npc.lifeMax += (int)(((double).25) * (double)npc.lifeMax);
                npc.life = npc.lifeMax;
                scaledBossRushHP = true;
                return;
            }

            int targetDefense = 24;

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

            npc.defense = targetDefense;
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (InfernumSaveSystem.InfernumModeEnabled)
                modifiers.SourceDamage *= 1.35f;
        }
    }

    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod("SOTS")]
    public class SOTSProjStatScaling : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            int[] types =
            {
                //Glowmoth
                ModContent.ProjectileType<WaveBall>(),
                ModContent.ProjectileType<GlowBombOrb>(),
                ModContent.ProjectileType<GlowBombShard>(),
                ModContent.ProjectileType<GlowSparkle>(),

                //Lux
                ModContent.ProjectileType<DogmaSphere>(),
                ModContent.ProjectileType<ChaosWave>(),
                ModContent.ProjectileType<ChaosStar>(),
                ModContent.ProjectileType<ChaosDart>(),
                ModContent.ProjectileType<ChaosDiamond>(),
                ModContent.ProjectileType<ChaosDiamondLaser>(),
                ModContent.ProjectileType<ChaosBall>(),
                ModContent.ProjectileType<ChaosDart2>(),
                ModContent.ProjectileType<ChaosEraser>(),
                ModContent.ProjectileType<ChaosEraser2>(),
                ModContent.ProjectileType<DogmaLaser>()
            };

            foreach (int type in types)
            {
                if (entity.type == type)
                    return true;
            }
            return false;
        }

        public override void SetDefaults(Projectile entity)
        {
            entity.GetGlobalProjectile<VoidDamageProjectile>().canDoVoidDamage = true;
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            float damageMod = 1f;

            if (InfernalUtilities.IsWorldLegendary())
            {
                damageMod *= 1.35f;
            }

            if (InfernalUtilities.IsInfernumActive() || InfernalUtilities.GetFargoDifficullty("MasochistMode"))
            {
                damageMod *= 2.2f;
            }
            else if (InfernalUtilities.GetFargoDifficullty("EternityMode"))
            {
                damageMod *= 1.675f;
            }
            else if (InfernalUtilities.GetCalDifficulty("death"))
            {
                damageMod *= 1.5f;
            }

            modifiers.SourceDamage *= damageMod;
        }
    }
}