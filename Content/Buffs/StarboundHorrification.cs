using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.TransformItems;

namespace InfernalEclipseAPI.Content.Buffs
{
    public class StarboundHorrification : ModBuff
    {
        public override void SetStaticDefaults() 
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
