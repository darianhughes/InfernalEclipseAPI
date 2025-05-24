using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Accessories;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.ShieldCraftingTree
{
    public class ShieldCraftingTree : GlobalItem
    {
        private Mod calamity
        {
            get
            {
                ModLoader.TryGetMod("CalamityMod", out Mod cal);
                return cal;
            }
        }

        private Mod thorium
        {
            get
            {
                ModLoader.TryGetMod("ThoriumMod", out Mod thor);
                return thor;
            }
        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "CalamityMod" &&
                item.ModItem.Name == "AsgardsValor" &&
                thorium != null)
            {
                ModItem moltenScale = thorium.Find<ModItem>("MoltenScale");
                moltenScale.UpdateAccessory(player, hideVisual);
                base.UpdateAccessory(item, player, hideVisual);
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;

            Color InfernalRed = Color.Lerp(
                Color.White,
                new Color(255, 80, 0), // Infernal red/orange
                (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );
            string moltenScaleInfo = "Nearby enemies will be set on fire";

            if (item.type == ModContent.ItemType<AsgardsValor>())
            {
                tooltips.Add(new TooltipLine(Mod, "MoltenScaleInfo", moltenScaleInfo)
                {
                    OverrideColor = new Color?(InfernalRed)
                });
            }
        }
    }
}
