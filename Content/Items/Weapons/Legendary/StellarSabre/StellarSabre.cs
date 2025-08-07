using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using InfernumMode.Content.Rarities.InfernumRarities;
using CalamityMod.Items.Materials;
using System;
using Terraria.Localization;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.StellarSabre
{
    public class StellarSabre : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 30; // will scale in ModifyWeaponDamage
            Item.DamageType = LegendaryMelee.Instance;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f; // will scale in ModifyWeaponKnockback
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<InfernumProfanedRarity>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<StellarStar>();
            Item.shootSpeed = 14f;
            Item.noMelee = false;
            
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (NPC.downedMoonlord)
                damage += 2.00f;
            else if (NPC.downedAncientCultist)
                damage += 1.25f;
            else if (NPC.downedGolemBoss)
                damage += 1.15f;
            else if (NPC.downedPlantBoss)
                damage += 0.75f;
            else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                damage += 0.25f;
        }

        public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback)
        {
            if (NPC.downedMoonlord)
                knockback += 3;
            else if (NPC.downedAncientCultist)
                knockback += 2.5f;
            else if (NPC.downedGolemBoss)
                knockback += 2;
            else if (NPC.downedPlantBoss)
                knockback += 1.5f;
            else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                knockback += 1;
            else if (Main.hardMode)
                knockback += 0.5f;
        }
        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // 1. Drop a star from the sky (Starfury-style)
            Vector2 skyPos = player.Center + new Vector2(Main.rand.Next(-200, 201), -600f);
            Vector2 heading = (Main.MouseWorld - skyPos).SafeNormalize(Vector2.UnitY);
            Projectile.NewProjectile(source, skyPos, heading * 16f, type, damage, knockback, player.whoAmI);

            // 2. Shoot a star forward from the player
            Vector2 forward = velocity.SafeNormalize(Vector2.UnitX);
            Projectile.NewProjectile(source, player.Center, forward * 14f, type, damage, knockback, player.whoAmI);

            // 3. If Wall of Flesh defeated, shoot two stars forward
            if (Main.hardMode)
            {
                float spread = MathHelper.ToRadians(10);
                for (int i = -1; i <= 1; i += 2)
                {
                    Vector2 spreadVel = forward.RotatedBy(i * spread) * 13f;
                    Projectile.NewProjectile(source, player.Center, spreadVel, type, damage/3, knockback, player.whoAmI);
                }
            }

            return false; // Prevent vanilla shooting
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Starfury);
            recipe.AddIngredient(ItemID.FallenStar, 15);
            recipe.AddIngredient(ItemID.MeteoriteBar, 10);
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 8);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();

            // Add further upgrade recipes here for later tiers
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Color lerpedColor = Color.Lerp(Color.White, new Color(30, 144, 255), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));

            TooltipLine line4 = new(Mod, "Lore", Language.GetTextValue("Mods.InfernalEclipseAPI.Items.StellarSabre.Lore"));
            tooltips.Add(line4);

            if (!NPC.downedMoonlord)
            {
                TooltipLine line3 = new(Mod, "Progression2", Language.GetTextValue("Mods.InfernalEclipseAPI.LegendaryTooltip.Base"));
                line3.OverrideColor = lerpedColor;
                tooltips.Add(line3);
            }

            TooltipLine line = new(Mod, "Progression", GetProgressionTooltip());
            line.OverrideColor = lerpedColor;
            tooltips.Add(line);

            TooltipLine line5 = new(Mod, "DedicatedItem", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Playtester"));
            line5.OverrideColor = lerpedColor;
            tooltips.Add(line5);
        }

        private string GetProgressionTooltip()
        {
            if (NPC.downedMoonlord)
                return Language.GetTextValue("Mods.InfernalEclipseAPI.Items.StellarSabre.Progression.Full");
            if (NPC.downedAncientCultist)
                return Language.GetTextValue("Mods.InfernalEclipseAPI.Items.StellarSabre.Progression.MoonLord");
            if (NPC.downedGolemBoss)
                return Language.GetTextValue("Mods.InfernalEclipseAPI.Items.StellarSabre.Progression.Cultist");
            if (NPC.downedPlantBoss)
                return Language.GetTextValue("Mods.InfernalEclipseAPI.Items.StellarSabre.Progression.Golem");
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                return Language.GetTextValue("Mods.InfernalEclipseAPI.Items.StellarSabre.Progression.Plantera");
            if (Main.hardMode)
                return Language.GetTextValue("Mods.InfernalEclipseAPI.Items.StellarSabre.Progression.Mechs");
            return Language.GetTextValue("Mods.InfernalEclipseAPI.Items.StellarSabre.Progression.WoF");
        }
    }
}
