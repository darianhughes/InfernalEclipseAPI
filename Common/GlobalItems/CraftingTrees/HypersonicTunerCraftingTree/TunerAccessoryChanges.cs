using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using SOTS.Void;
using SOTSBardHealer;
using ThoriumMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.HypersonicTunerCraftingTree
{
    [ExtendsFromMod("SOTSBardHealer")]
    public class TunerAccessoryChanges : GlobalItem
    {
        private Mod Ragnarok
        {
            get
            {
                ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok);
                return ragnarok;
            }
        }

        private Mod CalBardHealer
        {
            get
            {
                ModLoader.TryGetMod("CalamityBardHealer", out Mod calbh);
                return calbh;
            }
        }

        private Mod SOTSBardHealer
        {
            get
            {
                ModLoader.TryGetMod("SOTSBardHealer", out Mod sotsbh);
                return sotsbh;
            }
        }

        private Mod ThoriumRework
        {
            get
            {
                ModLoader.TryGetMod("ThoriumRework", out Mod thorRe);
                return thorRe;
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

            if (SOTSBardHealer != null)
            {
                if (item.type == SOTSBardHealer.Find<ModItem>("HypersonicTuner").Type)
                {
                    if (hideVisual)
                    {
                        VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
                        voidPlayer.voidMeterMax2 -= player.GetThoriumPlayer().bardResourceMax2;
                        voidPlayer.voidRegenSpeed -= player.GetThoriumPlayer().inspirationRegenBonus - 1f;
                        player.GetModPlayer<SecretsOfThoriumPlayer>().HypersonicTuner = false;

                        ModItem infrasonicTuner = SOTSBardHealer.Find<ModItem>("InfrasonicTuner");
                        infrasonicTuner.UpdateAccessory(player, hideVisual);
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

            if (item.type == SOTSBardHealer.Find<ModItem>("HypersonicTuner").Type)
            {
                for (int i = 0; i < tooltips.Count; ++i)
                {
                    if (tooltips[i].Mod == "Terraria")
                    {
                        if (tooltips[i].Name == "Tooltip0")
                        {
                            TooltipLine tooltip = tooltips[i];

                            if (ThoriumRework != null) tooltip.Text = $"{Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Tuner1")}";
                            else tooltip.Text = $"{Language.GetTextValue("Mods.InfernalEclipseAPI.Tuner2")}";
                        }
                        else if (tooltips[i].Name.Contains("Tooltip"))
                        {
                            tooltips[i].Hide();
                        }
                    }
                }
            }
            }
        }
}
