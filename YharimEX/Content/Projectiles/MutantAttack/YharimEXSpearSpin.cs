using InfernalEclipseAPI.YharimEX.Content.NPCs.Bosses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using YharimEX.Content.Projectiles.FargoProjectile;
using YharimEX.Core.Globals;
using YharimEX.Core.Systems;

namespace YharimEX.Content.Projectiles
{
    public class YharimEXSpearSpin : ModProjectile
    {
        public override string Texture => "YharimEX/Assets/Projectiles/YharimEXSpear";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Penetrator");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 152;
            Projectile.height = 152;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
            CooldownSlot = 1;
            if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
            {
                SetupFargoProjectile SetupFargoProjectile = Projectile.GetGlobalProjectile<SetupFargoProjectile>();
                SetupFargoProjectile.TimeFreezeImmune = true;
                SetupFargoProjectile.DeletionImmuneRank = 2;
            }
        }

        private bool predictive;
        private int direction = 1;

        public override void AI()
        {
            if (Projectile.localAI[1] == 0)
            {
                Projectile.localAI[1] = Main.rand.NextBool() ? -1 : 1;
                Projectile.timeLeft = (int)Projectile.ai[1];
            }

            NPC yharimEX = Main.npc[(int)Projectile.ai[0]];
            if (yharimEX.active && yharimEX.type == ModContent.NPCType<YharimEXBoss>())
            {
                Projectile.Center = yharimEX.Center;
                direction = yharimEX.direction;

                if (yharimEX.ai[0] == 4 || yharimEX.ai[0] == 13 || yharimEX.ai[0] == 21)
                {
                    Projectile.rotation += (float)Math.PI / 6.85f * Projectile.localAI[1];

                    if (++Projectile.localAI[0] > 8)
                    {
                        Projectile.localAI[0] = 0;
                        if (YharimEXGlobalUtilities.HostCheck && Projectile.Distance(Main.player[yharimEX.target].Center) > 360)
                        {
                            Vector2 speed = Vector2.UnitY.RotatedByRandom(Math.PI / 2) * Main.rand.NextFloat(6f, 9f);
                            if (yharimEX.Center.Y < Main.player[yharimEX.target].Center.Y)
                                speed *= -1f;
                            float ai1 = Projectile.timeLeft + Main.rand.Next(Projectile.timeLeft / 2);
                            Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.position + Main.rand.NextVector2Square(0f, Projectile.width),
                                speed, ModContent.ProjectileType<YharimEXEyeHoming>(), Projectile.damage, 0f, Projectile.owner, yharimEX.target, ai1);
                        }
                    }

                    if (Projectile.timeLeft % 20 == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                    }

                    if (yharimEX.ai[0] == 13)
                        predictive = true;

                    Projectile.alpha = 0;
                }
                else
                {
                    Projectile.alpha = 255;
                }
            }
            else
            {
                Projectile.Kill();
                return;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), target.Center + Main.rand.NextVector2Circular(100, 100), Vector2.Zero, ModContent.ProjectileType<YharimEXBombSmall>(), 0, 0f, Projectile.owner);
            if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
            {
                if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
                {
                    if (YharimEXWorldFlags.DeathMode & !YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
                    {
                        target.YharimPlayer().MaxLifeReduction += 100;
                    }
                    else if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
                    {
                        EternityDebuffs.ManageOnHitDebuffs(target);
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.Opacity;

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
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

            if (Projectile.ai[1] > 0)
            {
                Texture2D glow = ModContent.Request<Texture2D>("YharimEX/Assets/Projectiles/YharimEXSpearAimGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                float modifier = Projectile.timeLeft / Projectile.ai[1];
                Color glowColor = new Color(255, 191, 51, 210);
                if (predictive)
                    glowColor = new Color(255, 0, 0, 210);
                glowColor *= 1f - modifier;
                float glowScale = Projectile.scale * 8f * modifier;
                Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(glow.Bounds), glowColor, 0, glow.Bounds.Size() / 2, glowScale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}