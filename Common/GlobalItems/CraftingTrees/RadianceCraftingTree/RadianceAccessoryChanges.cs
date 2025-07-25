using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Melee;
using Terraria.Localization;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.RadianceCraftingTree
{
    public class RadianceAccessoryChanges : GlobalItem
    {
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

            if (thorium != null)
            {
                if (item.type == ModContent.ItemType<Radiance>())
                {
                    ModItem spiritGrace = thorium.Find<ModItem>("SpiritsGrace");
                    spiritGrace.UpdateAccessory(player, hideVisual);
                }
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
            string graceInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Grace");

            if (thorium != null)
            {
                if (item.type == ModContent.ItemType<Radiance>())
                {
                    tooltips.Add(new TooltipLine(Mod, "graceInfo", graceInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }
            }
        }
    }
}
