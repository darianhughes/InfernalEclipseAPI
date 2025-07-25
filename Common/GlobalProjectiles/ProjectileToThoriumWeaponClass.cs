using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using ThoriumMod.Items.Cultist;
using ThoriumMod;
using Terraria;
using ThoriumMod.Projectiles;
using CalamityMod.Projectiles.Melee;

namespace InfernalEclipseAPI.Common.GlobalProjectiles
{
    [ExtendsFromMod("ThoriumMod")]
    public class ProjectileToThoriumWeaponClass : GlobalProjectile
    {
        public override void SetDefaults(Projectile entity)
        {
            if (!InfernalConfig.Instance.ChanageWeaponClasses) return;

            if (entity.type == ModContent.ProjectileType<AncientFirePro>() || entity.type == ModContent.ProjectileType<AncientFirePro2>() || entity.type == ModContent.ProjectileType<BurningMeteor>())
            {
                entity.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            }
        }
    }
}
