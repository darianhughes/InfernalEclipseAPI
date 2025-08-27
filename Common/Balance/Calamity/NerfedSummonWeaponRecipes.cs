using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.Balance.Calamity
{
    public class NerfedSummonWeaponRecipes : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int index = 0; index < Recipe.numRecipes; ++index)
            {
                Recipe recipe = Main.recipe[index];
                Item obj;
                if (recipe.TryGetResult(1309, out obj))
                    recipe.AddIngredient(ItemID.Emerald);
                if (recipe.TryGetResult(ModContent.ItemType<SquirrelSquireStaff>(), out obj))
                    recipe.AddIngredient(ItemID.Squirrel);
                if (recipe.TryGetResult(ModContent.ItemType<WulfrumController>(), out obj))
                {
                    recipe.requiredItem.Clear();
                    recipe.AddIngredient(ModContent.ItemType<WulfrumMetalScrap>(), 20);
                    recipe.AddIngredient(ModContent.ItemType<EnergyCore>(), 3);
                }
            }
        }
    }
}
