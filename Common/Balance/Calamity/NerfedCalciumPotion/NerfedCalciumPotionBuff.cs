using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Potions;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.Balance.Calamity.NerfedCalciumPotion;

public class NerfedCalciumPotionBuff : GlobalItem
{
    public override void SetDefaults(Item entity)
    {
        if (!InfernalConfig.Instance.CalamityBalanceChanges  || entity.type != ModContent.ItemType<CalciumPotion>())
            return;
        entity.buffTime = 18000;
    }
}