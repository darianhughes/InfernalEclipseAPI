using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Projectiles;
using ThoriumMod.Buffs;
using CalamityMod;

namespace InfernalEclipseAPI.Common.Projectiles
{
    [ExtendsFromMod("ThoriumMod")]
    public class MagicStaffProjectileChange : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            int magickStaffProjID = ModContent.ProjectileType<MagickStaffPro>();

            if (projectile.type == magickStaffProjID)
            {
                target.AddBuff(BuffID.OnFire, 300);      // 5 seconds
                target.AddBuff(BuffID.Frostburn, 300);
                target.AddBuff(BuffID.Poisoned, 300);
                target.AddBuff(ModContent.BuffType<Stunned>(), 30); // 1/2 second
            }
        }
    }
}
