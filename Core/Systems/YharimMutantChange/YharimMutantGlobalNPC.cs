using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FargowiltasSouls;
using FargowiltasSouls.Content.Bosses.AbomBoss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems.YharimMutantChange
{
    [ExtendsFromMod("FargowiltasSouls")]
    public class YharimMutantGlobalNPC : GlobalNPC
    {
        public override void ModifyTypeName(NPC npc, ref string typeName)
        {
            if (InfernalConfig.Instance.UseAprilFoolsMutant)
            {
                if (npc.type == ModContent.NPCType<FargowiltasSouls.Content.Bosses.MutantBoss.MutantBoss>())
                    typeName = "Godkiller Yharim";
                if (npc.type == ModContent.NPCType<AbomBoss>())
                    typeName = "Abominationn";
            }
        }

        public override void SetDefaults(NPC entity)
        {
            if (entity.type == ModContent.NPCType<AbomBoss>())
                entity.GivenName = "Abominationn";
        }
    }
}
