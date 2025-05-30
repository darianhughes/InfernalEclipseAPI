using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.HallowedBarFix
{
    public class BossDropFix : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (!DownedAllMechBosses())
            {
                if (IsMechBoss(npc.type))
                {
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is CommonDrop commonDrop)
                            return commonDrop.itemId == ItemID.HallowedBar;

                        if (rule is DropBasedOnExpertMode expertDrop)
                        {
                            return (expertDrop.ruleForNormalMode is CommonDrop normalDrop && normalDrop.itemId == ItemID.HallowedBar)
                                || (expertDrop.ruleForExpertMode is CommonDrop expert && expert.itemId == ItemID.HallowedBar);
                        }

                        return false;
                    });
                }
            }
        }

        private bool DownedAllMechBosses()
        {
            return NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
        }

        private bool IsMechBoss(int npcType)
        {
            return npcType == NPCID.SkeletronPrime ||
                   npcType == NPCID.Retinazer ||
                   npcType == NPCID.Spazmatism ||
                   npcType == NPCID.TheDestroyer;
        }
    }
}
