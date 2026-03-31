using InfernalEclipseAPI.YharimEX.Content.NPCs.Bosses;
using InfernalEclipseAPI.YharimEX.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using YharimEX.Assets.Sounds.Attacks;

namespace InfernalEclipseAPI.YharimEX.Content.Projectiles.MutantAttack
{
    public class YharimEXSword : ModProjectile
    {
        public override string Texture => "InfernalEclipseAPI/YharimEX/Assets/Projectiles/YharimEXSphere";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 110;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
                return true;

            //linger the hitbox so player doesn't phase through by flying towards it
            Rectangle trailHitbox = projHitbox;
            trailHitbox.X = (int)Projectile.oldPosition.X;
            trailHitbox.Y = (int)Projectile.oldPosition.Y;
            if (trailHitbox.Intersects(targetHitbox))
                return true;

            //lerp so there's no gap
            trailHitbox = projHitbox;
            trailHitbox.X = (int)MathHelper.Lerp(Projectile.position.X, Projectile.oldPosition.X, 0.5f);
            trailHitbox.Y = (int)MathHelper.Lerp(Projectile.position.Y, Projectile.oldPosition.Y, 0.5f);
            if (trailHitbox.Intersects(targetHitbox))
                return true;

            return false;
        }

        public override void AI()
        {
            //the important part
            NPC npc = YharimEXUtils.NPCExists(Projectile.ai[0], ModContent.NPCType<YharimEXBoss>());
            if (npc != null)
            {
                bool validSwordState =
                    npc.ai[0] == 9 && !(npc.localAI[2] == 2 && npc.ai[1] > 20)
                    || npc.ai[0] == 45 && npc.ai[2] != 0
                    || npc.ai[0] == 46 && npc.ai[1] <= 20;
                if (!validSwordState && YharimEXUtils.HostCheck)
                {
                    Projectile.Kill();
                    return;
                }

                if (Projectile.localAI[0] == 0)
                {
                    Projectile.localAI[0] = 1;
                    Projectile.localAI[1] = Projectile.DirectionFrom(npc.Center).ToRotation();
                }

                Vector2 offset = new Vector2(Projectile.ai[1], 0).RotatedBy(npc.ai[3] + Projectile.localAI[1]);
                Projectile.Center = npc.Center + offset;
            }
            else
            {
                Projectile.Kill();
                return;
            }

            //not important part
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 10;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }

            Projectile.scale = Projectile.Opacity;

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame > 1)
                    Projectile.frame = 0;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.velocity.X = target.Center.X < Main.npc[(int)Projectile.ai[0]].Center.X ? -15f : 15f;
            target.velocity.Y = -10f;
        }

        public override void OnKill(int timeleft)
        {
            SoundEngine.PlaySound(YharimEXSoundRegistry.YharimEXSword with { Volume = 2f}, Projectile.Center);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 208;
            Projectile.Center = Projectile.position;

            int dust = DustID.SolarFlare;

            for (int index1 = 0; index1 < 2; ++index1)
            {
                int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0.0f, 0.0f, 100, new Color(), 1.5f);
                Main.dust[index2].position = new Vector2(Projectile.width / 2, 0.0f).RotatedBy(6.28318548202515 * Main.rand.NextDouble(), new Vector2()) * (float)Main.rand.NextDouble() + Projectile.Center;
            }
            for (int index1 = 0; index1 < 5; ++index1)
            {
                int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dust, 0.0f, 0.0f, 0, new Color(), 2.5f);
                Main.dust[index2].position = new Vector2(Projectile.width / 2, 0.0f).RotatedBy(6.28318548202515 * Main.rand.NextDouble(), new Vector2()) * (float)Main.rand.NextDouble() + Projectile.Center;
                Main.dust[index2].noGravity = true;
                Dust dust1 = Main.dust[index2];
                dust1.velocity *= 1f;
                int index3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dust, 0.0f, 0.0f, 100, new Color(), 1.5f);
                Main.dust[index3].position = new Vector2(Projectile.width / 2, 0.0f).RotatedBy(6.28318548202515 * Main.rand.NextDouble(), new Vector2()) * (float)Main.rand.NextDouble() + Projectile.Center;
                Dust dust2 = Main.dust[index3];
                dust2.velocity *= 1f;
                Main.dust[index3].noGravity = true;
            }

            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dust, 0f, 0f, 100, default, 3f);
                Main.dust[d].velocity *= 1.4f;
            }

            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 7f;
                d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.5f);
                Main.dust[d].velocity *= 3f;
            }

            for (int index1 = 0; index1 < 10; ++index1)
            {
                int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dust, 0f, 0f, 100, new Color(), 2f);
                Main.dust[index2].noGravity = true;
                Main.dust[index2].velocity *= 21f * Projectile.scale;
                Main.dust[index2].noLight = true;
                int index3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dust, 0f, 0f, 100, new Color(), 1f);
                Main.dust[index3].velocity *= 12f;
                Main.dust[index3].noGravity = true;
                Main.dust[index3].noLight = true;
            }

            for (int i = 0; i < 10; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dust, 0f, 0f, 100, default, Main.rand.NextFloat(2f, 3.5f));
                if (Main.rand.NextBool(3))
                    Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= Main.rand.NextFloat(9f, 12f);
                Main.dust[d].position = Projectile.Center;
            }

            if (YharimEXUtils.HostCheck) //explosion
                Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<YharimEXBombSmall>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>("InfernalEclipseAPI/YharimEX/Assets/Projectiles/YharimEXSphereGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            int rect1 = glow.Height;
            int rect2 = 0;
            Rectangle glowrectangle = new(0, rect2, glow.Width, rect1);
            Vector2 gloworigin2 = glowrectangle.Size() / 2f;
            Color glowcolor = Color.Lerp(Color.Red, Color.Transparent, 0.85f);

            for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f) //reused betsy fireball scaling trail thing
            {

                Color color27 = glowcolor * 0.5f;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                float scale = Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.oldPos[(int)i];
                Main.EntitySpriteDraw(glow, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(glowrectangle), color27,
                    Projectile.velocity.ToRotation() + MathHelper.PiOver2, gloworigin2, scale * 1.5f, SpriteEffects.None, 0);
            }

            glowcolor = Color.Lerp(new Color(196, 247, 255, 0), Color.Transparent, 0.8f);
            Main.EntitySpriteDraw(glow, Projectile.position + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(glowrectangle), glowcolor,
                    Projectile.velocity.ToRotation() + MathHelper.PiOver2, gloworigin2, Projectile.scale * 1.5f, SpriteEffects.None, 0);

            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
        }
    }
}