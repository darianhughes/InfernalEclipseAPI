using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;

namespace InfernalEclipseAPI.Content.Projectiles
{
    [ExtendsFromMod("ThoriumMod")]
    public class RadioMicPro : BardProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_645";

        public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;

        public override void SetBardDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Prevent default drawing of the projectile's texture
            return false;
        }

        public override void AI()
        {
            Projectile.ai[1] += 1f;

            if (Projectile.ai[1] >= 0f)
            {
                // Always use pink dust
                int dustType = DustID.PinkTorch;

                Projectile.scale += 0.4f;

                int numDusts = 30;
                for (int i = 0; i < numDusts; i++)
                {
                    Vector2 offset = Vector2.UnitY.RotatedBy((float)i * MathHelper.TwoPi / numDusts) * new Vector2(6f, 14f) * Projectile.scale;
                    offset = offset.RotatedBy(Projectile.velocity.ToRotation());

                    Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, dustType, offset.SafeNormalize(Vector2.UnitY), 0, default, 1f);
                    dust.noGravity = true;
                }

                Projectile.ai[1] = -5f;
            }

            // Resize based on scale
            Vector2 center = Projectile.Center;
            Projectile.Size = new Vector2(20f * Projectile.scale);
            Projectile.Center = center;
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 4; i++) // 4 projectiles
            {
                Vector2 velocity = Main.rand.NextVector2Unit() * 6f;
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    target.Center,
                    velocity,
                    ModContent.ProjectileType<RadioMicShadowBurst>(),
                    Projectile.damage / 10,
                    1f,
                    Projectile.owner
                );
            }
            target.AddBuff(BuffID.ShadowFlame, 180);

            SoundEngine.PlaySound(SoundID.Item20, target.Center);
        }

        public override Color? GetAlpha(Color lightColor) => Color.HotPink;
    }
}
