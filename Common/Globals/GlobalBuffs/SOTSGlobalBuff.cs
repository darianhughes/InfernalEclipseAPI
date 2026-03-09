using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOTS;
using SOTS.Buffs;

namespace InfernalEclipseAPI.Common.Globals.GlobalBuffs
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class SOTSGlobalBuff : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            if (type == ModContent.BuffType<GoodVibes>())
            {
                if (!(Math.Abs(player.velocity.X) >= 0.10000000149011612 || (double)Math.Abs(player.velocity.Y) >= 0.10000000149011612))
                {
                    player.SOTSPlayer().attackSpeedMod -= 0.10f;
                    player.lifeRegen -= 4;
                }
            }
        }
    }
}
