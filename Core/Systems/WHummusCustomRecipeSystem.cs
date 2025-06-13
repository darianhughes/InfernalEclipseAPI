using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems
{
    public class WHummusCustomRecipeSystem : ModSystem
    {
        public override void PostAddRecipes()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium) || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return; // Exit early if Thorium not loaded

            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                if (recipe == null)
                    continue;

                if (recipe.HasResult(thorium.Find<ModItem>("MyceliumGattlingGun")))
                {
                    int funggatType = thorium.Find<ModItem>("Funggat")?.Type ?? 0;
                    if (funggatType > 0 && !recipe.HasIngredient(funggatType))
                    {
                        recipe.AddIngredient(funggatType);
                    }

                    if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
                    {
                        int fungicideType = calamity.Find<ModItem>("Fungicide")?.Type ?? 0;
                        if (fungicideType > 0 && !recipe.HasIngredient(fungicideType))
                        {
                            recipe.AddIngredient(fungicideType);
                        }
                    }
                }
            }
        }
    }
}
