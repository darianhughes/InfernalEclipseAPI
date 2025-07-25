using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;
using Steamworks;
using CalamityMod.Enums;
using CalamityMod.Sounds;
using CalamityMod;
using InfernalEclipseAPI.Content.Projectiles;

namespace InfernalEclipseAPI.Content.RogueThrower.StealthStrikes
{
    public class StealthStrikeGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool isStealthStrike = false;
        public StealthStrikeType stealthType = StealthStrikeType.None;
        private bool appliedChanges = false;

        private static HashSet<int> stealthCompatibleProjectiles;

        //BURST VARIABLES
        private int burstShotsFired = 0;
        private int burstTimer = 0;
        private const int burstDelayTicks = 5;
        private int initialDelay = 5;
        private bool buffApplied = false;
        private float initialSpeed = 0f;

        // GEL GLOVE specific variables
        private int gelGloveShurikenTimer = 0;

        public void SetupAsStealthStrike(StealthStrikeType type)
        {
            isStealthStrike = true;
            stealthType = type;
        }

        //SPAWN PROJECTILE INHERETING
        static StealthStrikeGlobalProjectile()
        {
            stealthCompatibleProjectiles = new HashSet<int>();

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                TryAdd(thorium, "ValadiumBattleAxePro");
                TryAdd(thorium, "MagicCardPro");
                TryAdd(thorium, "SoulslasherPro");
                TryAdd(thorium, "LodestoneStaffPro");
                TryAdd(thorium, "LodestoneStaffPro2");
                TryAdd(thorium, "LodestoneStaffPro3");
                TryAdd(thorium, "LodestoneStaffPro4");
                TryAdd(thorium, "LodestoneStaffPro5");
                TryAdd(thorium, "ClockWorkBombPro1");
                TryAdd(thorium, "ClockWorkBombPro2");
                TryAdd(thorium, "ClockWorkBombPro3");
            }
        }

        private static void TryAdd(Mod mod, string name)
        {
            if (mod.TryFind(name, out ModProjectile proj))
            {
                stealthCompatibleProjectiles.Add(proj.Type);
            }
        }

        private bool ShouldInheritStealth(int type)
        {
            return stealthCompatibleProjectiles.Contains(type);
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_Parent parentSource &&
                parentSource.Entity is Projectile parentProj &&
                parentProj.TryGetGlobalProjectile(out StealthStrikeGlobalProjectile parentStealth) &&
                parentStealth.isStealthStrike &&
                ShouldInheritStealth(projectile.type))
            {
                isStealthStrike = true;
                stealthType = parentStealth.stealthType;

                // Prevent Lodestone re-triggering the cascade
                if (stealthType == StealthStrikeType.LodestoneJavelin)
                {
                    if (parentStealth.cameFromLodestoneStealth)
                    {
                        projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
                        cameFromLodestoneStealth = true;
                        return;
                    }
                }

                // PLAYING CARD TRAIL SETUP
                if (stealthType == StealthStrikeType.PlayingCard)
                {
                    projectile.oldPos = new Vector2[10]; // Trail length
                    ProjectileID.Sets.TrailingMode[projectile.type] = 0;
                    projectile.extraUpdates = 1;
                }

                // SOULSLASHER behavior
                if (stealthType == StealthStrikeType.Soulslasher)
                {
                    projectile.extraUpdates += 1;
                    projectile.localNPCHitCooldown = 5;
                    projectile.usesLocalNPCImmunity = true;
                }

                //CLOCKWORK BOMB
                if (stealthType == StealthStrikeType.ClockworkBomb)
                {
                    isStealthStrike = true;
                    stealthType = StealthStrikeType.ClockworkBomb;
                }

                // VALADIUM LOCAL IMMUNITY
                if (parentStealth.cameFromValadiumStealth)
                {
                    cameFromValadiumStealth = true;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                }
            }
        }

        //SPRITE STUFF
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (!isStealthStrike && stealthType != StealthStrikeType.PlayingCard)
                return true;

            if (projectile.ModProjectile != null && projectile.ModProjectile.Mod.Name == "ThoriumMod")
            {
                Texture2D texture = TextureAssets.Projectile[projectile.type].Value;

                int frameCount = Main.projFrames[projectile.type];
                int frameHeight = texture.Height / frameCount;

                Rectangle sourceRectangle = new Rectangle(0, frameHeight * projectile.frame, texture.Width, frameHeight);
                Vector2 origin = sourceRectangle.Size() / 2f;

                // Draw trail
                if (stealthType == StealthStrikeType.PlayingCard && projectile.oldPos != null)
                {
                    for (int i = 0; i < projectile.oldPos.Length; i++)
                    {
                        Vector2 oldDrawPos = projectile.oldPos[i] + projectile.Size / 2f - Main.screenPosition;
                        float opacity = (projectile.oldPos.Length - i) / (float)projectile.oldPos.Length;
                        Color trailColor = lightColor * opacity * 0.5f;

                        Main.EntitySpriteDraw(
                            texture,
                            oldDrawPos,
                            sourceRectangle,
                            trailColor,
                            projectile.rotation,
                            origin,
                            projectile.scale,
                            SpriteEffects.None,
                            0
                        );
                    }
                }

                // Draw main projectile
                Vector2 drawPos = projectile.Center - Main.screenPosition;
                Color drawColor = lightColor * ((255 - projectile.alpha) / 255f);

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    sourceRectangle,
                    drawColor,
                    projectile.rotation,
                    origin,
                    projectile.scale,
                    SpriteEffects.None,
                    0
                );

                return false;
            }

            return true;
        }

        //ON HIT
        public bool soulslasherAggressiveHoming = false;
        public bool cameFromLodestoneStealth = false;
        public bool cameFromValadiumStealth = false;
        public bool alreadyTriggeredOnce = false;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!isStealthStrike)
                return;

            //WDC
            if (stealthType == StealthStrikeType.WhiteDwarfCutter)
            {
                if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                    thorium.TryFind("WhiteFlare", out ModProjectile whiteFlareMod))
                {
                    int whiteFlareType = whiteFlareMod.Type;

                    float damagePercent = 0.0003f; // 0.03% of max HP
                    int flareDamage = Math.Max(1, (int)(target.lifeMax * damagePercent));

                    int flareID = Projectile.NewProjectile(
                        projectile.GetSource_OnHit(target),
                        target.Center,
                        Vector2.Zero,
                        whiteFlareType,
                        flareDamage,
                        0f,
                        projectile.owner
                    );

                    SoundEngine.PlaySound(SoundID.Item92, target.Center);
                }
            }

            //Soul Slasher
            if (isStealthStrike && stealthType == StealthStrikeType.Soulslasher)
            {
                soulslasherAggressiveHoming = true;
            }

            //SSS
            if (stealthType == StealthStrikeType.SoftServeSunderer)
            {
                if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                    thorium.TryFind("SoftServeSundererPro", out ModProjectile sssProj))
                {
                    int projType = sssProj.Type;

                    // Store target position
                    Vector2 targetCenter = target.Center;

                    // Delay spawns using a loop with different spawn delays
                    for (int i = 0; i < 30; i++)
                    {
                        Vector2 spawnPos = new Vector2(
                            Main.rand.Next((int)targetCenter.X - 800, (int)targetCenter.X + 800),
                            Main.rand.Next((int)targetCenter.Y - Main.screenHeight + 200, (int)targetCenter.Y - Main.screenHeight + 800)
                        );

                        Vector2 direction = Vector2.Normalize(targetCenter - spawnPos) * 14f;

                        int projID = Projectile.NewProjectile(
                            new EntitySource_Misc("SoftServeStealthStrike"),
                            spawnPos,
                            direction,
                            projType,
                            projectile.damage,
                            0f,
                            projectile.owner,
                            ai0: 0f,
                            ai1: i * 2f // Delay using AI slot
                        );

                        Main.projectile[projID].localAI[0] = 1f; // Set homing flag here
                    }

                    SoundEngine.PlaySound(SoundID.Item74, targetCenter);
                }
            }

            //Terra Knife
            if (stealthType == StealthStrikeType.TerraKnife)
            {
                int projType = ModContent.ProjectileType<TerratomereSlashCreator>();

                Projectile.NewProjectile(new EntitySource_Misc("TerraKnifeStealthStrike"), target.Center, Vector2.Zero, projType, projectile.damage * 2, 0f, projectile.owner, target.whoAmI, Main.rand.NextFloat(MathHelper.TwoPi), 1);
            }

            //Lodestone Javelin
            if (stealthType == StealthStrikeType.LodestoneJavelin && !cameFromLodestoneStealth)
            {
                if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                    thorium.TryFind("LodestoneStaffPro", out ModProjectile cascadeProj))
                {
                    int cascadeType = cascadeProj.Type;

                    Vector2 spawnPos = target.Center;

                    int damage = (int)Math.Round(projectile.damage * 0.5f);

                    int projID = Projectile.NewProjectile(
                        projectile.GetSource_OnHit(target),
                        spawnPos,
                        Vector2.Zero,
                        cascadeType,
                        damage,
                        0f,
                        projectile.owner
                    );

                    if (Main.projectile.IndexInRange(projID))
                    {
                        Projectile newProj = Main.projectile[projID];

                        if (newProj.TryGetGlobalProjectile(out StealthStrikeGlobalProjectile spawnedStealth))
                        {
                            spawnedStealth.isStealthStrike = true;
                            spawnedStealth.stealthType = StealthStrikeType.LodestoneJavelin;
                            spawnedStealth.cameFromLodestoneStealth = true;
                        }

                        newProj.DamageType = ModContent.GetInstance<RogueDamageClass>();

                        newProj.localAI[0] = 45f;

                        // Optional: sync the projectile to clients if needed
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projID);
                    }
                }
            }

            //Valadium Throwing Axe
            if (stealthType == StealthStrikeType.ValadiumAxe && !alreadyTriggeredOnce && !cameFromValadiumStealth)
            {
                if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                    thorium.TryFind("ValadiumBattleAxePro", out ModProjectile axeProj))
                {
                    int axeType = axeProj.Type;
                    Vector2 center = target.Center;

                    int damage = (int)Math.Round(projectile.damage * 0.4f);

                    for (int i = 0; i < 8; i++)
                    {
                        float angle = MathHelper.TwoPi * i / 8f;
                        Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 128f;
                        Vector2 spawnPos = center + offset;
                        Vector2 velocity = Vector2.Normalize(target.Center - spawnPos) * 10f;

                        int id = Projectile.NewProjectile(
                            projectile.GetSource_OnHit(target),
                            spawnPos,
                            velocity,
                            axeType,
                            damage,
                            0f,
                            projectile.owner
                        );

                        if (Main.projectile.IndexInRange(id))
                        {
                            Projectile newAxe = Main.projectile[id];

                            if (newAxe.TryGetGlobalProjectile(out StealthStrikeGlobalProjectile axeStealth))
                            {
                                axeStealth.isStealthStrike = true;
                                axeStealth.stealthType = StealthStrikeType.ValadiumAxe;
                                axeStealth.cameFromValadiumStealth = true;
                            }

                            newAxe.DamageType = ModContent.GetInstance<RogueDamageClass>();

                            // **Apply local immunity immediately here:**
                            newAxe.usesLocalNPCImmunity = true;
                            newAxe.localNPCHitCooldown = 10;

                            // Spawn glowing dust poof effect like Thorium's ValadiumBattleAxePro
                            for (int k = 0; k < 8; k++)
                            {
                                int dust = Dust.NewDust(
                                    newAxe.position,
                                    newAxe.width,
                                    newAxe.height,
                                    62, // glowing dust type
                                    Main.rand.NextFloat(-3f, 3f),
                                    Main.rand.NextFloat(-3f, 3f),
                                    75,
                                    default(Color),
                                    1.5f
                                );
                                Main.dust[dust].noGravity = true;
                            }

                            for (int j = 0; j < 5; j++)
                            {
                                int dust2 = Dust.NewDust(
                                    newAxe.position,
                                    newAxe.width,
                                    newAxe.height,
                                    DustID.Obsidian, // secondary dust for color variation
                                    Main.rand.NextFloat(-2f, 2f),
                                    Main.rand.NextFloat(-2f, 2f),
                                    75,
                                    default(Color),
                                    1f
                                );
                                Main.dust[dust2].noGravity = true;
                            }
                        }

                        SoundEngine.PlaySound(SoundID.Item8, center);
                    }
                }

                // Prevent this from happening again
                alreadyTriggeredOnce = true;
            }
        }

        //SETDEFAULTS OVERIDES
        public override void SetDefaults(Projectile entity)
        {
            //DAMAGE OVERRIDE FOR TERRATOMERE SLASHES
            if (entity.type == ModContent.ProjectileType<TerratomereSlash>())
            {
                if (entity.ai[0] == 1)
                    entity.DamageType = ModContent.GetInstance<RogueDamageClass>();
            }

            //DAMAGE OVERRIDE FOR SHURIKEN DURING STEALTH STRIKE
            //if (entity.type == ProjectileID.Shuriken)
            //{
            //    if (entity.ai[0] == 7)
            //        entity.DamageType = DamageClass.Throwing; //scales with rouge damage bonuses but not stealth
            //}
        }

        //AI
        public override void AI(Projectile projectile)
        {
            //AI OVERIDE FOR TERRATOMERE SLASHES
            if (projectile.type == ModContent.ProjectileType<TerratomereSlashCreator>())
            {
                float slashDirection;
                if (projectile.ai[1] > MathHelper.Pi)
                    slashDirection = Main.rand.NextFloatDirection();
                else
                 slashDirection = projectile.ai[1] + Main.rand.NextFloatDirection() * 0.2f;
                NPC target = Main.npc[(int)projectile.ai[0]];

                if (projectile.ai[2] == 1)
                {
                    if (projectile.timeLeft % Terratomere.SmallSlashCreationRate == 0)
                    {
                        SoundEngine.PlaySound(CommonCalamitySounds.SwiftSliceSound, projectile.Center);
                        if (Main.myPlayer == projectile.owner)
                        {
                            float maxOffset = target.width * 0.4f;
                            if (maxOffset > 300f)
                                maxOffset = 300f;

                            Vector2 spawnOffset = slashDirection.ToRotationVector2() * Main.rand.NextFloatDirection() * maxOffset;
                            Vector2 sliceVelocity = spawnOffset.SafeNormalize(Vector2.UnitY) * 0.1f;

                            Projectile.NewProjectile(projectile.GetSource_FromThis(), target.Center + spawnOffset, sliceVelocity, ModContent.ProjectileType<TerratomereSlash>(), (int)(projectile.damage * Terratomere.SmallSlashDamageFactor), 0f, projectile.owner, 1);
                        }
                    }
                }
            }


            if (!isStealthStrike) return;

            if (!appliedChanges)
            {
                appliedChanges = true;

                //ICY TOMAHAWK PEN AND LIFE CHANGES
                if (stealthType == StealthStrikeType.IcyTomahawk)
                {
                    if (projectile.penetrate < 8 || projectile.penetrate == -1)
                        projectile.penetrate = 8;

                    if (projectile.timeLeft < 240)
                        projectile.timeLeft = 240;
                }

                //CLOCKWORK SIZE AND LIFETIME CHANGES
                if (stealthType == StealthStrikeType.ClockworkBomb)
                {
                    // Skip scaling for original ClockworkBombPro projectile
                    if (projectile.ModProjectile != null && projectile.ModProjectile.Name == "ClockWorkBombPro")
                    {
                        // Don't apply scaling or lifetime changes to the original projectile
                        return;
                    }

                    float scaleFactor = 3f;

                    // Capture the current center so we can preserve it
                    Vector2 center = projectile.Center;

                    // Resize hitbox
                    projectile.width = (int)(projectile.width * scaleFactor);
                    projectile.height = (int)(projectile.height * scaleFactor);

                    // Recenter the projectile to keep visual alignment
                    projectile.Center = center;

                    // Visual scale — this scales rendering *only*
                    projectile.scale *= scaleFactor;

                    // Make it last longer
                    projectile.timeLeft = 120;
                }
                //FADE TIMER
                if (stealthType == StealthStrikeType.ClockworkBomb)
                {
                    int fadeDuration = 120;

                    if (projectile.timeLeft < fadeDuration)
                    {
                        projectile.alpha = (int)(255f * (fadeDuration - projectile.timeLeft) / fadeDuration);
                        if (projectile.alpha > 255)
                            projectile.alpha = 255;
                    }
                    else
                    {
                        projectile.alpha = 0;
                    }
                }

                //WHITE FLARE
                if (projectile.ModProjectile != null && projectile.ModProjectile.Mod.Name == "ThoriumMod" && projectile.ModProjectile.Name == "WhiteFlare")
                {
                    // Shrink stealth strike flares
                    if (projectile.TryGetGlobalProjectile(out StealthStrikeGlobalProjectile stealthGlobal) &&
                        stealthGlobal.isStealthStrike &&
                        stealthGlobal.stealthType == StealthStrikeType.WhiteDwarfCutter)
                    {
                        projectile.scale = 0.5f;
                    }
                }
            }

            // GELGLOVE
            if (stealthType == StealthStrikeType.GelGlove)
            {
                // Only allow shuriken firing while actually flying (not during charging)
                if (projectile.velocity.LengthSquared() > 1.25f) // adjust threshold as needed
                {
                    projectile.localAI[1]++;

                    if (projectile.localAI[1] >= 30)
                    {
                        projectile.localAI[1] = 0;

                        Player player = Main.player[projectile.owner];
                        var calPlayer = player.GetModPlayer<CalamityMod.CalPlayer.CalamityPlayer>();

                        NPC target = FindNearestEnemy(projectile.Center, 600f);
                        Vector2 shootVelocity = projectile.velocity.SafeNormalize(Vector2.UnitX) * 10f;

                        if (target != null)
                            shootVelocity = (target.Center - projectile.Center).SafeNormalize(Vector2.UnitX) * 15f;

                        int shurikenType = ProjectileID.Shuriken;

                        Projectile.NewProjectile(
                            projectile.GetSource_FromThis(),
                            projectile.Center,
                            shootVelocity,
                            shurikenType,
                            projectile.damage - projectile.damage / 4,
                            projectile.knockBack,
                            projectile.owner,
                            7 //uses an ai slot to tell the game to switch the damage type (see above)
                        );
                    }
                }
                else
                {
                    // Reset timer while stationary to avoid leftover charge
                    projectile.localAI[1] = 0;
                }
            }

            // CHLOROPHYTETOMAHAWK
            if (stealthType == StealthStrikeType.ChlorophyteTomahawk)
            {
                projectile.localAI[1]++;

                if (projectile.localAI[1] >= 30)
                {
                    projectile.localAI[1] = 0;

                    Player player = Main.player[projectile.owner];
                    var calPlayer = player.GetModPlayer<CalamityMod.CalPlayer.CalamityPlayer>();

                    int sporeType = 567;
                    int damage = (int)Math.Round(projectile.damage * 0.25);

                    // Radius within which spores spawn
                    float spawnRadius = 128f;

                    // Generate a random position within a circle around the tomahawk's center
                    Vector2 randomOffset = Main.rand.NextVector2Circular(spawnRadius, spawnRadius);

                    Vector2 spawnPosition = projectile.Center + randomOffset;

                    // Velocity = zero, so spores just spawn in place
                    Vector2 spawnVelocity = Vector2.Zero;

                    int sporeID = Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        spawnPosition,
                        spawnVelocity,
                        sporeType,
                        damage,
                        projectile.knockBack,
                        projectile.owner
                    );
                }
            }

            // ShadeShuriken AI
            if (stealthType == StealthStrikeType.ShadeShuriken)
            {
                projectile.scale = 3f;
                projectile.penetrate = 9;  // Set EVERY tick here to override resets

                if (!appliedChanges)
                {
                    appliedChanges = true;

                    float scaleFactor = 3f;
                    Vector2 center = projectile.Center;

                    projectile.width = (int)(projectile.width * scaleFactor);
                    projectile.height = (int)(projectile.height * scaleFactor);
                    projectile.Center = center;

                    projectile.timeLeft = Math.Max(projectile.timeLeft, 180);
                }

                float homingRange = 600f;
                float homingSpeed = 20f;
                float lerpStrength = 0.15f;

                NPC target = null;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(projectile) && Vector2.Distance(projectile.Center, npc.Center) < homingRange)
                    {
                        target = npc;
                        break;
                    }
                }

                if (target != null)
                {
                    Vector2 desiredVelocity = projectile.DirectionTo(target.Center) * homingSpeed;
                    projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, lerpStrength);
                }
            }

            //CAPTAIN'S
            if (stealthType == StealthStrikeType.CaptainsPoignard)
            {
                Player player = Main.player[projectile.owner];

                if (initialSpeed == 0f)
                {
                    initialSpeed = projectile.velocity.Length();
                }

                burstTimer++;

                if (burstShotsFired == 0)
                {
                    // Wait initial delay before first shot
                    if (burstTimer >= initialDelay)
                    {
                        // Apply buff once at start
                        if (!buffApplied)
                        {
                            int buffID = ModLoader.GetMod("ThoriumMod")?.Find<ModBuff>("ThrowingSpeed")?.Type ?? -1;
                            if (buffID != -1)
                                player.AddBuff(buffID, 600);

                            buffApplied = true;
                        }

                        ShootBurstProjectile(projectile, player, initialSpeed);
                        burstShotsFired++;
                        burstTimer = 0;
                    }
                }
                else if (burstShotsFired > 0 && burstShotsFired < 5)
                {
                    if (burstTimer >= burstDelayTicks)
                    {
                        ShootBurstProjectile(projectile, player, initialSpeed);
                        burstShotsFired++;
                        burstTimer = 0;
                    }
                }
                else if (burstShotsFired >= 5)
                {
                    projectile.Kill();
                }
            }

            //SOULSLASHER
            if (isStealthStrike && stealthType == StealthStrikeType.Soulslasher && soulslasherAggressiveHoming)
            {
                float homingRange = 700f;
                float homingTurnSpeed = MathHelper.ToRadians(20f);
                float currentSpeed = projectile.velocity.Length();

                NPC target = null;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(projectile))
                    {
                        float distance = Vector2.Distance(projectile.Center, npc.Center);
                        if (distance < homingRange && Collision.CanHit(projectile.Center, 1, 1, npc.Center, 1, 1))
                        {
                            target = npc;
                            break;
                        }
                    }
                }

                if (target != null)
                {
                    Vector2 desiredDirection = Vector2.Normalize(target.Center - projectile.Center);
                    Vector2 currentDirection = Vector2.Normalize(projectile.velocity);

                    float angleToTarget = currentDirection.ToRotation().AngleTowards(desiredDirection.ToRotation(), homingTurnSpeed);

                    projectile.velocity = new Vector2((float)Math.Cos(angleToTarget), (float)Math.Sin(angleToTarget)) * currentSpeed;
                }
            }

            //HOMING
            if (stealthType == StealthStrikeType.PlayingCard || stealthType == StealthStrikeType.TerraKnife2)
            {
                float homingRange = 500f;
                float homingSpeed = 12f;
                float lerpStrength = 0.1f;

                NPC target = null;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(projectile) && Vector2.Distance(projectile.Center, npc.Center) < homingRange)
                    {
                        target = npc;
                        break;
                    }
                }

                if (target != null)
                {
                    Vector2 desiredVelocity = projectile.DirectionTo(target.Center) * homingSpeed;
                    projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, lerpStrength);
                }
            }

            //ICY DUST FOR ICY TOMAHAWK
            if (stealthType == StealthStrikeType.IcyTomahawk && Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(
                    projectile.position,
                    projectile.width,
                    projectile.height,
                    DustID.Frost,
                    projectile.velocity.X * 0.2f,
                    projectile.velocity.Y * 0.2f,
                    100,
                    default,
                    1.2f
                );

                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
            }

            // SPORE DUST FOR CHLOROPHYTE TOMAHAWK
            if (stealthType == StealthStrikeType.ChlorophyteTomahawk && Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(
                    projectile.position,
                    projectile.width,
                    projectile.height,
                    DustID.JungleSpore,  // Vanilla green spore dust ID
                    0f,
                    0f,
                    100,
                    default,
                    1.1f
                );

                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;  // subtle floating effect
                Main.dust[dust].fadeIn = 1.1f;     // fade-in for that soft spore glow
            }
        }

        // Helper method to find nearest enemy NPC for Gel Glove
        private NPC FindNearestEnemy(Vector2 position, float maxDistance)
        {
            NPC nearest = null;
            float minDist = maxDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy())
                {
                    float dist = Vector2.Distance(position, npc.Center);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = npc;
                    }
                }
            }

            return nearest;
        }

        //ON DEATH EFFECT
        public override void Kill(Projectile projectile, int timeLeft)
        {
            if (!isStealthStrike || stealthType != StealthStrikeType.SoulBomb)
                return;

            for (int i = 0; i < 6; i++)
            {
                Vector2 spawnPos = projectile.Center + Main.rand.NextVector2Circular(16f, 16f);
                Vector2 velocity = Main.rand.NextVector2Circular(12f, 12f);

                Projectile.NewProjectile(
                    projectile.GetSource_Death(),
                    spawnPos,
                    velocity,
                    ModContent.ProjectileType<RogueSpectreBlast>(),
                    projectile.damage / 10,
                    projectile.knockBack,
                    projectile.owner
                );
            }
            SoundEngine.PlaySound(SoundID.NPCDeath39, projectile.Center);
        }

        //BURST EFFECT
        private void ShootBurstProjectile(Projectile projectile, Player player, float speed)
        {
            Vector2 baseVelocity = projectile.velocity.SafeNormalize(Vector2.Zero);
            Vector2 velocity = baseVelocity.RotatedByRandom(MathHelper.ToRadians(5)) * speed * 1.2f;

            Projectile.NewProjectile(
                projectile.GetSource_FromThis(),
                player.Center,
                velocity,
                projectile.type,
                projectile.damage,
                projectile.knockBack,
                player.whoAmI
            );

            SoundEngine.PlaySound(SoundID.Item1, projectile.Center);
        }

        //GUARANTEED CRIT SET
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!isStealthStrike) return;

            if (stealthType == StealthStrikeType.ZephyrsRuin)
            {
                modifiers.SetCrit();
            }
            if (stealthType == StealthStrikeType.ShadeShuriken)
            {
                modifiers.SourceDamage *= 0.6f;
            }
        }
    }

    public class SoftServeSundererGlobal : GlobalProjectile
    {
        private static int thoriumSoftServeType = -1;

        public override void AI(Projectile projectile)
        {
            // Lazy-load the Thorium projectile type
            if (thoriumSoftServeType == -1)
            {
                if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                    thorium.TryFind("SoftServeSundererPro", out ModProjectile sssProj))
                {
                    thoriumSoftServeType = sssProj.Type;
                }
            }

            // Only proceed if projectile is the Thorium SoftServeSundererPro
            if (projectile.type == thoriumSoftServeType && projectile.localAI[0] == 1f)
            {
                NPC target = null;
                float homingRange = 400f;
                float homingStrength = 0.1f;
                float speed = projectile.velocity.Length();

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(projectile))
                    {
                        float distance = Vector2.Distance(projectile.Center, npc.Center);
                        if (distance < homingRange)
                        {
                            target = npc;
                            break;
                        }
                    }
                }

                if (target != null)
                {
                    Vector2 desiredVelocity = Vector2.Normalize(target.Center - projectile.Center) * speed;
                    projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, homingStrength);
                }
            }
        }
    }
}