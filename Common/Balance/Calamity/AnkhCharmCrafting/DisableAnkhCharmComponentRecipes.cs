using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.Balance.Calamity.AnkhCharmCrafting
{
    public class DisableAnkhCharmComponentRecipes : ModSystem
    {
        private static int[] disabledRecipes = new int[10]
        {
            887,
            885,
            888,
            3781,
            892,
            891,
            893,
            886,
            890,
            889
        };

        public override void PostAddRecipes()
        {
            if (ModContent.GetInstance<InfernalConfig>().CalamityBalanceChanges)
            {
                for (int index1 = 0; index1 < Recipe.numRecipes; ++index1)
                {
                    Recipe recipe = Main.recipe[index1];
                    for (int index2 = 0; index2 < disabledRecipes.Length; ++index2)
                    {
                        Item obj;
                        if (recipe.TryGetResult(disabledRecipes[index2], out obj))
                            recipe.DisableRecipe();
                    }
                }
            }
        }
    }
}
