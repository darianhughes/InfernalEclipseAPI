using System.Collections.Generic;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.QuiverCraftingTree
{
    public class QuiverAccessoryChanges : GlobalItem
    {
        private Mod thorium
        {
            get
            {
                ModLoader.TryGetMod("ThoriumMod", out Mod thor);
                return thor;
            }
        }

        private Mod sots
        {
            get
            {
                ModLoader.TryGetMod("SOTS", out Mod sots);
                return sots;
            }
        }

        private Mod calamity
        {
            get
            {
                ModLoader.TryGetMod("CalamityMod", out Mod cal);
                return cal;
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;

            if (item.type == ModContent.ItemType<PlanebreakersPouch>())
            {
                if (sots != null)
                {
                    ModItem bagofammo = sots.Find<ModItem>("BagOfAmmoGathering");
                    ModItem voidammobag = sots.Find<ModItem>("InfinityPouch");

                    if (hideVisual == false)
                    {
                        voidammobag.UpdateAccessory(player, hideVisual);
                    }
                    else
                    {
                        bagofammo.UpdateAccessory(player, hideVisual);
                    }
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

            if (item.type == ModContent.ItemType<PlanebreakersPouch>() && sots != null)
            {
                foreach (TooltipLine tooltip in tooltips)
                {
                    if (tooltip.Name.Contains("Tooltip") && tooltip.Text.Contains(Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.BagOfAmmoConsume.ElementalQuiver")))
                    {
                        tooltip.Text = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.BagOfAmmoConsume.ReplaceTooltip");
                        tooltip.OverrideColor = new Color?(InfernalRed);
                    }
                }

                tooltips.Add(new TooltipLine(Mod, "MergedTreeTooltip", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.BagOfAmmoOther")) { OverrideColor = InfernalRed });
                tooltips.Add(new TooltipLine(Mod, "MergedTreeTooltip", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.InfintyBag")) { OverrideColor = InfernalRed });
            }
        }
    }
}
