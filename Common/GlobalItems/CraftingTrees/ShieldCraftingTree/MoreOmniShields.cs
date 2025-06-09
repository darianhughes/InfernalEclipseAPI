using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Items;
using System.Reflection;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.ShieldCraftingTree
{
    [ExtendsFromMod("ThoriumMod")]
    public class MoreOmniShields : GlobalItem
    {
        private Mod sots
        {
            get
            {
                ModLoader.TryGetMod("SOTS", out Mod sots);
                return sots;
            }
        }

        private Mod clamity
        {
            get
            {
                ModLoader.TryGetMod("Clamity", out Mod clam);
                return clam;
            }
        }

        private Mod souls
        {
            get
            {
                ModLoader.TryGetMod("FargowiltasSouls", out Mod souls);
                return souls;
            }
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (IsOmniShield(equippedItem) && IsBarrier(incomingItem)) return false;
            if (IsOmniShield(incomingItem) && IsBarrier(equippedItem)) return false;
            if (IsBarrier(equippedItem) && IsBarrier(incomingItem)) return false;

            // Existing Thorium accessory type mutual exclusivity
            if (equippedItem.ModItem is ThoriumItem modItem1 && modItem1.accessoryType != AccessoryType.Normal &&
                incomingItem.ModItem is ThoriumItem modItem2 && modItem2.accessoryType != AccessoryType.Normal)
            {
                return modItem1.accessoryType != modItem2.accessoryType;
            }
            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        bool IsOmniShield(Item item) => item.ModItem is ThoriumItem t && t.accessoryType == AccessoryType.OmniShield;

        bool IsBarrier(Item item)
        {
            int supremeBarrierType = 0;
            if (clamity != null)
                supremeBarrierType = clamity?.Find<ModItem>("SupremeBarrier")?.Type ?? 0;

            int bulwarkType = 0;
            if (sots != null)
                bulwarkType = sots?.Find<ModItem>("BulwarkOfTheAncients")?.Type ?? 0;

            int colossusType = 0;
            if (souls != null)
                colossusType = souls.Find<ModItem>("ColossusSoul").Type;

            int rampartType = ModContent.ItemType<RampartofDeities>();

            if (InfernalConfig.Instance.CalamityBalanceChanges || InfernalConfig.Instance.MergeCraftingTrees)
            {
                return item.type == supremeBarrierType || item.type == rampartType;
            }
            if (InfernalConfig.Instance.SOTSBalanceChanges)
            {
                return item.type == bulwarkType;
            }
            return item.type == colossusType;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (souls != null)
            {
                if (item.type == souls.Find<ModItem>("ColossusSoul").Type)
                {
                    int index = tooltips.FindIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("ItemName"));
                    if (index != -1)
                        tooltips.Insert(index + 1, new TooltipLine(Mod, "AccessoryWarning", "-Omni Shield-")
                        {
                            OverrideColor = new Color?(new Color(102, byte.MaxValue, byte.MaxValue))
                        });
                }
            }
            if (clamity != null && (InfernalConfig.Instance.CalamityBalanceChanges || InfernalConfig.Instance.MergeCraftingTrees))
            {
                if (item.type == clamity.Find<ModItem>("SupremeBarrier").Type)
                {
                    int index = tooltips.FindIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("ItemName"));
                    if (index != -1)
                        tooltips.Insert(index + 1, new TooltipLine(((ModType)this).Mod, "AccessoryWarning", "-Omni Shield-")
                        {
                            OverrideColor = new Color?(new Color(102, (int)byte.MaxValue, (int)byte.MaxValue))
                        });
                }
            }
            if (sots != null && InfernalConfig.Instance.SOTSBalanceChanges)
            {
                if (item.type == sots.Find<ModItem>("BulwarkOfTheAncients").Type)
                {
                    int index = tooltips.FindIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("ItemName"));
                    if (index != -1)
                        tooltips.Insert(index + 1, new TooltipLine(((ModType)this).Mod, "AccessoryWarning", "-Omni Shield-")
                        {
                            OverrideColor = new Color?(new Color(102, (int)byte.MaxValue, (int)byte.MaxValue))
                        });
                }
            }
            if (item.type == ModContent.ItemType<RampartofDeities>() && (InfernalConfig.Instance.MergeCraftingTrees || InfernalConfig.Instance.CalamityBalanceChanges))
            {
                int index = tooltips.FindIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("ItemName"));
                if (index != -1)
                    tooltips.Insert(index + 1, new TooltipLine(((ModType)this).Mod, "AccessoryWarning", "-Omni Shield-")
                    {
                        OverrideColor = new Color?(new Color(102, (int)byte.MaxValue, (int)byte.MaxValue))
                    });
            }
        }
    }
}
