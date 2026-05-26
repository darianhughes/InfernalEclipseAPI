using Microsoft.Xna.Framework;
using System.Collections.Generic;
using InfernumMode.Content.Rarities.InfernumRarities;
using CalamityMod.Items.Materials;
using Terraria.Localization;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;
using CalamityMod;
using Microsoft.Xna.Framework.Input;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.SplitFirebrand
{
    public class SplitFirebrand : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = LegendarySummon.Instance;
            Item.width = 46;
            Item.height = 48;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = 1;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<InfernumProfanedRarity>();
            Item.UseSound = SoundID.Item152;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SplitFirebrandProjectile>();
            Item.shootSpeed = 14f;
            Item.noMelee = true;
            Item.noUseGraphic = true;

        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (NPC.downedMoonlord)
                damage += 3.75f;
            else if (NPC.downedAncientCultist)
                damage += 2.00f;
            else if (NPC.downedGolemBoss)
                damage += 1.35f;
            else if (NPC.downedPlantBoss)
                damage += 2.75f;
            else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                damage += 5.50f;
            else if (Main.hardMode)
                damage += 1.75f;
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
        public override bool MeleePrefix() => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[type] >= 1)
                return false;

            float ai2 = NPC.downedMoonlord ? 1f : 0f;

            Projectile.NewProjectile(
                source,
                position,
                velocity,
                type,
                damage,
                knockback,
                player.whoAmI,
                0f, // ai0
                0f, // ai1
                ai2 // ai2
            );

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.AddIngredient(ModContent.ItemType<AncientBoneDust>(), 3);
            recipe.AddIngredient(ItemID.Leather, 3);
            recipe.AddIngredient(ItemID.RedString);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        /*
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

            if (Main.keyState.IsKeyDown(Keys.LeftShift))
            {
                TooltipLine line5 = new(Mod, "DedicatedItem", $"{Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.DedTo", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Dedicated.Jill"))}\n{Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Playtester")}");
                line5.OverrideColor = lerpedColor;
                tooltips.Add(line5);
            }
            else
            {
                TooltipLine line5 = new(Mod, "DedicatedItem", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Playtester"));
                line5.OverrideColor = lerpedColor;
                tooltips.Add(line5);
            }
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
        */
    }
}
