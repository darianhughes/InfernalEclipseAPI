using InfernalEclipseAPI.Core.Graphics.Automators;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using InfernalEclipseAPI.Core.Utils;
using Microsoft.Xna.Framework;
using static Microsoft.Xna.Framework.MathHelper;

namespace InfernalEclipseAPI.Content.Items.Weapons.BossRush.Swordofthe14thGlitch
{
    public class HomingTerraBeam : ModProjectile, IDrawAdditive
    {
        public ref float Time => ref Projectile.ai[0];

        public override string Texture => "InfernalEclipseAPI/Content/Items/Weapons/BossRush/Swordofthe14thGlitch/TerraBeam";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            Projectile.hide = false;
        }

        public override void AI()
        {
            // Fade out prior to dying, and fade in as the beam materializes.
            Projectile.Opacity = InfernalUtilities.InverseLerp(0f, 6f, Time) * InfernalUtilities.InverseLerp(0f, 32f, Projectile.timeLeft);
            if (Projectile.timeLeft <= 16)
                Projectile.damage = 0;

            // Slow down rapidly at first.
            if (Time <= 15f)
                Projectile.velocity *= Swordofthe14thGlitch.HomingBeamDecelerationFactor;

            // Make the time go by a lot quicker if moving slowly after the initial slowdown.
            if (Time >= 20f && Projectile.velocity.Length() <= 8f)
                Projectile.timeLeft -= 3;

            // Rapidly fly towards the nearest target.
            NPC potentialTarget = Projectile.FindTargetWithinRange(Swordofthe14thGlitch.HomingBeamSearchRange);
            if (Time >= 20f && potentialTarget is not null && potentialTarget.active)
            {
                Vector2 idealVelocity = Projectile.DirectionToSafe(potentialTarget.Center) * (Projectile.velocity.Length() + Swordofthe14thGlitch.HomingBeamAcceleration / Swordofthe14thGlitch.HomingBeamFlySpeedInterpolant);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, idealVelocity, Swordofthe14thGlitch.HomingBeamFlySpeedInterpolant);
            }

            // Define rotation.
            Projectile.rotation = Projectile.velocity.ToRotation() + PiOver4;

            // Create bright red dust that respects color.
            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(14f, 14f) + Projectile.velocity * 0.15f,
                    DustID.FireworksRGB,
                    -Projectile.velocity * 0.25f,
                    0,
                    new Color(255, 48, 48),
                    1.1f
                );
                d.noGravity = true;
                d.fadeIn = Main.rand.NextFloat(1.1f, 1.35f);
            }

            // Emit stronger red light.
            Lighting.AddLight(Projectile.Center, 1.30f, 0.22f, 0.22f);

            // Increment time.
            Time++;
        }

        public void DrawAdditive(SpriteBatch spriteBatch)
        {
            // Brighter afterimages in red.
            InfernalUtilities.DrawAfterimagesCentered(
                Projectile,
                ProjectileID.Sets.TrailingMode[Type],
                new Color(255, 64, 64) * Projectile.Opacity,
                positionClumpInterpolant: 0.56f
            );

            // Layered additive passes for a glow/bloom look (uses the projectile texture).
            var tex = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            Vector2 origin = tex.Size() * 0.5f;
            float op = Projectile.Opacity;

            // Core, mid, outer glows
            Main.EntitySpriteDraw(tex, pos, null,
                Color.White * (0.80f * op),
                Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);

            Main.EntitySpriteDraw(tex, pos, null,
                new Color(255, 64, 64) * (0.55f * op),
                Projectile.rotation, origin, Projectile.scale * 1.15f, SpriteEffects.None);

            Main.EntitySpriteDraw(tex, pos, null,
                new Color(255, 24, 24) * (0.35f * op),
                Projectile.rotation, origin, Projectile.scale * 1.35f, SpriteEffects.None);
        }
        public override Color? GetAlpha(Color lightColor) => Color.Red * Projectile.Opacity; // ignores world lighting, stays red
    }
}
