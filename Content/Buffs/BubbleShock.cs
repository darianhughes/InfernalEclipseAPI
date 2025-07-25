using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Core.Players;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Buffs.Healer;

namespace InfernalEclipseAPI.Content.Buffs
{
    public class BubbleShock : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.pvpBuff[Type] = true;
        }
    }

    [ExtendsFromMod("ThoriumMod")]
    public class BubblBulwarkBuffChanges : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            if (type == ModContent.BuffType<BubbleBulwarkWandBuff>())
            {
                player.GetModPlayer<HealerPlayer>().buffBubbleBulwarkWandCooldown = true;
            }
        }
    }
}
