using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.SummonItems;
using CalamityMod.Tiles.DraedonSummoner;
using CalamityMod.Tiles.Furniture.CraftingStations;
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

            foreach (var recipe in Main.recipe)
            {
                //Skeletron
                if (recipe.HasResult(ModContent.ItemType<DungeonsCurse>()) && InfernalConfig.Instance.CalamityRecipeTweaks)
                {
                    recipe.AddIngredient(ItemID.Bone, 5);
                }

                //DoG is only post sentinals
                if (recipe.HasIngredient(3467) && recipe.HasIngredient(ModContent.ItemType<GalacticaSingularity>()) && recipe.HasIngredient(ModContent.ItemType<Necroplasm>()) && recipe.HasResult(ModContent.ItemType<CosmicWorm>()))
                    recipe.DisableRecipe();

                //Why is this post-Calamaitas?
                if (recipe.HasIngredient(ModContent.ItemType<AshesofAnnihilation>()) && recipe.HasIngredient(ModContent.ItemType<AshesofCalamity>()) && recipe.HasResult(ModContent.ItemType<CeremonialUrn>()))
                    recipe.DisableRecipe();

                //Fight providence, idc
                if (recipe.HasResult(ModContent.ItemType<RuneofKos>()))
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

                //Crabulon

                //Aquatic Scourage
                if (recipe.HasIngredient(319) && recipe.HasIngredient(2626) && recipe.HasIngredient(ModContent.ItemType<SulphurousSand>()) && recipe.HasResult(ModContent.ItemType<Seafood>()))
                    recipe.DisableRecipe();

                //Calamitas Clone - Shadow of Calamitas
                if (recipe.HasResult(ModContent.ItemType<EyeofDesolation>()))
                    recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 10);

                //Clamitas, Supreme Clam

                //Forgotten One

                //Ravager

                //The Primoridals
                if (hasThorium && ModLoader.TryGetMod("ThoriumRework", out Mod thorRework))
                {
                    if (thorium.TryFind("DoomSayersCoin", out ModItem ragnarokCoin))
                    {
                        if (recipe.HasResult(ragnarokCoin))
                        {
                            recipe.AddIngredient<CosmiliteBar>(3);
                        }
                    }
                }

                //Moon Lord
                if (recipe.HasResult(3601))
                    recipe.AddIngredient(ModContent.ItemType<MeldConstruct>(), 12);
                Mod mod1;

                //We have infernum, right?
                if (Terraria.ModLoader.ModLoader.TryGetMod("InfernumMode", out mod1))
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
            recipe5.AddTile(ModContent.TileType<DraedonsForge>());
            recipe5.Register();

            //Yharon Post-DoG
            Recipe recipe6 = Recipe.Create(ModContent.ItemType<YharonEgg>(), 1);
            recipe6.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 15);
            recipe6.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            recipe6.AddIngredient(ModContent.ItemType<UnholyEssence>(), 50);
            recipe6.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 5);
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
