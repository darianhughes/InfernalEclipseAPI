using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using YouBoss.Content.Items.ItemReworks;

namespace InfernalEclipseAPI.Content.Items.Weapons.NovaBomb
{
    public class NovaBomb : Supernova
    {
        public override void SetDefaults()
        {
            Item.width = 53;
            Item.height = 56;
            Item.damage = 300000;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 70;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 70;
            Item.knockBack = 18f;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true; 
            Item.shoot = ModContent.ProjectileType<NovaBombProj>();
            Item.shootSpeed = 16f;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 150;
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override float StealthDamageMultiplier => 0.7f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (stealth.WithinBounds(Main.maxProjectiles))
                Main.projectile[stealth].Calamity().stealthStrike = true;
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
        }

        public override void AddRecipes()
        {
            if (ModLoader.TryGetMod("CalamityHunt", out Mod calamityHunt) && calamityHunt.TryFind("ChromaticMass", out ModItem ChormaticMass))
            {
                CreateRecipe().
                    AddIngredient<AshesofAnnihilation>(3).
                    AddIngredient<MiracleMatter>(3).
                    AddIngredient(ChormaticMass.Type, 3).
                    AddIngredient<DarkPlasma>(10).
                    AddIngredient<MeldConstruct>(15).
                    AddIngredient<Voidstone>(10).
                    AddIngredient<Rock>().
                    AddTile<DraedonsForge>().
                    Register();
            }
            else 
            { 
                CreateRecipe().
                    AddIngredient<AshesofAnnihilation>(3).
                    AddIngredient<MiracleMatter>(3).
                    AddIngredient<ShadowspecBar>(3).
                    AddIngredient<DarkPlasma>(10).
                    AddIngredient<MeldConstruct>(15).
                    AddIngredient<Voidstone>(10).
                    AddIngredient<Rock>().
                    AddTile<DraedonsForge>().
                    Register();
            }
        }
    }
}
