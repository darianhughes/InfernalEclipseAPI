using System;
using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Items;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;
using InfernalEclipseAPI.Core.World;
using InfernumMode.Content.Rarities.InfernumRarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.CelestialIllumination
{
    // Leaving you to the balance on this one unfortunately. I am notoriously bad at balancing things.
    // I also wanted to make a cool visual where you have stars orbiting around the player when you charge it up, and once you reach max charge, they converge and shoot the beam, but I couldn't D:
    public class CelestialIllumination : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.DamageType = ModContent.GetInstance<LegendaryMagic>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.damage = 600;
            Item.rare = ModContent.RarityType<InfernumProfanedRarity>();
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.shoot = ModContent.ProjectileType<CelestialIlluminationStar>();
            Item.shootSpeed = 10f;
            Item.knockBack = 1f;
            Item.consumable = false;
            Item.maxStack = 1;
            Item.mana = 45;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float distance = CalamityConditions.DownedProvidence.IsMet() ? 20f : 14f;
            if (player.altFunctionUse == 2)
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.channel = true;
                player.GetModPlayer<CelestialIlluminationPlayer>().CelestialStarCharge = 0;
                Projectile.NewProjectile(source, player.Center + (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * distance, (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 2f, ModContent.ProjectileType<CelestialBeam>(), damage, knockback, player.whoAmI);
                return false;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.channel = false;
                if (!NPC.downedMoonlord)
                {
                    Projectile.NewProjectile(source, player.Center + (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20f, velocity.RotatedByRandom(MathHelper.ToRadians(7.5f)), ModContent.ProjectileType<CelestialIlluminationStar>(), 400, 2f, player.whoAmI);
                }
                else if (CalamityConditions.DownedGuardians.IsMet())
                {
                    Projectile.NewProjectile(source, player.Center + (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20f, velocity.RotatedByRandom(MathHelper.ToRadians(10f)), ModContent.ProjectileType<CelestialIlluminationStar>(), 400, 2f, player.whoAmI);
                    Projectile.NewProjectile(source, player.Center + (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20f, velocity.RotatedByRandom(MathHelper.ToRadians(10f)), ModContent.ProjectileType<CelestialIlluminationStar>(), 400, 2f, player.whoAmI);
                }
                else if (ModContent.GetInstance<InfernalDownedBossSystem>().DownedSentinels())
                {
                    Projectile.NewProjectile(source, player.Center + (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20f, velocity.RotatedByRandom(MathHelper.ToRadians(12.5f)), ModContent.ProjectileType<CelestialIlluminationStar>(), 500, 2f, player.whoAmI);
                    Projectile.NewProjectile(source, player.Center + (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20f, velocity.RotatedByRandom(MathHelper.ToRadians(12.5f)), ModContent.ProjectileType<CelestialIlluminationStar>(), 500, 2f, player.whoAmI);
                    Projectile.NewProjectile(source, player.Center + (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 20f, velocity.RotatedByRandom(MathHelper.ToRadians(12.5f)), ModContent.ProjectileType<CelestialIlluminationStar>(), 500, 2f, player.whoAmI);
                }
                return false;
            }
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.useTime = 180;
                Item.useAnimation = 180;
                Item.channel = true;
                Item.noUseGraphic = false;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.channel = true;
                Item.useTime = 25;
                Item.useAnimation = 25;
                Item.noUseGraphic = false;

            }
            return base.CanUseItem(player);
        }
        public override bool AltFunctionUse(Player player)
        {
            if (CalamityConditions.DownedGuardians.IsMet() && player.GetModPlayer<CelestialIlluminationPlayer>().CelestialStarCharge >= 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (CalamityConditions.DownedDevourerOfGods.IsMet())
                damage += 2.00f;
            else if (ModContent.GetInstance<InfernalDownedBossSystem>().DownedSentinels())
                damage += 1.25f;
            else if (CalamityConditions.DownedProvidence.IsMet())
                damage += 1.15f;
            else if (CalamityConditions.DownedGuardians.IsMet())
                damage += 0.75f;
            else if (NPC.downedMoonlord)
                damage += 0.25f;
        }
        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            if (CalamityConditions.DownedDevourerOfGods.IsMet())
                crit += 10;
            else if (ModContent.GetInstance<InfernalDownedBossSystem>().DownedSentinels())
                crit += 10;
            else if (CalamityConditions.DownedProvidence.IsMet())
                crit += 10;
            else if (CalamityConditions.DownedGuardians.IsMet())
                crit += 10;
            else if (NPC.downedMoonlord)
                crit += 10;
        }
        public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback)
        {
            if (CalamityConditions.DownedDevourerOfGods.IsMet())
                knockback += 3f;
            else if (ModContent.GetInstance<InfernalDownedBossSystem>().DownedSentinels())
                knockback += 2.75f;
            else if (CalamityConditions.DownedProvidence.IsMet())
                knockback += 2.5f;
            else if (CalamityConditions.DownedGuardians.IsMet())
                knockback += 2f;
            else if (NPC.downedMoonlord)
                knockback += 1f;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Color lerpedColor = Color.Lerp(Color.White, new Color(30, 144, 255), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));

            TooltipLine line4 = new(Mod, "Lore", Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Lore"));
            tooltips.Add(line4);

            if (!CalamityConditions.DownedDevourerOfGods.IsMet())
            {
                TooltipLine line3 = new(Mod, "Progression2", Language.GetTextValue("Mods.InfernalEclipseAPI.LegendaryTooltip.Base"));
                line3.OverrideColor = lerpedColor;
                tooltips.Add(line3);
            }

            TooltipLine line = new(Mod, "Progression", GetProgressionTooltip());
            TooltipLine ContributorItem = new(Mod, "Progression", GetContributorItemTooltip());
            line.OverrideColor = lerpedColor;
            tooltips.Add(line);
            tooltips.Add(ContributorItem);
        }
        private string GetProgressionTooltip()
        {
            if (CalamityConditions.DownedDevourerOfGods.IsMet())
                return Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Progression.Full");
            else if (ModContent.GetInstance<InfernalDownedBossSystem>().DownedSentinels())
                return Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Progression.Sentinels");
            else if (CalamityConditions.DownedProvidence.IsMet())
                return Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Progression.Providence");
            if (CalamityConditions.DownedGuardians.IsMet())
                return Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Progression.ProfanedGuardians");
            if (NPC.downedMoonlord)
                return Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Progression.MoonLord");
            return Language.GetTextValue("Mods.InfernalEclipseAPI.Items.CelestialIllumination.Progression.Deus");
        }
        private string GetContributorItemTooltip()
        {
            return Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Contributor");
        }
    }
}
