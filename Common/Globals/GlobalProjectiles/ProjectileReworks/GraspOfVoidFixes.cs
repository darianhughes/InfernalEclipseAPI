namespace InfernalEclipseAPI.Common.GlobalProjectiles.ProjectileReworks
{
    //WH
    public class GraspOfVoidFixes : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile projectile)
        {
            ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok);
            if (ragnarok == null)
                return;

            int pro1Type = ragnarok.Find<ModProjectile>("GraspofVoidPro1")?.Type ?? -1;
            int pro2Type = ragnarok.Find<ModProjectile>("GraspofVoidPro2")?.Type ?? -1;

            if (projectile.type == pro1Type || projectile.type == pro2Type)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 80;

                projectile.usesIDStaticNPCImmunity = false;
            }
        }
    }
}
