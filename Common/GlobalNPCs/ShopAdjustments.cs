using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using InfernalEclipseAPI.Content.Items.Materials;
using InfernalEclipseAPI.Core.Players;
using InfernalEclipseAPI.Core.World;

namespace InfernalEclipseAPI.Common.GlobalNPCs
{
    public class ShopAdjustments : GlobalNPC
    {
        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            if (npc.type == NPCID.GoblinTinkerer && InfernalConfig.Instance.BossKillCheckOnOres)
            {
                // Replace Tinkerer's Workshop by filtering the Entries list
                for (int i = 0; i < items.Length; i++)
                {
                    var item = items[i];
                    if (item != null && !item.IsAir && item.type == ItemID.TinkerersWorkshop)
                    {
                        // Replace workshop with blueprint
                        items[i] = new Item(ModContent.ItemType<TinkerersRepairBlueprints>());
                    }
                }

                bool someoneHasOwnedWorkshop = false;

                static bool HasWorkshop(Item[] arr)
                {
                    if (arr is null) return false;
                    foreach (var it in arr)
                        if (!it.IsAir && it.type == ItemID.TinkerersWorkshop)
                            return true;
                    return false;
                }

                foreach (var player in Main.ActivePlayers)
                {
                    if (HasWorkshop(player.inventory) || HasWorkshop(player.armor) ||
                        HasWorkshop(player.bank?.item) || HasWorkshop(player.bank2?.item) ||
                        HasWorkshop(player.bank3?.item) || HasWorkshop(player.bank4?.item))
                    {
                        player.GetModPlayer<InfernalPlayer>().workshopHasBeenOwned = true;
                    }

                    if (player.GetModPlayer<InfernalPlayer>().workshopHasBeenOwned)
                    {
                        someoneHasOwnedWorkshop = true;
                        break;
                    }
                }

                if (someoneHasOwnedWorkshop || InfernalWorld.craftedWorkshop || Main.hardMode) // start selling it again after its been obtained at least once; and will always sell it again in hardmode
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
