using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using CalamityMod;

namespace InfernalEclipseAPI.Common.Projectiles
{
    public class ProjectileBalanceChanges : GlobalProjectile
    {
        public override void SetDefaults(Projectile entity)
        {
            if (entity.type == ProjectileID.Shuriken && InfernalConfig.Instance.ChanageWeaponClasses)
            {
                entity.DamageType = ModContent.GetInstance<RogueDamageClass>();
            }

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
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = 1;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 1;
                    }
                }

                if (!ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                {
                    if (thorium.TryFind("MoltenThresherPro", out ModProjectile moltenThresherProj))
                        moltenThresherType = moltenThresherProj.Type;

                    if (thorium.TryFind("BatScythePro", out ModProjectile batScytheProj))
                        batScytheType = batScytheProj.Type;

                    if (thorium.TryFind("BatScythePro2", out ModProjectile batScytheProj2))
                        batScytheType2 = batScytheProj2.Type;

                    if (thorium.TryFind("FallingTwilightPro", out ModProjectile fallingTwilightProj))
                        fallingTwilightType = fallingTwilightProj.Type;

                    if (thorium.TryFind("BloodHarvestPro", out ModProjectile bloodHarvestProj))
                        bloodHarvestType = bloodHarvestProj.Type;

                    if (thorium.TryFind("TrueFallingTwilightPro", out ModProjectile trueFallingTwilightProj))
                        trueFallingTwilightType = trueFallingTwilightProj.Type;

                    if (thorium.TryFind("TrueBloodHarvestPro", out ModProjectile trueBloodHarvestProj))
                        trueBloodHarvestType = trueBloodHarvestProj.Type;

                    if (thorium.TryFind("TheBlackScythePro", out ModProjectile theBlackScytheProj))
                        theBlackScytheType = theBlackScytheProj.Type;

                    if (thorium.TryFind("TitanScythePro", out ModProjectile titanScytheProj))
                        titanScytheType = titanScytheProj.Type;

                    if (thorium.TryFind("BoneBatonPro", out ModProjectile boneBatonProj))
                        boneBatonType = boneBatonProj.Type;
                }
            } 

            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                if (!ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                {
                    if (ragnarok.TryFind("WindSlashPro", out ModProjectile windSlashProj))
                        windSlashType = windSlashProj.Type;
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
            }

            if (ModLoader.TryGetMod("CalamityBardHealer", out Mod calBardHeal) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
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
        }

        private bool GetProj(Projectile entity, Mod mod, string item)
        {
            if (entity.type == mod.Find<ModProjectile>(item).Type)
            {
                return true;
            }
            return false;
        }

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
        private static int windSlashType = -1;

        private float GetScaleForProjectile(int type)
        {
            if (type == moltenThresherType) return 1.5f;
            if (type == batScytheType) return 1.5f;
            if (type == batScytheType2) return 3f;
            if (type == fallingTwilightType) return 1.5f;
            if (type == bloodHarvestType) return 1.5f;
            if (type == trueFallingTwilightType) return 1.5f;
            if (type == trueBloodHarvestType) return 1.5f;
            if (type == theBlackScytheType) return 1.5f;
            if (type == titanScytheType) return 2f;
            if (type == windSlashType) return 2f;
            if (type == boneBatonType) return 2f;
            return 1f;
        }

        private bool IsEligibleForChildScaling(int type)
        {
            // Add only child projectiles that should inherit scale
            // Example: return type == someKnownChildProjectileType;
            return false; // default: no other projectiles should scale
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (!ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
            {
                if (projectile.type != moltenThresherType
                    && projectile.type != titanScytheType
                    && projectile.type != batScytheType
                    && projectile.type != batScytheType2
                    && projectile.type != bloodHarvestType
                    && projectile.type != fallingTwilightType
                    && projectile.type != trueFallingTwilightType
                    && projectile.type != trueBloodHarvestType
                    && projectile.type != theBlackScytheType
                    && projectile.type != windSlashType
                    && projectile.type != boneBatonType
                    )
                {
                    return true;
                }

                Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
                int frameCount = Main.projFrames[projectile.type];
                int frameHeight = texture.Height / frameCount;
                Rectangle sourceRectangle = new Rectangle(0, frameHeight * projectile.frame, texture.Width, frameHeight);
                Vector2 origin = sourceRectangle.Size() / 2f;

                float baseScale = GetScaleForProjectile(projectile.type);
                float scale = projectile.scale * baseScale;
                Vector2 drawPos = projectile.Center - Main.screenPosition;
                Color drawColor = lightColor * ((255 - projectile.alpha) / 255f);

                Player owner = Main.player[projectile.owner];
                SpriteEffects effect = (owner.direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    sourceRectangle,
                    drawColor,
                    projectile.rotation,
                    origin,
                    scale,
                    effect,
                    0
                );
                return false;
            }
            return true;
        }

        public override void AI(Projectile projectile)
        {
            if (projectile.localAI[1] == 1f)
                return;

            if (!IsEligibleForChildScaling(projectile.type))
                return;

            Projectile ownerProj = Main.projectile[projectile.owner];

            if (ownerProj != null && ownerProj.active)
            {
                float parentScale = GetScaleForProjectile(ownerProj.type);
                if (parentScale > 1f)
                {
                    projectile.scale *= parentScale;
                    projectile.localAI[1] = 1f;
                }
            }
        }

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
