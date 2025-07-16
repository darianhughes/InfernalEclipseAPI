using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using Microsoft.Xna.Framework;
using ThoriumMod.Projectiles.Bard;

namespace InfernalEclipseAPI.Content.Projectiles
{
    [ExtendsFromMod("ThoriumMod")]
    public class RadioMicShadowBurst : BardProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_645";

        public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;

        public override bool PreDraw(ref Color lightColor)
        {
            // Prevent default drawing of the projectile's texture
            return false;
        }

        public override void SetBardDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
        }

        public override void AI()
        {
            Projectile.rotation += 0.3f;
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 150, Color.Black, 1.2f);
            Main.dust[dust].noGravity = true;
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Apply Shadowflame debuff for 3 seconds (180 ticks)
            target.AddBuff(BuffID.ShadowFlame, 180);
        }

        public override Color? GetAlpha(Color lightColor) => Color.Black;
    }
}
