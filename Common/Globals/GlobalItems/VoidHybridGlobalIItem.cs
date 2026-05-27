using System.Collections.Generic;
using System.Linq;
using InfernalEclipseAPI.Core.Interfaces;
using SOTS.Buffs;
using SOTS.Items.Planetarium;
using SOTS.Void;
using Terraria.Localization;
using Terraria.UI;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class VoidHybridGlobalIItem : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.ModItem is IVoidHybrid;
        }

        public override void SetDefaults(Item item)
        {
            if (item.DamageType == DamageClass.Magic)
                item.DamageType = ModContent.GetInstance<VoidMagic>();
            else if (item.DamageType != DamageClass.Default)
                item.DamageType = ModContent.GetInstance<VoidGeneric>();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem is VoidItem)
                return;

            TooltipLine tooltipLine1 = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
            if (tooltipLine1 != null)
            {
                string str = (tooltipLine1.Text.Split(' ', StringSplitOptions.None)).First();
                string textValue = Language.GetTextValue("Mods.SOTS.Common.Damage");
                tooltipLine1.Text = Language.GetTextValue("Mods.SOTS.Common.Void2", str, textValue);
                if (item.CountsAsClass(DamageClass.Melee))
                    tooltipLine1.Text = Language.GetTextValue("Mods.SOTS.Common.VoidM", str, textValue);
                if (item.CountsAsClass(DamageClass.Ranged))
                    tooltipLine1.Text = Language.GetTextValue("Mods.SOTS.Common.VoidR", str, textValue);
                if (item.CountsAsClass(DamageClass.Magic))
                    tooltipLine1.Text = Language.GetTextValue("Mods.SOTS.Common.VoidM2", str, textValue);
                if (item.CountsAsClass(DamageClass.Summon))
                {
                    tooltipLine1.Text = Language.GetTextValue("Mods.SOTS.Common.VoidSPercent", textValue);
                    tooltips.FirstOrDefault((x => x.Name == "CritChance" && x.Mod == "Terraria"))?.Hide();
                }
            }

            int num = ((IVoidHybrid)item.ModItem).VoidCost;

            string voidCostString = Language.GetTextValue("Mods.SOTS.Common.CV", VoidCost(Main.LocalPlayer, item, num));
            if (item.mana > 0)
                tooltips.FirstOrDefault(x => x.Name == "UseMana" && x.Mod == "Terraria").Text = voidCostString;
            else
                tooltips.Insert(tooltips.FindIndex(x => x.Name == "Knockback" && x.Mod == "Terraria") + 1, new TooltipLine(((ModType)this).Mod, "VoidCost", voidCostString));
        }

        public static int VoidCost(Player player, Item item, int baseCost)
        {
            int num = (int)(baseCost * VoidPlayer.ModPlayer(player).voidCost);

            if (num < 1)
                num = 1;

            return num;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (item.ModItem is VoidItem)
                return true;

            if (player.HasBuff(ModContent.BuffType<VoidRecovery>()))
                return false;

            VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
            int baseCost = ((IVoidHybrid)item.ModItem).VoidCost;
            int num = VoidCost(player, item, baseCost);

            if (voidPlayer.safetySwitch && voidPlayer.voidMeter < num && !voidPlayer.frozenVoid)
                return false;

            if (item.mana > 0) 
            {
                if (player.statMana < item.mana)
                    return false;
            }

            ++player.GetModPlayer<BeadPlayer>().attackNum;
            if (player.whoAmI == Main.myPlayer)
                voidPlayer.voidMeter -= num;

            return true;
        }
    }
}
