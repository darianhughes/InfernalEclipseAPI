using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalProjectiles
{
    public class ThoriumStealthStrikeProjectiles : GlobalProjectile
    {
        public bool isStealthStrike = false;
        public override bool InstancePerEntity => true;

        // Track stealth state per player
        public static Dictionary<int, bool> StealthStrikeFromPlayer = new();

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.owner >= 0 && StealthStrikeFromPlayer.TryGetValue(projectile.owner, out bool wasStealth) && wasStealth)
            {
                isStealthStrike = true;
                StealthStrikeFromPlayer[projectile.owner] = false; // Reset flag
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (isStealthStrike)
            {
                modifiers.SetCrit(); // Guaranteed crit
            }
        }
    }
}
