using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.SharkToothTree
{
    internal class SharkToothRecipeChanges : ModSystem
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
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;

            for (int index = 0; index < Recipe.numRecipes; ++index)
            {
                Recipe recipe = Main.recipe[index];

                if (sots != null && thorium != null)
                {
                    if (recipe.HasResult(sots.Find<ModItem>("WitchHeart").Type))
                    {
                        if (ModLoader.TryGetMod("Consolaria", out Mod console))
                            recipe.AddIngredient(console.Find<ModItem>("SoulofBlight").Type, 3);
                        else
                            recipe.AddIngredient(ItemID.BeetleHusk, 3);
                    }

                    if (recipe.HasResult(sots.Find<ModItem>("MidnightPrism").Type))
                    {
                        recipe.AddIngredient(thorium.Find<ModItem>("DragonTalonNecklace").Type);
                    }

                    if (recipe.HasResult(ModContent.ItemType<SandSharkToothNecklace>()))
                    {
                        recipe.RemoveIngredient(ItemID.SharkToothNecklace);
                        recipe.AddIngredient(sots.Find<ModItem>("MidnightPrism").Type);
                        recipe.AddIngredient(ItemID.FragmentSolar, 3);
                    }
                }
                else if (thorium != null)
                {
                    if (recipe.HasResult(ModContent.ItemType<SandSharkToothNecklace>()))
                    {
                        recipe.RemoveIngredient(ItemID.SharkToothNecklace);
                        recipe.AddIngredient(thorium.Find<ModItem>("DragonTalonNecklace").Type);
                        recipe.AddIngredient(ItemID.FragmentSolar, 3);
                    }
                }
            }
        }
    }
}
