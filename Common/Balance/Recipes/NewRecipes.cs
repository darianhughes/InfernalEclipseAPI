using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.Balance.Recipes
{
    public class NewRecipes : ModSystem
    {
        public static RecipeGroup EvilBarRecipeGroup;
        public override void Unload()
        {
            EvilBarRecipeGroup = null;
        }

        public override void AddRecipeGroups()
        {
            EvilBarRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CrimtaneBar)}", ItemID.DemoniteBar, ItemID.CrimtaneBar);
            RecipeGroup.RegisterGroup("LimitedResourcesRecipes:EvilBar", EvilBarRecipeGroup);
        }
        public override void AddRecipes()
        {
            Recipe.Create(ItemID.Terragrim)
            .AddIngredient(ItemID.EnchantedSword, 1)
            .AddIngredient(ItemID.JungleSpores, 5)
            .AddRecipeGroup(EvilBarRecipeGroup, 5)
            .AddIngredient(ItemID.Obsidian, 3)
            .AddIngredient(ItemID.FossilOre, 3)
            .AddTile(TileID.Anvils)
            .Register();

            if (ModLoader.TryGetMod("ThoriumRework", out Mod thorRework))
            {
                ModLoader.TryGetMod("ThoriumMod", out Mod thorium);

                Recipe.Create(thorRework.Find<ModItem>("DeathsingerPotion").Type)
                    .AddIngredient(ItemID.BottledWater)
                    .AddIngredient<BloodOrb>(10)
                    .AddTile(TileID.AlchemyTable)
                    .Register();

                thorium.TryFind("ManaBerry", out ModItem manaberry);
                Recipe.Create(thorRework.Find<ModItem>("InspirationRegenerationPotion").Type)
                    .AddIngredient(ItemID.BottledWater)
                    .AddIngredient<BloodOrb>(10)
                    .AddIngredient(manaberry.Type)
                    .AddTile(TileID.AlchemyTable)
                    .Register();
            }

        }
    }
}
