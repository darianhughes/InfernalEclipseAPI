using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Buffs
{
    [ExtendsFromMod("ThoriumMod")]
    public class BrokenOath : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            // BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; // optional
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // -10 bonus healing while debuffed
            var thor = player.GetModPlayer<ThoriumMod.ThoriumPlayer>();
            if (thor != null)
            {
                thor.healBonus -= 10;
            }
        }
    }
}
