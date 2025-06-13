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

                if (hideVisual)
                {
                    hungeringBlossom.UpdateAccessory(player, hideVisual);
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees || calamity == null || thorium == null)
                return;

            Color InfernalRed = Color.Lerp(
               Color.White,
               new Color(255, 80, 0), // Infernal red/orange
               (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );

            string murkyInfo = "Every sixth magic cast is free and grants you the mana cost.";
            string hungeringInfo = "Grants the effects of Hungering Blossom when visibility is off";

            if (item.type == calamity.Find<ModItem>("SigilofCalamitas").Type)
            {
                tooltips.Add(new TooltipLine(Mod, "MurkyInfo", murkyInfo)
                {
                    OverrideColor = new Color?(InfernalRed)
                });
            }

            if (item.type == calamity.Find<ModItem>("EtherealTalisman").Type)
            {
                tooltips.Add(new TooltipLine(Mod, "MurkyInfo", murkyInfo)
                {
                    OverrideColor = new Color?(InfernalRed)
                });
                tooltips.Add(new TooltipLine(Mod, "HungeringInfo", hungeringInfo)
                {
                    OverrideColor = new Color?(InfernalRed)
                });
            }
        }
    }
}
