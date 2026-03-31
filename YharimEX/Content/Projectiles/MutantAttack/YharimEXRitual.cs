using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using InfernalEclipseAPI.YharimEX.Content.NPCs.Bosses;
using YharimEX.Assets.ExtraTextures;

namespace InfernalEclipseAPI.YharimEX.Content.Projectiles.MutantAttack
{
    public class YharimEXRitual : YharimEXArena
    {
        public override string Texture => "InfernalEclipseAPI/YharimEX/Assets/Projectiles/YharimEXSphere";

        private const float realRotation = MathHelper.Pi / 140f;
        private bool MutantDead;

        public YharimEXRitual() : base(realRotation, 1200f, ModContent.NPCType<YharimEXBoss>(), visualCount: 48) { }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 2;
        }

        public override bool? CanDamage()
        {
            if (MutantDead)
                return false;
            return base.CanDamage();
        }

        protected override void Movement(NPC npc)
        {
            //int[] unmovingArenaStates =
            //{
            //    19, //pillars
            //    13, 14, 15, //predictive dash
            //    21, 22, 23 //direct dash
            //};

            float targetRotation;
            //if (unmovingArenaStates.Contains((int)npc.ai[0])) //be stationary
            if (npc.ai[0] == 19) //pillars
            {
                Projectile.velocity = Vector2.Zero;

                targetRotation = -realRotation / 2; //denote arena isn't moving
            }
            else if (npc.ai[0] == 49) //golem
            {
                if (npc.HasValidTarget && npc.ai[1] < 30) //snap it to player at start
                {
                    Projectile.velocity = (Main.player[npc.target].Center - Projectile.Center) / 10f;

                    targetRotation = realRotation;
                }
                else
                {
                    Projectile.velocity = Vector2.Zero;

                    targetRotation = -realRotation / 2; //denote arena isn't moving
                }
            }
            else
            {
                Projectile.velocity = npc.Center - Projectile.Center;
                if (npc.ai[0] == 36)
                    Projectile.velocity /= 20f; //much faster for slime rain
                else if (npc.ai[0] == 22 || npc.ai[0] == 23 || npc.ai[0] == 25)
                    Projectile.velocity /= 40f; //move faster for direct dash, predictive throw
                else
                    Projectile.velocity /= 60f;

                targetRotation = realRotation;
            }

            const float increment = realRotation / 40;
            if (rotationPerTick < targetRotation)
            {
                rotationPerTick += increment;
                if (rotationPerTick > targetRotation)
                    rotationPerTick = targetRotation;
            }
            else if (rotationPerTick > targetRotation)
            {
                rotationPerTick -= increment;
                if (rotationPerTick < targetRotation)
                    rotationPerTick = targetRotation;
            }

            MutantDead = npc.ai[0] <= -6;
        }

        public override void AI()
        {
            base.AI();
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 1)
                    Projectile.frame = 0;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(target, info);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity * (targetPlayer == Main.myPlayer && !MutantDead ? 1f : 0.15f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color outerColor = Color.Red;
            outerColor.A = 0;

            Color darkColor = outerColor;
            Color mediumColor = Color.Lerp(outerColor, Color.White, 0.75f);
            Color lightColor2 = Color.Lerp(outerColor, Color.White, 0.5f);

            Vector2 auraPos = Projectile.Center;

            float leeway = Projectile.width / 2 * Projectile.scale;
            leeway *= 0.75f;
            float radius = threshold - leeway;

            var target = Main.LocalPlayer;

            var blackTile = TextureAssets.MagicPixel;
            var diagonalNoise = YharimEXTextureRegistry.WavyNoise;

            var maxOpacity = Projectile.Opacity;
            float scale = Projectile.scale * 0.5f;

            ManagedShader borderShader = ShaderManager.GetShader("InfernalEclipseAPI.YharimEXP2Aura");
            borderShader.TrySetParameter("colorMult", 7.35f);
            borderShader.TrySetParameter("time", Main.GlobalTimeWrappedHourly);
            borderShader.TrySetParameter("radius", radius * scale);
            borderShader.TrySetParameter("anchorPoint", auraPos);
            borderShader.TrySetParameter("screenPosition", Main.screenPosition);
            borderShader.TrySetParameter("screenSize", Main.ScreenSize.ToVector2());
            borderShader.TrySetParameter("playerPosition", target.Center);
            borderShader.TrySetParameter("maxOpacity", maxOpacity);
            borderShader.TrySetParameter("darkColor", darkColor.ToVector4());
            borderShader.TrySetParameter("midColor", mediumColor.ToVector4());
            borderShader.TrySetParameter("lightColor", lightColor2.ToVector4());

            Main.spriteBatch.GraphicsDevice.Textures[1] = diagonalNoise.Value;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.None, Main.Rasterizer, borderShader.WrappedEffect, Main.GameViewMatrix.TransformationMatrix);
            Rectangle rekt = new(Main.screenWidth / 2, Main.screenHeight / 2, Main.screenWidth, Main.screenHeight);
            Main.spriteBatch.Draw(blackTile.Value, rekt, null, default, 0f, blackTile.Value.Size() * 0.5f, 0, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        /*    Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = Projectile.GetAlpha(lightColor);
            Texture2D glow = ModContent.Request<Texture2D>("InfernalEclipseAPI/YharimEX/Assets/Projectiles/YharimEXSphereGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            int rect1 = glow.Height;
            int rect2 = 0;
            Rectangle glowrectangle = new(0, rect2, glow.Width, rect1);
            Vector2 gloworigin2 = glowrectangle.Size() / 2f;
            Color glowcolor = Color.Lerp(Color.Red, Color.Transparent, 0.8f);

            for (int x = 0; x < 32; x++)
            {
                Vector2 drawOffset = new Vector2(threshold * Projectile.scale / 2f, 0f).RotatedBy(Projectile.ai[0]);
                drawOffset = drawOffset.RotatedBy(2f * MathHelper.Pi / 32f * x);
                const int max = 4;
                for (int i = 0; i < max; i++)
                {
                    Color color27 = color26;
                    color27 *= (float)(max - i) / max;
                    Vector2 value4 = Projectile.oldPos[i] + Projectile.Hitbox.Size() / 2 + drawOffset.RotatedBy(rotationPerTick * -i);
                    float num165 = Projectile.rotation;
                    Main.EntitySpriteDraw(texture2D13, value4 - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, SpriteEffects.None, 0);
                    Main.EntitySpriteDraw(glow, value4 - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(glowrectangle), glowcolor * ((float)(max - i) / max),
                        Projectile.rotation, gloworigin2, Projectile.scale * 1.4f, SpriteEffects.None, 0);
                }
                Main.EntitySpriteDraw(texture2D13, Projectile.Center + drawOffset - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(glow, Projectile.Center + drawOffset - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(glowrectangle), glowcolor,
                    Projectile.rotation, gloworigin2, Projectile.scale * 1.3f, SpriteEffects.None, 0);
            }
            return false;
            */
        }
    }
}