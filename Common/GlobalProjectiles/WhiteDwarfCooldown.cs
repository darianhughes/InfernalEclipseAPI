using InfernalEclipseAPI.Core.Players;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace InfernalEclipseAPI.Common.GlobalProjectiles
{
    [ExtendsFromMod("ThoriumMod")]
    public class WhiteDwarfCooldown : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];

            var thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            var cdPlayer = player.GetModPlayer<RogueThrowerPlayer>();

            if (thoriumPlayer.setWhiteDwarf && hit.Crit)
            {
                if (cdPlayer.whiteDwarfCooldown > 0)
                {
                    thoriumPlayer.setWhiteDwarf = false;
                }
                else
                {
                    cdPlayer.whiteDwarfCooldown = 120; // 2s
                }
            }
        }
    }
}
