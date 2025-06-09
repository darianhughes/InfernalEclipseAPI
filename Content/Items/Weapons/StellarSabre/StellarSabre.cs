using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using InfernumMode.Content.Rarities.InfernumRarities;
using CalamityMod.Items.Materials;
using System;

namespace InfernalEclipseAPI.Content.Items.Weapons.StellarSabre
{
    public class StellarSabre : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 30; // will scale in ModifyWeaponDamage
            Item.DamageType = DamageClass.Melee;
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

            if (!NPC.downedMoonlord)
            {
                TooltipLine line3 = new(Mod, "Progression2", "This weapon may grow in power as you defeat difficult foes.");
                line3.OverrideColor = lerpedColor;
                tooltips.Add(line3);
            }

            TooltipLine line2 = new(Mod, "Progression2", GetProgressionTooltip2());
            line2.OverrideColor = lerpedColor;
            tooltips.Add(line2);

            TooltipLine line = new(Mod, "Progression", GetProgressionTooltip());
            line.OverrideColor = lerpedColor;
            tooltips.Add(line);

            TooltipLine line4 = new(Mod, "Lore", "'Even after everything, there was still one person Xander refused to attack...'");
            tooltips.Add(line4);

            TooltipLine line5 = new(Mod, "DedicatedItem", "- Playtester Item -");
            line5.OverrideColor = lerpedColor;
            tooltips.Add(line5);
        }

        private string GetProgressionTooltip()
        {
            if (NPC.downedMoonlord)
                return "The Stellar Sabre is fully awaked, and its maximum power is unleashed!";
            if (NPC.downedAncientCultist)
                return "With the defeat of the Lunatic Cultist, stars explode on impact.";
            if (NPC.downedGolemBoss)
                return "With the defeat of Golem, stars home in on enemies.";
            if (NPC.downedPlantBoss)
                return "With the defeat of Plantera, stars now split on hit.";
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                return "With the defeat of Draedon's machines, it now inflicts Daybroken.";
            if (Main.hardMode)
                return "With the defeat of the Wall of Flesh, this sword now fires extra stars.";
            return "No extra power has been gained...";
        }

        private string GetProgressionTooltip2()
        {
            if (NPC.downedMoonlord)
                return "The Stellar Sabre is fully awakened!";
            if (NPC.downedAncientCultist)
                return "Defeat the celestial of the moon to gain more power...";
            if (NPC.downedGolemBoss)
                return "Defeat the leader of the ancient cult to gain more power...";
            if (NPC.downedPlantBoss)
                return "Defeat the construct of the Lizhard Temple to gain more power...";
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                return "Defeat the gaurdian of the jungle to gain more power...";
            if (Main.hardMode)
                return "Defeat the mechanical beasts to gain more power...";
            return "Defeat the keeper of Hell to gain more power...";
        }
    }
}
