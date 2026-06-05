using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using InfernalEclipseAPI.Core.Systems;
using ThoriumRework;
using CalamityMod.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod.Particles;
using CalamityMod.Buffs;
using CalamityMod.Buffs.DamageOverTime;
using Terraria.DataStructures;
using ThoriumMod;

namespace InfernalEclipseAPI.Content.Buffs.SoulBurn
{
    public class AblazeGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public int ablazeTime;

        public override void AI(Projectile projectile)
        {
            if (ablazeTime > 0)
            {
                ablazeTime--;

                DrawEffects(projectile);
            }
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_Parent parentSource && parentSource.Entity is Projectile parentProjectile)
            {
                if (parentProjectile.GetGlobalProjectile<AblazeGlobalProjectile>().ablazeTime > 0)
                {
                    ablazeTime = parentProjectile.GetGlobalProjectile<AblazeGlobalProjectile>().ablazeTime;
                }
            }
        }

        public override void ModifyHitNPC( Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (ablazeTime > 0)
            {
                if (NPC.downedMoonlord)
                {
                    modifiers.SourceDamage += 0.25f;
                }
                else if (NPC.downedGolemBoss)
                {
                    modifiers.SourceDamage += 0.2f;
                }
                else if (NPC.downedPlantBoss)
                {
                    modifiers.SourceDamage += 0.15f;
                }
                else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                {
                    modifiers.SourceDamage += 0.1f;
                }
                else
                {
                    modifiers.SourceDamage += 0.05f;
                }
            }
        }

        public override void OnHitNPC( Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ablazeTime > 0)
            {
                if (NPC.downedMoonlord)
                {
                    target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 180);
                    target.AddBuff(ModContent.BuffType<StaticDischarge>(), 180);
                }
                else if (NPC.downedPlantBoss || NPC.downedGolemBoss)
                {
                    target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 180);
                }
                else
                {
                    target.AddBuff(BuffID.OnFire3, 180);
                }
            }
        }

        internal static void DrawEffects(Projectile projectile)
        {
            if (Main.rand.NextBool(4))
            {
                Vector2 position = projectile.Center +
                    new Vector2(
                        Main.rand.NextFloat(-projectile.width / 2f, projectile.width / 2f),
                        Main.rand.NextFloat(-projectile.height / 2f, projectile.height / 2f)
                    );

                Vector2 velocity =
                    new Vector2(0f, Main.rand.NextBool(4) ? -5f : -9f)
                    .RotatedByRandom(MathHelper.ToRadians(25f))
                    * Main.rand.NextFloat(0.1f, 1.9f);

                GeneralParticleHandler.SpawnParticle(
                    new CritSpark(
                        position,
                        velocity,
                        Main.rand.NextBool() ? Color.Red : Color.DarkRed,
                        Color.IndianRed,
                        0.8f,
                        15,
                        2f,
                        1.9f
                    )
                );
            }

            if (Main.rand.NextBool(4))
            {
                Vector2 dustPos = projectile.position - Vector2.One * 2f;

                Vector2 dustVel =
                    projectile.velocity * 0.25f +
                    new Vector2(0f, Main.rand.NextFloat(-5f, -1f));

                Dust dust = Dust.NewDustDirect(
                    dustPos,
                    projectile.width + 4,
                    projectile.height + 4,
                    87,
                    dustVel.X,
                    dustVel.Y
                );

                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(0.7f, 1.2f);
                dust.alpha = 235;
            }

            Lighting.AddLight(
                projectile.Center,
                0.25f,
                0.25f,
                0.1f
            );

            if (NPC.downedMoonlord)
            {
                Vector2 projSize = ((Entity)projectile).Center + new Vector2(Utils.NextFloat(Main.rand, (float)(-((Entity)projectile).width / 2), (float)(((Entity)projectile).width / 2)), Utils.NextFloat(Main.rand, (float)(-((Entity)projectile).height / 2), (float)(((Entity)projectile).height / 2)));
            _ = Utils.RotatedByRandom(new Vector2(0f, Utils.NextBool(Main.rand, 4) ? (-2f) : (-8f)), (double)MathHelper.ToRadians(Utils.NextBool(Main.rand, 3) ? 10f : 35f)) * Utils.NextFloat(Main.rand, 0.1f, 1.9f);
            if (Utils.NextBool(Main.rand, 4))
            {
                Dust.NewDustPerfect(projSize, 278, (Vector2?)(Utils.RotatedByRandom(new Vector2(2f, 2f), 100.0) * Utils.NextFloat(Main.rand, 0.3f, 0.7f)), 0, default(Color), Utils.NextFloat(Main.rand, 0.2f, 0.6f)).color = (Utils.NextBool(Main.rand, 3) ? Color.Yellow : Color.LightSkyBlue);
            }
            }
        }
    }
}