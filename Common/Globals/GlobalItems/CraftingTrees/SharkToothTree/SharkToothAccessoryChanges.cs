using CalamityMod.Items.Accessories;
using FargowiltasSouls.Content.NPCs.EternityModeNPCs.VanillaEnemies.SolarEclipse;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Localization;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.SharkToothTree
{
    public class SharkToothAccessoryChanges : GlobalItem
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

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;

            if (sots != null & thorium != null)
            {
                if (item.ModItem != null &&
                    item.ModItem.Mod.Name == "SOTS" &&
                    item.ModItem.Name == "PrismarineNecklace" &&
                    thorium != null)
                {
                    player.GetArmorPenetration(DamageClass.Generic) -= 3;
                }
            }
            else if (sots != null)
            {
                if (item.type == ModContent.ItemType<SandSharkToothNecklace>())
                {
                    ModItem midnightPrism = sots.Find<ModItem>("MidnightPrism");
                    midnightPrism.UpdateAccessory(player, hideVisual);
                    player.GetArmorPenetration(DamageClass.Generic) -= 8;
                }
            }

            if (sots != null)
            {
                if (item.type == ModContent.ItemType<ReaperToothNecklace>())
                {
                    ModItem midnightPrism = sots.Find<ModItem>("MidnightPrism");
                    midnightPrism.UpdateAccessory(player, hideVisual);
                    player.GetArmorPenetration(DamageClass.Generic) -= 8;
                }
            }

            if (thorium != null)
            {
                if (item.type == ModContent.ItemType<SandSharkToothNecklace>())
                {
                    player.GetArmorPenetration(DamageClass.Generic) += 2;
                }

                if (item.ModItem != null &&
                    item.ModItem.Mod.Name == "ThoriumMod" &&
                    item.ModItem.Name == "DragonTalonNecklace" &&
                    thorium != null)
                {
                    player.GetArmorPenetration(DamageClass.Generic) -= 4;
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
            Color NoSOTSPink = Color.Lerp(
                Color.White,
                new Color(251, 198, 207),
                (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );
            Color NoThorYellow = Color.Lerp(
                Color.White,
                new Color(255, 255, 197),
                (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );

            string prisma1 = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Prisma");
            string prisma2 = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Prisma2");
            string prisma3 = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Prisma3");
            string midnight1 = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Midnight1");
            string midnight2 = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Midnight2");
            string sandsharkOrig = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ToothNecklace.SandShark.OrigTooltip");
            string prismaOrig = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ToothNecklace.Prismarine.OrigTooltip");
            string reaperOrig = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ToothNecklace.Reaper.OrigTooltip");


            if (sots != null & thorium != null)
            {
                if (item.type == sots.Find<ModItem>("PrismarineNecklace").Type)
                {
                    foreach (TooltipLine tooltip in tooltips)
                    {
                        if (tooltip.Text.Contains(prismaOrig))
                        {
                            tooltip.Text = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ToothNecklace.Prismarine.CommonReplace");
                        }
                    }
                }

                if (item.type == ModContent.ItemType<SandSharkToothNecklace>())
                {
                    foreach (TooltipLine tooltip in tooltips)
                    {
                        if (tooltip.Text.Contains(sandsharkOrig))
                        {
                            tooltip.Text = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ToothNecklace.SandShark.CommonReplace");
                        }
                    }
                }
            }
            else if (sots != null)
            {
                if (item.type == ModContent.ItemType<SandSharkToothNecklace>())
                {
                    foreach (TooltipLine tooltip in tooltips)
                    {
                        if (tooltip.Text.Contains(sandsharkOrig))
                        {
                            tooltip.Text = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ToothNecklace.SandShark.SOTSReplace");
                        }
                    }
                    tooltips.Add(new TooltipLine(Mod, "prisma1", prisma1)
                    {
                        OverrideColor = new Color?(NoThorYellow)
                    });
                    tooltips.Add(new TooltipLine(Mod, "prisma2", prisma2)
                    {
                        OverrideColor = new Color?(NoThorYellow)
                    });
                    tooltips.Add(new TooltipLine(Mod, "midnight1", midnight1)
                    {
                        OverrideColor = new Color?(NoThorYellow)
                    });
                    tooltips.Add(new TooltipLine(Mod, "midnight2", midnight2)
                    {
                        OverrideColor = new Color?(NoThorYellow)
                    });
                    tooltips.Add(new TooltipLine(Mod, "prisma3", prisma3)
                    {
                        OverrideColor = new Color?(NoThorYellow)
                    });
                }
            }
            else if (thorium != null)
            {
                if (item.type == ModContent.ItemType<SandSharkToothNecklace>())
                {
                    foreach (TooltipLine tooltip in tooltips)
                    {
                        if (tooltip.Text.Contains(sandsharkOrig))
                        {
                            tooltip.Text = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ToothNecklace.SandShark.ThoriumReplace"); ;
                        }
                    }
                }
            }

            if (sots != null)
            {
                if (item.type == ModContent.ItemType<ReaperToothNecklace>())
                {
                    foreach (TooltipLine tooltip in tooltips)
                    {
                        if (tooltip.Text.Contains(reaperOrig))
                        {
                            tooltip.Text = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ToothNecklace.Reaper.SOTSReplace");
                        }
                    }
                    tooltips.Add(new TooltipLine(Mod, "prisma1", prisma1)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "prisma2", prisma2)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "midnight1", midnight1)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "midnight2", midnight2)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "prisma3", prisma3)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }
            }

            if (thorium != null)
            {
                if (item.type == thorium.Find<ModItem>("DragonTalonNecklace").Type)
                {
                    foreach (TooltipLine tooltip in tooltips)
                    {
                        if (tooltip.Text.Contains("Increases armor penetration by 12"))
                        {
                            tooltip.Text = "Increases armor penetration by 8";
                        }
                    }
                }
            }
        }
    }
}
