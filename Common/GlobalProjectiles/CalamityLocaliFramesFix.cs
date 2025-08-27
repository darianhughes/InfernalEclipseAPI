using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalProjectiles
{
    public class CalamityLocaliFramesFix : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile projectile)
        {
            var calamity = ModLoader.GetMod("CalamityMod");
            if (calamity == null)
                return;

            int pro1Type = calamity.Find<ModProjectile>("AcidGunStream")?.Type ?? -1;

            if (projectile.type == pro1Type)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 20;

                //Make sure it's NOT using static ID-based immunity
                projectile.usesIDStaticNPCImmunity = false;
            }
        }
    }
}
