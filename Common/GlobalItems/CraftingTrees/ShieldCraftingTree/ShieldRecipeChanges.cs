using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;
using Terraria.ID;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.ShieldCraftingTree
{
    internal class ShieldRecipeChanges : ModSystem
    {
        private Mod calamity
        {
            get
            {
                ModLoader.TryGetMod("CalamityMod", out Mod cal);
                return cal;
            }
        }

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

                //if (ModLoader.TryGetMod("Clamity", out Mod clam) && !InfernalConfig.Instance.CalamityRecipeTweaks)
                //{
                //    if (recipe.HasResult(clam.Find<ModItem>("SupremeBarrier").Type))
                //    {
                //        recipe.RemoveIngredient(ModContent.ItemType<ShadowspecBar>());
                //        recipe.AddIngredient(ModContent.ItemType<Rock>());
                //    }
                //}

                if (!InfernalConfig.Instance.MergeCraftingTrees)
                    return;

                if (thorium != null) 
                { 
                    if (recipe.HasResult(ModContent.ItemType<AsgardsValor>()))
                        recipe.AddIngredient(thorium.Find<ModItem>("MoltenScale"), 1);

                    if (recipe.HasResult(ModContent.ItemType<DeificAmulet>()) && recipe.HasIngredient(ItemID.StarVeil))
                    {
                        recipe.RemoveIngredient(ItemID.StarVeil);
                        recipe.AddIngredient(ItemID.FragmentStardust, 5);
                        recipe.AddIngredient(thorium.Find<ModItem>("SweetVengeance"), 1);
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;

            if (thorium != null)
            {
                Recipe recipe1 = Recipe.Create(thorium.Find<ModItem>("MoltenScale").Type, 1);

                recipe1.AddRecipeGroup(RecipeGroupID.IronBar, 10);
                recipe1.AddIngredient(ItemID.FlarefinKoi, 1);
                recipe1.AddIngredient(ItemID.Obsidifish, 2);
                recipe1.AddIngredient(ItemID.Fireblossom, 1);
                recipe1.AddTile(TileID.Anvils);

                recipe1.Register();
            }
            }
        }
}
