using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Buffs.DamageOverTime;

namespace InfernalEclipseAPI.Common.GlobalProjectiles.ProjectileReworks
{
    public class PyrogenGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (!projectile.hostile)
                return;

            // Try to get Clamity mod
            if (!ModLoader.TryGetMod("Clamity", out Mod clam))
                return;

            // Helper: check if projectile matches a Clamity projectile internal name
            bool IsClamityProj(string name)
            {
                return clam.Find<ModProjectile>(name)?.Type == projectile.type;
            }

            int intendedDamage = 0;
            bool applyDebuff = false;

            if (IsClamityProj("FireBarrage") || IsClamityProj("FireBarrageHoming"))
            {
                intendedDamage = 80;
                applyDebuff = true;
            }
            else if (IsClamityProj("Fireblast"))
            {
                intendedDamage = 130;
                applyDebuff = true;
            }
            else if (IsClamityProj("FireBomb") || IsClamityProj("Firethrower"))
            {
                intendedDamage = 70;
                applyDebuff = true;
            }
            else if (IsClamityProj("FireBombExplosion"))
            {
                intendedDamage = 100;
                applyDebuff = true;
            }
            else
            {
                return;
            }

            // Apply intended damage if too low
            if (info.Damage < intendedDamage)
            {
                target.Hurt(PlayerDeathReason.ByProjectile(target.whoAmI, projectile.whoAmI), intendedDamage, 0);
            }

            // Apply Brimstone Flames debuff if possible
            if (applyDebuff)
            {
                int brimstoneFlamesBuff = ModContent.BuffType<BrimstoneFlames>();

                if (brimstoneFlamesBuff != -1)
                {
                    target.AddBuff(brimstoneFlamesBuff, 300);
                }
            }
        }
    }
}
