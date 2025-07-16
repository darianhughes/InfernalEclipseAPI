using CalamityMod.Items.Potions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

#nullable disable
namespace InfernalEclipseAPI.Common.Balance.Calamity.NerfedCalciumPotion;

public class NerfedCalciumPotionRecipe : ModSystem
{
    public override void PostAddRecipes()
    {
        if (!InfernalConfig.Instance.CalamityBalanceChanges)
            return;
        for (int index = 0; index < Recipe.numRecipes; ++index)
        {
            Recipe recipe = Main.recipe[index];
            Item obj;
            if (recipe.TryGetResult(ModContent.ItemType<CalciumPotion>(), out obj))
            {
                recipe.RemoveIngredient(126);
                recipe.AddIngredient(ItemID.BottledWater, 1);
            }
        }
    }
}
