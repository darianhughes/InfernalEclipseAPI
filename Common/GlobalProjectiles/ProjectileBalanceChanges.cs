using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using CalamityMod;
using InfernalEclipseAPI.Core.DamageClasses;

namespace InfernalEclipseAPI.Common.Projectiles
{
    public class ProjectileBalanceChanges : GlobalProjectile
    {
        public override void SetDefaults(Projectile entity)
        {
            if (entity.type == ProjectileID.PewMaticHornShot && InfernalConfig.Instance.VanillaBalanceChanges)
            {
                entity.penetrate = 2;
            }

            if (entity.type == ModContent.ProjectileType<PauldronDash>() && InfernalConfig.Instance.CalamityBalanceChanges)
            {
                //entity.idStaticNPCHitCooldown = 480;
            }

            if (ModLoader.TryGetMod("Clamity", out Mod clam))
            {
                if (entity.type == clam.Find<ModProjectile>("FireBarrage").Type)
                {
                    entity.damage = 135;
                }
                if (entity.type == clam.Find<ModProjectile>("FireBarrageHoming").Type)
                {
                    entity.damage = 130;
                }
                if (entity.type == clam.Find<ModProjectile>("Fireblast").Type)
                {
                    entity.damage = 140;
                }
                if (entity.type == clam.Find<ModProjectile>("FireBombExplosion").Type)
                {
                    entity.damage = 135;
                }
                if (entity.type == clam.Find<ModProjectile>("Firethrower").Type)
                {
                    entity.damage = 150;
                }
            }

            if (ModLoader.TryGetMod("Thorium", out Mod thorium) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                if (entity.type == thorium.Find<ModProjectile>("SeashellCastanettessPro1").Type)
                {
                    entity.penetrate = 2;
                }

                if (entity.type == thorium.Find<ModProjectile>("Cube").Type)
                {
                    entity.penetrate = 3;
                }

                if (entity.type == thorium.Find<ModProjectile>("GeyserPro2").Type)
                {
                    entity.scale *= 5;
                }

                if (entity.type == thorium.Find<ModProjectile>("GraniteBarrier").Type)
                {
                }

                if (entity.type == thorium.Find<ModProjectile>("PalmCrossPro").Type)
                {
                    entity.scale *= 2;
                }

                if (entity.type == thorium.Find<ModProjectile>("TorpedoPro2").Type)
                {
                    entity.scale *= 2;
                    entity.penetrate = 10;
                }

                if (entity.type == thorium.Find<ModProjectile>("StoneThrowingSpearPro").Type)
                {
                    entity.penetrate = 2;
                }

                if (entity.type == thorium.Find<ModProjectile>("IcyTomahawkPro").Type)
                {
                    entity.penetrate = 5;
                }

                if (entity.type == thorium.Find<ModProjectile>("AquaiteScythePro").Type || entity.type == thorium.Find<ModProjectile>("BloodHarvestPro").Type || entity.type == thorium.Find<ModProjectile>("FallingTwilightPro").Type || entity.type == thorium.Find<ModProjectile>("TrueHallowedScythePro").Type || entity.type == thorium.Find<ModProjectile>("HallowedScythePro").Type)
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = 6;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 6;
                    }
                }

