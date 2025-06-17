using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Accessories;
using ThoriumMod.Items.BossMini;
using ThoriumMod.Items.SummonItems;
using SOTS.Items;
using ThoriumMod.Items.Donate;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.NucleogenesisTree
{
    public class SummonerAccessoryChanges : GlobalItem
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

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;


            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "ThoriumMod" &&
                item.ModItem.Name == "ScryingGlass")
            {
                --player.maxTurrets;
            }

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
                    scryingGlass.UpdateAccessory(player, hideVisual);
                    --player.maxTurrets;
                }

                if (item.ModItem != null &&
                    item.ModItem.Mod.Name == "SOTS" &&
                    item.ModItem.Name == "FortressGenerator")
                {
                    scryingGlass.UpdateAccessory(player, hideVisual);
                    --player.maxTurrets;
                    necroticSkull.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    --player.maxTurrets;

                    player.lifeRegen -= 2;
                    ref StatModifier local2 = ref player.GetDamage(DamageClass.Generic);
                    local2 -= 0.1f;

                }

                if (item.type == ModContent.ItemType<StatisBlessing>())
                {
                    scryingGlass.UpdateAccessory(player, hideVisual);
                    --player.maxTurrets;
                    necroticSkull.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    cystralScorpion.UpdateAccessory(player, hideVisual);
                    fortressGenerator.UpdateAccessory(player, hideVisual);
                    player.lifeRegen -= 2;
                    --player.maxMinions;
                    ref StatModifier local = ref player.GetDamage(DamageClass.Generic);
                    local -= 0.1f;
                    steamkeeperWatch.UpdateAccessory(player, hideVisual);
                    ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                    local2 -= 0.1f;
                }

                if (item.type == ModContent.ItemType<StatisCurse>())
                {
                    scryingGlass.UpdateAccessory(player, hideVisual);
                    --player.maxTurrets;
                    necroticSkull.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    cystralScorpion.UpdateAccessory(player, hideVisual);
                    fortressGenerator.UpdateAccessory(player, hideVisual);
                    player.lifeRegen -= 2;
                    --player.maxMinions;
                    ref StatModifier local = ref player.GetDamage(DamageClass.Generic);
                    local -= 0.1f;
                    steamkeeperWatch.UpdateAccessory(player, hideVisual);
                    ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                    local2 -= 0.1f;
                    yumasPendant.UpdateAccessory(player, hideVisual);
                    ref StatModifier local3 = ref player.GetDamage(DamageClass.Summon);
                    local3 -= 0.1f;
                    --player.maxMinions;

                    ++player.maxTurrets;
                }

                if (item.type == ModContent.ItemType<Nucleogenesis>())
                {
                    scryingGlass.UpdateAccessory(player, hideVisual);
                    --player.maxTurrets;
                    necroticSkull.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    cystralScorpion.UpdateAccessory(player, hideVisual);
                    fortressGenerator.UpdateAccessory(player, hideVisual);
                    player.lifeRegen -= 2;
                    --player.maxMinions;
                    ref StatModifier local = ref player.GetDamage(DamageClass.Generic);
                    local -= 0.1f;
                    steamkeeperWatch.UpdateAccessory(player, hideVisual);
                    ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                    local2 -= 0.1f;
                    yumasPendant.UpdateAccessory(player, hideVisual);
                    ref StatModifier local3 = ref player.GetDamage(DamageClass.Summon);
                    local3 -= 0.1f;
                    --player.maxMinions;

                    ++player.maxTurrets;
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
                    scryingGlass.UpdateAccessory(player, hideVisual);
                    necroticSkull.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    cystralScorpion.UpdateAccessory(player, hideVisual);
                    steamkeeperWatch.UpdateAccessory(player, hideVisual);
                    ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                    local2 -= 0.1f;
                }

                if (item.type == ModContent.ItemType<StatisCurse>() || item.type == ModContent.ItemType<Nucleogenesis>())
                {
                    scryingGlass.UpdateAccessory(player, hideVisual);
                    --player.maxTurrets;
                    necroticSkull.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    cystralScorpion.UpdateAccessory(player, hideVisual);
                    steamkeeperWatch.UpdateAccessory(player, hideVisual);
                    ref StatModifier local2 = ref player.GetDamage(DamageClass.Summon);
                    local2 -= 0.1f;
                    yumasPendant.UpdateAccessory(player, hideVisual);
                    ref StatModifier local3 = ref player.GetDamage(DamageClass.Summon);
                    local3 -= 0.1f;
                    --player.maxMinions;

                    ++player.maxTurrets;
                }
            }
            else if (sots != null)
            {
                ModItem fortressGenerator = sots.Find<ModItem>("FortressGenerator");

                if (item.type == ModContent.ItemType<StatisBlessing>())
                {
                    fortressGenerator.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    ++player.maxTurrets;
                    ref StatModifier local = ref player.GetDamage(DamageClass.Generic);
                    local -= 0.1f;
                }

                if (item.type == ModContent.ItemType<StatisCurse>() || item.type == ModContent.ItemType<Nucleogenesis>())
                {
                    fortressGenerator.UpdateAccessory(player, hideVisual);
                    --player.maxMinions;
                    ++player.maxTurrets;
                    ref StatModifier local = ref player.GetDamage(DamageClass.Generic);
                    local -= 0.1f;

                    ++player.maxTurrets;
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

            string scryInfo = "Pressing the 'Accessory Ability' key will toggle an increased range of view";
            string skullInfo = "Minion and sentry attacks briefly deal 2 more damage after killing an enemy";
            string scorpionInfo = "Minion attacks have a 15% chance to produce a crystal scorpion\nMinion attacks deal 3 more damage while a crystal scorpion is active";
            string fortressInfoNoThor = "Increases life regeneration by 2\nGenerates 4 platforms to the left and right of you\nYou can right click to drag the platforms, but they will always remain symmetrical\nSentries can be summoned on top of the platforms\nAbsorbs 25% of damage done to players on your team when above 25% life and grants immunity to knockback";
            string fortressInfo = "Generates 4 platforms to the left and right of you\nYou can right click to drag the platforms, but they will always remain symmetrical\nSentries can be summoned on top of the platforms\nAbsorbs 25% of damage done to players on your team when above 25% life and grants immunity to knockback";
            string watchInfo = "Your sentries last forever and gain up to 15% damage over the course of 2 minutes";
            string yumasInfo = "Taking damage releases a ghostly protector";

            string twoMoreSentries = "Increases max sentries by 2";
            string threeMoreSentries = "Increases max sentries by 3";

            if (sots != null & thorium != null)
            {
                if (item.type == sots.Find<ModItem>("PlatformGenerator").Type)
                {
                    tooltips.Add(new TooltipLine(Mod, "scry", scryInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }

                if (item.type == sots.Find<ModItem>("FortressGenerator").Type)
                {
                    foreach (TooltipLine tooltip in tooltips)
                    {
                        if (tooltip.Text.Contains("Increases damage by 10% and life regeneration by 2"))
                        {
                            tooltip.Text = "Increases summon damage by 10%";
                        }
                    }

                    tooltips.Add(new TooltipLine(Mod, "scry", scryInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "skull", skullInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }

                if (item.type == ModContent.ItemType<StatisBlessing>())
                {
                    tooltips.Add(new TooltipLine(Mod, "sentries", twoMoreSentries)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "scry", scryInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "skull", skullInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "scorpion", scorpionInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "fortress", fortressInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "watch", watchInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }

                if (item.type == ModContent.ItemType<StatisCurse>() || item.type == ModContent.ItemType<Nucleogenesis>())
                {
                    tooltips.Add(new TooltipLine(Mod, "sentries", threeMoreSentries)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "scry", scryInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "skull", skullInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "scorpion", scorpionInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "fortress", fortressInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "watch", watchInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "yuma", yumasInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }
            }
            else if (thorium != null)
            {
                if (item.type == ModContent.ItemType<StatisBlessing>())
                {
                    tooltips.Add(new TooltipLine(Mod, "sentries", twoMoreSentries)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "scry", scryInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "skull", skullInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "scorpion", scorpionInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "watch", watchInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }

                if (item.type == ModContent.ItemType<StatisCurse>() || item.type == ModContent.ItemType<Nucleogenesis>())
                {
                    tooltips.Add(new TooltipLine(Mod, "sentries", threeMoreSentries)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "scry", scryInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "skull", skullInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "scorpion", scorpionInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "watch", watchInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "yuma", yumasInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }
            }
            else if (sots != null)
            {
                if (item.type == ModContent.ItemType<StatisBlessing>())
                {
                    tooltips.Add(new TooltipLine(Mod, "sentries", twoMoreSentries)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "fortress", fortressInfoNoThor)
                    {
                        OverrideColor = new Color?(NoThorYellow)
                    });
                }

                if (item.type == ModContent.ItemType<StatisCurse>() || item.type == ModContent.ItemType<Nucleogenesis>())
                {
                    tooltips.Add(new TooltipLine(Mod, "sentries", threeMoreSentries)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "fortress", fortressInfoNoThor)
                    {
                        OverrideColor = new Color?(NoThorYellow)
                    });
                }
            }
        }
    }
}
