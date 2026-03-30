using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using YharimEX.Core.Globals;
using Terraria.Graphics.Effects;
using ReLogic.Content;

namespace YharimEX.Content.Projectiles
{
    public class YharimEXScythe2 : YharimEXScythe1
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            screamTex = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/ScreamyFace", AssetRequestMode.AsyncLoad);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.hide = false;
        }

        public override void PostAI()
        {
            if (Projectile.timeLeft == 180) //draw attention to myself
            {
                for (int i = 0; i < 20; i++)
                {
                    int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, 0, 0, 0, default, 2.5f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 6f;
                }
                if (YharimEXGlobalUtilities.HostCheck)
                    Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<YharimEXIronParry>(), 0, 0f, Main.myPlayer);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            lightColor.R = (byte)(255 * Projectile.Opacity);

            Main.spriteBatch.End();
            Effect shieldEffect = Filters.Scene["CalamityMod:HellBall"].GetShader().Shader;
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, shieldEffect, Main.GameViewMatrix.TransformationMatrix);

            float noiseScale = 0.6f;

            // Define shader parameters
            shieldEffect.Parameters["time"].SetValue(Projectile.timeLeft / 60f * 0.24f);
            shieldEffect.Parameters["blowUpPower"].SetValue(3.2f);
            shieldEffect.Parameters["blowUpSize"].SetValue(0.4f);
            shieldEffect.Parameters["noiseScale"].SetValue(noiseScale);

            float opacity = Projectile.Opacity;
            shieldEffect.Parameters["shieldOpacity"].SetValue(opacity);
            shieldEffect.Parameters["shieldEdgeBlendStrenght"].SetValue(4f);

            Color edgeColor = Color.Black * opacity;
            Color shieldColor = Color.Lerp(Color.Red, Color.Magenta, 0.5f) * opacity;

            // Define shader parameters for ball color
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

            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {

        }
    }
}