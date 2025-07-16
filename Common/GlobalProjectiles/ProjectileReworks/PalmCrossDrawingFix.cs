using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace InfernalEclipseAPI.Common.GlobalProjectiles.ProjectileReworks
{
    [ExtendsFromMod("ThoriumMod")]
    public class PalmCrossDrawingFix : GlobalProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return !ModLoader.TryGetMod("WHummusMultiModBalancing", out _);
        }

        public override bool AppliesToEntity(Projectile projectile, bool lateInstantiation)
        {
            return projectile.ModProjectile?.Name == "PalmCrossPro";
        }

        public override void PostAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            bool holdingChannel = player.channel && !player.noItems && !player.CCed;
            bool previouslyHolding = projectile.localAI[1] == 1f;

            if (holdingChannel)
            {
                projectile.localAI[1] = 1f;
            }
            else
            {
                if (previouslyHolding)
                {
                    int dustType = 87;
                    if (player.GetModPlayer<ThoriumPlayer>().darkAura)
                        dustType = 86;

                    float empower = 0f;
                    var field = projectile.ModProjectile?.GetType().GetField("empower");
                    if (field != null)
                        empower = Convert.ToSingle(field.GetValue(projectile.ModProjectile));

                    float burstSpeed = 3.5f + empower * 0.5f;
                    int dustCount = 50;
                    float radius = 112f;

                    for (int i = 0; i < dustCount; ++i)
                    {
                        float angle = MathHelper.TwoPi * i / dustCount;
                        Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                        float scale = 0.5f + empower * 0.1f;
                        int dustIndex = Dust.NewDust(projectile.Center + offset, 0, 0, dustType, 0f, 0f, 0, default, scale);
                        Dust dust = Main.dust[dustIndex];
                        dust.noGravity = true;
                        dust.velocity = Vector2.Normalize(offset) * burstSpeed;
                    }
                }
                projectile.localAI[1] = 0f;
            }
        }
    }
}
