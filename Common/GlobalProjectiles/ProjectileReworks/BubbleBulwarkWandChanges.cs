using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Buffs.Healer;
using ThoriumMod.Projectiles.Healer;
using Microsoft.Xna.Framework;
using InfernalEclipseAPI.Content.Buffs;
using Terraria.ID;

namespace InfernalEclipseAPI.Common.GlobalProjectiles.ProjectileReworks
{
    [ExtendsFromMod("ThoriumMod")]
    public class BubbleBulwarkWandChanges : GlobalProjectile
    {
        public override bool PreAI(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<BubbleBulwarkWandPro>())
            {
                for (int index1 = 0; index1 < 2; ++index1)
                {
                    int index2 = Dust.NewDust(
                        projectile.position - projectile.velocity,
                        projectile.width, projectile.height, DustID.GemTopaz, 0f, 0f, 100, new Color(), 0.65f);
                    Main.dust[index2].velocity *= 0.2f;
                    Main.dust[index2].noGravity = true;
                }
                for (int index3 = 0; index3 < 1; ++index3)
                {
                    int index4 = Dust.NewDust(
                        projectile.position - projectile.velocity,
                        projectile.width, projectile.height, DustID.Smoke, 0f, 0f, 150, new Color(), 1f);
                    Main.dust[index4].velocity *= 0.2f;
                    Main.dust[index4].noGravity = true;
                }

                if (projectile.timeLeft < 160)
                {
                    float num1 = projectile.Center.X;
                    float num2 = projectile.Center.Y;
                    float num3 = 750f;
                    Player player1 = Main.player[projectile.owner];
                    Player player2 = null;
                    int num4 = ModContent.BuffType<BubbleBulwarkWandBuff>();
                    int debuffType = ModContent.BuffType<BubbleShock>();
                    for (int index = 0; index < 255; ++index)
                    {
                        Player player3 = Main.player[index];
                        if (player3.active && !player3.dead && player3 != player1 && !player3.HasBuff(num4) && !player3.HasBuff(debuffType) &&
                            projectile.DistanceSQ(player3.Center) < num3 * num3 &&
                            Collision.CanHit(projectile.Center, 1, 1, player3.Center, 1, 1))
                        {
                            float num5 = player3.position.X + player3.width / 2f;
                            float num6 = player3.position.Y + player3.height / 2f;
                            if (Math.Abs(projectile.position.X + projectile.width / 2f - num5) +
                                Math.Abs(projectile.position.Y + projectile.height / 2f - num6) < num3)
                            {
                                player2 = player3;
                                num1 = num5;
                                num2 = num6;
                            }
                        }
                    }
                    if (player2 == null)
                        return false;

                    // --- NERFED HOMING STRENGTH ---
                    float num7 = 4f; // was 8f (lower = weaker homing)
                    Vector2 vector2 = new Vector2(
                        projectile.position.X + projectile.width * 0.5f,
                        projectile.position.Y + projectile.height * 0.5f);
                    float num8 = num1 - vector2.X;
                    float num9 = num2 - vector2.Y;
                    float num10 = (float)Math.Sqrt(num8 * num8 + num9 * num9);
                    if (num10 > 0.0)
                        num10 = num7 / num10;
                    float num11 = num8 * num10;
                    float num12 = num9 * num10;

                    // --- INCREASED SMOOTHING FACTOR ---
                    projectile.velocity.X = (float)(((double)projectile.velocity.X * 40.0 + num11) / 41.0); // was 20/21
                    projectile.velocity.Y = (float)(((double)projectile.velocity.Y * 40.0 + num12) / 41.0);

                    if (projectile.penetrate == 1 || projectile.DistanceSQ(player2.Center) >= 1600.0)
                        return false;

                    // --- NERFED BUFF TIME ---
                    if (!player2.HasBuff(debuffType))
                    {
                        player2.AddBuff(num4, 1800, true, false); // default: 1800
                    }

                    --projectile.penetrate;
                    projectile.timeLeft = 10;
                }
                else
                {
                    projectile.velocity *= 0.915f;
                }
                return false;
            }
            return true;
        }
    }
}
