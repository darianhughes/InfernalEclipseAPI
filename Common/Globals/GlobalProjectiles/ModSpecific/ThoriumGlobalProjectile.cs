using ThoriumMod;
using ThoriumMod.Projectiles;
using CalamityMod.Projectiles.Melee;
using CalamityMod;
using Terraria;
using Terraria.DataStructures;
using InfernalEclipseAPI.Core.Systems;
using ThoriumMod.Projectiles.Thrower;
using System.Collections.Generic;

namespace InfernalEclipseAPI.Common.Globals.GlobalProjectiles.ModSpecific
{
    [ExtendsFromMod("ThoriumMod")]
    public class ThoriumGlobalProjectile : GlobalProjectile
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

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && !InfernalCrossmod.Hummus.Loaded)
            {
                int silverSpearTipProj = thorium.Find<ModProjectile>("SpearExtra")?.Type ?? -1;

                if (entity.type == silverSpearTipProj)
                {
                    if (entity.ModProjectile is ThoriumProjectile thoriumProj)
                    {
                        entity.aiStyle = 0;
                    }
                }
            }
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.Name.Contains("Super Plasma Cannon Pro"))
            {
                if (projectile.damage > 500)
                    projectile.damage = 500;
            }

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && !InfernalCrossmod.Hummus.Loaded)
            {
                int spiritBenderProj = thorium.Find<ModProjectile>("SpiritBendersStaffPro")?.Type ?? -1;

                if (projectile.type == spiritBenderProj)
                {
                    if (projectile.ModProjectile is ThoriumProjectile thoriumProj)
                    {
                        thoriumProj.lifeStealHealer = 2;
                    }
                }

                {
                    int silverSpearTipProj = thorium.Find<ModProjectile>("SpearExtra")?.Type ?? -1;
                    int moltenSpearTipProj = thorium.Find<ModProjectile>("SpearExtraFlame")?.Type ?? -1;
                    int crystalSpearTipProj = thorium.Find<ModProjectile>("SpearExtraCrystal")?.Type ?? -1;

                    if (projectile.type == silverSpearTipProj || projectile.type == moltenSpearTipProj || projectile.type == crystalSpearTipProj)
                    {
                        if (projectile.ModProjectile is ThoriumProjectile thoriumProj)
                        {
                            projectile.ai[2] = -1f;
                        }
                    }
                }

                if (projectile.type == ModContent.ProjectileType<ShadowPurgeCaltropPro>())
                {
                    List<Projectile> caltrops = new();

                    // Gather all active caltrops for this player
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        Projectile p = Main.projectile[i];

                        if (!p.active)
                            continue;

                        if (p.owner != projectile.owner)
                            continue;

                        if (p.type != ModContent.ProjectileType<ShadowPurgeCaltropPro>())
                            continue;

                        caltrops.Add(p);
                    }

                    // If we exceed 10, remove the one with lowest timeLeft
                    if (caltrops.Count > 10)
                    {
                        Projectile lowest = null;

                        foreach (var p in caltrops)
                        {
                            if (lowest == null || p.timeLeft < lowest.timeLeft)
                                lowest = p;
                        }

                        if (lowest != null && lowest.active)
                        {
                            lowest.Kill();
                        }
                    }
                }
            }
        }
    }
}
