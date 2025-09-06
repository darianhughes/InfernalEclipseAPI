using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using ReLogic.Content;

namespace InfernalEclipseAPI.Content.Items.Weapons.BossRush.NovaBomb
{
    public class NovaBombProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.NebulaBlaze1;

        // Tuning
        const float DetectRadius = 900f;
        const float HomingSpeed = 12f;
        const float HomingInertia = 12f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }

        const int NebulaFrameCount = 4;
        const int NebulaTicksPerFrame = 5;

        public override void AI()
        {
            if (++Projectile.frameCounter >= NebulaTicksPerFrame)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % NebulaFrameCount;
            }

            // Dusty “cosmic particle” look
            for (int i = 0; i < 2; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0f, 0f, 150, default, 1.1f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = -Projectile.velocity * 0.2f + Main.rand.NextVector2Circular(1.2f, 1.2f);
            }

            // Home to nearest valid NPC
            NPC target = null;
            float dist = DetectRadius;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (!npc.active || !npc.CanBeChasedBy(this)) continue;
                float d2 = Vector2.Distance(Projectile.Center, npc.Center);
                if (d2 < dist && Collision.CanHitLine(Projectile.Center, 1, 1, npc.Center, 1, 1))
                {
                    dist = d2;
                    target = npc;
                }
            }

            if (target != null)
            {
                Vector2 desired = Projectile.DirectionTo(target.Center) * HomingSpeed;
                Projectile.velocity = (Projectile.velocity * HomingInertia + desired) / (HomingInertia + 1f);
            }

            Projectile.rotation += 0.2f * Math.Sign(Projectile.velocity.X == 0 ? 1 : Projectile.velocity.X);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SpawnBlackHole();
        }

        public override void OnKill(int timeLeft)
        {
            SpawnBlackHole();
        }

        private void SpawnBlackHole()
        {
            if (Projectile.localAI[0] > 0f) return; // ensure single spawn
            Projectile.localAI[0] = 1f;

            if (Projectile.owner == Main.myPlayer)
            {
                // radius & pull packed into ai[]
                float radius = 160f;     // 320px diameter AOE
                float pull = 0.6f;       // pull strength
                int dmg = (int)(Projectile.damage * 0.85f);
                float kb = 1f;

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<NovaBombBlackHoleProj>(),
                    dmg,
                    kb,
                    Projectile.owner,
                    radius,
                    pull
                );
            }

            // small pop
            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.6f, Pitch = -0.2f }, Projectile.Center);
            SoundEngine.PlaySound(Supernova.StealthExplosionSound with { Pitch = Projectile.ai[2] }, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleCrystalShard, 0f, 0f, 150, default, 1.3f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = Main.rand.NextVector2Circular(4f, 4f);
            }
        }

        public override Color? GetAlpha(Color lightColor) => new Color(178, 114, 255) * Projectile.Opacity; // purple

        public override bool PreDraw(ref Color lightColor)
        {
            var tex = TextureAssets.Projectile[ProjectileID.NebulaBlaze1].Value;
            var frame = tex.Frame(1, NebulaFrameCount, 0, Projectile.frame); // (cols=1, rows=4)
            var origin = frame.Size() * 0.5f;
            var tint = new Color(178, 114, 255) * Projectile.Opacity;

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                frame,
                tint,
                Projectile.rotation,
                origin,
                1f,
                SpriteEffects.None,
                0
            );
            return false;
        }
    }

    public class NovaBombBlackHoleProj : ModProjectile
    {
        public static Asset<Texture2D> screamTex;
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        ref float Radius => ref Projectile.ai[0];
        ref float Pull => ref Projectile.ai[1];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            screamTex = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/ScreamyFace", AssetRequestMode.AsyncLoad);
        }

        public override void SetDefaults()
        {
            Projectile.width = 320;
            Projectile.height = 320;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.hide = true;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override void OnSpawn(IEntitySource src)
        {
            if (Radius <= 0) Radius = 160f;
            if (Pull <= 0) Pull = 0.6f;
            Projectile.scale = Radius / 160f;
        }

        public override void AI()
        {
            int size = (int)(Radius * 2f);
            Projectile.position = Projectile.Center - new Vector2(size / 2f);
            Projectile.width = Projectile.height = size;

            float pulse = 1f + 0.04f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3.2f);
            Projectile.scale = (Radius / 160f) * pulse;

            if (Main.rand.NextBool(2))
            {
                float r = Projectile.width * 0.5f;
                if (Projectile.ai[0] > 0f) r = Projectile.ai[0];

                Vector2 edge = Main.rand.NextVector2CircularEdge(r, r);
                Vector2 spawnPos = Projectile.Center + edge;

                Vector2 inward = (-edge).SafeNormalize(Vector2.UnitY) * 1.6f;
                Vector2 swirl = edge.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero) * 0.6f;

                var d = Dust.NewDustPerfect(
                    spawnPos,
                    DustID.PurpleCrystalShard,
                    inward + swirl,
                    180,
                    new Color(180, 130, 255),
                    1.15f
                );
                d.noGravity = true;
            }

            // pull enemies inward
            ApplyGravitationalPull();
        }

        private void ApplyGravitationalPull()
        {
            // OLD; NOT WORKING
            // NPC movement is server-authoritative; don't fight rubberbanding.
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            Vector2 center = Projectile.Center;
            float radius = Math.Max(32f, Radius);
            float pull = Pull;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.townNPC)
                    continue;
                if (!npc.CanBeChasedBy(Projectile))
                    continue;

                float dist = Vector2.Distance(npc.Center, center);
                if (dist > radius)
                    continue;

                // toward center
                Vector2 dir = (center - npc.Center).SafeNormalize(Vector2.Zero);

                // Stronger near the center; bosses affected less but not immune
                float edge = 1f - dist / radius;           // 0..1
                float bossScale = npc.boss ? 0.35f : 1f;
                float accel = pull * (0.35f + edge) * bossScale;

                npc.velocity += dir * accel;
                // slight swirl for style (small so it doesn't cancel the pull)
                npc.velocity += new Vector2(-dir.Y, dir.X) * (accel * 0.15f);

                // clamp speed (higher near center)
                float maxSpeed = MathHelper.Lerp(6f, 24f, edge);
                if (npc.velocity.LengthSquared() > maxSpeed * maxSpeed)
                    npc.velocity = npc.velocity.SafeNormalize(Vector2.UnitY) * maxSpeed;

                npc.netUpdate = true;
            }

            /* THROWER UNI VERSION
            float pullRadius = Projectile.width * 1.25f + 150f;
            float pullStrength = 0.6f;
            Vector2 center = Projectile.Center;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && !npc.boss && !npc.dontTakeDamage && npc.lifeMax > 5 && !npc.immortal && npc.CanBeChasedBy(Projectile))
                {
                    float dist = Vector2.Distance(npc.Center, center);
                    if (dist < pullRadius)
                    {
                        Vector2 pull = center - npc.Center;
                        float strength = pullStrength * (1f - dist / pullRadius); // Linear falloff
                        pull.Normalize();
                        npc.velocity += pull * strength;
                    }
                }
            }
            */
        }

        // circle vs AABB since your build lacks the helper
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 c = Projectile.Center;
            float cx = MathHelper.Clamp(c.X, targetHitbox.Left, targetHitbox.Right);
            float cy = MathHelper.Clamp(c.Y, targetHitbox.Top, targetHitbox.Bottom);
            float dx = c.X - cx, dy = c.Y - cy;
            return (dx * dx + dy * dy) <= Radius * Radius;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Shader + scream texture pass
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            lightColor.R = (byte)(255 * Projectile.Opacity);

            Main.spriteBatch.End();
            Effect shieldEffect = Filters.Scene["CalamityMod:HellBall"].GetShader().Shader;
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, shieldEffect, Main.GameViewMatrix.TransformationMatrix);

            float noiseScale = 0.6f;

            shieldEffect.Parameters["time"].SetValue(Projectile.timeLeft / 60f * 0.24f);
            shieldEffect.Parameters["blowUpPower"].SetValue(3.2f);
            shieldEffect.Parameters["blowUpSize"].SetValue(0.4f);
            shieldEffect.Parameters["noiseScale"].SetValue(noiseScale);

            float opacity = Projectile.Opacity;
            shieldEffect.Parameters["shieldOpacity"].SetValue(opacity);
            shieldEffect.Parameters["shieldEdgeBlendStrenght"].SetValue(4f);

            Color deepPurple = new Color(112, 40, 222);
            Color lightPurple = new Color(178, 114, 255);
            Color edgeColor = Color.Black * opacity;
            Color shieldColor = Color.Lerp(deepPurple, lightPurple, 0.5f) * opacity;

            shieldEffect.Parameters["shieldColor"].SetValue(shieldColor.ToVector3());
            shieldEffect.Parameters["shieldEdgeColor"].SetValue(edgeColor.ToVector3());

            Vector2 pos = Projectile.Center - Main.screenPosition;
            float scale = 0.715f;
            Main.spriteBatch.Draw(
                screamTex.Value,
                pos,
                null,
                Color.White,
                0,
                screamTex.Size() * 0.5f,
                scale * 0.2f * Projectile.scale * Projectile.Opacity,
                0,
                0
            );

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D vortexTexture = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/SoulVortex").Value;
            Texture2D centerTexture = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeBloom").Value;
            for (int i = 0; i < 10; i++)
            {
                float angle = MathHelper.TwoPi * i / 3f + Main.GlobalTimeWrappedHourly * MathHelper.TwoPi;
                Color outerColor = Color.Lerp(deepPurple, lightPurple, i * 0.15f);
                Color drawColor = Color.Lerp(outerColor, Color.Black, i * 0.2f) * 0.5f;
                drawColor.A = 0;

                Vector2 drawPosition = Projectile.Center - Main.screenPosition;
                drawPosition += (angle + Main.GlobalTimeWrappedHourly * i / 16f).ToRotationVector2() * 6f;

                Main.EntitySpriteDraw(
                    vortexTexture,
                    drawPosition,
                    null,
                    drawColor * Projectile.Opacity,
                    -angle + MathHelper.PiOver2,
                    vortexTexture.Size() * 0.5f,
                    (Projectile.scale * (1 - i * 0.05f) * 0.2f) * Projectile.Opacity,
                    SpriteEffects.None,
                    0
                );
            }

            Main.EntitySpriteDraw(
                centerTexture,
                Projectile.Center - Main.screenPosition,
                null,
                Color.Black * Projectile.Opacity,
                Projectile.rotation,
                centerTexture.Size() * 0.5f,
                (Projectile.scale * 0.9f * 0.2f) * Projectile.Opacity,
                SpriteEffects.None,
                0
            );

            // Trail rendering
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = texture2D13.Height / Main.projFrames[Projectile.type];
            int y3 = num156 * Projectile.frame;
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color tint = new Color(178, 114, 255) * Projectile.Opacity;
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                float fade = (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.oldPos[i];
                float num165 = Projectile.oldRot[i];
                Main.EntitySpriteDraw(
                    texture2D13,
                    value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
                    rectangle,
                    tint * fade * 0.8f,
                    num165,
                    origin2,
                    Projectile.scale * 0.2f,
                    SpriteEffects.None,
                    0
                );
            }

            // Current frame draw
            Main.EntitySpriteDraw(
                texture2D13,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                rectangle,
                Projectile.GetAlpha(lightColor),
                Projectile.rotation,
                origin2,
                Projectile.scale * 0.2f,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}
