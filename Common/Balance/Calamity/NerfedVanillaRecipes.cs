using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.Balance.Calamity
{
    public class NerfedVanillaRecipes : ModSystem
    {
        public override void PostAddRecipes()
        {
            if (ModContent.GetInstance<InfernalConfig>().CalamityBalanceChanges)
            {
                for (int index = 0; index < Recipe.numRecipes; ++index)
                {
                    Recipe recipe = Main.recipe[index];
                    Item obj;
                    if (recipe.TryGetResult(158, out obj))
                        recipe.AddIngredient(ItemID.SunplateBlock, 5);
                    if (recipe.TryGetResult(4276, out obj))
                    {
                        recipe.RemoveIngredient(178);
                        recipe.AddIngredient(ItemID.Amber, 4);
                    }
                }
            }
        }
    }
}
