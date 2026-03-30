using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Buffs.StatDebuffs;
using Terraria.ModLoader;
using Terraria;

namespace YharimEX.Content.Projectiles.DLCAttack
{
    public class YharimEXOldDuke : YharimEXFishron
    {
        public override string Texture => "CalamityMod/NPCs/OldDuke/OldDuke";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 7;
        }
        public override bool PreAI()
        {
            return true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 60 * 10);
            base.OnHitPlayer(target, info);
        }
    }
}
