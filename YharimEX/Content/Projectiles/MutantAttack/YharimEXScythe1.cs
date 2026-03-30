using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using YharimEX.Content.NPCs.Bosses;
using YharimEX.Core.Systems;
using Terraria.Graphics.Effects;
using ReLogic.Content;

namespace YharimEX.Content.Projectiles
{
    public class YharimEXScythe1 : ModProjectile
    {
        public static Asset<Texture2D> screamTex;
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            screamTex = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/ScreamyFace", AssetRequestMode.AsyncLoad);
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.alpha = 0;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            CooldownSlot = 1;

            Projectile.hide = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write7BitEncodedInt(Projectile.timeLeft);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.timeLeft = reader.Read7BitEncodedInt();
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            targetHitbox.Y = targetHitbox.Center.Y;
            targetHitbox.Height = Math.Min(targetHitbox.Width, targetHitbox.Height);
            targetHitbox.Y -= targetHitbox.Height / 2;
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void AI()
        {
            /*if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
            }*/
            if (Projectile.rotation == 0)
                Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);

            float modifier = (180f - Projectile.timeLeft + 90) / 180f; //2f - Projectile.timeLeft / 240f;
            if (modifier < 0)
                modifier = 0;
            if (modifier > 1)
                modifier = 1;
            Projectile.rotation += 0.1f + 0.7f * modifier;
            //Projectile.alpha = (int)(127f * (1f - modifier));

            if (Projectile.timeLeft < 180) //240)
            {
                if (Projectile.velocity == Vector2.Zero)
                {
                    Projectile.velocity = Projectile.ai[1].ToRotationVector2();
                    Projectile.netUpdate = true;
                }
                Projectile.velocity *= 1f + Projectile.ai[0];
            }
            /*for (int i = 0; i < 6; i++)
            {
                Vector2 offset = new Vector2(0, -20).RotatedBy(Projectile.rotation);
                offset = offset.RotatedByRandom(MathHelper.Pi / 6);
                int d = Dust.NewDust(Projectile.Center, 0, 0, 229, 0f, 0f, 150);
                Main.dust[d].position += offset;
                float velrando = Main.rand.Next(20, 31) / 10;
                Main.dust[d].velocity = Projectile.velocity / velrando;
                Main.dust[d].noGravity = true;
            }*/
        }

        public override void PostAI()
        {
            /*if (Projectile.FargoSouls().GrazeCD == 6)
                Projectile.FargoSouls().GrazeCD = 60;
            else if (Projectile.FargoSouls().GrazeCD == 7)
                Projectile.FargoSouls().GrazeCD = 5;*/
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Shader + scream texture pass
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
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

            Color edgeColor = Color.Black * opacity;
            Color shieldColor = Color.Lerp(Color.Red, Color.Magenta, 0.5f) * opacity;

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
                scale * 0.25f * Projectile.scale * Projectile.Opacity, // quarter size
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
                Color outerColor = Color.Lerp(Color.Red, Color.Magenta, i * 0.15f);
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
                    (Projectile.scale * (1 - i * 0.05f) * 0.25f) * Projectile.Opacity, // quarter size
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
                (Projectile.scale * 0.9f * 0.25f) * Projectile.Opacity, // quarter size
                SpriteEffects.None,
                0
            );

            // Trail rendering
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = texture2D13.Height / Main.projFrames[Projectile.type];
            int y3 = num156 * Projectile.frame;
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = Projectile.GetAlpha(lightColor);
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Color color27 = color26 * ((ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type]);
                Vector2 value4 = Projectile.oldPos[i];
                float num165 = Projectile.oldRot[i];
                Main.EntitySpriteDraw(
                    texture2D13,
                    value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
                    rectangle,
                    color27,
                    num165,
                    origin2,
                    Projectile.scale * 0.25f, // quarter size
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
                Projectile.scale * 0.25f, // quarter size
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {

            if (YharimEXWorldFlags.EternityMode && YharimEXCrossmodSystem.FargowiltasSouls.Loaded) target.AddBuff(YharimEXCrossmodSystem.FargowiltasSouls.Mod.Find<ModBuff>("MutantFangBuff").Type, 180);
            target.AddBuff(BuffID.Bleeding, 600);
        }
    }
}