using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using InfernalEclipseAPI.Core.Systems;
using SOTS.Projectiles.Minions;
using SOTS.Projectiles.Planetarium;

namespace InfernalEclipseAPI.Common.GlobalProjectiles
{
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    public class SOTSProjectileDebuffs : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.type == ModContent.ProjectileType<ThundershockShortbow>())
            {
                target.AddBuff(BuffID.Electrified, 60 * 3);
            }

            if (projectile.type == ModContent.ProjectileType<AncientSteelLantern>())
            {
                target.AddBuff(BuffID.OnFire, 120);
            }

            if (projectile.type == ModContent.ProjectileType<PermafrostSpirit>())
            {
                target.AddBuff(BuffID.Frostburn, 60 * 3);
            }

            if (projectile.type == ModContent.ProjectileType<EarthenSpirit>())
            {
                target.AddBuff(ModContent.BuffType<Crumbling>(), 60);
            }

            if (projectile.type == ModContent.ProjectileType<OtherworldlySpirit>())
            {
                target.AddBuff(BuffID.Electrified, 120);
            }

            if (projectile.type == ModContent.ProjectileType<TidalSpirit>())
            {
                target.AddBuff(ModContent.BuffType<CrushDepth>(), 60);
            }

            if (projectile.type == ModContent.ProjectileType<InfernoSpirit>())
            {
                target.AddBuff(BuffID.OnFire3, 60);
            }

            if (projectile.type == ModContent.ProjectileType<EvilSpirit>())
            {
                target.AddBuff(ModContent.BuffType<BrainRot>(), 60);
            }

            if (projectile.type == ModContent.ProjectileType<VoidspaceCell>())
            {
                target.AddBuff(BuffID.CursedInferno, 120);
            }
        }
    }
}
