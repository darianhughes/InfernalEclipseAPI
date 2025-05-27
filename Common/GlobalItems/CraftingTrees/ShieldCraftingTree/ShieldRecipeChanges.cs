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
using System.Security.Policy;

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

                    if (recipe.HasResult(thorium.Find<ModItem>("MantleoftheProtector")))
                    {
                        recipe.RemoveIngredient(ItemID.Silk);
                        recipe.RemoveIngredient(ItemID.CrossNecklace);
                        recipe.AddIngredient(thorium.Find<ModItem>("CapeoftheSurvivor"));
                        recipe.AddIngredient<DeificAmulet>(1);
                        recipe.AddIngredient<EffulgentFeather>(1);
                        recipe.RemoveTile(TileID.TinkerersWorkbench);
                        recipe.AddTile(TileID.LunarCraftingStation);
                    }

                    if (sots != null)
                    {
                        if (recipe.HasResult(thorium.Find<ModItem>("TerrariumDefender")))
                        {
                            recipe.RemoveIngredient(ItemID.AnkhShield);
                            recipe.RemoveIngredient(thorium.Find<ModItem>("HolyAegis").Type);
                            recipe.RemoveIngredient(ItemID.FrozenTurtleShell);
                            recipe.AddIngredient(sots.Find<ModItem>("ChiseledBarrier").Type, 1);
                            recipe.AddIngredient(sots.Find<ModItem>("OlympianAegis").Type, 1);
                            recipe.AddIngredient(ItemID.FrozenShield, 1);
                            recipe.AddIngredient(sots.Find<ModItem>("TerminalCluster").Type, 1);
                        }
                    }
                    else
                    {
                        if (recipe.HasResult(thorium.Find<ModItem>("TerrariumDefender")))
                        {
                            recipe.RemoveIngredient(ItemID.AnkhShield);
                            recipe.RemoveIngredient(thorium.Find<ModItem>("HolyAegis").Type);
                            recipe.RemoveIngredient(ItemID.FrozenTurtleShell);
                            recipe.AddIngredient(ItemID.FrozenShield, 1);
                            recipe.AddIngredient(thorium.Find<ModItem>("LifeQuartzShield").Type, 1);
                        }
                    }
                }

                if (recipe.HasResult(ModContent.ItemType<RampartofDeities>()))
                {
                    if (thorium != null)
                    {
                        recipe.RemoveIngredient(ItemID.FrozenShield);
                        recipe.RemoveIngredient(ModContent.ItemType<DeificAmulet>());
                        recipe.AddIngredient(ModContent.ItemType<ExoPrism>(), 5);
                        recipe.AddIngredient(thorium.Find<ModItem>("TerrariumDefender"), 1);
                        recipe.AddIngredient(thorium.Find<ModItem>("MantleoftheProtector"), 1);
                    }
                    else if (sots != null)
                    {
                        recipe.AddIngredient(ModContent.ItemType<ExoPrism>(), 5);
                        recipe.AddIngredient(sots.Find<ModItem>("ChiseledBarrier").Type, 1);
                        recipe.AddIngredient(sots.Find<ModItem>("OlympianAegis").Type, 1);
                        recipe.AddIngredient(sots.Find<ModItem>("TerminalCluster").Type, 1);
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
