using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Buffs.Tag
{
    public class BlossomsBlessingTag : ModBuff
    {
        public override string Texture => "InfernalEclipseAPI/Assets/Textures/Empty";

        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }
}
