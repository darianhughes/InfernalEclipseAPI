using Microsoft.Xna.Framework;
using CalamityMod.Items.Accessories;
using Terraria.Localization;
using System.Collections.Generic;
using InfernalEclipseAPI.Core.Configs;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.NucleogenesisTree
{
    public class SummonerAccessoryChanges : GlobalItem
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

        private Mod clamity
        {
            get
            {
                ModLoader.TryGetMod("Clamity", out Mod clam);
                return clam;
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;


            //if (item.ModItem != null &&
            //    item.ModItem.Mod.Name == "ThoriumMod" &&
            //    item.ModItem.Name == "ScryingGlass")
            //{
            //    --player.maxTurrets;
            //}

            if (sots != null && thorium != null)
            {
                ModItem scryingGlass = thorium.Find<ModItem>("ScryingGlass");
                ModItem necroticSkull = thorium.Find<ModItem>("NecroticSkull");
                ModItem cystralScorpion = thorium.Find<ModItem>("CrystalScorpion");
                ModItem fortressGenerator = sots.Find<ModItem>("FortressGenerator");
                ModItem steamkeeperWatch = thorium.Find<ModItem>("SteamkeeperWatch");
                ModItem yumasPendant = thorium.Find<ModItem>("YumasPendant");

                if (item.ModItem != null &&
                    item.ModItem.Mod.Name == "SOTS" &&
                    item.ModItem.Name == "PlatformGenerator")
                {
                    //scryingGlass.UpdateAccessory(player, hideVisual);
                    //--player.maxTurrets;
                }

                if (item.ModItem != null &&
                    item.ModItem.Mod.Name == "SOTS" &&
                    item.ModItem.Name == "FortressGenerator")
                {
                    //scryingGlass.UpdateAccessory(player, hideVisual);
                    //--player.maxTurrets;
                    necroticSkull.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    --player.maxTurrets;

                    if (clamity != null)
                    {
                        ModItem cyanPearl = clamity.Find<ModItem>("CyanPearl");
                        cyanPearl.UpdateAccessory(player, hideVisual);
                        ref StatModifier local = ref player.GetDamage(DamageClass.Summon);
                        local -= 0.07f;
                    }

                    player.lifeRegen -= 2;
                    player.hasPaladinShield = false;
                    ref StatModifier local2 = ref player.GetDamage(DamageClass.Generic);
                    local2 -= 0.1f;
                }

                if (item.type == ModContent.ItemType<StatisBlessing>())
                {
                    //scryingGlass.UpdateAccessory(player, hideVisual);
                    //--player.maxTurrets;
                    necroticSkull.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    cystralScorpion.UpdateAccessory(player, hideVisual);
                    //fortressGenerator.UpdateAccessory(player, hideVisual);
                    //player.lifeRegen -= 2;
                    //player.hasPaladinShield = false;
                    //--player.maxMinions;
                    //ref StatModifier local = ref player.GetDamage(DamageClass.Generic);
                    //local -= 0.1f;
                    //steamkeeperWatch.UpdateAccessory(player, hideVisual);
                    //ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                    //local2 -= 0.1f;
                }

                if (item.type == ModContent.ItemType<StatisCurse>())
                {
                    //scryingGlass.UpdateAccessory(player, hideVisual);
                    //--player.maxTurrets;
                    necroticSkull.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    cystralScorpion.UpdateAccessory(player, hideVisual);
                    //fortressGenerator.UpdateAccessory(player, hideVisual);
                    //player.lifeRegen -= 2;
                    //player.hasPaladinShield = false;
                    //--player.maxMinions;
                    //ref StatModifier local = ref player.GetDamage(DamageClass.Generic);
                    //local -= 0.1f;
                    //steamkeeperWatch.UpdateAccessory(player, hideVisual);
                    //ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                    //local2 -= 0.1f;
                    //yumasPendant.UpdateAccessory(player, hideVisual);
                    //ref StatModifier local3 = ref player.GetDamage(DamageClass.Summon);
                    //local3 -= 0.1f;
                    //--player.maxMinions;

                    //++player.maxTurrets;
                }

                if (item.type == ModContent.ItemType<StarTaintedGenerator>())
                {
                    steamkeeperWatch.UpdateAccessory(player, hideVisual);
                    ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                    local2 -= 0.1f;
                }

                if (item.type == ModContent.ItemType<Nucleogenesis>())
                {
                    //scryingGlass.UpdateAccessory(player, hideVisual);
                    //--player.maxTurrets;
                    necroticSkull.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    cystralScorpion.UpdateAccessory(player, hideVisual);
                    //fortressGenerator.UpdateAccessory(player, hideVisual);
                    //player.lifeRegen -= 2;
                    //player.hasPaladinShield = false;
                    //--player.maxMinions;
                    //ref StatModifier local = ref player.GetDamage(DamageClass.Generic);
                    //local -= 0.1f;
                    steamkeeperWatch.UpdateAccessory(player, hideVisual);
                    ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                    local2 -= 0.1f;
                    //yumasPendant.UpdateAccessory(player, hideVisual);
                    //ref StatModifier local3 = ref player.GetDamage(DamageClass.Summon);
                    //local3 -= 0.1f;
                    //--player.maxMinions;

                    /*
                    if (clamity != null)
                    {
                        ModItem cyanPearl = clamity.Find<ModItem>("CyanPearl");
                        cyanPearl.UpdateAccessory(player, hideVisual);
                        ref StatModifier local3 = ref player.GetDamage(DamageClass.Summon);
                        local3 -= 0.07f;
                    }

                    ++player.maxTurrets;
                    */
                }
            }
            else if (thorium != null)
            {
                ModItem scryingGlass = thorium.Find<ModItem>("ScryingGlass");
                ModItem necroticSkull = thorium.Find<ModItem>("NecroticSkull");
                ModItem cystralScorpion = thorium.Find<ModItem>("CrystalScorpion");
                ModItem steamkeeperWatch = thorium.Find<ModItem>("SteamkeeperWatch");
                ModItem yumasPendant = thorium.Find<ModItem>("YumasPendant");

                if (item.type == ModContent.ItemType<StatisBlessing>())
                {
                    //scryingGlass.UpdateAccessory(player, hideVisual);
                    necroticSkull.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    cystralScorpion.UpdateAccessory(player, hideVisual);
                    //steamkeeperWatch.UpdateAccessory(player, hideVisual);
                    //ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                    //local2 -= 0.1f;
                }

                if (item.type == ModContent.ItemType<StatisCurse>())
                {
                    //scryingGlass.UpdateAccessory(player, hideVisual);
                    //--player.maxTurrets;
                    necroticSkull.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    cystralScorpion.UpdateAccessory(player, hideVisual);
                    //steamkeeperWatch.UpdateAccessory(player, hideVisual);
                    //ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                    //local2 -= 0.1f;
                    //yumasPendant.UpdateAccessory(player, hideVisual);
                    //ref StatModifier local3 = ref player.GetDamage(DamageClass.Summon);
                    //local3 -= 0.1f;
                    //--player.maxMinions;

                    ++player.maxTurrets;
                }

                if (item.type == ModContent.ItemType<StarTaintedGenerator>())
                {
                    steamkeeperWatch.UpdateAccessory(player, hideVisual);
                    ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                    local2 -= 0.1f;
                }

                if (item.type == ModContent.ItemType<Nucleogenesis>())
                {
                    //scryingGlass.UpdateAccessory(player, hideVisual);
                    //--player.maxTurrets;
                    necroticSkull.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    cystralScorpion.UpdateAccessory(player, hideVisual);
                    steamkeeperWatch.UpdateAccessory(player, hideVisual);
                    ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                    local2 -= 0.1f;
                    //yumasPendant.UpdateAccessory(player, hideVisual);
                    //ref StatModifier local3 = ref player.GetDamage(DamageClass.Summon);
                    //local3 -= 0.1f;
                    //--player.maxMinions;

                    /*
                    if (clamity != null)
                    {
                        ModItem cyanPearl = clamity.Find<ModItem>("CyanPearl");
                        cyanPearl.UpdateAccessory(player, hideVisual);
                        ref StatModifier local = ref player.GetDamage(DamageClass.Summon);
                        local -= 0.07f;
                    }

                    ++player.maxTurrets;
                    */
                }
            }
            else if (sots != null)
            {
                if (item.ModItem != null &&
                    item.ModItem.Mod.Name == "SOTS" &&
                    item.ModItem.Name == "FortressGenerator")
                {
                    player.hasPaladinShield = false;
                }

                ModItem fortressGenerator = sots.Find<ModItem>("FortressGenerator");

                if (item.type == ModContent.ItemType<StatisBlessing>())
                {
                    //fortressGenerator.UpdateAccessory(player, hideVisual);
                    //player.hasPaladinShield = false;
                    //--player.maxMinions;
                    //++player.maxTurrets;
                    //ref StatModifier local = ref player.GetDamage(DamageClass.Generic);
                    //local -= 0.1f;
                }

                if (item.type == ModContent.ItemType<Nucleogenesis>())
                {
                    //fortressGenerator.UpdateAccessory(player, hideVisual);
                    //player.hasPaladinShield = false;
                    //--player.maxMinions;
                    //++player.maxTurrets;
                    //ref StatModifier local = ref player.GetDamage(DamageClass.Generic);
                    //local -= 0.1f;

                    /*
                    if (clamity != null)
                    {
                        ModItem cyanPearl = clamity.Find<ModItem>("CyanPearl");
                        cyanPearl.UpdateAccessory(player, hideVisual);
                        ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                        local2 -= 0.07f;
                    }

                    ++player.maxTurrets;
                    */
                }
            }
        }

        public void AddTooltip(List<TooltipLine> tooltips, string stealthTooltip, Color overrideColor = default)
        {
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
                TooltipLine customLine = new TooltipLine(Mod, "MergedTreeTooltip", stealthTooltip);
                if (overrideColor != default)
                    customLine.OverrideColor = overrideColor;

                tooltips.Insert(insertIndex, customLine);
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

            //string scryInfo = "Pressing the 'Accessory Ability' key will toggle an increased range of view";
            string skullInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Skull");
            string scorpionInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.CrystalScorpian");
            string fortressInfoNoThor = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.FortressNoThor");
            string fortressInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Fortress");
            string watchInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Watch");
            //string yumasInfo = "Taking damage releases a ghostly protector";

            string oneMoreSentry = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.OneSentry");
            string twoMoreSentries = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.TwoSentry");
            string threeMoreSentries = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ThreeSentry");

            string cyanPearlInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.CyanPearl");

            string fortessOrig1 = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.FortressGenerator.OrigTooltip1");
            string fortessOrig2 = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.FortressGenerator.OrigTooltip2");
            string replaceInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.FortressGenerator.Replace");

            if (sots != null & thorium != null)
            {
                //if (item.type == sots.Find<ModItem>("PlatformGenerator").Type)
                //{
                //    tooltips.Add(new TooltipLine(Mod, "scry", scryInfo)
                //    {
                //        OverrideColor = new Color?(InfernalRed)
                //    });
                //}

                if (item.type == ModContent.ItemType<StatisBlessing>())
                {
                    AddTooltip(tooltips, oneMoreSentry, InfernalRed);
                    //tooltips.Add(new TooltipLine(Mod, "scry", scryInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                    AddTooltip(tooltips, skullInfo, InfernalRed);
                    AddTooltip(tooltips, scorpionInfo, InfernalRed);
                    //tooltips.Add(new TooltipLine(Mod, "fortress", fortressInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                    //tooltips.Add(new TooltipLine(Mod, "watch", watchInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                }

                if (item.type == ModContent.ItemType<StatisCurse>())
                {
                    AddTooltip(tooltips, oneMoreSentry, InfernalRed);
                    //tooltips.Add(new TooltipLine(Mod, "scry", scryInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                    AddTooltip(tooltips, skullInfo, InfernalRed);
                    AddTooltip(tooltips, scorpionInfo, InfernalRed);
                    //tooltips.Add(new TooltipLine(Mod, "fortress", fortressInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                    //tooltips.Add(new TooltipLine(Mod, "watch", watchInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                    //tooltips.Add(new TooltipLine(Mod, "yuma", yumasInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                }

                if (item.type == ModContent.ItemType<Nucleogenesis>())
                {
                    AddTooltip(tooltips, oneMoreSentry, InfernalRed);
                    //tooltips.Add(new TooltipLine(Mod, "scry", scryInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                    AddTooltip(tooltips, skullInfo, InfernalRed);
                    AddTooltip(tooltips, scorpionInfo, InfernalRed);
                    //AddTooltip(tooltips, fortressInfo, InfernalRed);
                    //AddTooltip(tooltips, watchInfo, InfernalRed);
                    //tooltips.Add(new TooltipLine(Mod, "yuma", yumasInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                    //AddTooltip(tooltips, cyanPearlInfo, InfernalRed);
                }
            }
            else if (thorium != null)
            {
                if (item.type == ModContent.ItemType<StatisBlessing>())
                {
                    AddTooltip(tooltips, twoMoreSentries, InfernalRed);
                    //tooltips.Add(new TooltipLine(Mod, "scry", scryInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                    AddTooltip(tooltips, skullInfo, InfernalRed);
                    AddTooltip(tooltips, scorpionInfo, InfernalRed);
                    //tooltips.Add(new TooltipLine(Mod, "watch", watchInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                }

                if (item.type == ModContent.ItemType<StatisCurse>())
                {
                    AddTooltip(tooltips, threeMoreSentries, InfernalRed);
                    //tooltips.Add(new TooltipLine(Mod, "scry", scryInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                    AddTooltip(tooltips, skullInfo, InfernalRed);
                    AddTooltip(tooltips, scorpionInfo, InfernalRed);
                    //tooltips.Add(new TooltipLine(Mod, "watch", watchInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                    //tooltips.Add(new TooltipLine(Mod, "yuma", yumasInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                }

                if (item.type == ModContent.ItemType<Nucleogenesis>())
                {
                    AddTooltip(tooltips, threeMoreSentries, InfernalRed);
                    //tooltips.Add(new TooltipLine(Mod, "scry", scryInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                    AddTooltip(tooltips, skullInfo, InfernalRed);
                    AddTooltip(tooltips, scorpionInfo, InfernalRed);
                    //AddTooltip(tooltips, watchInfo, InfernalRed);
                    //tooltips.Add(new TooltipLine(Mod, "yuma", yumasInfo)
                    //{
                    //    OverrideColor = new Color?(InfernalRed)
                    //});
                    //AddTooltip(tooltips, cyanPearlInfo, InfernalRed);
                }
            }
        }
    }
}
