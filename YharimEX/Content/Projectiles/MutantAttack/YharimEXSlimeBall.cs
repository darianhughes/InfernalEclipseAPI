using InfernalEclipseAPI.YharimEX.Content.NPCs.Bosses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using YharimEX.Core.Globals;
using YharimEX.Core.Systems;

namespace YharimEX.Content.Projectiles
{
    public class YharimEXSlimeBall : ModProjectile
    {
        public override string Texture => "YharimEX/Assets/Projectiles/YharimEXSlimeBall";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            CooldownSlot = 1;
            Projectile.scale = 1.5f;
            Projectile.alpha = 50;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 90 * (Projectile.extraUpdates + 1);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - (float)Math.PI / 2;
            if (Projectile.localAI[0] == 0) //choose a texture to use
            {
                Projectile.localAI[0] += Main.rand.Next(1, 4);
            }

            if (Projectile.timeLeft % Projectile.MaxUpdates == 0)
            {
                //if (Projectile.timeLeft < 45 * Projectile.MaxUpdates) Projectile.velocity *= 1.015f;

                if (++Projectile.frameCounter >= 6)
                {
                    Projectile.frameCounter = 0;
                    if (++Projectile.frame >= Main.projFrames[Projectile.type])
                        Projectile.frame = 0;
                }
            }

            if (++Projectile.localAI[1] > 10 && YharimEXGlobalUtilities.BossIsAlive(ref YharimEXGlobalNPC.yharimEXBoss, ModContent.NPCType<YharimEXBoss>()))
            {
                float yOffset = Projectile.Center.Y - Main.npc[YharimEXGlobalNPC.yharimEXBoss].Center.Y;
                if (Math.Sign(yOffset) == Math.Sign(Projectile.velocity.Y) && Projectile.Distance(Main.npc[YharimEXGlobalNPC.yharimEXBoss].Center) > 1200 + Projectile.ai[0])
                    Projectile.timeLeft = 0;
            }
        }

        public override void OnKill(int timeleft)
        {
            for (int i = 0; i < 20; i++)
            {
                int num469 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.BlueTorch, -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.BlueTorch, -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f, 100);
                Main.dust[num469].velocity *= 2f;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Slimed, 240);
            if (YharimEXWorldFlags.EternityMode && YharimEXCrossmodSystem.FargowiltasSouls.Loaded) target.AddBuff(YharimEXCrossmodSystem.FargowiltasSouls.Mod.Find<ModBuff>("MutantFangBuff").Type, 180);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            int sheetClamped = (int)Projectile.localAI[0];
            if (sheetClamped < 1)
                sheetClamped = 1;
            if (sheetClamped > 3)
                sheetClamped = 3;
            Texture2D texture2D13 = ModContent.Request<Texture2D>($"YharimEX/Assets/Projectiles/YharimEXSlimeBall_{sheetClamped}", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            int num156 = texture2D13.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = lightColor;
            color26 = Projectile.GetAlpha(color26);

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Color color27 = color26 * 0.5f;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.oldPos[i];
                float num165 = Projectile.oldRot[i];
                Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}