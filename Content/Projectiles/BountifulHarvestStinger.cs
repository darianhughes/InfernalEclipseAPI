using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace InfernalEclipseAPI.Content.Projectiles
{
    [ExtendsFromMod("ThoriumMod")]
    public class BountifulHarvestStinger : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Stinger;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Stinger);

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.Stinger;

            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int poisonDebuff = BuffID.Poisoned;
            int poisonDuration = 120; // 2 seconds

            target.AddBuff(poisonDebuff, poisonDuration);
        }

    }
}
