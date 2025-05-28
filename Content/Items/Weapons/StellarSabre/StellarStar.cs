using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Buffs.DamageOverTime;

namespace InfernalEclipseAPI.Content.Items.Weapons.StellarSabre
{
    public class StellarStar : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Starfury; // Use Starfury projectile texture
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 1; // Starfury behavior
            AIType = ProjectileID.Starfury;
        }

        public override void AI()
        {
            // HOMING (Golem+)
            if (NPC.downedGolemBoss)
            {
                float homingRange = 320f;
                float homingStrength = 0.08f;
                NPC target = null;

                for (int i = 0; i < Main.maxNPCs; ++i)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(Projectile) && Vector2.Distance(Projectile.Center, npc.Center) < homingRange)
                    {
                        target = npc;
                        break;
                    }
                }

                if (target != null)
                {
                    Vector2 desiredVelocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, homingStrength);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // DAYBROKEN (Mechs+)
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120); // 2 seconds

            // SPLIT ON HIT (Plantera+)
            if (NPC.downedPlantBoss && Projectile.owner == Main.myPlayer && Projectile.penetrate > 1)
            {
                int numSplits = 2;
                float spread = MathHelper.ToRadians(24);

                for (int i = 0; i < numSplits; i++)
                {
                    float rotation = MathHelper.Lerp(-spread, spread, (float)i / (numSplits - 1));
                    Vector2 newVelocity = Projectile.velocity.RotatedBy(rotation) * 0.85f;
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        newVelocity,
                        Type,
                        Projectile.damage / 2,
                        Projectile.knockBack * 0.8f,
                        Projectile.owner
                    );
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            // EXPLODE ON DEATH (Cultist+)
            if (NPC.downedAncientCultist)
            {
                int explosionRadius = 48;
                int explosionDamage = Projectile.damage / 2;
                float explosionKnockback = Projectile.knockBack;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && npc.Distance(Projectile.Center) < explosionRadius)
                    {
                        NPC.HitInfo explosionInfo = new NPC.HitInfo()
                        {
                            Damage = explosionDamage,
                            Knockback = explosionKnockback,
                            HitDirection = npc.Center.X > Projectile.Center.X ? 1 : -1,
                            Crit = false
                        };
                        npc.StrikeNPC(explosionInfo, false); // The second param is fromNet, set to false for local
                    }
                }

                // Visual: fireworks dust
                for (int k = 0; k < 16; k++)
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FireworkFountain_Yellow);
            }

            // STARFALL (Moon Lord+)
            if (NPC.downedMoonlord && Projectile.owner == Main.myPlayer)
            {
                Vector2 fallVelocity = new Vector2(0, 14f);
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    new Vector2(Projectile.Center.X, Projectile.Center.Y - 300),
                    fallVelocity,
                    ProjectileID.Starfury, // vanilla star
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );
            }
        }
    }
}
