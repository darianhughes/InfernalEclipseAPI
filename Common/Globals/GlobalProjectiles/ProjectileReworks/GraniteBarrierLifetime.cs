namespace InfernalEclipseAPI.Common.GlobalProjectiles.ProjectileReworks
{
    public class GraniteBarrierLifetime : GlobalProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod))
            {
                if (projectile.ModProjectile != null &&
                    projectile.ModProjectile.Mod == thoriumMod &&
                    projectile.ModProjectile.Name == "GraniteBarrier" &&
                    projectile.localAI[0] == 0f)
                {
                    projectile.timeLeft = 3600;
                    projectile.localAI[0] = 1f;
                }
            }
        }
    }
}
