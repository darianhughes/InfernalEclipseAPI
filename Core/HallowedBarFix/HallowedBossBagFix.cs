using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOTS.Projectiles.Permafrost;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace InfernalEclipseAPI.Core.HallowedBarFix
{
    public class HallowedBossBagFix : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (DownedAllMechBosses())
                return;

            if (IsMechBossBag(item.type))
            {
                itemLoot.RemoveWhere(rule =>
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

        private bool DownedAllMechBosses()
        {
            return NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
        }

        private bool IsMechBossBag(int type)
        {
            return type == ItemID.TwinsBossBag ||
                   type == ItemID.SkeletronPrimeBossBag ||
                   type == ItemID.DestroyerBossBag;
        }
    }
}
