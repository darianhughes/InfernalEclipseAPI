using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.Projectiles.Boss;
using InfernumMode.Content.BehaviorOverrides.BossAIs.SupremeCalamitas;
using SOTS.Void;

namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs
{
    [ExtendsFromMod("SOTS")]
    public class SOTSVoidDamage : GlobalProjectile
    {
        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (projectile.type == ModContent.ProjectileType<SupremeCataclysmFist>() || projectile.type == ModContent.ProjectileType<SupremeCatastropheSlash>() || projectile.type == ModContent.ProjectileType<SupremeCataclysmFistOld>() || projectile.type == ModContent.ProjectileType<CatastropheSlash>())
            {
                int damage = 1 + projectile.damage / 6;
                VoidPlayer.VoidDamage(Mod, target, damage);
            }
        }
    }
}
