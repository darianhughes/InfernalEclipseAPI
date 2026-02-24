using ThoriumMod.Projectiles;
using ThoriumMod.Buffs;

namespace InfernalEclipseAPI.Common.GlobalProjectiles.ProjectileReworks
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
                if (!target.boss)
                    target.AddBuff(ModContent.BuffType<Stunned>(), 30); // 1/2 second
            }
        }
    }
}
