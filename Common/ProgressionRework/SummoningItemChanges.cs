using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.SummonItems;
using CalamityMod.Tiles.DraedonSummoner;
using CalamityMod.Tiles.Furniture.CraftingStations;
using InfernalEclipseAPI.Content.Items.Materials;
using InfernumMode.Content.Items.SummonItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.ProgressionRework
{
    internal sealed class SummoningItemChanges : ModSystem
    {
        public override void PostAddRecipes()
        {
            base.PostAddRecipes();

            bool hasThorium = false;
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                hasThorium = true;
            }

            bool hasSOTS = false;
            if (ModLoader.TryGetMod("SOTS", out Mod sots))
                hasSOTS = true;

            bool hasRagnarok = false;
            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
                hasRagnarok = true;

            foreach (var recipe in Main.recipe)
            {
                //Crabulon is locked behind Glowmoth if SOTS is enabled but remove the Grand Thunder Bird requirment regardless if Raganrok is enabled.

                if (recipe.HasResult(ModContent.ItemType<DecapoditaSprout>()))
                {
                    if (hasRagnarok)
                        recipe.RemoveIngredient(ragnarok.Find<ModItem>("StormFeather").Type);

                    if (hasSOTS)
                        recipe.AddIngredient<InfectedMothwingSpore>();
                }

                // Slime God
                if (hasRagnarok)
                {
                    if (recipe.HasResult<OverloadedSludge>() && recipe.HasIngredient(ragnarok.Find<ModItem>("EnchantedMarble")) && recipe.HasIngredient(ragnarok.Find<ModItem>("EmpoweredGranite")))
                    {
                        recipe.ChangeIngredientStack(ragnarok.Find<ModItem>("EnchantedMarble").Type, 1);
                        recipe.ChangeIngredientStack(ragnarok.Find<ModItem>("EmpoweredGranite").Type, 1);
                    }
                }

                //Star Scouter post-Advisor if SOTS enabled, otherwise, still lock it after Evil Boss 2
                if (hasThorium && hasSOTS)
                {
                    if (recipe.HasResult(thorium.Find<ModItem>("StarCaller")))
                    {
                        if (hasRagnarok)
                        {
                            if (recipe.HasIngredient(ModContent.ItemType<BloodSample>()))
                                recipe.DisableRecipe(); //we only need one recipe enabled
                            if (recipe.HasIngredient(ModContent.ItemType<RottenMatter>()))
                                recipe.RemoveIngredient(ModContent.ItemType<RottenMatter>());
                        }

                        recipe.AddIngredient(sots.Find<ModItem>("OtherworldlyAlloy"), 3);
                        recipe.AddIngredient(sots.Find<ModItem>("StarlightAlloy"), 3);
                        recipe.AddIngredient(sots.Find<ModItem>("HardlightAlloy"), 3);
                    }
                }

                //DoG is only post sentinals
                if (recipe.HasIngredient(3467) && recipe.HasIngredient(ModContent.ItemType<GalacticaSingularity>()) && recipe.HasIngredient(ModContent.ItemType<Necroplasm>()) && recipe.HasResult(ModContent.ItemType<CosmicWorm>()))
                    recipe.DisableRecipe();

                //Why is this post-Calamaitas?
                if (recipe.HasIngredient(ModContent.ItemType<AshesofAnnihilation>()) && recipe.HasIngredient(ModContent.ItemType<AshesofCalamity>()) && recipe.HasResult(ModContent.ItemType<CeremonialUrn>()))
                    recipe.DisableRecipe();

                //Fight providence, idc
                if (recipe.HasResult(ModContent.ItemType<RuneofKos>()) && recipe.HasIngredient(ModContent.ItemType<UnholyEssence>()))
                    recipe.DisableRecipe();

                //Plantera
                if (recipe.HasResult(ModContent.ItemType<Portabulb>()))
                    recipe.AddIngredient(ModContent.ItemType<PlantyMush>(), 20);

                //Plaguebring is only post golem
                if (recipe.HasIngredient(ModContent.ItemType<PlagueCellCanister>()) && recipe.HasRecipeGroup(RecipeGroupID.IronBar) && recipe.HasIngredient(ItemID.Obsidian) && recipe.HasResult(ModContent.ItemType<Abombination>()))
                    recipe.DisableRecipe();

                //Hive Mind should be post EoW
                if (recipe.HasResult(ModContent.ItemType<Teratoma>()))
                    recipe.AddIngredient(ItemID.ShadowScale, 1);

                //Perforators should be post BoC
                if (recipe.HasResult(ModContent.ItemType<BloodyWormFood>()))
                    recipe.AddIngredient(ItemID.TissueSample, 1);

                //Aquatic Scourage
                if (recipe.HasIngredient(319) && recipe.HasIngredient(2626) && recipe.HasIngredient(ModContent.ItemType<SulphurousSand>()) && recipe.HasResult(ModContent.ItemType<Seafood>()))
                    recipe.DisableRecipe();

                //Shadow of Calamitas
                if (recipe.HasResult(ModContent.ItemType<EyeofDesolation>()))
                    recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 10);

                //Clamitas, Supreme Clam

                //Forgotten One

                //Ravager
                if (hasRagnarok)
                {
                    if (recipe.HasResult<DeathWhistle>())
                    {
                        recipe.RemoveIngredient(thorium.Find<ModItem>("CursedCloth").Type);
                    }
                }

                //The Primoridals
                if (hasThorium && ModLoader.TryGetMod("ThoriumRework", out Mod thorRework))
                {
                    if (thorium.TryFind("DoomSayersCoin", out ModItem ragnarokCoin))
                    {
                        if (recipe.HasResult(ragnarokCoin))
                        {
                            thorium.TryFind("ShootingStarFragment", out ModItem bardFragment);
                            thorium.TryFind("CelestialFragment", out ModItem healerFragment);
                            thorium.TryFind("WhiteDwarfFragment", out ModItem throwerFragment);
                            if (hasRagnarok && ragnarok.TryFind("EldritchShellFragment", out ModItem eShellFrag))
                            {
                                recipe.RemoveIngredient(eShellFrag.Type);
                            }
                            recipe.RemoveIngredient(ItemID.FragmentSolar);
                            recipe.RemoveIngredient(ItemID.FragmentVortex);
                            recipe.RemoveIngredient(ItemID.FragmentNebula);
                            recipe.RemoveIngredient(ItemID.FragmentStardust);
                            recipe.RemoveIngredient(bardFragment.Type);
                            recipe.RemoveIngredient(healerFragment.Type);
                            recipe.RemoveIngredient(throwerFragment.Type);
                            recipe.RemoveIngredient(ModContent.ItemType<MeldBlob>());
                            recipe.RemoveIngredient(ModContent.ItemType<AshesofCalamity>());
                            recipe.AddIngredient<GalacticaSingularity>(4);
                            recipe.AddIngredient<AscendantSpiritEssence>(3);
                            recipe.AddIngredient<AshesofCalamity>(10);
                            
                        }
                    }
                }

                //Moon Lord
                if (recipe.HasResult(3601))
                    recipe.AddIngredient(ModContent.ItemType<MeldConstruct>(), 12);
                Mod mod1;

                //We have infernum, right?
                if (ModLoader.TryGetMod("InfernumMode", out mod1))
                {
                    if (recipe.HasResult(mod1.Find<ModItem>("RedBait")))
                        recipe.AddIngredient(ItemID.ChumBucket, 10);
                    if (recipe.HasResult(mod1.Find<ModItem>("RadiantCrystal")))
                    {
                        recipe.AddIngredient(ItemID.EmpressButterfly, 5);
                        recipe.RemoveIngredient(526);
                    }
                    if (recipe.HasResult(mod1.Find<ModItem>("SparklingTunaCan")) && recipe.HasIngredient(2339))
                        recipe.DisableRecipe();
                }

                //Yharon
                if (recipe.HasIngredient(ModContent.ItemType<LifeAlloy>()) && recipe.HasResult(ModContent.ItemType<YharonEgg>()))
                {
                    recipe.DisableRecipe();
                }
            }
        }
        public override void AddRecipes()
        {

            //Skeletron
            Recipe recipe1 = Recipe.Create(ModContent.ItemType<DungeonsCurse>(), 1);
            recipe1.AddIngredient(ItemID.ClothierVoodooDoll, 1);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();

            //Supreme Calamitas
            Recipe recipe5 = Recipe.Create(ModContent.ItemType<CeremonialUrn>(), 1);
            recipe5.AddIngredient(ItemID.ClayBlock, 30);
            recipe5.AddIngredient(ModContent.ItemType<UnholyCore>(), 5);
            recipe5.AddIngredient(ModContent.ItemType<AshesofCalamity>(), 30);
            recipe5.AddIngredient(ModContent.ItemType<DemonicBoneAsh>(), 20);
            recipe5.AddTile(ModContent.TileType<SCalAltar>());
            recipe5.Register();

            //Sentinals
            Recipe recipe2 = Recipe.Create(ModContent.ItemType<RuneofKos>(), 1);
            recipe2.AddIngredient(ItemID.LunarBar, 3);
            recipe2.AddIngredient(ModContent.ItemType<DivineGeode>(), 10);
            recipe2.AddIngredient(ItemID.FragmentSolar, 5);
            recipe2.AddTile(TileID.LunarCraftingStation);
            recipe2.Register();

            //Yharon Post-Providence, DoG & Primordials
            Recipe recipe6 = Recipe.Create(ModContent.ItemType<YharonEgg>(), 1);
            recipe6.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 15);
            recipe6.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            recipe6.AddIngredient(ModContent.ItemType<UnholyEssence>(), 50);
            recipe6.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 5);

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && !ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
            {
                thorium.TryFind("DeathEssence", out ModItem death);
                thorium.TryFind("OceanEssence", out ModItem ocean);
                thorium.TryFind("InfernoEssence", out ModItem inferno);

                if (death != null && ocean != null && inferno != null)
                {
                    recipe6.AddIngredient(death.Type);
                    recipe6.AddIngredient(ocean.Type);
                    recipe6.AddIngredient(inferno.Type);
                }
            }

            recipe6.AddTile(ModContent.TileType<CosmicAnvil>());
            recipe6.Register();

            //Plaguebringer post Golem
            Recipe recipe7 = Recipe.Create(ModContent.ItemType<Abombination>(), 1);
            recipe7.AddIngredient(ModContent.ItemType<PlagueCellCanister>(), 20);
            recipe7.AddIngredient(ItemID.MartianConduitPlating, 30);
            recipe7.AddIngredient(ModContent.ItemType<DubiousPlating>(), 5);
            recipe7.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 5);
            recipe7.AddIngredient(ModContent.ItemType<OldPowerCell>(), 1);
            recipe7.AddTile(ModContent.TileType<CodebreakerTile>());
            recipe7.Register();

            //Aquatic Scourage
            Recipe recipe8 = Recipe.Create(ModContent.ItemType<Seafood>(), 1);
            recipe8.AddIngredient(ItemID.Sashimi, 5);
            recipe8.AddIngredient(ModContent.ItemType<MolluskHusk>(), 20);
            recipe8.AddIngredient(ModContent.ItemType<SulphuricScale>(), 20);
            recipe8.AddIngredient(ModContent.ItemType<PlantyMush>(), 30);
            recipe8.AddTile(TileID.CookingPots);
            recipe8.Register();

            //Pirate Map
            Recipe recipe11 = Recipe.Create(1315, 1);
            recipe11.AddIngredient(ItemID.AncientCloth, 10);
            recipe11.AddIngredient(ItemID.BlackInk, 2);
            recipe11.AddIngredient(ItemID.GoldCoin, 20);
            recipe11.AddTile(TileID.Loom);
            recipe11.AddTile(TileID.Bookcases);
            recipe11.Register();
        }
    }
}
