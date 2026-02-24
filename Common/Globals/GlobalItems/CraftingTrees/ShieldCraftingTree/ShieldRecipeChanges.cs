using CalamityMod.Items.Materials;
using CalamityMod.Items.Accessories;
using InfernalEclipseAPI.Core.Systems;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.ShieldCraftingTree
{
    internal class ShieldRecipeChanges : ModSystem
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
            for (int index = 0; index < Recipe.numRecipes; ++index)
            {
                Recipe recipe = Main.recipe[index];

                if (ModLoader.TryGetMod("Clamity", out Mod clam) && (ModLoader.TryGetMod("FargowiltasCrossmod", out _) || ModLoader.TryGetMod("ssm", out _)))
                {
                    if (recipe.HasResult(clam.Find<ModItem>("SupremeBarrier").Type))
                    {
                        recipe.DisableRecipe();
                    }
                }

                if (!InfernalConfig.Instance.MergeCraftingTrees)
                    return;

                if (sots != null)
                {
                    if (recipe.HasResult<OrnateShield>())
                    {
                        recipe.AddIngredient(sots.Find<ModItem>("ShatterHeartShield"));
                    }
                }

                if (clam != null)
                {
                    if (recipe.HasResult<AsgardsValor>())
                    {
                        recipe.AddIngredient(clam.Find<ModItem>("HuskOfCalamity"), 3);
                    }
                }

                if (thorium != null) 
                { 
                    //if (recipe.HasResult(ModContent.ItemType<AsgardsValor>()))
                        //recipe.AddIngredient(thorium.Find<ModItem>("MoltenScale"), 1);

                    if (recipe.HasResult(ModContent.ItemType<DeificAmulet>()) && recipe.HasIngredient(ItemID.StarVeil))
                    {
                        recipe.RemoveIngredient(ItemID.StarVeil);
                        recipe.RemoveIngredient(ItemID.SweetheartNecklace);
                        recipe.AddIngredient(ItemID.FragmentStardust, 5);
                        recipe.AddIngredient(thorium.Find<ModItem>("SweetVengeance"), 1);
                    }

                    if (recipe.HasResult(thorium.Find<ModItem>("MantleoftheProtector")))
                    {
                        recipe.RemoveIngredient(ItemID.Silk);
                        recipe.RemoveIngredient(ItemID.CrossNecklace);
                        recipe.AddIngredient(thorium.Find<ModItem>("CapeoftheSurvivor"));
                        recipe.AddIngredient<DeificAmulet>();
                        recipe.AddIngredient<EffulgentFeather>(5);
                        recipe.RemoveTile(TileID.TinkerersWorkbench);
                        recipe.AddTile(TileID.LunarCraftingStation);
                    }

                    ModItem plasmaGen = null;
                    if (thorium.TryFind("PlasmaGenerator", out plasmaGen))
                    {
                        if (recipe.HasResult(plasmaGen))
                            recipe.AddIngredient(thorium.Find<ModItem>("MoltenScale"));
                    }

                    if (recipe.HasResult(ModContent.ItemType<AsgardianAegis>()))
                    {
                        if (ModLoader.TryGetMod("ssm", out _))
                        {
                            recipe.RemoveIngredient(thorium.Find<ModItem>("TerrariumDefender").Type);
                            if (!recipe.HasIngredient(ModContent.ItemType<AsgardsValor>()))
                                recipe.AddIngredient(ModContent.ItemType<AsgardsValor>());
                        }
                        if (plasmaGen != null)
                        {
                            recipe.AddIngredient(plasmaGen);
                        }
                        else
                        {
                            recipe.AddIngredient(thorium.Find<ModItem>("MoltenScale"));
                        }
                    }

                    if (recipe.HasResult(thorium.Find<ModItem>("TerrariumDefender")))
                    {
                        if (sots != null)
                        {
                            recipe.RemoveIngredient(ItemID.AnkhShield);
                            recipe.RemoveIngredient(thorium.Find<ModItem>("HolyAegis").Type);
                            recipe.RemoveIngredient(ItemID.FrozenTurtleShell);
                            recipe.AddIngredient(sots.Find<ModItem>("BulwarkOfTheAncients").Type);
                            if (!recipe.HasIngredient(ItemID.FrozenShield)) recipe.AddIngredient(ItemID.FrozenShield);

                        }
                        else
                        {
                            recipe.RemoveIngredient(ItemID.AnkhShield);
                            recipe.RemoveIngredient(thorium.Find<ModItem>("HolyAegis").Type);
                            recipe.RemoveIngredient(ItemID.FrozenTurtleShell);
                            if(!recipe.HasIngredient(ItemID.FrozenShield)) recipe.AddIngredient(ItemID.FrozenShield);
                            if (!recipe.HasIngredient(thorium.Find<ModItem>("LifeQuartzShield").Type)) recipe.AddIngredient(thorium.Find<ModItem>("LifeQuartzShield").Type);
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
                        recipe.AddIngredient(thorium.Find<ModItem>("TerrariumDefender"));
                        recipe.AddIngredient(thorium.Find<ModItem>("MantleoftheProtector"));
                    }
                    else if (sots != null)
                    {
                        recipe.AddIngredient(ModContent.ItemType<ExoPrism>(), 5);
                        recipe.AddIngredient(sots.Find<ModItem>("BulwarkOfTheAncients").Type);
                    }

                    if (clam != null)
                    {
                        recipe.AddIngredient(clam.Find<ModItem>("EnchantedMetal"), 5);
                    }
                    
                    if (ModLoader.TryGetMod("FargowiltasCrossmod", out _))
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<ExoPrism>());
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
