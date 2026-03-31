using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using InfernalEclipseAPI.YharimEX.Content.NPCs.Bosses;
using InfernalEclipseAPI.YharimEX.Core.Systems;
using InfernumMode.Core.GlobalInstances.Systems;
using InfernalEclipseAPI.YharimEX.Core.Globals;
using YharimEX.Assets.ExtraTextures;
using YharimEX.Assets.Sounds.Attacks;

namespace InfernalEclipseAPI.YharimEX.Content.Projectiles.MutantAttack
{
    public class YharimEXNuke : ModProjectile, IPixelatedPrimitiveRenderer
    {
        public override string Texture => "CalamityMod/Projectiles/Boss/AresGaussNukeProjectile";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fish Nuke");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.scale = 2f;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = WorldSaveSystem.InfernumModeEnabled ? 120 : 180;
            CooldownSlot = 1;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1f;
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            }

            if (!YharimEXUtils.BossIsAlive(ref YharimEXGlobalNPC.yharimEXBoss, ModContent.NPCType<YharimEXBoss>())
                || Main.npc[YharimEXGlobalNPC.yharimEXBoss].dontTakeDamage)
            {
                Projectile.Kill();
                return;
            }

            if (WorldSaveSystem.InfernumModeEnabled && Main.getGoodWorld)
            {
                if (++Projectile.localAI[2] > YharimEXBoss.HyperMax + 1)
                {
                    Projectile.localAI[2] = 0;
                    Projectile.AI();
                }
            }

            Projectile.velocity.Y += Projectile.ai[0];
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;

            if (++Projectile.localAI[0] >= 24f)
            {
                Projectile.localAI[0] = 0f;
                /*for (int index1 = 0; index1 < 36; ++index1)
                {
                    Vector2 vector2 = (Vector2.UnitX * -8f + -Vector2.UnitY.RotatedBy(index1 * 3.14159274101257 / 36 * 2, new Vector2()) * new Vector2(2f, 4f)).RotatedBy(Projectile.rotation - (float)Math.PI / 2, new Vector2());
                    int index2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.IceTorch, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].scale = 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].position = Projectile.Center + vector2 * 6f;
                    Main.dust[index2].velocity = Projectile.velocity * 0.0f
                }*/
            }

            /*Vector2 vector21 = Vector2.UnitY.RotatedBy(Projectile.rotation, new Vector2()) * 8f * 2;
            int index21 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch, 0.0f, 0.0f, 0, new Color(), 1f);
            Main.dust[index21].position = Projectile.Center + vector21;
            Main.dust[index21].scale = 1.5f;
            Main.dust[index21].noGravity = true;*/
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(YharimEXSoundRegistry.YharimEXNukeFishronExplosion with { Volume = 6}, Projectile.Center);
            int num1 = 36;
            for (int index1 = 0; index1 < num1; ++index1)
            {
                Vector2 vector2_1 = (Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f).RotatedBy((index1 - (num1 / 2 - 1)) * 6.28318548202515 / num1, new Vector2()) + Projectile.Center;
                Vector2 vector2_2 = vector2_1 - Projectile.Center;
                int index2 = Dust.NewDust(vector2_1 + vector2_2, 0, 0, DustID.DungeonWater, vector2_2.X * 2f, vector2_2.Y * 2f, 100, new Color(), 1.4f);
                Main.dust[index2].noGravity = true;
                Main.dust[index2].noLight = true;
                Main.dust[index2].velocity = vector2_2;
            }

            if (YharimEXUtils.HostCheck)
            {
                Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<YharimEXPhantasmalBlast>(), 0, 0f, Projectile.owner);
            }
        }

        public float WidthFunction(float completionRatio)
        {
            float baseWidth = Projectile.width * 1.2f;
            return MathHelper.SmoothStep(baseWidth, 3.5f, completionRatio);
        }

        public Color ColorFunction(float completionRatio)
        {
            return Color.Lerp(Color.DarkOrange, Color.Orange, completionRatio);
        }

        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            ManagedShader shader = ShaderManager.GetShader("InfernalEclipseAPI.YharimEXBlobTrail");
            YharimEXUtils.SetTexture1(YharimEXTextureRegistry.ColorNoiseMap.Value);
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(WidthFunction, ColorFunction, _ => Projectile.Size * 0.5f, Pixelate: true, Shader: shader), 25);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            /*for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 5)
            {
                Color color27 = Color.Cyan;
                color27.A = 0;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.oldPos[i];
                float num165 = Projectile.oldRot[i];
                Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, SpriteEffects.None, 0);
            }*/

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}