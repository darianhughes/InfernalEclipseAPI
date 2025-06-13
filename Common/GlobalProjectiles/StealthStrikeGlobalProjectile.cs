using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using InfernalEclipseAPI.Core.Enums;

namespace InfernalEclipseAPI.Common.GlobalProjectiles
{
    public class StealthStrikeGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool isStealthStrike = false;
        public StealthStrikeType stealthType = StealthStrikeType.None;
        private bool appliedChanges = false;
        public void SetupAsStealthStrike(StealthStrikeType type)
        {
            isStealthStrike = true;
            stealthType = type;
        }

        public override void AI(Projectile projectile)
        {
            if (!isStealthStrike) return;

            if (!appliedChanges)
            {
                appliedChanges = true;

                if (stealthType == StealthStrikeType.IcyTomahawk)
                {
                    if (projectile.penetrate < 8 || projectile.penetrate == -1)
                        projectile.penetrate = 8;

                    if (projectile.timeLeft < 240)
                        projectile.timeLeft = 240;
                }
            }

            if (stealthType == StealthStrikeType.IcyTomahawk && Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(
                    projectile.position,
                    projectile.width,
                    projectile.height,
                    DustID.Frost,
                    projectile.velocity.X * 0.2f,
                    projectile.velocity.Y * 0.2f,
                    100,
                    default,
                    1.2f
                );

                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!isStealthStrike) return;

            if (stealthType == StealthStrikeType.ZephyrsRuin)
            {
                modifiers.SetCrit();
            }
        }
    }
}