using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Buffs.DamageOverTime;
using Terraria.ModLoader;
using Terraria;

namespace YharimEX.Content.Projectiles.DLCAttack
{
    public class YharimEXAresBomb : YharimEXBomb
    {
        public override string Texture => "YharimEX/Assets/Projectiles/YharimEXAresBomb";
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 60 * 5);
            base.OnHitPlayer(target, info);
        }
    }
}
