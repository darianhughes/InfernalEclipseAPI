using InfernalEclipseAPI.Core.Players;

namespace InfernalEclipseAPI.Common.GlobalProjectiles
{
    //Proived by Wardrobe Hummus
    public class HealerAccessoryCooldown : GlobalProjectile
    {
        /*
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
        */
    }
}
