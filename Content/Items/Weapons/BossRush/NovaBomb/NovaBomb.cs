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
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.Infernum_Tooltips().DeveloperItem = true;
        }

        public override bool AltFunctionUse(Player player) => true;

        // Block use if either projectile is already active for this player
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                KillOwnedBlackHoles(player);
                SoundEngine.PlaySound(SoundID.Item110 with { Pitch = -0.2f }, player.Center);
                return false;
            }

            int homingType = ModContent.ProjectileType<NovaBombProj>();
            int blackHoleType = ModContent.ProjectileType<NovaBombBlackHoleProj>();

            return player.ownedProjectileCounts[homingType] == 0 &&
                   player.ownedProjectileCounts[blackHoleType] == 0;
        }

        private static void KillOwnedBlackHoles(Player player)
        {
            int bhType = ModContent.ProjectileType<NovaBombBlackHoleProj>();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.owner == player.whoAmI && p.type == bhType)
                    p.Kill();
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<AshesofAnnihilation>(3);
            recipe.AddIngredient<MiracleMatter>(3);
            if (ModLoader.TryGetMod("CalamityHunt", out Mod calamityHunt) && calamityHunt.TryFind("ChromaticMass", out ModItem ChormaticMass))
            {
                recipe.AddIngredient(ChormaticMass.Type, 3);
            }
            else recipe.AddIngredient<ShadowspecBar>(3);
            if (ModLoader.TryGetMod("NoxusPort", out Mod noxus)) recipe.AddIngredient(noxus.Find<ModItem>("EntropicBar").Type, 3);
            if (ModLoader.TryGetMod("NoxusBoss", out Mod wotg)) recipe.AddIngredient(wotg.Find<ModItem>("MetallicChunk").Type);
            recipe.AddIngredient<DarkPlasma>(10);
            recipe.AddIngredient<MeldConstruct>(15);
            recipe.AddIngredient<Voidstone>(10);
            recipe.AddIngredient<Rock>();
            recipe.AddTile<DraedonsForge>();
            recipe.Register();
        }
    }
}