                if (entity.type == thorium.Find<ModProjectile>("BoneReaperPro").Type)
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = 3;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 3;
                    }
                }

                if (entity.type == thorium.Find<ModProjectile>("BatScythePro2").Type)
                {
                    //if (entity.usesLocalNPCImmunity)
                    //{
                    //    entity.localNPCHitCooldown = 1;
                    //}

                    //if (entity.usesIDStaticNPCImmunity)
                    //{
                    //    entity.idStaticNPCHitCooldown = 1;
                    //}
                }

                if (!ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                {
                    moltenThresherType = thorium.Find<ModProjectile>("MoltenThresherPro")?.Type ?? -1;
                    batScytheType = thorium.Find<ModProjectile>("BatScythePro")?.Type ?? -1;
                    batScytheType2 = thorium.Find<ModProjectile>("BatScythePro2")?.Type ?? -1;
                    fallingTwilightType = thorium.Find<ModProjectile>("FallingTwilightPro")?.Type ?? -1;
                    bloodHarvestType = thorium.Find<ModProjectile>("BloodHarvestPro")?.Type ?? -1;
                    trueFallingTwilightType = thorium.Find<ModProjectile>("TrueFallingTwilightPro")?.Type ?? -1;
                    trueBloodHarvestType = thorium.Find<ModProjectile>("TrueBloodHarvestPro")?.Type ?? -1;
                    theBlackScytheType = thorium.Find<ModProjectile>("TheBlackScythePro")?.Type ?? -1;
                    titanScytheType = thorium.Find<ModProjectile>("TitanScythePro")?.Type ?? -1;
                    boneBatonType = thorium.Find<ModProjectile>("BoneBatonPro")?.Type ?? -1;
                    trueHallowedType = thorium.Find<ModProjectile>("TrueHallowedScythePro")?.Type ?? -1;
                    crimsonType = thorium.Find<ModProjectile>("CrimtaneScythePro")?.Type ?? -1;
                    iceType = thorium.Find<ModProjectile>("IceShaverPro")?.Type ?? -1;
                    darkType = thorium.Find<ModProjectile>("DemoniteScythePro")?.Type ?? -1;
                }
            } 

            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                if (!ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                {
                    windSlashType = ragnarok.Find<ModProjectile>("WindSlashPro")?.Type ?? -1;
                    marbleType = ragnarok.Find<ModProjectile>("MarbleScythePro")?.Type ?? -1;
                }
                
                if (entity.type == ragnarok.Find<ModProjectile>("GelScythePro2").Type)
                {
                    entity.penetrate = 5;
                    entity.scale = 2;
                }

                //if (entity.type == ragnarok.Find<ModProjectile>("WindSlashPro").Type)
                //{
                //    entity.scale *= 2;
                //}

                if (entity.type == ragnarok.Find<ModProjectile>("ProfanedBellBlast").Type)
                {
                    entity.penetrate = 3;
                    entity.scale = 0.75f;
                }

                if (entity.type == ragnarok.Find<ModProjectile>("ElysianSongPro").Type)
                {
                    entity.penetrate = 20;
                    entity.scale = 1.5f;
                }

                if (entity.type == ragnarok.Find<ModProjectile>("TendrilStrike").Type)
                {
                    entity.scale = 1.5f;
                }

                if (entity.type == ragnarok.Find<ModProjectile>("MarbleScythePro").Type || entity.type == ragnarok.Find<ModProjectile>("ProfanedScythePro").Type)
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = 6;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 6;
                    }
                }

                if (entity.type == ragnarok.Find<ModProjectile>("ScoriaDualscythePro").Type)
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = 3;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 3;
                    }
                }

                if (entity.type == ragnarok.Find<ModProjectile>("AuricDamruShock").Type)
                {
                    entity.scale = 2;
                }

                if (entity.type == ragnarok.Find<ModProjectile>("GraspofVoidPro1").Type)
                {
                    entity.penetrate = 6;
                }
            }

            if (ModLoader.TryGetMod("CalamityBardHealer", out Mod calBardHeal) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                if (!ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                {
                    whirlwindType = calBardHeal.Find<ModProjectile>("Whirlwind")?.Type ?? -1;
                }

                if (entity.type == calBardHeal.Find<ModProjectile>("ExoSound").Type)
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = -1;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 1;
                    }
                }

                if (ModLoader.TryGetMod("CatalystMod", out _))
                {
                    if (entity.type == calBardHeal.Find<ModProjectile>("StarBirth").Type)
                    {
                        //entity.scale *= 0.3f;
                    }
                }
            }

            if (ModLoader.TryGetMod("ThoriumRework", out Mod thorRework) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                if (GetProj(entity, thorRework, "DemonBloodSword") ||
                    GetProj(entity, thorRework, "DragonTooth") ||
                    GetProj(entity, thorRework, "DreadRazor") || 
                    GetProj(entity, thorRework, "IllumiteBlade") ||
                    GetProj(entity, thorRework, "LodeStoneClaymore") ||
                    GetProj(entity, thorRework, "SoulRender") || 
                    GetProj(entity, thorRework, "TerrariumSaber") ||
                    GetProj(entity, thorRework, "TitanSword") ||
                    GetProj(entity, thorRework, "ToothOfTheConsumer") ||
                    GetProj(entity, thorRework, "BeholderBlade"))
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = 60;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 60;
                    }
                }

                if (GetProj(entity, thorRework, "GrandThunder"))
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = -1;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = -1;
                    }
                }
            }

            if (ModLoader.TryGetMod("SOTS", out Mod sots) && InfernalConfig.Instance.SOTSBalanceChanges)
            {
                if (GetProj(entity, sots, "BetrayersSlash"))
                {
                    entity.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
                }

                if (GetProj(entity, sots, "SootBall"))
                {
                    entity.usesIDStaticNPCImmunity = false;

                    entity.localNPCHitCooldown = entity.idStaticNPCHitCooldown;
                    entity.usesLocalNPCImmunity = true;
                }
            }

            if (ModLoader.TryGetMod("SOTSBardHealer", out Mod sotsBH) && InfernalConfig.Instance.SOTSBalanceChanges)
            {
                if (GetProj(entity, sotsBH, "DualStylophonePro"))
                {
                    entity.penetrate = 3;
                }

                if (GetProj(entity, sotsBH, "GoopwoodSplit") || GetProj(entity, sotsBH, "ForbiddenMaelstrom") || GetProj(entity, sotsBH, "Serpentbite"))
                {
                    if (InfernalConfig.Instance.SOTSThrowerToRogue) entity.DamageType = ModContent.GetInstance<VoidRogue>();
                }
            }
        }

        private bool GetProj(Projectile entity, Mod mod, string item)
        {
            mod.TryFind(item, out ModProjectile projectile);
            if (projectile == null)
            {
                return false;
            }
            if (entity.type == projectile.Type)
            {
                return true;
            }
            return false;
        }

        public override bool InstancePerEntity => true;

        private static int moltenThresherType = -1;
        private static int fallingTwilightType = -1;
        private static int bloodHarvestType = -1;
        private static int trueFallingTwilightType = -1;
        private static int trueBloodHarvestType = -1;
        private static int titanScytheType = -1;
        private static int theBlackScytheType = -1;
        private static int batScytheType = -1;
        private static int batScytheType2 = -1;
        private static int boneBatonType = -1;
        private static int trueHallowedType = -1;
        private static int windSlashType = -1;
        private static int crimsonType = -1;
        private static int iceType = -1;
        private static int darkType = -1;
        private static int whirlwindType = -1;
        private static int marbleType = -1;

        private float GetScaleForProjectile(int type) => type switch
        {
            var t when t == moltenThresherType => 1.5f,
            var t when t == batScytheType => 1.5f,
            var t when t == batScytheType2 => 3f,
            var t when t == fallingTwilightType => 1.5f,
            var t when t == bloodHarvestType => 1.5f,
            var t when t == trueFallingTwilightType => 1.5f,
            var t when t == trueBloodHarvestType => 1.5f,
            var t when t == theBlackScytheType => 1.5f,
            var t when t == titanScytheType => 2f,
            var t when t == trueHallowedType => 1.3f,
            var t when t == boneBatonType => 2f,
            var t when t == windSlashType => 3f,
            var t when t == crimsonType => 1.2f,
            var t when t == iceType => 1.2f,
            var t when t == darkType => 1.1f,
            var t when t == whirlwindType => 1.5f,
            var t when t == marbleType => 1.75f,
            _ => 1f,
        };

        public override void AI(Projectile projectile)
        {
            ApplyScaling(projectile);
        }

        private void ApplyScaling(Projectile projectile)
        {
            float scale = GetScaleForProjectile(projectile.type);
            if (scale == 1f) return;

            if (projectile.localAI[2] == 1f) return;

            Vector2 originalSize = new Vector2(projectile.width, projectile.height);
            Vector2 oldCenter = projectile.Center;

            projectile.scale *= scale;
            projectile.width = (int)(originalSize.X * scale);
            projectile.height = (int)(originalSize.Y * scale);
            projectile.Center = oldCenter;

            projectile.localAI[2] = 1f;
        }

        public void EnsureScaled(Projectile projectile)
        {
            ApplyScaling(projectile);
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (!ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
            {
                int[] staticProjectiles = new int[]
                {
                    moltenThresherType, batScytheType, batScytheType2,
                    fallingTwilightType, bloodHarvestType, trueFallingTwilightType,
                    trueBloodHarvestType, theBlackScytheType, titanScytheType,
                    boneBatonType, windSlashType, trueHallowedType,
                    crimsonType, iceType, darkType, marbleType,
                };

                if (!Array.Exists(staticProjectiles, t => t == projectile.type))
                    return true;

                Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
                int frameHeight = texture.Height / Main.projFrames[projectile.type];
                Rectangle sourceRectangle = new Rectangle(0, frameHeight * projectile.frame, texture.Width, frameHeight);
                Vector2 origin = sourceRectangle.Size() / 2f;
                Vector2 drawPos = projectile.Center - Main.screenPosition;
                SpriteEffects effects = projectile.type == windSlashType
                    ? SpriteEffects.None
                    : (Main.player[projectile.owner].direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

                Color drawColor = lightColor;
                if (projectile.type == windSlashType)
                {
                    int fadeDuration = 30;
                    if (projectile.timeLeft < fadeDuration)
                        drawColor *= projectile.timeLeft / (float)fadeDuration;
                }

                Main.EntitySpriteDraw(texture, drawPos, sourceRectangle, drawColor, projectile.rotation, origin, projectile.scale, effects, 0);

                // Draw glowmask for known Thorium projectiles
                string glowPath = projectile.type switch
                {
                    var t when t == moltenThresherType => "ThoriumMod/Projectiles/Scythe/MoltenThresherPro_Glowmask",
                    _ => null
                };

                if (glowPath != null)
                {
                    Texture2D glowTexture = ModContent.Request<Texture2D>(glowPath).Value;

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                    Main.EntitySpriteDraw(glowTexture, drawPos, sourceRectangle, Color.White, projectile.rotation, origin, projectile.scale, effects, 0);

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                }

                return false;
            }
            return true;
        }

        public override void PostDraw(Projectile projectile, Color lightColor) { }

        public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
        {
            if (!ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
            {
                float scale = GetScaleForProjectile(projectile.type);
                if (scale != 1f)
                {
                    Vector2 center = hitbox.Center.ToVector2();
                    hitbox.Width = (int)(hitbox.Width * scale);
                    hitbox.Height = (int)(hitbox.Height * scale);
                    hitbox.X = (int)(center.X - hitbox.Width / 2);
                    hitbox.Y = (int)(center.Y - hitbox.Height / 2);
                }
            }
        }
    }
}
