using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;

namespace InfernalEclipseAPI.Common.Balance.Calamity
{
    public class BloodOrbPotionsTweak : ModSystem
    {
        public override void PostAddRecipes()
        {
            if (!InfernalConfig.Instance.DisableBloodOrbPotions)
            {
                return;
            }

            for (int index = 0; index < Recipe.numRecipes; ++index)
            {
                Recipe recipe = Main.recipe[index];
                if (recipe.HasIngredient(126) && recipe.HasIngredient(ModContent.ItemType<BloodOrb>()))
                    recipe.DisableRecipe();
                if (recipe.HasIngredient(353) && recipe.HasIngredient(ModContent.ItemType<BloodOrb>()))
                    recipe.DisableRecipe();
            }
        }
    }
}
