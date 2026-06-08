using CalamityMod.Items.Materials;
using InfernalEclipseAPI.Core.Configs;
using SOTS.Items.CritBonus;

namespace InfernalEclipseAPI.Content.Items.Accessories.ExoSights
{
    [ExtendsFromMod("SOTS")]
    internal class ExoSightsRecipeTreeChanges : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int index = 0; index < Recipe.numRecipes; ++index)
            {
                Recipe recipe = Main.recipe[index];

                if (!InfernalConfig.Instance.SOTSBalanceChanges) return;

                if (recipe.HasResult<SoulCharm>())
                {
                    recipe.AddIngredient(ItemID.Ectoplasm, 5);
                }

                if (recipe.HasResult<BagOfCharms>())
                {
                    recipe.AddIngredient<UnholyEssence>(3);
                }

                if (recipe.HasResult<FocusReticle>())
                {
                    recipe.AddIngredient<DarksunFragment>(3);
                }
            }
        }
    }
}
