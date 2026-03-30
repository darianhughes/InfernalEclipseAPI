using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using InfernalEclipseAPI.YharimEX.Content.NPCs.Bosses;
using InfernalEclipseAPI.YharimEX.Core.Systems;
using CalamityMod.World;
using InfernumMode.Core.GlobalInstances.Systems;

namespace YharimEX.Content.Projectiles.DLCAttack
{
    public class DLCYharimSpearSpin : ModProjectile
    {
        public override string Texture => "YharimEX/Assets/Projectiles/YharimAtlantis";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 138;
            Projectile.height = 138;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
            CooldownSlot = 1;
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

            NPC mutant = Main.npc[(int)Projectile.ai[0]];
            if (mutant.active && mutant.type == ModContent.NPCType<YharimEXBoss>())
            {
                Projectile.Center = mutant.Center;
                direction = mutant.direction;

                if (mutant.GetGlobalNPC<CalDLCEmodeAttacks>().DLCAttackChoice != CalDLCEmodeAttacks.DLCAttack.None)
                {
                    Projectile.rotation += (float)Math.PI / 6.85f * Projectile.localAI[1];

                    if (++Projectile.localAI[0] > 7)
                    {
                        Projectile.localAI[0] = 0;
                        if (YharimEXUtils.HostCheck)
                        {
                            Vector2 speed = Vector2.UnitY.RotatedByRandom(Math.PI / 2) * Main.rand.NextFloat(1f, 2f);
                            if (mutant.Center.Y < Main.player[mutant.target].Center.Y)
                                speed *= -1f;
                            float ai1 = Projectile.timeLeft + Main.rand.Next(Projectile.timeLeft / 2);
                            int p = Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.position + Main.rand.NextVector2Square(0f, Projectile.width),
                                speed, ModContent.ProjectileType<YharimFrostMist>(), Projectile.damage, 0f, Projectile.owner, ai1: 55);
                        }
                    }

                    if (Projectile.timeLeft % 20 == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                    }

                    if (mutant.ai[0] == 13)
                        predictive = true;

                    Projectile.alpha = 0;
                }
                else
                {
                    Projectile.alpha = 0;
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
            Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), target.Center + Main.rand.NextVector2Circular(100, 100), Vector2.Zero, ModContent.ProjectileType<YharimEXPhantasmalBlast>(), 0, 0f, Projectile.owner);
            if (CalamityWorld.death)
            {
                target.YharimPlayer().MaxLifeReduction += 100;
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

            if (WorldSaveSystem.InfernumModeEnabled) //reflects projs indicator trail
            {
                for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.1f)
                {
                    Texture2D glow = ModContent.Request<Texture2D>("YharimEX/Assets/Projectiles/YharimEXPenetratorSpinGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                    Color color27 = Color.Lerp(new Color(51, 255, 191, 210), Color.Transparent, (float)Math.Cos(Projectile.ai[0]) / 3 + 0.3f);
                    color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                    float scale = Projectile.scale - (float)Math.Cos(Projectile.ai[0]) / 5;
                    scale *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                    int max0 = Math.Max((int)i - 1, 0);
                    Vector2 center = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
                    float smoothtrail = i % 1 * (float)Math.PI / 6.85f;
                    bool withinangle = Projectile.rotation > -Math.PI / 2 && Projectile.rotation < Math.PI / 2;
                    if (withinangle && direction == 1)
                        smoothtrail *= -1;
                    else if (!withinangle && direction == -1)
                        smoothtrail *= -1;

                    center += Projectile.Size / 2;

                    Vector2 offset = (Projectile.Size / 4).RotatedBy(Projectile.oldRot[(int)i] - smoothtrail * -Projectile.direction);
                    Main.EntitySpriteDraw(
                        glow,
                        center - offset - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
                        null,
                        color27,
                        Projectile.rotation,
                        glow.Size() / 2,
                        scale * 0.4f,
                        SpriteEffects.None,
                        0);
                }
            }

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
                Color glowColor = predictive ? new Color(0, 0, 255, 210) : new Color(51, 255, 191, 210);
                glowColor *= 1f - modifier;
                float glowScale = Projectile.scale * 8f * modifier;
                Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(glow.Bounds), glowColor, 0, glow.Bounds.Size() / 2, glowScale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
