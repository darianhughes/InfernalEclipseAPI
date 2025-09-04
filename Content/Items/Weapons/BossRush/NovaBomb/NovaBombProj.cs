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

namespace InfernalEclipseAPI.Content.Items.Weapons.BossRush.NovaBomb
{
    public class NovaBombProj : ModProjectile
    {
        /* OLD
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public new Color variedColor = new Color(178, 114, 255);   // Soft glowing purple
        public new Color mainColor = new Color(112, 40, 222);      // Deep violet core
        public new Color randomColor = new Color(245, 210, 255);   // Pale pinkish-white sparkles
        public new int colorTimer = 0;
        public new int time = 0;
        public new bool homing = false;
        public new bool returning = false;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 53;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            base.AI();
        }

        public override void OnKill(int timeLeft)
        {
           base.OnKill(timeLeft);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) // Add to regular plz
        {
            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, 5, targetHitbox);
        public override bool PreDraw(ref Color lightColor)
        {
            Color auraColor = Projectile.GetAlpha(Color.Lerp(Color.White, randomColor, 0.3f)) * 0.25f;
            for (int i = 0; i < 7; i++)
            {
                Texture2D centerTexture = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/BossRush/NovaBomb/NovaBombProjectile").Value;
                Vector2 rotationalDrawOffset = (MathHelper.TwoPi * i / 7f + Main.GlobalTimeWrappedHourly * 8f).ToRotationVector2();
                rotationalDrawOffset *= MathHelper.Lerp(3f, 5.25f, (float)Math.Cos(Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f);
                Main.EntitySpriteDraw(centerTexture, Projectile.Center - Main.screenPosition + rotationalDrawOffset, null, auraColor, Projectile.rotation, centerTexture.Size() * 0.5f, Projectile.scale * 1.1f, SpriteEffects.None, 0f);
            }

            if (!homing)
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], Color.Lerp(Color.White, randomColor, 0.3f), 1);
            return true;
        }
        */
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Fireball;

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

        public override void AI()
        {
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

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[ProjectileID.Fireball].Value;
            Rectangle frame = tex.Frame();
            Vector2 origin = frame.Size() * 0.5f;
            Color tint = new Color(178, 114, 255);

            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Vector2 pos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                Main.EntitySpriteDraw(tex, pos, frame, tint * 0.4f * fade, Projectile.oldRot[i], origin, 0.9f, 0, 0);
            }
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, frame, tint, Projectile.rotation, origin, 1f, 0, 0);
            return false;
        }
    }

    public class NovaBombBlackHoleProj : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        ref float Radius => ref Projectile.ai[0];
        ref float Pull => ref Projectile.ai[1];
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

        public override bool PreDraw(ref Color lightColor)
        {
            var sb = Main.spriteBatch;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            float visualScale = Projectile.scale; // FULL size (no 0.25f)

            // --- optional shader pass (guarded) ---
            try
            {
                var filter = Filters.Scene["CalamityMod:HellBall"];
                var fx = filter?.GetShader()?.Shader;
                if (fx != null)
                {
                    sb.End();
                    sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, fx, Main.GameViewMatrix.TransformationMatrix);

                    fx.Parameters["time"]?.SetValue((600 - Projectile.timeLeft) / 60f * 0.24f);
                    fx.Parameters["blowUpPower"]?.SetValue(3.2f);
                    fx.Parameters["blowUpSize"]?.SetValue(0.4f);
                    fx.Parameters["noiseScale"]?.SetValue(0.6f);
                    fx.Parameters["shieldOpacity"]?.SetValue(Projectile.Opacity);
                    fx.Parameters["shieldEdgeBlendStrenght"]?.SetValue(4f);

                    Color deep = new Color(112, 40, 222);
                    Color light = new Color(178, 114, 255);
                    fx.Parameters["shieldColor"]?.SetValue(Color.Lerp(deep, light, 0.5f).ToVector3());
                    fx.Parameters["shieldEdgeColor"]?.SetValue(Color.Black.ToVector3());

                    Texture2D bloom = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeBloom").Value;
                    sb.Draw(bloom, pos, null, Color.White * Projectile.Opacity, 0f, bloom.Size() * 0.5f, visualScale * 0.9f, 0, 0);

                    sb.End();
                }
            }
            catch { /* filter missing -> skip */ }

            // --- additive vortex rings ---
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D vortex = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/SoulVortex").Value;
            Texture2D center = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeBloom").Value;

            for (int i = 0; i < 10; i++)
            {
                float angle = MathHelper.TwoPi * i / 3f + Main.GlobalTimeWrappedHourly * MathHelper.TwoPi;
                Color ring = Color.Lerp(new Color(140, 80, 255), new Color(200, 150, 255), i * 0.15f) * 0.7f;
                Vector2 drawPos = pos + (angle + Main.GlobalTimeWrappedHourly * i / 16f).ToRotationVector2() * 8f;

                Main.EntitySpriteDraw(vortex, drawPos, null, ring * Projectile.Opacity,
                    -angle + MathHelper.PiOver2, vortex.Size() * 0.5f,
                    visualScale * (1f - i * 0.05f), SpriteEffects.None, 0);
            }

            // dark core
            Main.EntitySpriteDraw(center, pos, null, Color.Black * Projectile.Opacity, 0f, center.Size() * 0.5f, visualScale * 0.8f, 0, 0);

            sb.End();
            // restore default
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}
