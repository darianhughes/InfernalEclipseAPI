using InfernalEclipseAPI.Core.Systems;

namespace InfernalEclipseAPI.Common.GlobalProjectiles
{
    public class InfernalGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile projectile)
        {
            var calamity = ModLoader.GetMod("CalamityMod");

            int pro1Type = calamity.Find<ModProjectile>("AcidGunStream")?.Type ?? -1;
            int pro2Type = calamity.Find<ModProjectile>("WaterLeechProj")?.Type ?? -1;

            if (projectile.type == pro1Type || projectile.type == pro2Type)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 20;

                projectile.usesIDStaticNPCImmunity = false;
            }
        }

        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];

            if (InfernalCrossmod.YouBoss.Loaded)
            {
                if (projectile.type == InfernalCrossmod.YouBoss.Mod.Find<ModProjectile>("FirstFractalHoldout").Type)
                {
                    if (player.mount.Active && player.altFunctionUse == 2)
                    {
                        player.mount.Dismount(player);
                    }
                    player.RemoveAllGrapplingHooks();
                }
            }

            return base.PreAI(projectile);
        }
    }
}
