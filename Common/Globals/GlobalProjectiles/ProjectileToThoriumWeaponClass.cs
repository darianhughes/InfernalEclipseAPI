using ThoriumMod;
using ThoriumMod.Projectiles;
using CalamityMod.Projectiles.Melee;
using CalamityMod;

namespace InfernalEclipseAPI.Common.GlobalProjectiles
{
    [ExtendsFromMod("ThoriumMod")]
    public class ProjectileToThoriumWeaponClass : GlobalProjectile
    {
        public override void SetDefaults(Projectile entity)
        {
            if (entity.type == ModContent.ProjectileType<IceLancePro>() ||
                entity.type == ModContent.ProjectileType<SandStoneSpearPro>() ||
                entity.type == ModContent.ProjectileType<ForkPro>() ||
                entity.type == ModContent.ProjectileType<CoralPolearmPro>() ||
                entity.type == ModContent.ProjectileType<CoralPolearmPro2>() ||
                entity.type == ModContent.ProjectileType<CoralPolearmPro3>() ||
                entity.type == ModContent.ProjectileType<HarpyTalonPro>() ||
                entity.type == ModContent.ProjectileType<PearlPikePro>() ||
                entity.type == ModContent.ProjectileType<MoonlightPro>() ||
                entity.type == ModContent.ProjectileType<MoonlightPro2>() ||
                entity.type == ModContent.ProjectileType<EnergyStormPartisanPro>() ||
                entity.type == ModContent.ProjectileType<FleshSkewerPro>() ||
                entity.type == ModContent.ProjectileType<HellishHalberdPro>() ||
                entity.type == ModContent.ProjectileType<HellishHalberdPro2>() ||
                entity.type == ModContent.ProjectileType<ValadiumSpearPro>() ||
                entity.type == ModContent.ProjectileType<BloodGloryPro1>() ||
                entity.type == ModContent.ProjectileType<BloodGloryPro3>()
            )
            {
                entity.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            }

            if (!InfernalConfig.Instance.ChanageWeaponClasses) return;

            if (entity.type == ModContent.ProjectileType<AncientFirePro>() || entity.type == ModContent.ProjectileType<AncientFirePro2>() || entity.type == ModContent.ProjectileType<BurningMeteor>())
            {
                entity.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            }
        }
    }
}
