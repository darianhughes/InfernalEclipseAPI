using CalamityMod.Items.Fishing;
using CalamityMod.Items.TreasureBags.MiscGrabBags;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;


namespace InfernalEclipseAPI.Core.Systems;

public class FishingSystem : ModSystem
{
    private static int[] furnitures = new int[19]
    {
    2442,
    2443,
    2444,
    2445,
    2497,
    2495,
    2446,
    2447,
    2448,
    2449,
    2490,
    2496,
    5235,
    5252,
    5259,
    5256,
    5263,
    5264,
    5265
    };

    public override void PostAddRecipes()
    {
        for (int index = 0; index < Recipe.numRecipes; ++index)
        {
            Recipe recipe = Main.recipe[index];
            Item obj;
            if (InfernalConfig.Instance.CalamityBalanceChanges && recipe.TryGetResult(4881, out obj))
                recipe.DisableRecipe();
        }
    }

    public override void OnModLoad()
    {
        On_Player.GetAnglerReward += NerfedAnglerRewards;
    }

    private static void NerfedAnglerRewards(On_Player.orig_GetAnglerReward orig, Player self, NPC angler, int questItemType)
    {
        if (!InfernalConfig.Instance.CalamityBalanceChanges)
        {
            orig.Invoke(self, angler, questItemType);
        }
        else
        {
            EntitySource_Gift entitySourceGift = new EntitySource_Gift(angler, null);
            int anglerQuestsFinished = self.anglerQuestsFinished;
            float num1 = 1f;
            float num2 = (anglerQuestsFinished <= 50 ? num1 - anglerQuestsFinished * 0.01f : anglerQuestsFinished <= 100 ? (float)(0.5 - (anglerQuestsFinished - 50) * 0.004999999888241291) : anglerQuestsFinished > 150 ? 0.15f : (float)(0.25 - (anglerQuestsFinished - 100) * (1.0 / 500.0))) * 0.9f * ((float)(self.currentShoppingSettings.PriceAdjustment + 1.0) / 2f);
            if ((double)num2 < 0.10000000149011612)
                num2 = 0.1f;
            List<Item> objList = new List<Item>();
            GetItemSettings inventorySettings = GetItemSettings.NPCEntityToPlayerInventorySettings;
            Item obj1 = new Item();
            Item obj2;
            switch (anglerQuestsFinished)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    obj2 = new Item();
                    obj2.SetDefaults(5132);
                    obj2.stack = Main.rand.Next(2, 6);
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    obj2 = new Item();
                    obj2.SetDefaults(2674);
                    obj2.stack = Main.rand.Next(2, 6);
                    break;
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    obj2 = new Item();
                    obj2.SetDefaults(Main.rand.NextBool() ? 2002 : 4363);
                    obj2.stack = Main.rand.Next(2, 6);
                    break;
                case 16 /*0x10*/:
                case 17:
                case 18:
                case 19:
                case 20:
                    obj2 = new Item();
                    obj2.SetDefaults(2675);
                    obj2.stack = Main.rand.Next(2, 6);
                    break;
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                    obj2 = new Item();
                    obj2.SetDefaults(Main.rand.NextBool() ? 3191 : 3194);
                    obj2.stack = Main.rand.Next(2, 6);
                    break;
                case 27:
                case 28:
                case 29:
                case 30:
                    obj2 = new Item();
                    obj2.SetDefaults(2676);
                    obj2.stack = Main.rand.Next(2, 6);
                    break;
                default:
                    obj2 = new Item();
                    obj2.SetDefaults(ModContent.ItemType<GrandMarquisBait>());
                    obj2.stack = Main.rand.Next(2, 6);
                    break;
            }
          obj2.position = self.Center;
            Item obj3 = self.GetItem(self.whoAmI, obj2, inventorySettings);
            objList.Add(obj3);
            Item obj4;
            switch (anglerQuestsFinished)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    obj4 = new Item();
                    obj4.SetDefaults(73);
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    Item obj5 = new Item();
                    obj5.SetDefaults(73);
                    obj5.stack = 2;
                    obj4 = new Item();
                    obj4.SetDefaults(72);
                    obj4.stack = 50;
                    break;
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                    obj4 = new Item();
                    obj4.SetDefaults(73);
                    obj4.stack = 4;
                    break;
                case 15:
                case 16 /*0x10*/:
                case 17:
                case 18:
                case 19:
                    obj4 = new Item();
                    obj4.SetDefaults(73);
                    obj4.stack = 6;
                    break;
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                    obj4 = new Item();
                    obj4.SetDefaults(73);
                    obj4.stack = 8;
                    break;
                default:
                    obj4 = new Item();
                    obj4.SetDefaults(73);
                    obj4.stack = 10;
                    break;
            }
          obj4.position = self.Center;
            Item obj6 = self.GetItem(self.whoAmI, obj4, inventorySettings);
            objList.Add(obj6);
            switch (anglerQuestsFinished)
            {
                case 0:
                case 1:
                    Item obj7 = new Item();
                    obj7.SetDefaults(ModContent.ItemType<Spadefish>());
                    objList.Add(obj7);
                    break;
                case 2:
                    Item obj8 = new Item();
                    obj8.SetDefaults(ModContent.ItemType<StuffedFish>());
                    obj8.stack = Main.rand.Next(4, 10);
                    objList.Add(obj8);
                    break;
                case 3:
                    Item obj9 = new Item();
                    obj9.SetDefaults(2373);
                    objList.Add(obj9);
                    break;
                case 4:
                    Item obj10 = new Item();
                    obj10.SetDefaults(2360);
                    objList.Add(obj10);
                    break;
                case 5:
                    Item obj11 = new Item();
                    obj11.SetDefaults(2428);
                    objList.Add(obj11);
                    break;
                case 6:
                    Item obj12 = new Item();
                    obj12.SetDefaults(3120);
                    objList.Add(obj12);
                    break;
                case 7:
                    Item obj13 = new Item();
                    obj13.SetDefaults(2498);
                    objList.Add(obj13);
                    Item obj14 = new Item();
                    obj14.SetDefaults(2499);
                    objList.Add(obj14);
                    Item obj15 = new Item();
                    obj15.SetDefaults(2500);
                    objList.Add(obj15);
                    Item obj16 = new Item();
                    obj16.SetDefaults(ModContent.ItemType<SandyAnglingKit>());
                    objList.Add(obj16);
                    break;
                case 8:
                    Item obj17 = new Item();
                    obj17.SetDefaults(4067);
                    objList.Add(obj17);
                    break;
                case 9:
                    Item obj18 = new Item();
                    obj18.SetDefaults(3200);
                    objList.Add(obj18);
                    break;
                case 10:
                    Item obj19 = new Item();
                    obj19.SetDefaults(2367);
                    objList.Add(obj19);
                    break;
                case 11:
                    Item obj20 = new Item();
                    obj20.SetDefaults(3037);
                    objList.Add(obj20);
                    break;
                case 12:
                    Item obj21 = new Item();
                    obj21.SetDefaults(5139);
                    objList.Add(obj21);
                    break;
                case 13:
                    Item obj22 = new Item();
                    obj22.SetDefaults(2368);
                    objList.Add(obj22);
                    break;
                case 14:
                    Item obj23 = new Item();
                    obj23.SetDefaults(2417);
                    objList.Add(obj23);
                    Item obj24 = new Item();
                    obj24.SetDefaults(2418);
                    objList.Add(obj24);
                    Item obj25 = new Item();
                    obj25.SetDefaults(2419);
                    objList.Add(obj25);
                    Item obj26 = new Item();
                    obj26.SetDefaults(ModContent.ItemType<SandyAnglingKit>());
                    objList.Add(obj26);
                    break;
                case 15:
                    Item obj27 = new Item();
                    obj27.SetDefaults(3096);
                    objList.Add(obj27);
                    break;
                case 16 /*0x10*/:
                    Item obj28 = new Item();
                    obj28.SetDefaults(2369);
                    objList.Add(obj28);
                    break;
                case 17:
                    Item obj29 = new Item();
                    obj29.SetDefaults(2375);
                    objList.Add(obj29);
                    break;
                case 18:
                    Item obj30 = new Item();
                    obj30.SetDefaults(3032);
                    objList.Add(obj30);
                    break;
                case 19:
                    Item obj31 = new Item();
                    obj31.SetDefaults(5303);
                    objList.Add(obj31);
                    break;
                case 20:
                    Item obj32 = new Item();
                    obj32.SetDefaults(4263);
                    objList.Add(obj32);
                    break;
                case 21:
                    Item obj33 = new Item();
                    obj33.SetDefaults(4819);
                    objList.Add(obj33);
                    break;
                case 22:
                    Item obj34 = new Item();
                    obj34.SetDefaults(2374);
                    objList.Add(obj34);
                    break;
                case 23:
                    Item obj35 = new Item();
                    obj35.SetDefaults(2423);
                    objList.Add(obj35);
                    break;
                case 24:
                    Item obj36 = new Item();
                    obj36.SetDefaults(4828);
                    objList.Add(obj36);
                    break;
                case 25:
                    Item obj37 = new Item();
                    obj37.SetDefaults(3031);
                    objList.Add(obj37);
                    break;
                case 26:
                    Item obj38 = new Item();
                    obj38.SetDefaults(3064);
                    objList.Add(obj38);
                    break;
                case 27:
                    Item obj39 = new Item();
                    obj39.SetDefaults(5302);
                    objList.Add(obj39);
                    break;
                case 28:
                    Item obj40 = new Item();
                    obj40.SetDefaults(3183);
                    objList.Add(obj40);
                    break;
                case 29:
                    Item obj41 = new Item();
                    obj41.SetDefaults(4820);
                    objList.Add(obj41);
                    break;
                case 30:
                    Item obj42 = new Item();
                    obj42.SetDefaults(2294);
                    objList.Add(obj42);
                    break;
            }
            if (anglerQuestsFinished >= 30 && Main.hardMode && !self.HasItemInAnyInventory(2422))
            {
                Item obj43 = new Item();
                obj43.SetDefaults(2422);
                objList.Add(obj43);
            }
            if (Main.rand.NextBool((int)(12.0 * (double)num2)) && anglerQuestsFinished > 30)
            {
                Item obj44 = new Item();
                obj44.SetDefaults(Main.hardMode ? ModContent.ItemType<BleachedAnglingKit>() : ModContent.ItemType<SandyAnglingKit>());
                objList.Add(obj44);
            }
            if (Main.rand.NextBool((int)(500.0 * (double)num2)) && anglerQuestsFinished > 30)
            {
                Item obj45 = new Item();
                obj45.SetDefaults(2294);
                objList.Add(obj45);
            }
            if (Main.rand.NextBool((int)(150.0 * (double)num2)) && anglerQuestsFinished > 16 /*0x10*/)
            {
                Item obj46 = new Item();
                obj46.SetDefaults(2367);
                objList.Add(obj46);
                Item obj47 = new Item();
                obj47.SetDefaults(2368);
                objList.Add(obj47);
                Item obj48 = new Item();
                obj48.SetDefaults(2369);
                objList.Add(obj48);
            }
            if (Main.rand.NextBool((int)(150.0 * (double)num2)) && anglerQuestsFinished > 13)
            {
                Item obj49 = new Item();
                obj49.SetDefaults(2417);
                objList.Add(obj49);
                Item obj50 = new Item();
                obj50.SetDefaults(2418);
                objList.Add(obj50);
                Item obj51 = new Item();
                obj51.SetDefaults(2419);
                objList.Add(obj51);
            }
            if (Main.rand.NextBool((int)(150.0 * (double)num2)) && anglerQuestsFinished > 7)
            {
                Item obj52 = new Item();
                obj52.SetDefaults(2498);
                objList.Add(obj52);
                Item obj53 = new Item();
                obj53.SetDefaults(2499);
                objList.Add(obj53);
                Item obj54 = new Item();
                obj54.SetDefaults(2500);
                objList.Add(obj54);
            }
            if (Main.rand.NextBool((int)(140.0 * (double)num2)) && Main.hardMode && anglerQuestsFinished > 10)
            {
                Item obj55 = new Item();
                obj55.SetDefaults(2494);
                objList.Add(obj55);
            }
            if (Main.rand.NextBool((int)(140.0 * (double)num2)) && anglerQuestsFinished > 25)
            {
                Item obj56 = new Item();
                obj56.SetDefaults(3031);
                objList.Add(obj56);
            }
            if (Main.rand.NextBool((int)(140.0 * (double)num2)) && anglerQuestsFinished > 27)
            {
                Item obj57 = new Item();
                obj57.SetDefaults(5302);
                objList.Add(obj57);
            }
            if (Main.rand.NextBool((int)(140.0 * (double)num2)) && anglerQuestsFinished > 29)
            {
                Item obj58 = new Item();
                obj58.SetDefaults(4820);
                objList.Add(obj58);
            }
            if (Main.rand.NextBool((int)(140.0 * (double)num2)) && anglerQuestsFinished > 11)
            {
                Item obj59 = new Item();
                obj59.SetDefaults(4263);
                objList.Add(obj59);
            }
            if (Main.rand.NextBool((int)(140.0 * (double)num2)) && anglerQuestsFinished > 21)
            {
                Item obj60 = new Item();
                obj60.SetDefaults(4819);
                objList.Add(obj60);
            }
            if (Main.rand.NextBool((int)(140.0 * (double)num2)) && anglerQuestsFinished > 18)
            {
                Item obj61 = new Item();
                obj61.SetDefaults(3032);
                objList.Add(obj61);
            }
            if (Main.rand.NextBool((int)(140.0 * (double)num2)) && anglerQuestsFinished > 19)
            {
                Item obj62 = new Item();
                obj62.SetDefaults(3032);
                objList.Add(obj62);
            }
            if (Main.rand.NextBool((int)(140.0 * (double)num2)) && anglerQuestsFinished > 28)
            {
                Item obj63 = new Item();
                obj63.SetDefaults(3183);
                objList.Add(obj63);
            }
            if (Main.rand.NextBool((int)(120.0 * (double)num2)) && anglerQuestsFinished > 4)
            {
                Item obj64 = new Item();
                obj64.SetDefaults(2360);
                objList.Add(obj64);
            }
            if (Main.rand.NextBool((int)(120.0 * (double)num2)) && anglerQuestsFinished > 8)
            {
                Item obj65 = new Item();
                obj65.SetDefaults(4067);
                objList.Add(obj65);
            }
            if (Main.rand.NextBool((int)(120.0 * (double)num2)) && anglerQuestsFinished > 24)
            {
                Item obj66 = new Item();
                obj66.SetDefaults(4828);
                objList.Add(obj66);
            }
            if (Main.rand.NextBool((int)(80.0 * (double)num2)) && anglerQuestsFinished > 3)
            {
                Item obj67 = new Item();
                obj67.SetDefaults(2373);
                objList.Add(obj67);
            }
            if (Main.rand.NextBool((int)(80.0 * (double)num2)) && anglerQuestsFinished > 22)
            {
                Item obj68 = new Item();
                obj68.SetDefaults(2374);
                objList.Add(obj68);
            }
            if (Main.rand.NextBool((int)(80.0 * (double)num2)) && anglerQuestsFinished > 17)
            {
                Item obj69 = new Item();
                obj69.SetDefaults(2375);
                objList.Add(obj69);
            }
            if (Main.rand.NextBool((int)(60.0 * (double)num2)) && anglerQuestsFinished > 6)
            {
                Item obj70 = new Item();
                obj70.SetDefaults(3120);
                objList.Add(obj70);
            }
            if (Main.rand.NextBool((int)(60.0 * (double)num2)) && anglerQuestsFinished > 11)
            {
                Item obj71 = new Item();
                obj71.SetDefaults(3037);
                objList.Add(obj71);
            }
            if (Main.rand.NextBool((int)(60.0 * (double)num2)) && anglerQuestsFinished > 15)
            {
                Item obj72 = new Item();
                obj72.SetDefaults(3096);
                objList.Add(obj72);
            }
            if (Main.rand.NextBool((int)(50.0 * (double)num2)) && anglerQuestsFinished > 12)
            {
                Item obj73 = new Item();
                obj73.SetDefaults(5139);
                objList.Add(obj73);
            }
            if (Main.rand.NextBool((int)(50.0 * (double)num2)))
            {
                Item obj74 = new Item();
                int furniture = furnitures[Main.rand.Next(furnitures.Length)];
                obj74.SetDefaults(furniture);
                objList.Add(obj74);
            }
            PlayerLoader.AnglerQuestReward(self, num2, objList);
            foreach (Item obj75 in objList)
            {
                obj75.position = self.Center;
                Item obj76 = self.GetItem(self.whoAmI, obj75, GetItemSettings.NPCEntityToPlayerInventorySettings);
                if (obj76.stack > 0)
                {
                    int num3 = Item.NewItem(entitySourceGift, (int)self.position.X, (int)self.position.Y, self.width, self.height, obj76.type, obj76.stack, false, 0, true, false);
                    if (Main.netMode == 1)
                        NetMessage.SendData(21, -1, -1, null, num3, 1f, 0.0f, 0.0f, 0, 0, 0);
                }
            }
        }
    }
}
