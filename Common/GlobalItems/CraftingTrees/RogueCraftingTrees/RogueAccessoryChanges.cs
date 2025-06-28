using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Accessories;
using CalamityMod.CalPlayer;
using CalamityMod;
using Microsoft.Xna.Framework;

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

            string boneInfo = "Picking up a rogue item has a 33% chance to duplicate it and increase your rogue attack speed\nThis effect can only trigger once every half-second while out of combat";
            string bloodyfilthyInfo = "Stealth strikes have +8 armor penetration, deal 8% more damage, and heal for 2 HP";
            string magnetoInfo = "Increases pickup range of rogue items on the ground";

            string shinobiSigil = "6% increased throwing critical strike chance\nDealing two consecutive throwing critical strikes will discharge dark lightning and increase throwing speed briefly\nThis effect has a cooldown of 2 seconds.";

            if (thorium != null)
            {
                if (item.type == thorium.Find<ModItem>("ShinobiSigil").Type)
                {
                    tooltips.Add(new TooltipLine(Mod, "ShinobiCooldown", "This effect has a cooldowwn of 2 seconds")
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }

                if (item.type == ModContent.ItemType<VampiricTalisman>())
                {
                    tooltips.Add(new TooltipLine(Mod, "shinobiInfo", shinobiSigil)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }

                if (clamity != null)
                {
                    if (item.type == clamity.Find<ModItem>("DraculasCharm").Type)
                    {
                        tooltips.Add(new TooltipLine(Mod, "shinobiInfo", shinobiSigil)
                        {
                            OverrideColor = new Color?(InfernalRed)
                        });
                    }
                }

                if (item.type == ModContent.ItemType<FilthyGlove>() || item.type == ModContent.ItemType<BloodstainedGlove>())
                {
                    tooltips.Add(new TooltipLine(Mod, "boneInfo", boneInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }

                if (item.type == thorium.Find<ModItem>("MagnetoGrip").Type)
                {
                    tooltips.Add(new TooltipLine(Mod, "bloodyfilthyInfo", bloodyfilthyInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }

                if (item.type == ModContent.ItemType<Nanotech>() || item.type == ModContent.ItemType<ElectriciansGlove>())
                {
                    tooltips.Add(new TooltipLine(Mod, "magnetoInfo", magnetoInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "boneInfo", boneInfo)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }
            }
        }
    }
}
