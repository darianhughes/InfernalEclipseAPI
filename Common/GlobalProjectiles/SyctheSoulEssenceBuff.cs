using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Core.Players;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.Projectiles
{
    public class SyctheSoulEssenceBuff : GlobalProjectile
    {
        public override bool InstancePerEntity => false;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.owner < 0 || projectile.owner >= byte.MaxValue || !InfernalConfig.Instance.ThoriumBalanceChangess) return;

            HealerPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<HealerPlayer>();
            if (!modPlayer.fifthScytheTypes.Contains(projectile.type) || !modPlayer.CanTriggerChargeEffect())
                return;
            modPlayer.TriggerScytheCharge();
        }
    }
}
