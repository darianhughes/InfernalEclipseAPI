using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.QuiverCraftingTree
{
    public class QuiverRecipeChanges : ModSystem
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

        public override void PostAddRecipes()
        {
            for (int index = 0; index < Recipe.numRecipes; ++index)
            {
                Recipe recipe = Main.recipe[index];

                if (!InfernalConfig.Instance.MergeCraftingTrees)
                    return;

                if (recipe.HasResult<PlanebreakersPouch>())
                {
                    if (sots != null)
                    {
                        if (!recipe.HasIngredient(sots.Find<ModItem>("BlazingQuiver")))
                        {
                            if (thorium != null)
                            {
                                recipe.AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 2);
                            }

                            recipe.AddIngredient(sots.Find<ModItem>("BagOfAmmoGathering"));
                            recipe.AddIngredient(sots.Find<ModItem>("InfinityPouch"));
                        }
                    }
                    else
                    {
                        if (thorium != null)
                        {
                            if (!recipe.HasIngredient(thorium.Find<ModItem>("TerrariumCore"))) recipe.AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 3);
                        }
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;

            if (sots != null)
            {
                Recipe blazingQuiverRecipe = Recipe.Create(ModContent.ItemType<PlanebreakersPouch>());
                blazingQuiverRecipe.AddIngredient(sots.Find<ModItem>("BlazingQuiver"));
                blazingQuiverRecipe.AddIngredient<DeadshotBrooch>();
                blazingQuiverRecipe.AddIngredient(sots.Find<ModItem>("BagOfAmmoGathering"));
                blazingQuiverRecipe.AddIngredient(sots.Find<ModItem>("InfinityPouch"));
                if (thorium != null && !ModLoader.TryGetMod("ssm", out _)) blazingQuiverRecipe.AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 2);
                blazingQuiverRecipe.AddIngredient<AscendantSpiritEssence>(4);
                blazingQuiverRecipe.AddTile<CosmicAnvil>();
                blazingQuiverRecipe.Register();
            }
        }
    }
}
