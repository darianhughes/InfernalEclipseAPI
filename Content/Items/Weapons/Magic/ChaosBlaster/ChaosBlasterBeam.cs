using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Core.DamageClasses.MythicClass;
using Luminance.Assets;
using Luminance.Common.Utilities;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoxusBoss.Assets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Weapons.Magic.ChaosBlaster
{
    [ExtendsFromMod("NoxusBoss")]
    public class ChaosBlasterBeam : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public ref float Time => ref Projectile.ai[0];
        public ref float LaserbeamLength => ref Projectile.ai[1];
        public static int Lifetime => Luminance.Common.Utilities.Utilities.SecondsToFrames(3.75f);
        public static float MaxLaserbeamLength = 5600f;
        public static Color LensFlareColor = new Color(byte.MaxValue, 174, 147);
        public static float MouseAimSpeedInterpolant = 0.15f;

        public override string Texture => "Terraria/Images/Projectile_" + 633.ToString();

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 8000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 96;
            Projectile.height = 96;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.timeLeft = Lifetime;
            Projectile.localNPCHitCooldown = 1;
            Projectile.MaxUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.hide = true;
            Projectile.DamageType = MythicMagic.Instance;
            Projectile.ArmorPenetration = 500;
        }

        public override void AI()
        {
            // Despawn if owner invalid
            if (Projectile.owner < 0 || Projectile.owner >= byte.MaxValue || !Owner.active || Owner.dead)
            {
                Projectile.Kill();
                return;
            }

            // Fire sound on the 2nd tick if close enough to the local player
            if (Time == 2 && Main.LocalPlayer.WithinRange(Projectile.Center, 3000f))
            {
                var fire = new SoundStyle("InfernalEclipseAPI/Assets/Sounds/SolynStarBeamFire")
                {
                    Volume = 1.5f,
                    MaxInstances = 1
                };
                SoundEngine.PlaySound(fire, Projectile.Center);
            }

            AimTowardsMouse();

            // Position beam source a short distance from the player toward the mouse
            Projectile.Center = Owner.Center + Vector2.Normalize(Main.MouseWorld - Owner.Center).SafeNormalize(Vector2.UnitX) * 100f;

            // Extend beam length
            LaserbeamLength = MathHelper.Clamp(LaserbeamLength + 175f, 0f, MaxLaserbeamLength);

            CreateOuterParticles();
            Time++;
        }

        public void AimTowardsMouse()
        {
            if (Main.myPlayer != Projectile.owner)
                return;

            Vector2 oldVel = Projectile.velocity;
            float targetRot = Projectile.AngleTo(Main.MouseWorld);
            Projectile.velocity = Utils.ToRotationVector2(Utils.AngleLerp(Projectile.velocity.ToRotation(), targetRot, MouseAimSpeedInterpolant));

            if (Projectile.velocity != oldVel)
            {
                Projectile.netUpdate = true;
                Projectile.netSpam = 0;
            }
        }

        public void CreateOuterParticles()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            Vector2 perp = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2);

            for (int i = 0; i < 6; i++)
            {
                int dustType = Main.rand.NextBool() ? 264 : 226;
                float t = Main.rand.NextFloat();
                float dir = Main.rand.NextBool() ? -1f : 1f;

                Vector2 pos =
                    Projectile.Center +
                    Projectile.velocity * t * LaserbeamLength +
                    perp * LaserWidthFunction(0.5f) * dir * 0.9f;

                Dust d = Dust.NewDustPerfect(pos, dustType);
                d.velocity = perp * dir * Main.rand.NextFloat(2f, 8f);
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(0.8f, 1.4f);
                d.color = Main.rand.NextBool() ? Color.HotPink : Color.Wheat;
            }
        }

        public float LaserWidthFunction(float completionRatio)
        {
            float reachFactor = Luminance.Common.Utilities.Utilities.Convert01To010(
                Luminance.Common.Utilities.Utilities.InverseLerp(0.15f, 0.85f, LaserbeamLength / MaxLaserbeamLength));

            float startTaper = Luminance.Common.Utilities.Utilities.InverseLerp(0f, 0.05f, completionRatio);

            float baseWidth =
                reachFactor * startTaper * 32f +
                MathF.Cos(Main.GlobalTimeWrappedHourly * 90f) * 6f +
                Projectile.width;

            float dyingShrink = Luminance.Common.Utilities.Utilities.InverseLerp(0f, 8f, Lifetime - Time);
            float endCurve = Luminance.Common.Utilities.Utilities.InverseLerp(0.05f, 0.012f, completionRatio);
            float circle = MathF.Sqrt(1.001f - endCurve * endCurve);

            return Utils.Remap(LaserbeamLength, 0f, MaxLaserbeamLength, 4f, baseWidth, clamped: true)
                   * dyingShrink * circle;
        }

        public float BloomWidthFunction(float completionRatio)
            => LaserWidthFunction(completionRatio) * 1.9f;

        public Color LaserColorFunction(float completionRatio)
        {
            float grow = Luminance.Common.Utilities.Utilities.InverseLerp(0f, 0.45f, LaserbeamLength / MaxLaserbeamLength);
            float start = Luminance.Common.Utilities.Utilities.InverseLerp(0f, 0.032f, completionRatio);
            float end = Luminance.Common.Utilities.Utilities.InverseLerp(0.95f, 0.81f, completionRatio);
            float a = grow * start * end;

            return Projectile.GetAlpha(new Color(255, 45, 123)) * a;
        }

        public static Color BloomColorFunction(float completionRatio)
        {
            float bump = Luminance.Common.Utilities.Utilities.InverseLerpBump(0.02f, 0.05f, 0.81f, 0.95f, completionRatio);
            return new Color(255, 10, 150) * bump * 0.34f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float wobble = Luminance.Common.Utilities.Utilities.Cos01(Main.GlobalTimeWrappedHourly * 85f);

            // Control points for the primitive trail
            List<Vector2> points = Projectile.GetLaserControlPoints(12, LaserbeamLength);
            if (points.Count > 0)
                points[0] -= Projectile.velocity * 10f;

            bool usedShader = false;

            try
            {
                // Bloom pass
                var bloom = ShaderManager.GetShader("NoxusBoss.PrimitiveBloomShader");
                if (bloom != null)
                {
                    bloom.TrySetParameter("innerGlowIntensity", 0.45f);
                    var settings = new PrimitiveSettings(BloomWidthFunction, BloomColorFunction, Shader: bloom, UseUnscaledMatrix: true);
                    PrimitiveRenderer.RenderTrail(points, settings, 46);
                }

                // Main beam pass
                var beam = ShaderManager.GetShader("NoxusBoss.SolynTagTeamBeamShader");
                if (beam != null)
                {
                    beam.TrySetParameter("secondaryColor", LensFlareColor.ToVector4());
                    beam.TrySetParameter("lensFlareColor", LensFlareColor.ToVector4());
                    beam.SetTexture(GennedAssets.Textures.Noise.DendriticNoiseZoomedOut.Value, 1, SamplerState.LinearWrap);

                    var settings = new PrimitiveSettings(LaserWidthFunction, LaserColorFunction, Shader: beam, UseUnscaledMatrix: true);
                    PrimitiveRenderer.RenderTrail(points, settings, 75);
                }

                usedShader = bloom != null || beam != null;
            }
            catch (Exception)
            {
            }

            // Fallback: draw lines if shaders unavailable
            if (!usedShader && points.Count >= 2)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    float t = i / (float)(points.Count - 1);
                    Vector2 a = points[i] - Main.screenPosition;
                    Vector2 b = points[i + 1] - Main.screenPosition;

                    Color bloomColor = BloomColorFunction(t);
                    float bloomWidth = BloomWidthFunction(t);
                    Utils.DrawLine(Main.spriteBatch, a, b, bloomColor, bloomColor, bloomWidth * 0.5f);

                    Color beamColor = LaserColorFunction(t);
                    float beamWidth = LaserWidthFunction(t);
                    Utils.DrawLine(Main.spriteBatch, a, b, beamColor, beamColor, beamWidth * 0.3f);
                }
            }

            // Lens flare / bloom sprites at the origin
            float flareScale =
                Luminance.Common.Utilities.Utilities.InverseLerp(0f, 12f, Time) *
                Luminance.Common.Utilities.Utilities.InverseLerp(0f, 7f, Projectile.timeLeft) *
                MathHelper.Lerp(1f, 1.2f, wobble) *
                0.45f;

            Vector2 originOnScreen = Projectile.Center - Main.screenPosition;

            var smallBloom = GennedAssets.Textures.GreyscaleTextures.BloomCircleSmall.Value;
            var shineFlare = MiscTexturesRegistry.ShineFlareTexture.Value;

            if (smallBloom != null && shineFlare != null)
            {
                // 3 flares
                for (int i = 0; i < 3; i++)
                {
                    Color c = Projectile.GetAlpha(LensFlareColor with { A = 0 });
                    Main.spriteBatch.Draw(
                        shineFlare,
                        originOnScreen,
                        null,
                        c,
                        0f,
                        shineFlare.Size() * 0.5f,
                        flareScale * 2f,
                        SpriteEffects.None,
                        0f);
                }

                // 2 small blooms
                for (int i = 0; i < 2; i++)
                {
                    Color c = Projectile.GetAlpha(LensFlareColor with { A = 0 });
                    Main.spriteBatch.Draw(
                        smallBloom,
                        originOnScreen,
                        null,
                        c,
                        0f,
                        smallBloom.Size() * 0.5f,
                        flareScale * 2f,
                        SpriteEffects.None,
                        0f);
                }
            }

            return false; // handled drawing
        }

        public override void DrawBehind(
            int index,
            List<int> behindNPCsAndTiles,
            List<int> behindNPCs,
            List<int> behindProjectiles,
            List<int> overPlayers,
            List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = 0f;
            float width = LaserWidthFunction(0.25f) * 1.8f;

            Vector2 start = Projectile.Center;
            Vector2 end = start + Projectile.velocity.SafeNormalize(Vector2.Zero) * LaserbeamLength * 0.95f;

            bool hit = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, width, ref _);
            return hit;
        }

        public override bool ShouldUpdatePosition() => false;
    }
}
