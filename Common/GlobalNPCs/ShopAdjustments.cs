using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalNPCs
{
    public class ShopAdjustments : GlobalNPC
    {
        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            if (npc.type == NPCID.GoblinTinkerer && InfernalConfig.Instance.BossKillCheckOnOres)
            {
                // Remove Tinkerer's Workshop by filtering the Entries list
                for (int i = 0; i < items.Length; i++)
                {
                    var item = items[i];
                    if (item != null && !item.IsAir && item.type == ItemID.TinkerersWorkshop)
                    {
                        // Remove workshop by turning this slot into air
                        items[i] = new Item();
                    }
                }

                // Only add the item back if either Brain of Cthulhu or Eater of Worlds has been defeated
                if (NPC.downedBoss2 || NPC.downedBoss3)
                {
                    // Find first empty slot
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i] == null || items[i].IsAir)
                        {
                            items[i] = new Item(ItemID.TinkerersWorkshop);
                            break;
                        }
                    }
                }
            }
        }
    }
}
