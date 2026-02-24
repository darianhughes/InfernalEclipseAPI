using CalamityMod.Items.Accessories;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.GauntletCraftingTree
{
    public class GauntletRecipeChanges : ModSystem
    {
        private Mod thorium
        {
            get
            {
                ModLoader.TryGetMod("ThoriumMod", out Mod thor);
                return thor;
            }
        }

        public override void PostAddRecipes()
        {
            for (int index = 0; index < Recipe.numRecipes; ++index)
            {
                Recipe recipe = Main.recipe[index];

                if (!InfernalConfig.Instance.MergeCraftingTrees)
                    return;

                if (recipe.HasResult<ElementalGauntlet>())
                {
                    if (thorium != null) if (!recipe.HasIngredient(thorium.Find<ModItem>("TerrariumCore"))) recipe.AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 3);
                }
            }
        }
    }
}
