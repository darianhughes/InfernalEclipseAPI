using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Materials;
using SOTS.Items.CritBonus;
using Terraria;
using Terraria.ModLoader;

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
