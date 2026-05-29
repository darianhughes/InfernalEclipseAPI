using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace WHummusMultiModBalancing
{
    public class PrismBreakTag : ModBuff
    {
        public override string Texture => "WHummusMultiModBalancing/Content/Empty";

        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }
}
