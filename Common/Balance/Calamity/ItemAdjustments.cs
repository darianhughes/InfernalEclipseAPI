using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Accessories;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Weapons.Summon;
using Terraria.GameContent.ItemDropRules;
using InfernumMode.Content.Items.SummonItems;

namespace InfernalEclipseAPI.Common.Balance.Calamity
{
    public class ItemAdjustments : GlobalItem
    {
        //idk honestly i was on something when i wrote this code
        public override void ModifyItemLoot(Item item, ItemLoot loot)
        {
            if (!InfernalConfig.Instance.BossKillCheckOnOres) { return; }

            if (item.type == 3982 || item.type == 3983 || item.type == 3984 || item.type == 3985 || item.type == 4406 || item.type == 3981 || item.type == 3986 || item.type == 3980 || item.type == 3987 || item.type == 4878 || item.type == 4408 || item.type == 5003 || item.type == 3979)
            {
                ((ItemLoot)loot).RemoveWhere((Predicate<IItemDropRule>)(cobalto => cobalto is CommonDrop commonDrop1 && commonDrop1.itemId == 364), true);
                ((ItemLoot)loot).RemoveWhere((Predicate<IItemDropRule>)(cobaltb => cobaltb is CommonDrop commonDrop2 && commonDrop2.itemId == 381), true);
                ((ItemLoot)loot).RemoveWhere((Predicate<IItemDropRule>)(pallo => pallo is CommonDrop commonDrop3 && commonDrop3.itemId == 1104), true);
                ((ItemLoot)loot).RemoveWhere((Predicate<IItemDropRule>)(pallb => pallb is CommonDrop commonDrop4 && commonDrop4.itemId == 1184), true);
                ((ItemLoot)loot).RemoveWhere((Predicate<IItemDropRule>)(orio => orio is CommonDrop commonDrop5 && commonDrop5.itemId == 1105), true);
                ((ItemLoot)loot).RemoveWhere((Predicate<IItemDropRule>)(orib => orib is CommonDrop commonDrop6 && commonDrop6.itemId == 1191), true);
                ((ItemLoot)loot).RemoveWhere((Predicate<IItemDropRule>)(mytho => mytho is CommonDrop commonDrop7 && commonDrop7.itemId == 365), true);
                ((ItemLoot)loot).RemoveWhere((Predicate<IItemDropRule>)(mythb => mythb is CommonDrop commonDrop8 && commonDrop8.itemId == 382), true);
                ((ItemLoot)loot).RemoveWhere((Predicate<IItemDropRule>)(adamo => adamo is CommonDrop commonDrop9 && commonDrop9.itemId == 366), true);
                ((ItemLoot)loot).RemoveWhere((Predicate<IItemDropRule>)(adamb => adamb is CommonDrop commonDrop10 && commonDrop10.itemId == 391), true);
                ((ItemLoot)loot).RemoveWhere((Predicate<IItemDropRule>)(tito => tito is CommonDrop commonDrop11 && commonDrop11.itemId == 1106), true);
                ((ItemLoot)loot).RemoveWhere((Predicate<IItemDropRule>)(titb => titb is CommonDrop commonDrop12 && commonDrop12.itemId == 1198), true);
            }
            if (item.type == 3979 && item.type == 2334)
            {
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode), 381, 5, 2, 3);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode), 1184, 5, 2, 3);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode), 364, 7, 4, 15);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode), 1104, 7, 4, 15);
            }
            if (item.type == 3980 && item.type == 2335)
            {
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode), 381, 14, 3, 7);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode), 1184, 14, 3, 7);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode && NPC.downedMechBossAny), 381, 14, 3, 7);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode && NPC.downedMechBossAny), 1184, 14, 3, 7);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode), 364, 8, 12, 21);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode), 1104, 8, 12, 21);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode && NPC.downedMechBossAny), 364, 8, 12, 21);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode && NPC.downedMechBossAny), 1104, 8, 12, 21);
            }
            if (item.type == 3980 && item.type == 2335)
            {
                ((ILoot)(object)loot).AddIf((Func<bool>)(() =>
                {
                    if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 || NPC.downedMechBoss2 && NPC.downedMechBoss3)
                        return true;
                    return NPC.downedMechBoss3 && NPC.downedMechBoss1;
                }), 1106, 10, 25, 34);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() =>
                {
                    if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 || NPC.downedMechBoss2 && NPC.downedMechBoss3)
                        return true;
                    return NPC.downedMechBoss3 && NPC.downedMechBoss1;
                }), 366, 10, 25, 34);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode && NPC.downedMechBossAny), 365, 10, 25, 34);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode && NPC.downedMechBossAny), 1105, 10, 25, 34);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() =>
                {
                    if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 || NPC.downedMechBoss2 && NPC.downedMechBoss3)
                        return true;
                    return NPC.downedMechBoss3 && NPC.downedMechBoss1;
                }), 1198, 18, 12, 21);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() =>
                {
                    if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 || NPC.downedMechBoss2 && NPC.downedMechBoss3)
                        return true;
                    return NPC.downedMechBoss3 && NPC.downedMechBoss1;
                }), 391, 18, 12, 21);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode && NPC.downedMechBossAny), 382, 18, 12, 21);
                ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode && NPC.downedMechBossAny), 1191, 18, 12, 21);
            }
            if (item.type != 3982 && item.type != 3983 && item.type != 3984 && item.type != 3985 && item.type != 4406 && item.type != 3986 && item.type != 3987 && item.type != 4878 && item.type != 4408 && item.type != 5003)
                return;
            ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode), 364, 7, 35, 45);
            ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode), 1104, 7, 35, 45);
            ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode && NPC.downedMechBossAny), 365, 7, 35, 45);
            ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode && NPC.downedMechBossAny), 1105, 7, 35, 45);
            ((ILoot)(object)loot).AddIf((Func<bool>)(() =>
            {
                if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 || NPC.downedMechBoss2 && NPC.downedMechBoss3)
                    return true;
                return NPC.downedMechBoss3 && NPC.downedMechBoss1;
            }), 1106, 7, 35, 45);
            ((ILoot)(object)loot).AddIf((Func<bool>)(() =>
            {
                if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 || NPC.downedMechBoss2 && NPC.downedMechBoss3)
                    return true;
                return NPC.downedMechBoss3 && NPC.downedMechBoss1;
            }), 366, 7, 35, 45);
            ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode), 381, 17, 5, 16 /*0x10*/);
            ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode), 1184, 17, 5, 16 /*0x10*/);
            ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode && NPC.downedMechBossAny), 382, 17, 5, 16 /*0x10*/);
            ((ILoot)(object)loot).AddIf((Func<bool>)(() => Main.hardMode && NPC.downedMechBossAny), 1191, 17, 5, 16 /*0x10*/);
            ((ILoot)(object)loot).AddIf((Func<bool>)(() =>
            {
                if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 || NPC.downedMechBoss2 && NPC.downedMechBoss3)
                    return true;
                return NPC.downedMechBoss3 && NPC.downedMechBoss1;
            }), 1198, 17, 5, 16 /*0x10*/);
            ((ILoot)(object)loot).AddIf((Func<bool>)(() =>
            {
                if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 || NPC.downedMechBoss2 && NPC.downedMechBoss3)
                    return true;
                return NPC.downedMechBoss3 && NPC.downedMechBoss1;
            }), 391, 17, 5, 16 /*0x10*/);
        }
    }
}
