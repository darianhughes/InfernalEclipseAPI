using InfernalEclipseAPI.Core.Systems;

namespace InfernalEclipseAPI.Common.GlobalProjectiles.ProjectileReworks
{
    [JITWhenModsEnabled(InfernalCrossmod.Thorium.Name)]
    [ExtendsFromMod(InfernalCrossmod.Thorium.Name)]
    public class ThoriumLocaliFramesFix : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile projectile)
        {
            Mod thorium = InfernalCrossmod.Thorium.Mod;

            int pro1Type = thorium.Find<ModProjectile>("CactusNeedlePro")?.Type ?? -1;
            int pro2Type = thorium.Find<ModProjectile>("BatScythePro2")?.Type ?? -1;
            int pro3type = thorium.Find<ModProjectile>("EchoWave")?.Type ?? -1;
            int pro4type = thorium.Find<ModProjectile>("PyroExplosion2")?.Type ?? -1;
            int pro5type = thorium.Find<ModProjectile>("PyroBurst")?.Type ?? -1;

            if (projectile.type == pro1Type)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 20;

                //Make sure it's NOT using static ID-based immunity
                projectile.usesIDStaticNPCImmunity = false;
            }

            if (projectile.type == pro2Type)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 10;

                //Make sure it's NOT using static ID-based immunity
                projectile.usesIDStaticNPCImmunity = false;
            }

            if (projectile.type == pro3type)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 20;

                //Make sure it's NOT using static ID-based immunity
                projectile.usesIDStaticNPCImmunity = false;
            }

            if (projectile.type == pro4type || projectile.type == pro5type)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 20;

                //Make sure it's NOT using static ID-based immunity
                projectile.usesIDStaticNPCImmunity = false;
            }

            if (InfernalCrossmod.ThoriumRework.Loaded)
            {
                Mod thoriumRework = InfernalCrossmod.ThoriumRework.Mod;

                int rework1Type = thoriumRework.Find<ModProjectile>("ValadiumHeavyScytheWave")?.Type ?? -1;
                int rework2Type = thoriumRework.Find<ModProjectile>("ValadiumHeavyScythe")?.Type ?? -1;

                if (projectile.type == rework1Type)
                {
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 40;

                    //Make sure it's NOT using static ID-based immunity
                    projectile.usesIDStaticNPCImmunity = false;
                }

                if (projectile.type == rework2Type)
                {
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 60;

                    //Make sure it's NOT using static ID-based immunity
                    projectile.usesIDStaticNPCImmunity = false;
                }
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Mod thorium = InfernalCrossmod.Thorium.Mod;

            int pyroExplosion = thorium.Find<ModProjectile>("PyroExplosion2")?.Type ?? -1;
            int pyroBurst = thorium.Find<ModProjectile>("PyroBurst")?.Type ?? -1;

            if (projectile.type != pyroExplosion && projectile.type != pyroBurst)
                return;

            // Kill Thorium's forced global iframes
            target.immune[projectile.owner] = 0;

            // Force local NPC immunity
            projectile.usesLocalNPCImmunity = true;
            projectile.usesIDStaticNPCImmunity = false;

            projectile.localNPCHitCooldown = 20;
        }
    }
}
