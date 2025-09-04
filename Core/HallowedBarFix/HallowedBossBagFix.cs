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
            if (!IsMechBossBag(item.type))
                return;

            // 1) Always remove vanilla bar drops from bags
            itemLoot.RemoveWhere(IsHallowedBarDrop);

            // 2) Re-add bars gated by "all mechs down" (bags are Expert/Master anyway)
            var condRule = new LeadingConditionRule(new AllMechsDown_Cond());

            // Use Expert vs Normal counts if someone spawns bags in normal; otherwise Expert path applies in real worlds
            var normalBars = ItemDropRule.Common(ItemID.HallowedBar, 1, 15, 30);
            var expertBars = ItemDropRule.Common(ItemID.HallowedBar, 1, 20, 35);
            condRule.OnSuccess(new DropBasedOnExpertMode(expertBars, normalBars));

            itemLoot.Add(condRule);
        }

        private static bool IsMechBossBag(int type) =>
            type == ItemID.TwinsBossBag ||
            type == ItemID.SkeletronPrimeBossBag ||
            type == ItemID.DestroyerBossBag;

        private static bool IsHallowedBarDrop(IItemDropRule rule)
        {
            if (rule is CommonDrop cd) return cd.itemId == ItemID.HallowedBar;
            if (rule is DropBasedOnExpertMode ex)
                return (ex.ruleForNormalMode is CommonDrop n && n.itemId == ItemID.HallowedBar)
                    || (ex.ruleForExpertMode is CommonDrop e && e.itemId == ItemID.HallowedBar);
            return false;
        }
    }
}
