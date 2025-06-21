using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.DamageClasses.MergedRogueClass
{
    public class RogueMergeTooltips : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return InfernalConfig.Instance.MergeThrowerIntoRogue;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.defense > 0 || item.accessory)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                        tooltips[i].Text = Regex.Replace(tooltips[i].Text, "thrower", "rogue", RegexOptions.IgnoreCase);
                }
            }
        }
    }
}
