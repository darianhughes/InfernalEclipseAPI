using Terraria.DataStructures;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ItemReworks.Armor
{
    public class IntergelaticArmorSetBonusDamageAdjustments : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.ModProjectile != null && projectile.ModProjectile.Mod.Name == "CatalystMod" && projectile.ModProjectile.Name == "IntergelacticMinionOrbitingRocks")
            {
                projectile.damage = (int)(projectile.damage * 1.5f);
            }

            if (projectile.ModProjectile != null && projectile.ModProjectile.Mod.Name == "CatalystMod" && projectile.ModProjectile.Name == "MiniStar")
            {
                projectile.damage /= 3;
            }
        }
    }
}
