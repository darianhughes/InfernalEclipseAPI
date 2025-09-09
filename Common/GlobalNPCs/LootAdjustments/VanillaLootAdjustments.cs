using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Items.Placeables.Relics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalNPCs.LootAdjustments
{
    public class VanillaLootAdjustments : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.BloodNautilus)
            {
                npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<DreadnautilusRelic>()));
                npcLoot.Add(ItemDropRule.ByCondition(new RevengenceMode(), ModContent.ItemType<DreadnautilusRelic>(), 1, 1, 1, 1));
            }
            if (ModLoader.TryGetMod("HypnosMod", out Mod hypnos))
            {
                if (npc.type == hypnos.Find<ModNPC>("HypnosBoss").Type)
                {
                    npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<HypnosRelic>()));
                    npcLoot.Add(ItemDropRule.ByCondition(new RevengenceMode(), ModContent.ItemType<HypnosRelic>(), 1, 1, 1, 1));
                }
            }
        }
    }

    public class RevengenceMode : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        private static LocalizedText Description;

        public RevengenceMode()
        {
            if (RevengenceMode.Description != null)
                return;
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            GameModeData gameModeInfo = Main.GameModeInfo;
            if (gameModeInfo.IsMasterMode)
                return false;
            if (Terraria.ModLoader.ModLoader.HasMod("CalamityMod"))
            {
                Mod mod1 = Terraria.ModLoader.ModLoader.GetMod("CalamityMod");
                bool flag = (bool)mod1.Call(new object[2]
                {
        (object) "GetDifficultyActive",
        (object) "revengeance"
                });
                if (!flag)
                    flag = (bool)mod1.Call(new object[2]
                    {
          (object) "GetDifficultyActive",
          (object) "death"
                    });
                Mod mod2;
                if (Terraria.ModLoader.ModLoader.TryGetMod("FargowiltasSouls", out mod2) && !flag)
                {
                    int num;
                    if (!(bool)mod2.Call(new object[1]
                    {
          (object) "EternityMode"
                    }))
                        num = (bool)mod2.Call(new object[1]
                        {
            (object) "MasochistMode"
                        }) ? 1 : 0;
                    else
                        num = 1;
                    flag = num != 0;
                }
                Mod mod3;
                if (Terraria.ModLoader.ModLoader.TryGetMod("InfernumMode", out mod3) && !flag)
                    flag = (bool)mod3.Call(new object[1]
                    {
          (object) "GetInfernumActive"
                    });
                return flag;
            }
            Terraria.ModLoader.ModLoader.HasMod("CalamityMod");
            return false;
        }

        public bool CanShowItemDropInUI() => true;

        public string GetConditionDescription() => RevengenceMode.Description.Value;
    }
}
