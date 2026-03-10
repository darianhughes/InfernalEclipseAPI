using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;
using System.Reflection;
using Terraria.DataStructures;
using ThoriumMod.NPCs.BossBoreanStrider;
using ThoriumMod.NPCs.BossThePrimordials;
using ThoriumMod.Projectiles.Boss;
using System.Linq;
using InfernalEclipseAPI.Core.Systems;
using ThoriumRework.Projectiles;
using InfernalEclipseAPI.Content.Buffs;
using ThoriumMod.Projectiles.Enemy;
using ThoriumMod.NPCs.BossQueenJellyfish;
using InfernalEclipseAPI.Common.Globals.GlobalNPCs;
using ThoriumMod.NPCs.BossViscount;
using CalamityMod;
using Microsoft.Xna.Framework;
using System.Security.Policy;
using Terraria;

namespace InfernalEclipseAPI.Content.DifficultyOverrides.Thorium
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class ThoriumBossStatScaling : GlobalNPC
    {
        private static bool GetCalDifficulty(string diff)
        {
            return ModLoader.TryGetMod("CalamityMod", out Mod calamity) &&
                   calamity.Call("GetDifficultyActive", diff) is bool b && b;
        }

        private static bool IsInfernumActive()
        {
            return InfernumSaveSystem.InfernumModeEnabled;
        }

        private static bool GetFargoDifficullty(string diff)
        {
            if (!ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSouls))
            {
                return false;
            }

            return fargoSouls.Call(diff) is bool active && active;
        }
        private static bool IsWorldLegendary()
        {
            FieldInfo findInfo = typeof(Main).GetField("_currentGameModeInfo", BindingFlags.Static | BindingFlags.NonPublic);
            GameModeData data = (GameModeData)findInfo.GetValue(null);
            return Main.getGoodWorld && data.IsMasterMode;
        }

        public static readonly int[] thorBossMinionTypes =
        [
            ModContent.NPCType<ThoriumMod.NPCs.BossThePrimordials.AquaiusBubble>(),
            ModContent.NPCType<ThoriumMod.NPCs.BossThePrimordials.ImpendingDread>(),
            ModContent.NPCType<UnstableAnger>(),
            ModContent.NPCType<InnerDespair>(),
            ModContent.NPCType<LucidBubble>(),
            ModContent.NPCType<BoreanHopper>(),
            ModContent.NPCType<BoreanMyte>(),
            ModContent.NPCType<DistractingJellyfish>(),
            ModContent.NPCType<SpittingJellyfish>(),
            ModContent.NPCType<ZealousJellyfish>(),
            ModContent.NPCType<ThoriumMod.NPCs.BossViscount.BiteyBaby>(),
        ];

        public static bool IsReworkNPC(NPC npc)
        {
            if (!InfernalCrossmod.ThoriumRework.Loaded) return false;

            return ThoriumReworkEntities.IsReworkedThoriumMinion(npc);
        }

        public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
        {
            return (npc.boss || thorBossMinionTypes.Contains(npc.type) || IsReworkNPC(npc)) && (npc.ModNPC?.Mod?.Name == "ThoriumMod" || npc.ModNPC?.Mod?.Name == "ThoriumRework");
        }

        public override void SetDefaults(NPC entity)
        {
            if (IsInfernumActive()) 
            {
                if (entity.type == ModContent.NPCType<BoreanStrider>())
                {
                    entity.defense = 125;
                }
                if (entity.type == ModContent.NPCType<BoreanStriderPopped>())
                {
                    entity.defense = 20;
                }
                if (entity.type == ModContent.NPCType<BoreanHopper>())
                {
                    entity.defense = 125;
                }
            }

            if (InfernalCrossmod.SOTS.Loaded)
            {
                if (entity.type == ModContent.NPCType<Viscount>() || entity.type == ModContent.NPCType<ThoriumMod.NPCs.BossViscount.BiteyBaby>())
                {
                    entity.GetGlobalNPC<SOTSGlobalNPC>().canDoVoidDamage = true;
                }

                if (entity.type == ModContent.NPCType<DreamEater>())
                {
                    entity.GetGlobalNPC<SOTSGlobalNPC>().canDoVoidDamage = true;
                    entity.GetGlobalNPC<SOTSGlobalNPC>().strongVoidDamge = true;
                }
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            //Boss Rush, 
            if (GetCalDifficulty("bossrush"))
            {
                string name = npc.ModNPC?.Name ?? "";

                //do this 
                if (name.Contains("BoreanStrider"))
                    npc.lifeMax *= 25; //less due to how much it already has

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
                if (npc.type == ModContent.NPCType<BoreanStrider>() || npc.type == ModContent.NPCType<BoreanStriderPopped>())
                {
                    npc.lifeMax += (int)(npc.lifeMax * 1.9f);
                }
                if (npc.ModNPC?.Name?.Contains("GraniteEnergyStorm") == true || npc.ModNPC?.Name?.Contains("BuriedChampion") == true || npc.ModNPC.Name.Contains("QueenJellyfish"))
                {
                    npc.lifeMax += npc.lifeMax;
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
                    if (npc.type == ModContent.NPCType<BoreanStrider>() || npc.type == ModContent.NPCType<BoreanStriderPopped>())
                    {
                        npc.lifeMax += (int)(npc.lifeMax * 1.65f);
                    }
                    if (npc.ModNPC?.Name?.Contains("GraniteEnergyStorm") == true || npc.ModNPC?.Name?.Contains("BuriedChampion") == true || npc.ModNPC.Name.Contains("QueenJellyfish"))
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
                    if (npc.type == ModContent.NPCType<BoreanStrider>() || npc.type == ModContent.NPCType<BoreanStriderPopped>())
                    {
                        npc.lifeMax += (int)(npc.lifeMax * 1.4f);
                    }
                    if (npc.ModNPC?.Name?.Contains("GraniteEnergyStorm") == true || npc.ModNPC?.Name?.Contains("BuriedChampion") == true || npc.ModNPC.Name.Contains("QueenJellyfish"))
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
                    if (npc.type == ModContent.NPCType<BoreanStrider>() || npc.type == ModContent.NPCType<BoreanStriderPopped>())
                    {
                        npc.lifeMax += (int)(npc.lifeMax * 1.15f);
                    }
                    if (npc.ModNPC?.Name?.Contains("GraniteEnergyStorm") == true || npc.ModNPC?.Name?.Contains("BuriedChampion") == true || npc.ModNPC.Name.Contains("QueenJellyfish"))
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

            if (name.Contains("SlagFury") || name.Contains("Aquaius") || name.Contains("Omnicide") || name.Contains("DreamEater") || name.Contains("BoreanStrider") || name.Contains("QueenJellyfish") || name.Contains("Viscount"))
                damageMod += 0.6f;
            else if (name.Contains("BiteyBaby"))
                damageMod += 1f;

            if (IsWorldLegendary())
            {
                damageMod += 1.1f;
            }
            if (IsInfernumActive() || GetFargoDifficullty("MasochistMode"))
            {
                damageMod += 1.35f;
            }
            else
            {
                if (GetFargoDifficullty("EternityMode"))
                {
                    damageMod += 1.25f;
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
            if (npc.ModNPC?.Name?.Contains("BoreanStrider") == true)
            {
                foreach (Player player in Main.player)
                {
                    if (player.active && !player.dead && npc.WithinRange(player.Center, 1000f))
                    {
                        player.AddBuff(ModContent.BuffType<LowGround>(), 1);
                    }
                }
                return;
            }

            if (npc.ModNPC.Name.Contains("FallenBeholder"))
                return;

            if (IsWorldLegendary())
            {
                npc.position += npc.velocity * 0.5f;
            }
            if (IsInfernumActive() || GetFargoDifficullty("MasochistMode"))
            {
                if (npc.ModNPC.Name.Contains("TheGrandThunderBird"))
                    npc.position += npc.velocity * 0.15f;
                else
                    npc.position += npc.velocity * 0.20f;
            }
            else
            {
                if (GetFargoDifficullty("EternityMode"))
                {
                    if (npc.ModNPC.Name.Contains("TheGrandThunderBird"))
                        npc.position += npc.velocity * 0.05f;
                    else
                        npc.position += npc.velocity * 0.10f;
                }
                else if (GetCalDifficulty("death"))
                {
                    if (!npc.ModNPC.Name.Contains("TheGrandThunderBird"))
                        npc.position += npc.velocity * 0.05f;
                }
            }
        }
    }

    [JITWhenModsEnabled("ThoriumRework")]
    [ExtendsFromMod("ThoriumRework")]
    public static class ThoriumReworkEntities
    {
        public static bool IsReworkedThoriumMinion(NPC npc)
        {
            return false;

            int[] reworkType =
            [
            ];

            if (reworkType.Contains(npc.type))
                return true;

            return false;
        }

        public static bool IsReworkedThoriumProjectile(Projectile projectile)
        {
            int[] reworkType =
            [
                ModContent.ProjectileType<ThoriumRework.Projectiles.ImpendingDread>(),
                ModContent.ProjectileType<ImpendingDreadF>(),
                ModContent.ProjectileType<ThoriumRework.Projectiles.LucidRay>(),
                ModContent.ProjectileType<LucidNuke>(),
                ModContent.ProjectileType<ThoriumRework.Projectiles.AquaiusBubble>(),
                ModContent.ProjectileType<AquaiusPunchAttack>(),
                ModContent.ProjectileType<DeathRain>(),
                ModContent.ProjectileType<InfernalRay>(),

                ModContent.ProjectileType<IceShard>(),
                ModContent.ProjectileType<Glacier>(),
                ModContent.ProjectileType<Glacier2>(),

                ModContent.ProjectileType<DancingJellyfish>(),
                ModContent.ProjectileType<ThoriumRework.Projectiles.BubbleBomb>(),
                ModContent.ProjectileType<HighTide>(),
                ModContent.ProjectileType<HighTideWave>(),
                ModContent.ProjectileType<BubbleColumn>(),
                ModContent.ProjectileType<JammingJellyfish>(),
                ModContent.ProjectileType<JellyfishShock>(),

                ModContent.ProjectileType<ThoriumRework.Projectiles.BiteyBaby>()
            ];

            foreach (int type in reworkType)
            {
                if (projectile.type == type)
                    return true;
            }

            return false;
        }
    }


    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class ThoriumBossProjStatScaling : GlobalProjectile
    {
        public static bool IsReworkNPC(Projectile projectile)
        {
            if (!InfernalCrossmod.ThoriumRework.Loaded) return false;

            return ThoriumReworkEntities.IsReworkedThoriumProjectile(projectile);
        }

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            int[] types =
            [
                //Primordials
                ModContent.ProjectileType<AquaSplash>(),
                ModContent.ProjectileType<AquaTyphoon>(),
                ModContent.ProjectileType<LucidFury>(),
                ModContent.ProjectileType<LucidMiasma>(),
                ModContent.ProjectileType<LucidPulse>(),
                ModContent.ProjectileType<LucidTyphoon>(),
                ModContent.ProjectileType<FlameFury>(),
                ModContent.ProjectileType<FlameLash>(),
                ModContent.ProjectileType<FlameNova>(),
                ModContent.ProjectileType<FlamePulsePro>(),
                ModContent.ProjectileType<AquaBomb2>(),
                ModContent.ProjectileType<LucidBomb2>(),
                ModContent.ProjectileType<ThoriumMod.Projectiles.Boss.LucidRay>(),
                ModContent.ProjectileType<DeathRaySpawn3>(),
                ModContent.ProjectileType<DeathCircle2>(),
                ModContent.ProjectileType<DeathRay>(),

                //Borean Strider
                ModContent.ProjectileType<BlizzardFang>(),
                ModContent.ProjectileType<BlizzardBoom>(),
                ModContent.ProjectileType<IceAnomaly>(),

                //Queen Jellyfish
                ModContent.ProjectileType<BubblePulse>(),
                ModContent.ProjectileType<ThoriumMod.Projectiles.Boss.BubbleBomb>(),

                //Viscount
                ModContent.ProjectileType<ViscountBlood>(),
                ModContent.ProjectileType<ViscountRipple>(),
                ModContent.ProjectileType<ViscountRipple2>(),
                ModContent.ProjectileType<ViscountRipple3>(),
                ModContent.ProjectileType<ViscountRockFall>(),
                ModContent.ProjectileType<ViscountRockSummon>(),
                ModContent.ProjectileType<ViscountRockSummon>(),
                ModContent.ProjectileType<ViscountStomp>(),
                ModContent.ProjectileType<ViscountStomp2>(),             
            ];

            foreach (int type in types)
            {
                if (entity.type == type || IsReworkNPC(entity))
                    return true;
            }
            return false;
        }

        private static bool GetCalDifficulty(string diff)
        {
            return ModLoader.TryGetMod("CalamityMod", out Mod calamity) &&
                   calamity.Call("GetDifficultyActive", diff) is bool b && b;
        }

        private static bool IsInfernumActive()
        {
            return InfernumSaveSystem.InfernumModeEnabled;
        }

        private static bool GetFargoDifficullty(string diff)
        {
            if (!ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSouls))
            {
                return false;
            }

            return fargoSouls.Call(diff) is bool active && active;
        }
        private static bool IsWorldLegendary()
        {
            FieldInfo findInfo = typeof(Main).GetField("_currentGameModeInfo", BindingFlags.Static | BindingFlags.NonPublic);
            GameModeData data = (GameModeData)findInfo.GetValue(null);
            return Main.getGoodWorld && data.IsMasterMode;
        }

        public override void SetDefaults(Projectile entity)
        {
            if (InfernalCrossmod.SOTS.Loaded)
            {
                if (entity.ModProjectile.Name.Contains("Blood") || entity.ModProjectile.Name.Contains("BiteyBaby") || entity.ModProjectile.Name.Contains("Ripple"))
                {
                    entity.GetGlobalProjectile<VoidDamageProjectile>().canDoVoidDamage = true;
                }
                if (entity.ModProjectile.Name.Contains("Lucid"))
                {
                    entity.GetGlobalProjectile<VoidDamageProjectile>().canDoVoidDamage = true;
                    entity.GetGlobalProjectile<VoidDamageProjectile>().strongVoidDamge = true;
                }
            }
            if ((entity.ModProjectile.Name.Contains("Viscount") && !entity.ModProjectile.Name.Contains("Rock") && !entity.ModProjectile.Name.Contains("Stomp")) || 
                (entity.ModProjectile.Name.Contains("Blizzard") || entity.ModProjectile.Name.Contains("Ice") || entity.ModProjectile.Name.Contains("Glacier")))
                entity.Calamity().DealsDefenseDamage = false;
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            float damageMod = 1f;
            if (projectile.ModProjectile.Name.Contains("BiteyBaby"))
            {
                damageMod += 0.25f;
            }
            if (IsWorldLegendary())
            {
                damageMod *= 1.35f;
            }

            if (IsInfernumActive() || GetFargoDifficullty("MasochistMode"))
            {
                if (projectile.ModProjectile.Name.Contains("Viscount") && !projectile.ModProjectile.Name.Contains("Rock") && !projectile.ModProjectile.Name.Contains("Stomp"))
                    damageMod *= 1.70f;
                else if (projectile.ModProjectile.Name.Contains("Blizzard") || projectile.ModProjectile.Name.Contains("Ice") || projectile.ModProjectile.Name.Contains("Glacier"))
                    damageMod *= 1.35f;
                else
                    damageMod *= 2.2f;
            }
            else if (GetFargoDifficullty("EternityMode"))
            {
                if (projectile.ModProjectile.Name.Contains("Viscount") && !projectile.ModProjectile.Name.Contains("Rock") && !projectile.ModProjectile.Name.Contains("Stomp"))
                    damageMod *= 1.60f;
                else if (projectile.ModProjectile.Name.Contains("Blizzard") || projectile.ModProjectile.Name.Contains("Ice") || projectile.ModProjectile.Name.Contains("Glacier"))
                    damageMod *= 1.25f;
                else
                    damageMod *= 1.675f;
            }
            else if (GetCalDifficulty("death"))
            {
                if (projectile.ModProjectile.Name.Contains("Viscount") && !projectile.ModProjectile.Name.Contains("Rock") && !projectile.ModProjectile.Name.Contains("Stomp"))
                    damageMod *= 1.45f;
                else if (projectile.ModProjectile.Name.Contains("Blizzard") || projectile.ModProjectile.Name.Contains("Ice") || projectile.ModProjectile.Name.Contains("Glacier"))
                    damageMod *= 1.1f;
                else
                    damageMod *= 1.15f;
            }

            modifiers.SourceDamage *= damageMod;
        }
    }

    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class ViscountBloodAccelerationGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private static int ViscountBloodType = -1;

        private bool initialized;
        private Vector2 storedDirection;
        private float targetSpeed;
        private float timer;

        private float StartingSpeedMultiplier = 0.005f;
        private float AccelerationTime = 40f;

        public override void Load()
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                thorium.TryFind<ModProjectile>("ViscountBlood", out var viscountBlood))
            {
                ViscountBloodType = viscountBlood.Type;
            }
        }

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ViscountBloodType;
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            /*
            if (Main.netMode != NetmodeID.Server)
                Main.NewText($"ViscountBlood matched: {projectile.type}");
            */

            timer = 0f;
            initialized = false;

            float originalSpeed = projectile.velocity.Length();
            if (originalSpeed <= 0.001f)
            {
                storedDirection = Vector2.UnitY;
                targetSpeed = 0f;
                return;
            }

            storedDirection = Vector2.Normalize(projectile.velocity);
            targetSpeed = originalSpeed;

            projectile.velocity = storedDirection * (targetSpeed * StartingSpeedMultiplier);
            initialized = true;
        }

        public override void AI(Projectile projectile)
        {
            if (!initialized || targetSpeed <= 0f)
                return;

            timer++;

            float progress = Utils.GetLerpValue(0f, AccelerationTime, timer, true);
            progress *= progress;

            float currentSpeed = MathHelper.Lerp(
                targetSpeed * StartingSpeedMultiplier,
                targetSpeed,
                progress
            );

            projectile.velocity = storedDirection * currentSpeed;
        }
    }
}