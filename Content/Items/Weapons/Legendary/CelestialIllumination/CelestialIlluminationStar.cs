using CalamityMod;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;
using Luminance.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.CelestialIllumination
{
    public class CelestialIlluminationStar : ModProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override string Texture => "InfernalEclipseAPI/Assets/Textures/Backgrounds/BlankPixel";
        public override string GlowTexture => "InfernalEclipseAPI/Assets/Glow";
        public static int SplitStarCount
        {
            get
            {
                if (CelestialIllumination.Tier() >= 0)
                    return 3;
                else return 5;
            }
        }
        private NPC HomingTarget
        {
            get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0] - 1];
            set
            {
                Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
            }
        }
        public ref float DelayTimer => ref Projectile.ai[1];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 0;
            ProjectileID.Sets.CultistIsResistantTo[Type] = CalamityConditions.DownedDevourerOfGods.IsMet();
        }
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = LegendaryMagic.Instance;
            Projectile.timeLeft = 360;
            Projectile.friendly = true;
        //    Projectile.aiStyle = ProjAIStyleID.Typhoon;
            Projectile.hostile = false;
            Projectile.extraUpdates = 1;
        }
    //    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    //    {
    //        if (Projectile.ai[3] == 0)
    //        {
    //            for (int i = 0; i < 3; i++)
    //            {
    //                Main.NewText("Split");
    //            //    int splitstar = Projectile.NewProjectile(
    //            //        Projectile.GetSource_FromThis(),
    //            //        Projectile.Center,
    //            //        Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 5f,
    //            //        ModContent.ProjectileType<CelestialIlluminationStar>(),
    //            //        Projectile.damage / 2,
    //            //        Projectile.knockBack / 2,
    //            //        Projectile.owner
    //            //    );
    //            //    Projectile split = Main.projectile[splitstar];
    //            //    split.scale = 0.3f;
    //            //    split.ai[3] = 1;
    //            //    split.ai[2] = 1;
    //            }
    //            Projectile.Kill();
    //        }
    //    }
        public override void AI()
            {
                if (CalamityConditions.DownedDevourerOfGods.IsMet())
                {
                    float maxDetectRadius = 475;
        
                    if (DelayTimer < 10)
                    {
                        DelayTimer += 1;
                        return;
                    }
        
                    if (HomingTarget == null)
                    {
                        HomingTarget = FindClosestNPC(maxDetectRadius);
                    }
        
                    if (HomingTarget != null && !IsValidTarget(HomingTarget))
                    {
                        HomingTarget = null;
                    }
        
                    if (HomingTarget == null)
                        return;
        
                    float length = Projectile.velocity.Length();
                    float targetAngle = Projectile.AngleTo(HomingTarget.Center);
                    Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(4.5f)).ToRotationVector2() * length;
                    Projectile.rotation = Projectile.velocity.ToRotation();
                }
                else
                {
                    base.AI();
                }
            }
        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;

            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            foreach (var target in Main.ActiveNPCs)
            {
                if (IsValidTarget(target))
                {
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }
        public bool IsValidTarget(NPC target) => target.CanBeChasedBy() && Collision.CanHit(Projectile.Center, 1, 1, target.position, target.width, target.height);
        public override bool PreDraw(ref Color lightColor)
        {
            float starScale = Projectile.scale * 0.2f;

            Texture2D bloomSmall = MiscTexturesRegistry.BloomCircleSmall.Value;

            Texture2D shineFlare = MiscTexturesRegistry.ShineFlareTexture.Value;

            Color color = Color.White with { A = 0 };

            Main.spriteBatch.Draw(
                    shineFlare,
                    Projectile.Center - Main.screenPosition,
                    null,
                    color,
                    0f,
                    shineFlare.Size() * 0.5f,
                    starScale,
                    SpriteEffects.None,
                    0f
                );

            Main.spriteBatch.Draw(
                    bloomSmall,
                    Projectile.Center - Main.screenPosition,
                    null,
                    color,
                    0f,
                    bloomSmall.Size() * 0.5f,
                    starScale,
                    SpriteEffects.None,
                    0f
                );

            float fade = MathHelper.Min(15f, Projectile.timeLeft) / 15f;

            Texture2D glowTex = ModContent.Request<Texture2D>(GlowTexture, AssetRequestMode.ImmediateLoad).Value;
            Vector2 glowOrigin = glowTex.Size() / 2f;

            int trailLength = Projectile.oldPos.Length;
            for (int i = 0; i < trailLength; i++)
            {
                float t = 1f - i / (float)trailLength;

                Color trailColor = Color.White * t * fade;
                float trailScale = Projectile.scale * t * fade * 0.55f;
                Vector2 trailPos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;

                Main.spriteBatch.Draw(
                    glowTex,
                    trailPos,
                    null,
                    trailColor,
                    Projectile.rotation,
                    glowOrigin,
                    trailScale,
                    SpriteEffects.None,
                    0f
                );
            }
            return false;
        }
    }
}