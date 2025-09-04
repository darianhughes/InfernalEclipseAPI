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
using InfernumMode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using YouBoss.Content.Items.ItemReworks;

namespace InfernalEclipseAPI.Content.Items.Weapons.BossRush.NovaBomb
{
    public class NovaBomb : ModItem
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
            Item.mana = 250;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Infernum_Tooltips().DeveloperItem = true;
        }

        // Block use if either projectile is already active for this player
        public override bool CanUseItem(Player player)
        {
            int homingType = ModContent.ProjectileType<NovaBombProj>();
            int blackHoleType = ModContent.ProjectileType<NovaBombBlackHoleProj>();

            return player.ownedProjectileCounts[homingType] == 0 &&
                   player.ownedProjectileCounts[blackHoleType] == 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AshesofAnnihilation>(3)
                .AddIngredient<MiracleMatter>(3)
                .AddIngredient(ModLoader.TryGetMod("CalamityHunt", out Mod calamityHunt) &&
                               calamityHunt.TryFind("ChromaticMass", out ModItem ChormaticMass)
                               ? ChormaticMass.Type : ModContent.ItemType<ShadowspecBar>())
                .AddIngredient<DarkPlasma>(10)
                .AddIngredient<MeldConstruct>(15)
                .AddIngredient<Voidstone>(10)
                .AddIngredient<Rock>()
                .AddTile<DraedonsForge>()
                .Register();
        }
    }
}
