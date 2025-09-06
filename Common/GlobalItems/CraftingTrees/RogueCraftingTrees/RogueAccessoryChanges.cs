using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Accessories;
using CalamityMod.CalPlayer;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.RogueCraftingTrees
{
    public class RogueAccessoryChanges : GlobalItem
    {
        private Mod thorium
        {
            get
            {
                ModLoader.TryGetMod("ThoriumMod", out Mod thor);
                return thor;
            }
        }

        private Mod SSM
        {
            get
            {
                ModLoader.TryGetMod("ssm", out Mod ssm);
                return ssm;
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

        private Mod clamity
        {
            get
            {
                ModLoader.TryGetMod("Clamity", out Mod clam);
                return clam;
            }
        }
        private Mod fargo
        {
            get
            {
                ModLoader.TryGetMod("FargowiltasSouls", out Mod farg);
                return farg;
            }
        }

        private Mod fargocross
        {
            get
            {
                ModLoader.TryGetMod("FargowiltasCrossmod", out Mod fargc);
                return fargc;
            }
        }


        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;

            if (thorium != null)
            {
                //Vampiric Talisman
                if (item.type == ModContent.ItemType<VampiricTalisman>())
                {
                    ModItem shinobiSigil = thorium.Find<ModItem>("ShinobiSigil");
                    shinobiSigil.UpdateAccessory(player, hideVisual);
                }

                if (clamity != null)
                {
                    if (item.type == clamity.Find<ModItem>("DraculasCharm").Type)
                    {
                        ModItem shinobiSigil = thorium.Find<ModItem>("ShinobiSigil");
                        shinobiSigil.UpdateAccessory(player, hideVisual);
                    }
                }

                //Nanotech
                ModItem scutterGem = calamity.Find<ModItem>("ScuttlersJewel");

                if (item.type == thorium.Find<ModItem>("BoneGrip").Type)
                {
                    scutterGem.UpdateAccessory(player, hideVisual);
                }

                if (item.type == ModContent.ItemType<FilthyGlove>() || item.type == ModContent.ItemType<BloodstainedGlove>())
                {
                    scutterGem.UpdateAccessory(player, hideVisual);
                    ModItem boneGrip = thorium.Find<ModItem>("BoneGrip");
                    boneGrip.UpdateAccessory(player, hideVisual);
                }

                if (item.type == thorium.Find<ModItem>("MagnetoGrip").Type)
                {
                    scutterGem.UpdateAccessory(player, hideVisual);
                    CalamityPlayer modPlayer = player.Calamity();
                    modPlayer.bloodyGlove = true;
                    modPlayer.filthyGlove = true;
                }

                if (item.type == ModContent.ItemType<Nanotech>() || item.type == ModContent.ItemType<ElectriciansGlove>())
                {
                    scutterGem.UpdateAccessory(player, hideVisual);
                    ModItem magnetoGrip = thorium.Find<ModItem>("MagnetoGrip");
                    magnetoGrip.UpdateAccessory(player, hideVisual);
                }

                if (fargo != null)
                {
                    if (item.type == fargo.Find<ModItem>("UniverseSoul").Type)
                    {
                        scutterGem.UpdateAccessory(player, hideVisual);
                        ModItem magnetoGrip = thorium.Find<ModItem>("MagnetoGrip");
                        magnetoGrip.UpdateAccessory(player, hideVisual);
                    }

                    if (item.type == fargo.Find<ModItem>("EternitySoul").Type)
                    {
                        scutterGem.UpdateAccessory(player, hideVisual);
                        ModItem magnetoGrip = thorium.Find<ModItem>("MagnetoGrip");
                        magnetoGrip.UpdateAccessory(player, hideVisual);
                    }

                    if (fargocross != null)
                    {
                        if (item.type == fargocross.Find<ModItem>("VagabondsSoul").Type)
                        {
                            scutterGem.UpdateAccessory(player, hideVisual);
                            ModItem magnetoGrip = thorium.Find<ModItem>("MagnetoGrip");
                            magnetoGrip.UpdateAccessory(player, hideVisual);
                        }
                    }
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
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;

            string scuttlerInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Scuttler");
            string boneInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Bone");
            string bloodyfilthyInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.BloodyFilthy");
            string magnetoInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Magneto");

            string shinobiSigil = "6% increased throwing critical strike chance\nDealing two consecutive throwing critical strikes will discharge dark lightning and increase throwing speed briefly\nThis effect has a cooldown of 2 seconds.";

            if (thorium != null)
            {
                if (item.type == thorium.Find<ModItem>("ShinobiSigil").Type)
                {
                    AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ShinobiNerf"), true);
                }

                if (item.type == ModContent.ItemType<VampiricTalisman>())
                {
                    AddTooltip(tooltips, shinobiSigil, true);
                }

                if (clamity != null)
                {
                    if (item.type == clamity.Find<ModItem>("DraculasCharm").Type)
                    {
                        AddTooltip(tooltips, shinobiSigil, true);
                    }
                }

                if (item.type == thorium.Find<ModItem>("BoneGrip").Type)
                {
                    AddTooltip(tooltips, scuttlerInfo, true);
                }

                if (item.type == ModContent.ItemType<FilthyGlove>() || item.type == ModContent.ItemType<BloodstainedGlove>())
                {
                    AddTooltip(tooltips, boneInfo, true);
                    AddTooltip(tooltips, scuttlerInfo, true);
                }

                if (item.type == thorium.Find<ModItem>("MagnetoGrip").Type)
                {
                    AddTooltip(tooltips, bloodyfilthyInfo, true);
                    AddTooltip(tooltips, scuttlerInfo, true);
                }

                if (item.type == ModContent.ItemType<Nanotech>() || item.type == ModContent.ItemType<ElectriciansGlove>())
                {
                    AddTooltip(tooltips, magnetoInfo, true);
                    AddTooltip(tooltips, boneInfo, true);
                    AddTooltip(tooltips, scuttlerInfo, true);
                }
            }
        }
    }
}
