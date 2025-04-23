using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Threading.Tasks;
using Terraria.Localization;
using CalamityMod.Items.SummonItems;
using InfernumMode.Content.Items.SummonItems;
using CalamityMod.Items.Tools.ClimateChange;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace InfernalEclipseAPI.Common.Balance.Recipes
{
    internal sealed class RecipeTweaks : ModSystem
    {
        public static RecipeGroup EvilSkinRecipeGroup;
        public override void Unload()
        {
            EvilSkinRecipeGroup = null;
        }
        public override void AddRecipeGroups()
        {
            EvilSkinRecipeGroup = new RecipeGroup(() => $"{Lang.GetItemNameValue(ItemID.ShadowScale)} or {Lang.GetItemNameValue(ItemID.TissueSample)}", ItemID.ShadowScale, ItemID.TissueSample);
            RecipeGroup.RegisterGroup("LimitedResourcesRecipes:EvilSkin", EvilSkinRecipeGroup);
        }
        public override void PostAddRecipes()
        {
            base.PostAddRecipes();

            foreach (var recipe in Main.recipe)
            {
                //Calamity
                //If any mods allow the terminus to be crafted, make it post-Primordial Wyrm.
                if (recipe.HasResult(ModContent.ItemType<Terminus>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EvokingSearune>(), 1);
                }

                if (InfernalConfig.Instance.CalamityRecipeTweaks)
                {
                    //Cosmilite post-DoG - Maybe add its own config?
                    if (recipe.HasResult<Cosmolight>())
                    {
                        recipe.AddIngredient<CosmiliteBar>(1);
                        recipe.RemoveTile(16);
                        recipe.AddTile(ModContent.TileType<CosmicAnvil>());
                    }
                }

                //Ragnarok
                if (ModLoader.TryGetMod("RagnarokMod", out Mod ragCal))
                {
                    //ModLoader.TryGetMod("ThoriumMod", out Mod thorium);

                    ////Referts the Providence spawning changes, if enabled, due to Primordials being retiered to post-DoG, and then referts the recipe to the Calamity recipe.
                    //if (recipe.HasResult<RuneofKos>())
                    //{
                    //    thorium.TryFind("InfernoEssence", out ModItem inferno);
                    //    thorium.TryFind("OceanEssence", out ModItem ocean);
                    //    thorium.TryFind("DeathEssence", out ModItem death);

                    //    if (recipe.HasIngredient(inferno))
                    //    {
                    //        recipe.RemoveIngredient(inferno.Type);
                    //        recipe.RemoveIngredient(ocean.Type);
                    //        recipe.RemoveIngredient(death.Type);
                    //    }
                    //}
                }

                if (InfernalConfig.Instance.SOTSBalanceChanges)
                {
                    //SOTS
                    if (ModLoader.TryGetMod("SOTS", out Mod sots))
                    {
                        // Frigid Pickaxe
                        if (sots.TryFind("FrigidPickaxe", out ModItem frigidPick))
                        {
                            if (recipe.HasResult(frigidPick))
                            {
                                recipe.AddRecipeGroup(EvilSkinRecipeGroup, 6);
                            }
                        }

                        // Challenger's Ring
                        if (sots.TryFind("ChallengerRing", out ModItem challRing))
                        {
                            if (recipe.HasResult(challRing))
                            {
                                if (sots.TryFind("PhaseBar", out ModItem phaseBar))
                                {
                                    recipe.AddIngredient(phaseBar, 6);
                                }
                                else
                                {
                                    recipe.AddIngredient(ItemID.Ectoplasm, 6);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
