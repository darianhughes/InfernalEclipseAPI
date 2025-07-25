using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    //WH
    public class PurgatoriumNerf : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.ModItem != null
                && item.ModItem.Mod.Name == "CalamityBardHealer"
                && item.ModItem.Name == "PurgatoriumPandemonium";
        }

        public override void SetDefaults(Item item)
        {
            if (item.ModItem == null)
                return;

            if (item.ModItem.Name == "PurgatoriumPandemonium" && item.ModItem.Mod.Name == "CalamityBardHealer")
            {
                var modItemInstance = item.ModItem;
                var instanceType = modItemInstance.GetType();
                var healAmountField = instanceType.GetField("healAmount", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (healAmountField != null)
                {
                    healAmountField.SetValue(modItemInstance, 6);
                }
            }
        }
    }
}
