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
                thorium != null)
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
            string moltenScaleInfo = "Nearby enemies will melt away";
            string chiseledBarrierInfo = "Surrounds you with 4 orbital projectiles\nLaunches attackers away from you with javelins";
            string chiseledHiddenInfo = "Projectiles disabled when hidden";
            string olympianAegisInfo = "Increases void gain by 2 and life regeneration by 1\nReduces damage taken by 7% and increases critical strike chance by 4%";
            string lifeQuartzShieldInfo1 = "Increases the rate at which you regenerate life";
            string lifeUnder25Info = "Receiving damage below 25% life surrounds you in a protective bubble\nWhile in the bubble, you will recover life equal to your bonus healing every second";
            string lifeUnder25Info2 = "Additionally, damage taken will be reduced by 10%\nThis effect needs to recharge for 30 seconds after triggering";
            string motpInfo = "Dispels up to one damaging debuff off of you every 6 seconds";
            string cotsInfo = "You quickly gain up to 20% damage reduction over time\nGetting hit briefly reduces this bonus to -10% damage reduction\nWhile this bonus is above 0%, you gain up to 2 life recovery and block 1 damage attacks";
            string sweetInfo1 = "Increases movement speed and when damaged";
            string sweetInfo2 = "Causes bees & stars to appear and douses the user in honey when damaged";
            string sweetAltInfo = "Causes bees to appear and douses the user in honey when damaged";
            string tdInfo = "When below 25% life, you will rapidly regenerate life and gain increased defense";
            string daInfo = "Grants bonus invincibility frames based on your missing health\nThis effect scales from 10 frames at full HP to 40 frames at 25% or less HP";
            string shsInfo = "Getting hit surrounds you with ice shards\nIncreases max life by 20";
            string pgInfo = "Generates a fiery barrier that burns incoming hostile projectile\nAfter burning a projectile, the shield must regenerate for 10 seconds\nWhile below 25% life, the shield generates twice as fast";

            if (sots != null && (item.type == ModContent.ItemType<AsgardsValor>() || item.type == ModContent.ItemType<AsgardianAegis>() || item.type == ModContent.ItemType<OrnateShield>()))
            {
                tooltips.Add(new TooltipLine(Mod, "shsInfo", shsInfo)
                {
                    OverrideColor = new Color?(InfernalRed)
                });
            }

            ModItem plasmaGen = null;
            if (thorium != null)
            {
                if (thorium.TryFind("PlasmaGenerator", out plasmaGen))
                {
                    if (item.type == plasmaGen.Type)
                    {
                        tooltips.Add(new TooltipLine(Mod, "MoltenScaleInfo", moltenScaleInfo)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                    }
                }
            }

            if (item.type == ModContent.ItemType<AsgardianAegis>() & thorium != null)
            {
                tooltips.Add(new TooltipLine(Mod, "MoltenScaleInfo", moltenScaleInfo)
                {
                    OverrideColor = new Color?(InfernalRed)
                });
                if (plasmaGen != null)
                {
                    tooltips.Add(new TooltipLine(Mod, "pgInfo", pgInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }
            }

            if (thorium != null)
            {
                if (item.type == ModContent.ItemType<DeificAmulet>())
                {
                    tooltips.Add(new TooltipLine(Mod, "sI1", sweetInfo1)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "sAI", sweetAltInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }

                if (item.type == thorium.Find<ModItem>("MantleoftheProtector").Type)
                {
                    tooltips.Add(new TooltipLine(Mod, "costI1", cotsInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "sI1", sweetInfo1)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "sI2", sweetInfo2)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "daI", daInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
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
                            tooltips.Add(new TooltipLine(Mod, "shsInfo", shsInfo)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });
                            if (plasmaGen != null)
                            {
                                tooltips.Add(new TooltipLine(Mod, "pgInfo", pgInfo)
                                {
                                    OverrideColor = new Color?(InfernalRed)
                                });
                            }

                            tooltips.Add(new TooltipLine(Mod, "MoltenScaleInfo", moltenScaleInfo)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });

                            tooltips.Add(new TooltipLine(Mod, "tdInfo", tdInfo)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });

                            tooltips.Add(new TooltipLine(Mod, "motpInfo", motpInfo)
                            {
                                OverrideColor = new Color?(InfernalRed) 
                            });
                            tooltips.Add(new TooltipLine(Mod, "cbI1", chiseledBarrierInfo)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });
                            tooltips.Add(new TooltipLine(Mod, "oaI", olympianAegisInfo)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });
                            tooltips.Add(new TooltipLine(Mod, "cbhI", chiseledHiddenInfo)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });

                            tooltips.Add(new TooltipLine(Mod, "costI1", cotsInfo)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });
                            tooltips.Add(new TooltipLine(Mod, "sI1", sweetInfo1)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });
                            tooltips.Add(new TooltipLine(Mod, "sI2", sweetInfo2)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });
                        }
                    }
                    if (item.type == ModContent.ItemType<RampartofDeities>())
                    {
                        tooltips.Add(new TooltipLine(Mod, "tdInfo", tdInfo)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });

                        tooltips.Add(new TooltipLine(Mod, "motpInfo", motpInfo)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                        tooltips.Add(new TooltipLine(Mod, "cbI1", chiseledBarrierInfo)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                        tooltips.Add(new TooltipLine(Mod, "oaI", olympianAegisInfo)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                        tooltips.Add(new TooltipLine(Mod, "cbhI", chiseledHiddenInfo)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });

                        tooltips.Add(new TooltipLine(Mod, "costI1", cotsInfo)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                        tooltips.Add(new TooltipLine(Mod, "sI1", sweetInfo1)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                        tooltips.Add(new TooltipLine(Mod, "sI2", sweetInfo2)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
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
                                tooltips.Add(new TooltipLine(Mod, "pgInfo", pgInfo)
                                {
                                    OverrideColor = new Color?(InfernalRed)
                                });
                            }
                            tooltips.Add(new TooltipLine(Mod, "tdInfo", tdInfo)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });

                            tooltips.Add(new TooltipLine(Mod, "lqI1", lifeQuartzShieldInfo1)
                            {
                                OverrideColor = new Color?(NoSOTSPink)
                            });
                            tooltips.Add(new TooltipLine(Mod, "lqI2", lifeUnder25Info) 
                            {
                                OverrideColor = new Color?(NoSOTSPink)
                            });
                            tooltips.Add(new TooltipLine(Mod, "lqI3", lifeUnder25Info2)
                            {
                                OverrideColor = new Color?(NoSOTSPink)
                            });

                            tooltips.Add(new TooltipLine(Mod, "costI1", cotsInfo)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });
                            tooltips.Add(new TooltipLine(Mod, "sI1", sweetInfo1)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });
                            tooltips.Add(new TooltipLine(Mod, "sI2", sweetInfo2)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });

                            tooltips.Add(new TooltipLine(Mod, "MoltenScaleInfo", moltenScaleInfo)
                            {
                                OverrideColor = new Color?(InfernalRed)
                            });
                        }
                    }
                    if (item.type == ModContent.ItemType<RampartofDeities>())
                    {
                        tooltips.Add(new TooltipLine(Mod, "tdInfo", tdInfo)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });

                        tooltips.Add(new TooltipLine(Mod, "lqI1", lifeQuartzShieldInfo1)
                        {
                            OverrideColor = new Color?(NoSOTSPink)
                        });
                        tooltips.Add(new TooltipLine(Mod, "lqI2", lifeUnder25Info)
                        {
                            OverrideColor = new Color?(NoSOTSPink)
                        });
                        tooltips.Add(new TooltipLine(Mod, "lqI3", lifeUnder25Info2)
                        {
                            OverrideColor = new Color?(NoSOTSPink)
                        });

                        tooltips.Add(new TooltipLine(Mod, "costI1", cotsInfo)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                        tooltips.Add(new TooltipLine(Mod, "sI1", sweetInfo1)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                        tooltips.Add(new TooltipLine(Mod, "sI2", sweetInfo2)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                    }
                }
            }
            else if (sots != null)
            {
                if (clamity != null)
                {
                    if (item.type == clamity.Find<ModItem>("SupremeBarrier").Type)
                    {
                        tooltips.Add(new TooltipLine(Mod, "shsInfo", shsInfo)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                        tooltips.Add(new TooltipLine(Mod, "cbI1", chiseledBarrierInfo)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                        tooltips.Add(new TooltipLine(Mod, "oaI", olympianAegisInfo)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                        tooltips.Add(new TooltipLine(Mod, "cbhI", chiseledHiddenInfo)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                    }
                }
                if (item.type == ModContent.ItemType<RampartofDeities>())
                {
                    tooltips.Add(new TooltipLine(Mod, "cbI1", chiseledBarrierInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "oaI", olympianAegisInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "cbhI", chiseledHiddenInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }
            }
        }
    }
}
