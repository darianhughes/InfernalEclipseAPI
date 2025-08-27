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
using Terraria.ID;
using ThoriumMod.Items.Donate;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs;
using Terraria.Localization;

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

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "CalamityMod" &&
                item.ModItem.Name == "OrnateShield" &&
                sots != null)
            {
                ModItem shatterShield = sots.Find<ModItem>("ShatterHeartShield");
                shatterShield.UpdateAccessory(player, hideVisual);
            }

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "CalamityMod" &&
                item.ModItem.Name == "AsgardsValor" &&
                thorium != null && sots != null)
            {
                //ModItem moltenScale = thorium.Find<ModItem>("ObsidianScale");
                //moltenScale.UpdateAccessory(player, hideVisual);
                //base.UpdateAccessory(item, player, hideVisual);
                ModItem shatterShield = sots.Find<ModItem>("ShatterHeartShield");
                shatterShield.UpdateAccessory(player, hideVisual);
            }

            ModItem plasmaGen = null;
            if (thorium != null)
            {
                if (thorium.TryFind<ModItem>("PlasmaGenerator", out plasmaGen))
                {
                    if (item.type == plasmaGen.Type)
                    {
                        ModItem moltenScale = thorium.Find<ModItem>("ObsidianScale");
                        moltenScale.UpdateAccessory(player, hideVisual);
                    }
                }
            }

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "CalamityMod" &&
                item.ModItem.Name == "AsgardianAegis" &&
                thorium != null)
            {
                ModItem moltenScale = thorium.Find<ModItem>("ObsidianScale");
                moltenScale.UpdateAccessory(player, hideVisual);
                if (plasmaGen != null)
                {
                    plasmaGen.UpdateAccessory(player, hideVisual);
                }
                if (sots != null)
                {
                    ModItem shatterShield = sots.Find<ModItem>("ShatterHeartShield");
                    shatterShield.UpdateAccessory(player, hideVisual);
                }
            }

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "ThoriumMod" &&
                item.ModItem.Name == "TerrariumDefender")
            {
                var CalPlayer = player.GetModPlayer<CalamityPlayer>();

                CalPlayer.DashID = null;
                player.dashType = 0;

                player.noKnockback = false;
                player.longInvince = false;
                player.fireWalk = false;
                player.statLifeMax2 -= 20;
                player.buffImmune[20] = false;
                player.buffImmune[22] = false;
                player.buffImmune[23] = false;
                player.buffImmune[30] = false;
                player.buffImmune[31] = false;
                player.buffImmune[32] = false;
                player.buffImmune[33] = false;
                player.buffImmune[35] = false;
                player.buffImmune[36] = false;
                player.buffImmune[46] = false;
                player.buffImmune[47] = false;
                player.buffImmune[156] = false;

                if (sots != null)
                {
                    ModItem olympianAegis = sots.Find<ModItem>("OlympianAegis");
                    ModItem chiseledBarrier = sots.Find<ModItem>("ChiseledBarrier");

                    olympianAegis.UpdateAccessory(player, hideVisual);
                    chiseledBarrier.UpdateAccessory(player, hideVisual);
                }
                else
                {
                    ModItem lifeQuartzShield = thorium.Find<ModItem>("LifeQuartzShield");

                    lifeQuartzShield.UpdateAccessory(player, hideVisual);
                }

                if (player.statLife <= player.statLifeMax2 * 0.25)
                {
                    if (thorium.TryFind("TerrariumDefenderBuff", out ModBuff tdBuff))
                        player.AddBuff(tdBuff.Type, 10);

                    player.lifeRegen += 20;
                    player.statDefense += 20;
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient && Main.myPlayer != player.whoAmI)
                {
                    Player localPlayer = Main.LocalPlayer;
                    if (Vector2.DistanceSquared(localPlayer.Center, player.Center) < 160000f)
                        localPlayer.AddBuff(BuffID.Regeneration, 30);
                }
            }

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "CalamityMod" &&
                item.ModItem.Name == "DeificAmulet")
            {
                if (thorium != null)
                {
                    ModItem sweetVengence = thorium.Find<ModItem>("SweetVengeance");

                    sweetVengence.UpdateAccessory(player, hideVisual);
                }
                base.UpdateAccessory(item, player, hideVisual);
            }

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "ThoriumMod" &&
                item.ModItem.Name == "MantleoftheProtector")
            {
                ModItem deificAmulet = calamity.Find<ModItem>("DeificAmulet");
                ModItem protectorCape = thorium.Find<ModItem>("CapeoftheSurvivor");
                ModItem sweetVengence = thorium.Find<ModItem>("SweetVengeance");

                deificAmulet.UpdateAccessory(player, hideVisual);
                protectorCape.UpdateAccessory(player, hideVisual);
                sweetVengence.UpdateAccessory(player, hideVisual);
                base.UpdateAccessory(item, player, hideVisual);
            }

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "CalamityMod" &&
                item.ModItem.Name == "RampartofDeities")
            {
                if (thorium != null)
                {
                    if (sots != null)
                    {
                        ModItem olympianAegis = sots.Find<ModItem>("OlympianAegis");
                        ModItem chiseledBarrier = sots.Find<ModItem>("ChiseledBarrier");

                        olympianAegis.UpdateAccessory(player, hideVisual);
                        chiseledBarrier.UpdateAccessory(player, hideVisual);
                    }
                    else
                    {
                        ModItem lifeQuartzShield = thorium.Find<ModItem>("LifeQuartzShield");

                        lifeQuartzShield.UpdateAccessory(player, hideVisual);
                    }

                    if (player.statLife <= player.statLifeMax2 * 0.25)
                    {
                        if (thorium.TryFind("TerrariumDefenderBuff", out ModBuff tdBuff))
                            player.AddBuff(tdBuff.Type, 10);

                        player.lifeRegen += 20;
                        player.statDefense += 20;
                    }
                    else if (Main.netMode == NetmodeID.MultiplayerClient && Main.myPlayer != player.whoAmI)
                    {
                        Player localPlayer = Main.LocalPlayer;
                        if (Vector2.DistanceSquared(localPlayer.Center, player.Center) < 160000f)
                            localPlayer.AddBuff(BuffID.Regeneration, 30);
                    }

                    ModItem motP = thorium.Find<ModItem>("MantleoftheProtector");
                    ModItem protectorCape = thorium.Find<ModItem>("CapeoftheSurvivor");
                    ModItem sweetVengence = thorium.Find<ModItem>("SweetVengeance");

                    protectorCape.UpdateAccessory(player, hideVisual);
                    sweetVengence.UpdateAccessory(player, hideVisual);
                    motP.UpdateAccessory(player, hideVisual);
                }
                else if (sots != null)
                {
                    ModItem olympianAegis = sots.Find<ModItem>("OlympianAegis");
                    ModItem chiseledBarrier = sots.Find<ModItem>("ChiseledBarrier");

                    olympianAegis.UpdateAccessory(player, hideVisual);
                    chiseledBarrier.UpdateAccessory(player, hideVisual);
                }
                base.UpdateAccessory(item, player, hideVisual);
            }

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "Clamity" &&
                item.ModItem.Name == "SupremeBarrier")
            {
                if (plasmaGen != null)
                {
                    plasmaGen.UpdateAccessory(player, hideVisual);
                }
                if (sots != null)
                {
                    ModItem shatterShield = sots.Find<ModItem>("ShatterHeartShield");
                    shatterShield.UpdateAccessory(player, hideVisual);
                }

                if (thorium != null)
                {
                    ModItem moltenScale = thorium.Find<ModItem>("ObsidianScale");
                    moltenScale.UpdateAccessory(player, hideVisual);
                    if (sots != null)
                    {
                        ModItem olympianAegis = sots.Find<ModItem>("OlympianAegis");
                        ModItem chiseledBarrier = sots.Find<ModItem>("ChiseledBarrier");

                        olympianAegis.UpdateAccessory(player, hideVisual);
                        chiseledBarrier.UpdateAccessory(player, hideVisual);
                    }
                    else
                    {
                        ModItem lifeQuartzShield = thorium.Find<ModItem>("LifeQuartzShield");

                        lifeQuartzShield.UpdateAccessory(player, hideVisual);
                    }

                    if (player.statLife <= player.statLifeMax2 * 0.25)
                    {
                        if (thorium.TryFind("TerrariumDefenderBuff", out ModBuff tdBuff))
                            player.AddBuff(tdBuff.Type, 10);

                        player.lifeRegen += 20;
                        player.statDefense += 20;
                    }
                    else if (Main.netMode == NetmodeID.MultiplayerClient && Main.myPlayer != player.whoAmI)
                    {
                        Player localPlayer = Main.LocalPlayer;
                        if (Vector2.DistanceSquared(localPlayer.Center, player.Center) < 160000f)
                            localPlayer.AddBuff(BuffID.Regeneration, 30);
                    }
                    ModItem motP = thorium.Find<ModItem>("MantleoftheProtector");
                    ModItem protectorCape = thorium.Find<ModItem>("CapeoftheSurvivor");
                    ModItem sweetVengence = thorium.Find<ModItem>("SweetVengeance");

                    protectorCape.UpdateAccessory(player, hideVisual);
                    sweetVengence.UpdateAccessory(player, hideVisual);
                    motP.UpdateAccessory(player, hideVisual);
                }
                else if (sots != null)
                {
                    ModItem olympianAegis = sots.Find<ModItem>("OlympianAegis");
                    ModItem chiseledBarrier = sots.Find<ModItem>("ChiseledBarrier");

                    olympianAegis.UpdateAccessory(player, hideVisual);
                    chiseledBarrier.UpdateAccessory(player, hideVisual);
                }
                base.UpdateAccessory(item, player, hideVisual);
            }
        }

        public void AddTooltip(List<TooltipLine> tooltips, string stealthTooltip, bool InfernalRedActive = false, bool NoSOTSPinkActive = false)
        {
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

            string moltenScaleInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.MoltenScale");
            string chiseledBarrierInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ChiseledBarrier");
            string chiseledHiddenInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ChiseledHidden");
            string olympianAegisInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Olympian");
            string lifeQuartzShieldInfo1 = "Increases the rate at which you regenerate life";
            string lifeUnder25Info = "Receiving damage below 25% life surrounds you in a protective bubble\nWhile in the bubble, you will recover life equal to your bonus healing every second";
            string lifeUnder25Info2 = "Additionally, damage taken will be reduced by 10%\nThis effect needs to recharge for 30 seconds after triggering";
            string motpInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Mantle");
            string cotsInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.COTS");
            string sweetInfo1 = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Sweet1");
            string sweetInfo2 = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.SweetAlt");
            string sweetAltInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Sweet2");
            string tdInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.TerraDefend");
            string daInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.DA");
            string shsInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.SHS");
            string pgInfo = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.PlasmaGen");

            if (sots != null && (item.type == ModContent.ItemType<AsgardsValor>() || item.type == ModContent.ItemType<AsgardianAegis>() || item.type == ModContent.ItemType<OrnateShield>()))
            {
                AddTooltip(tooltips, shsInfo, true);
            }

            ModItem plasmaGen = null;
            if (thorium != null)
            {
                if (thorium.TryFind("PlasmaGenerator", out plasmaGen))
                {
                    if (item.type == plasmaGen.Type)
                    {
                        AddTooltip(tooltips, moltenScaleInfo, true);
                    }
                }
            }

            if (item.type == ModContent.ItemType<AsgardianAegis>() & thorium != null)
            {
                AddTooltip(tooltips, moltenScaleInfo, true);
                if (plasmaGen != null)
                {
                    AddTooltip(tooltips, pgInfo, true);
                }
            }

            if (thorium != null)
            {
                if (item.type == ModContent.ItemType<DeificAmulet>())
                {
                    AddTooltip(tooltips, sweetInfo1, true);
                    AddTooltip(tooltips, sweetAltInfo, true);
                }

                if (item.type == thorium.Find<ModItem>("MantleoftheProtector").Type)
                {
                    AddTooltip(tooltips, cotsInfo, true);
                    AddTooltip(tooltips, sweetInfo1, true);
                    AddTooltip(tooltips, sweetInfo2, true);
                    AddTooltip(tooltips, daInfo, true);
                }

                if (sots != null)
                {
                    if (item.type == thorium.Find<ModItem>("TerrariumDefender").Type)
                    {
                        foreach (TooltipLine tooltip in tooltips)
                        {
                            if (tooltip.Text.Contains("Maximum life increased by 20"))
                            {
                                tooltip.Text = chiseledBarrierInfo;
                                tooltip.OverrideColor = new Color?(InfernalRed);
                            }
                            if (tooltip.Text.Contains("Prolonges after hit invincibility"))
                            {
                                tooltip.Text = olympianAegisInfo;
                                tooltip.OverrideColor = new Color?(InfernalRed);
                            }
                            if (tooltip.Text.Contains("Grants immunity to most status debuffs, knockback, and fire blocks"))
                            {
                                tooltip.Text = chiseledHiddenInfo;
                                tooltip.OverrideColor = new Color?(InfernalRed);
                            }
                        }
                    }
                    if (clamity != null)
                    {
                        if (item.type == clamity.Find<ModItem>("SupremeBarrier").Type)
                        {
                            AddTooltip(tooltips, shsInfo, true);
                            if (plasmaGen != null)
                            {
                                AddTooltip(tooltips, pgInfo, true);
                            }
                            AddTooltip(tooltips, moltenScaleInfo, true);

                            AddTooltip(tooltips, tdInfo, true);

                            AddTooltip(tooltips, motpInfo, true);
                            AddTooltip(tooltips, chiseledBarrierInfo, true);
                            AddTooltip(tooltips, olympianAegisInfo, true);
                            AddTooltip(tooltips, chiseledHiddenInfo, true);

                            AddTooltip(tooltips, cotsInfo, true);
                            AddTooltip(tooltips, sweetInfo1, true);
                            AddTooltip(tooltips, sweetInfo2, true);
                        }
                    }
                    if (item.type == ModContent.ItemType<RampartofDeities>())
                    {
                        AddTooltip(tooltips, tdInfo, true);

                        AddTooltip(tooltips, motpInfo, true);
                        AddTooltip(tooltips, chiseledBarrierInfo, true);
                        AddTooltip(tooltips, olympianAegisInfo, true);
                        AddTooltip(tooltips, chiseledHiddenInfo, true);

                        AddTooltip(tooltips, cotsInfo, true);
                        AddTooltip(tooltips, sweetInfo1, true);
                        AddTooltip(tooltips, sweetInfo2, true);
                    }
                }
                else
                {
                    if (item.type == thorium.Find<ModItem>("TerrariumDefender").Type)
                    {
                        foreach (TooltipLine tooltip in tooltips)
                        {
                            if (tooltip.Text.Contains("Maximum life increased by 20"))
                            {
                                tooltip.Text = lifeQuartzShieldInfo1;
                                tooltip.OverrideColor = new Color?(NoSOTSPink);
                            }
                            if (tooltip.Text.Contains("Prolonges after hit invincibility"))
                            {
                                tooltip.Text = lifeUnder25Info;
                                tooltip.OverrideColor = new Color?(NoSOTSPink);
                            }
                            if (tooltip.Text.Contains("Grants immunity to most status debuffs, knockback, and fire blocks"))
                            {
                                tooltip.Text = lifeUnder25Info2;
                                tooltip.OverrideColor = new Color?(NoSOTSPink);
                            }
                        }
                    }
                    if (clamity != null)
                    {
                        if (item.type == clamity.Find<ModItem>("SupremeBarrier").Type)
                        {
                            if (plasmaGen != null)
                            {
                                AddTooltip(tooltips, pgInfo, true);
                            }
                            AddTooltip(tooltips, tdInfo, true);

                            AddTooltip(tooltips, lifeQuartzShieldInfo1, false, true);
                            AddTooltip(tooltips, lifeUnder25Info, false, true);
                            AddTooltip(tooltips, lifeUnder25Info2, false, true);

                            AddTooltip(tooltips, cotsInfo, true);
                            AddTooltip(tooltips, sweetInfo1, true);
                            AddTooltip(tooltips, sweetInfo2, true);

                            AddTooltip(tooltips, moltenScaleInfo, true);
                        }
                    }
                    if (item.type == ModContent.ItemType<RampartofDeities>())
                    {
                        AddTooltip(tooltips, tdInfo, true);

                        AddTooltip(tooltips, lifeQuartzShieldInfo1, false, true);
                        AddTooltip(tooltips, lifeUnder25Info, false, true);
                        AddTooltip(tooltips, lifeUnder25Info2, false, true);

                        AddTooltip(tooltips, cotsInfo, true);
                        AddTooltip(tooltips, sweetInfo1, true);
                        AddTooltip(tooltips, sweetInfo2, true);
                    }
                }
            }
            else if (sots != null)
            {
                if (clamity != null)
                {
                    if (item.type == clamity.Find<ModItem>("SupremeBarrier").Type)
                    {
                        AddTooltip(tooltips, shsInfo, true);
                        AddTooltip(tooltips, chiseledBarrierInfo, true);
                        AddTooltip(tooltips, olympianAegisInfo, true);
                        AddTooltip(tooltips, chiseledHiddenInfo, true);
                    }
                }
                if (item.type == ModContent.ItemType<RampartofDeities>())
                {
                    AddTooltip(tooltips, chiseledBarrierInfo, true);
                    AddTooltip(tooltips, olympianAegisInfo, true);
                    AddTooltip(tooltips, chiseledHiddenInfo, true);
                }
            }
        }
    }
}
