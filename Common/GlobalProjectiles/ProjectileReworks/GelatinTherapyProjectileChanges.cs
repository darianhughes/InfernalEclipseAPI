using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalProjectiles.ProjectileReworks
{
    //WH
    public class GelatinTherapyProjectileChanges : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            if (projectile.type != KillTherapeuticSludgeSystem.SludgeProjType)
                return;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && projectile.Hitbox.Intersects(player.Hitbox))
                {
                    // Check if player is not at full health before assumed healing
                    if (player.statLife < player.statLifeMax2)
                    {
                        projectile.Kill(); // Kill immediately after healing
                        return;
                    }
                }
            }
        }
    }

    public class KillTherapeuticSludgeSystem : ModSystem
    {
        public static int SludgeProjType = -1;

        public override void Load()
        {
            if (ModLoader.HasMod("CalamityBardHealer"))
            {
                Mod calamityMod = ModLoader.GetMod("CalamityBardHealer");
                if (calamityMod != null && calamityMod.TryFind("TherapeuticSludge", out ModProjectile proj))
                {
                    SludgeProjType = proj.Type;
                }
            }
        }
    }
}
