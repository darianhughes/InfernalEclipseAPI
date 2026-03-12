using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Items;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;
using InfernumMode.Content.Rarities.InfernumRarities;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.CelestialIllumination
{
    public class CelestialIllumination : ModItem
    {
        readonly static int beam = ModContent.ProjectileType<CelestialIlluminationBeam>();
        readonly static int star = ModContent.ProjectileType<CelestialIlluminationStar>();
        public static int Tier()
        {
            // Ternary statements? What are those?
            if (NPC.downedMoonlord)
            {
                if (CalamityConditions.DownedProvidence.IsMet())
                {
                    if (CalamityConditions.DownedPolterghast.IsMet())
                    {
                        if (CalamityConditions.DownedDevourerOfGods.IsMet())
                        {
                            if (CalamityConditions.DownedYharon.IsMet())
                            {
                                return 5;
                            }
                            return 4;
                        }
                        return 3;
                    }
                    return 2;
                }
                return 1;
            }
            return 0;
        }
        public const int Deus = 0, MoonLord = 1, Providence = 2, Polterghast = 3, DoG = 4, Yharon = 5;
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;

            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.channel = true;

            Item.damage = 600;
            Item.knockBack = 1f;
            Item.noMelee = true;
            Item.DamageType = LegendaryMagic.Instance;
            Item.shootSpeed = 10f;
            //    Item.mana = 45;
            Item.shoot = ModContent.ProjectileType<CelestialIlluminationStar>();

            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ModContent.RarityType<InfernumProfanedRarity>();

            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }
        public override bool AltFunctionUse(Player player) => Tier() >= Polterghast && player.GetModPlayer<CelestialIlluminationPlayer>().StarCount >= CelestialIlluminationPlayer.MaxStars;
        public override void HoldItem(Player player)
        {
            if (Tier() >= Polterghast)
            {
                player.Calamity().rightClickListener = true;
            }
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (Tier() >= MoonLord)
            {
                velocity *= 1.35f;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                return true;
            }
            else
            {
                Item.UseSound = SoundID.Item9;
                return true;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Color lerp = Color.Lerp(Color.White, new Color(30, 144, 255), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));

            // The power of the cosmos is contained within this book
            TooltipLine lore = new(Mod, "Lore", Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Lore"));
            tooltips.Add(lore);

            if (Tier() != 5)
            {
                // This weapon may grow in power as you defeat difficult foes
                TooltipLine grow = new(Mod, "Progression2", Language.GetTextValue("Mods.InfernalEclipseAPI.LegendaryTooltip.Base"))
                {
                    OverrideColor = lerp
                };
                tooltips.Add(grow);
            }

            // With the defeat of _
            // Defeat _
            // or
            // The Celestial Illumination is fully awaked, and its maximum power is unleashed! (Max Tier)
            TooltipLine progression = new(Mod, "Progression", GetProgressionTooltip())
            {
                OverrideColor = lerp
            };
            tooltips.Add(progression);

            if (Main.keyState.IsKeyDown(Keys.LeftShift))
            {
                // Dedicated to Ropro0923 with the cool colour swap because yob gets one so I get one too (Ignoring the fact that Yob's a dev and I'm pointless piece of garbage :D)
                TooltipLine dedicated = new(Mod, "DedicatedItem", $"{Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.DedTo", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Dedicated.Ropro"))}")
                {
                    OverrideColor = CalamityUtils.ColorSwap(new Color(50, 205, 50), new Color(23, 167, 23), 2.5f)
                };
                tooltips.Add(dedicated);
            }

            // - Contributor Item-
            TooltipLine contributor = new(Mod, "Progression", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Contributor"))
            {
                OverrideColor = new Color(50, 205, 50)
            };
            tooltips.Add(contributor);
        }
        private static string GetProgressionTooltip()
        {
            return Tier() switch
            {
                5 => Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Progression.Full"),
                4 => Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Progression.Sentinels"),
                3 => Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Progression.Providence"),
                2 => Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Progression.ProfanedGuardians"),
                1 => Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Progression.MoonLord"),
                _ => Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Progression.Deus"),
            };
        }
    }
}