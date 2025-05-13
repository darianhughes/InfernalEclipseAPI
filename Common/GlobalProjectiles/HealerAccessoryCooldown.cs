using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Core.Players;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Minions;

namespace InfernalEclipseAPI.Common.GlobalProjectiles
{
    //Proived by Wardrobe Hummus
    public class HealerAccessoryCooldown : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!projectile.friendly || projectile.trap || projectile.minion || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;

            if (projectile.owner >= 0 && projectile.owner < Main.maxPlayers)
            {
                Player owner = Main.player[projectile.owner];
                if (owner.active && !owner.dead)
                {
                    HealerPlayer cc = owner.GetModPlayer<HealerPlayer>();
                    cc.OnProjectileHit();
                }
            }
        }

        public override bool InstancePerEntity => false;
    }
}
