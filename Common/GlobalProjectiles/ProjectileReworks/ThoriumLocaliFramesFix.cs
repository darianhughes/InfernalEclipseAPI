using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalProjectiles.ProjectileReworks
{
    [ExtendsFromMod("ThoriumMod")]
    public class ThoriumLocaliFramesFix : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile projectile)
        {
            var thorium = ModLoader.GetMod("ThoriumMod");
            if (thorium == null)
                return;

            int pro1Type = thorium.Find<ModProjectile>("CactusNeedlePro")?.Type ?? -1;
            int pro2Type = thorium.Find<ModProjectile>("BatScythePro2")?.Type ?? -1;

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
        }
    }
}
