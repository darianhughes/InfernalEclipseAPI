using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Accessories;
using ThoriumMod;
using CalamityMod;
using Steamworks;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.EtherealTalismanCraftingTree
{
    public class EtherealTalismanAccessoryChanges : GlobalItem
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
                ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
                return thorium;
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees || calamity == null || thorium == null)
                return;

            ModItem murkyCatalyst = thorium.Find<ModItem>("MurkyCatalyst");
            ModItem hungeringBlossom = thorium.Find<ModItem>("HungeringBlossom");

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "CalamityMod" &&
                item.ModItem.Name == "SigilofCalamitas")
            {
                murkyCatalyst.UpdateAccessory(player, hideVisual);
                player.statManaMax2 -= 20;
            }

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "CalamityMod" &&
                item.ModItem.Name == "EtherealTalisman")
            {
                murkyCatalyst.UpdateAccessory(player, hideVisual);
                player.statManaMax2 -= 20;

                if (hideVisual)
                {
                    hungeringBlossom.UpdateAccessory(player, hideVisual);
                }
            }
        }

        public void AddTooltip(List<TooltipLine> tooltips, string stealthTooltip, bool InfernalRedActive = false)
        {
            Color InfernalRed = Color.Lerp(
               Color.White,
               new Color(255, 80, 0), // Infernal red/orange
               (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );

            int maxTooltipIndex = -1;
            int maxNumber = -1;

            // Find the TooltipLine with the highest TooltipX name
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Mod == "Terraria" && tooltips[i].Name.StartsWith("Tooltip"))
                {
                    if (int.TryParse(tooltips[i].Name.Substring(7), out int num) && num > maxNumber)
                    {
                        maxNumber = num;
                        maxTooltipIndex = i;
                    }
                }
            }

            // If found, insert a new TooltipLine right after it with the desired color
            if (maxTooltipIndex != -1)
            {
                int insertIndex = maxTooltipIndex + 1;
                TooltipLine customLine = new TooltipLine(Mod, "StealthTooltip", stealthTooltip);
                if (InfernalRedActive)
                    customLine.OverrideColor = InfernalRed;

                tooltips.Insert(insertIndex, customLine);
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees || calamity == null || thorium == null)
                return;

            string murkyInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Murky");
            string hungeringInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Hungering");

            if (item.type == calamity.Find<ModItem>("SigilofCalamitas").Type)
            {
                AddTooltip(tooltips, murkyInfo, true);
            }

            if (item.type == calamity.Find<ModItem>("EtherealTalisman").Type)
            {
                AddTooltip(tooltips, murkyInfo, true);
                AddTooltip(tooltips, hungeringInfo, true);
            }
        }
    }
}
