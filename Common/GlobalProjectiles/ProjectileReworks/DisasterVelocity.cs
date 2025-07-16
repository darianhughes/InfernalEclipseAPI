using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalProjectiles.ProjectileReworks
{
    //Wardrobe Hummus
    public class DisasterVelocity : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private bool velocityModified = false;

        public override void AI(Projectile projectile)
        {
            if (!velocityModified && !ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
            {
                // Check for the specific modded projectile
                if (projectile.ModProjectile != null &&
                    projectile.ModProjectile.Mod.Name == "CalamityBardHealer" &&
                    projectile.ModProjectile.Name == "ScorchingMaelstrom")
                {
                    projectile.velocity *= 4f; // or any multiplier
                    velocityModified = true;
                }
            }
        }
    }
}
