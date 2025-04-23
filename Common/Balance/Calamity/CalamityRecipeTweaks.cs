using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Potions;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Tiles.DraedonSummoner;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.Balance.Calamity
{
    public class CalamityRecipeTweaks : ModSystem
    {
        public override void PostAddRecipes()
        {
            if (!InfernalConfig.Instance.CalamityRecipeTweaks)
            {
                return;
            }

            for (int index = 0; index < Recipe.numRecipes; ++index)
            {
                Recipe recipe = Main.recipe[index];
                if (recipe.HasIngredient(3997) && recipe.HasIngredient(ModContent.ItemType<DeificAmulet>()) && recipe.HasIngredient(ModContent.ItemType<AuricBar>()) && recipe.HasIngredient(ModContent.ItemType<AscendantSpiritEssence>()) && !recipe.HasIngredient<AsgardianAegis>() && recipe.HasTile(ModContent.TileType<CosmicAnvil>()) && recipe.HasResult(ModContent.ItemType<RampartofDeities>()))
                    recipe.DisableRecipe();
                if (recipe.HasResult(65))
                    recipe.DisableRecipe();
                if (recipe.HasResult(989))
                    recipe.DisableRecipe();
                if (recipe.HasResult(155))
                    recipe.DisableRecipe();
                if (recipe.HasResult(670))
                    recipe.DisableRecipe();
                if (recipe.HasResult(53))
                    recipe.DisableRecipe();
                if (recipe.HasResult(4341))
                    recipe.DisableRecipe();
                if (recipe.HasResult(54))
                    recipe.DisableRecipe();
                if (recipe.HasResult(987))
                    recipe.DisableRecipe();
                if (recipe.HasResult(857))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2423))
                    recipe.DisableRecipe();
                if (recipe.HasResult(934))
                    recipe.DisableRecipe();
                if (recipe.HasResult(285))
                    recipe.DisableRecipe();
                if (recipe.HasResult(212))
                    recipe.DisableRecipe();
                if (recipe.HasResult(863))
                    recipe.DisableRecipe();
                if (recipe.HasResult(159))
                    recipe.DisableRecipe();
                if (recipe.HasResult(158))
                    recipe.DisableRecipe();
                if (recipe.HasResult(950))
                    recipe.DisableRecipe();
                if (recipe.HasResult(906))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1323))
                    recipe.DisableRecipe();
                if (recipe.HasResult(211))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3084))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3102))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3119))
                    recipe.DisableRecipe();
                if (recipe.HasResult(49))
                    recipe.DisableRecipe();
                if (recipe.HasResult(930))
                    recipe.DisableRecipe();
                if (recipe.HasResult(156))
                    recipe.DisableRecipe();
                if (recipe.HasResult(886))
                    recipe.DisableRecipe();
                if (recipe.HasResult(885))
                    recipe.DisableRecipe();
                if (recipe.HasResult(887))
                    recipe.DisableRecipe();
                if (recipe.HasResult(891))
                    recipe.DisableRecipe();
                if (recipe.HasResult(892))
                    recipe.DisableRecipe();
                if (recipe.HasResult(888))
                    recipe.DisableRecipe();
                if (recipe.HasResult(893))
                    recipe.DisableRecipe();
                if (recipe.HasResult(889))
                    recipe.DisableRecipe();
                if (recipe.HasResult(890))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3781))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1321))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1253))
                    recipe.DisableRecipe();
                if (recipe.HasResult(946))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1991))
                    recipe.DisableRecipe();
                if (recipe.HasResult(4271))
                    recipe.DisableRecipe();
                if (recipe.HasResult(213))
                    recipe.DisableRecipe();
                if (recipe.HasResult(329))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2197))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2198))
                    recipe.DisableRecipe();
                if (recipe.HasResult(4276))
                    recipe.DisableRecipe();
                if (recipe.HasResult(4346))
                    recipe.DisableRecipe();
                if (recipe.HasResult(267))
                    recipe.DisableRecipe();
                if (recipe.HasResult(4988))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1141))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1293))
                    recipe.DisableRecipe();
                if (recipe.HasResult(975))
                    recipe.DisableRecipe();
                if (recipe.HasResult(953))
                    recipe.DisableRecipe();
                if (recipe.HasResult(848))
                    recipe.DisableRecipe();
                if (recipe.HasResult(866))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2585))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2430))
                    recipe.DisableRecipe();
                if (recipe.HasResult(256 /*0x0100*/))
                    recipe.DisableRecipe();
                if (recipe.HasResult(258))
                    recipe.DisableRecipe();
                if (recipe.HasResult(257))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1123))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2888))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1121))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1132))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2502))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1273))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1313))
                    recipe.DisableRecipe();
                if (recipe.HasResult(426))
                    recipe.DisableRecipe();
                if (recipe.HasResult(434))
                    recipe.DisableRecipe();
                if (recipe.HasResult(514))
                    recipe.DisableRecipe();
                if (recipe.HasResult(4912))
                    recipe.DisableRecipe();
                if (recipe.HasResult(490))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2998))
                    recipe.DisableRecipe();
                if (recipe.HasResult(489))
                    recipe.DisableRecipe();
                if (recipe.HasResult(491))
                    recipe.DisableRecipe();
                if (recipe.HasResult(758))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1255))
                    recipe.DisableRecipe();
                if (recipe.HasResult(788))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1178))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3018))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1259))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1155))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1157))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1305))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3021))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1294))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1258))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1122))
                    recipe.DisableRecipe();
                if (recipe.HasResult(899))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1248))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1295))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1296))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1297))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2611))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2624))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2622))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2623))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2621))
                    recipe.DisableRecipe();
                if (recipe.HasResult(2609))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3063))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3389))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3065))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1553))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3541))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3570))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3930))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3569))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3571))
                    recipe.DisableRecipe();
                if (recipe.HasIngredient(126) && recipe.HasIngredient(ModContent.ItemType<BloodOrb>()))
                    recipe.DisableRecipe();
                if (recipe.HasIngredient(353) && recipe.HasIngredient(ModContent.ItemType<BloodOrb>()))
                    recipe.DisableRecipe();
                if (recipe.HasIngredient(2766) && recipe.HasIngredient(1101) && recipe.HasResult(ModContent.ItemType<DeathWhistle>()))
                    recipe.DisableRecipe();
                if (recipe.HasIngredient(319) && recipe.HasIngredient(2626) && recipe.HasIngredient(ModContent.ItemType<SulphurousSand>()) && recipe.HasResult(ModContent.ItemType<Seafood>()))
                    recipe.DisableRecipe();
                if (recipe.HasIngredient(3467) && recipe.HasIngredient(ModContent.ItemType<GalacticaSingularity>()) && recipe.HasIngredient(ModContent.ItemType<Necroplasm>()) && recipe.HasResult(ModContent.ItemType<CosmicWorm>()))
                    recipe.DisableRecipe();
                if (recipe.HasIngredient(3458) && recipe.HasIngredient(ModContent.ItemType<LifeAlloy>()) && recipe.HasResult(ModContent.ItemType<ExoticPheromones>()))
                    recipe.DisableRecipe();
                if (recipe.HasIngredient(ModContent.ItemType<ArmoredShell>()) && recipe.HasIngredient(ModContent.ItemType<TwistingNether>()) && recipe.HasIngredient(ModContent.ItemType<DarkPlasma>()) && !recipe.HasIngredient(ModContent.ItemType<RuinousSoul>()) && recipe.HasResult(ModContent.ItemType<CosmicWorm>()))
                    recipe.DisableRecipe();
                if (recipe.HasIngredient(ModContent.ItemType<AshesofAnnihilation>()) && recipe.HasIngredient(ModContent.ItemType<AshesofCalamity>()) && recipe.HasResult(ModContent.ItemType<CeremonialUrn>()))
                    recipe.DisableRecipe();
                if (recipe.HasIngredient(ModContent.ItemType<EffulgentFeather>()) && recipe.HasIngredient(ModContent.ItemType<LifeAlloy>()) && !recipe.HasIngredient(ModContent.ItemType<CosmiliteBar>()) && recipe.HasResult(ModContent.ItemType<YharonEgg>()))
                    recipe.DisableRecipe();
                if (recipe.HasResult(ModContent.ItemType<RuneofKos>()))
                    recipe.DisableRecipe();
                if (recipe.HasResult(ModContent.ItemType<EyeofDesolation>()))
                    recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 10);
                if (recipe.HasResult(ModContent.ItemType<Portabulb>()))
                    recipe.AddIngredient(ModContent.ItemType<PlantyMush>(), 20);
                if (recipe.HasResult(ModContent.ItemType<Abombination>()))
                    recipe.DisableRecipe();
                if (recipe.HasResult(ModContent.ItemType<Teratoma>()))
                    recipe.AddIngredient(86, 1);
                if (recipe.HasResult(ModContent.ItemType<BloodyWormFood>()))
                    recipe.AddIngredient(1329, 1);
                if (recipe.HasResult(29))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1291))
                    recipe.DisableRecipe();
                if (recipe.HasResult(3601))
                    recipe.AddIngredient(ModContent.ItemType<MeldConstruct>(), 12);
                if (recipe.HasRecipeGroup("AnyEvilBlock") && recipe.HasResult(ModContent.ItemType<OverloadedSludge>()))
                    recipe.DisableRecipe();
                Mod mod1;
                if (Terraria.ModLoader.ModLoader.TryGetMod("InfernumMode", out mod1))
                {
                    if (recipe.HasResult(mod1.Find<ModItem>("RedBait")))
                        recipe.AddIngredient(4608, 10);
                    if (recipe.HasResult(mod1.Find<ModItem>("RadiantCrystal")))
                    {
                        recipe.AddIngredient(4961, 5);
                        recipe.RemoveIngredient(526);
                    }
                    if (recipe.HasResult(mod1.Find<ModItem>("SparklingTunaCan")) && recipe.HasIngredient(2339))
                        recipe.DisableRecipe();
                }
                Mod mod2;
                if (Terraria.ModLoader.ModLoader.TryGetMod("CatalystMod", out mod2))
                {
                    if (recipe.HasIngredient(mod2.Find<ModItem>("MetanovaBar")) && recipe.HasIngredient(ModContent.ItemType<MysteriousCircuitry>()))
                        recipe.DisableRecipe();
                    if (recipe.HasIngredient(mod2.Find<ModItem>("MetanovaBar")) && recipe.HasResult(ModContent.ItemType<PlasmaRifle>()))
                        recipe.DisableRecipe();
                    if (recipe.HasIngredient(mod2.Find<ModItem>("MetanovaBar")) && recipe.HasResult(ModContent.ItemType<Auralis>()))
                        recipe.DisableRecipe();
                    if (recipe.HasIngredient(mod2.Find<ModItem>("MetanovaBar")) && recipe.HasResult(ModContent.ItemType<FreedomStar>()))
                        recipe.DisableRecipe();
                }
                if (Terraria.ModLoader.ModLoader.HasMod("Remnants") && recipe.HasIngredient(3460) && !recipe.HasIngredient(5349) && recipe.HasResult(3467))
                    recipe.DisableRecipe();
                if (Terraria.ModLoader.ModLoader.HasMod("CalamityMod") && recipe.HasResult(3467))
                    recipe.AddIngredient(ModContent.ItemType<ExodiumCluster>(), 4);
            }
        }

        public override void AddRecipes()
        {
            if (!InfernalConfig.Instance.CalamityRecipeTweaks) return;

            short[] numArray1 = new short[47]
            {
              (short) 2997,
              (short) 2351,
              (short) 290,
              (short) 295,
              (short) 305,
              (short) 298,
              (short) 297,
              (short) 299,
              (short) 296,
              (short) 304,
              (short) 2329,
              (short) 300,
              (short) 2324,
              (short) 2349,
              (short) 2347,
              (short) 301,
              (short) 292,
              (short) 2346,
              (short) 289,
              (short) 2345,
              (short) 2323,
              (short) 2326,
              (short) 303,
              (short) 2344,
              (short) 294,
              (short) 293,
              (short) 2328,
              (short) 2348,
              (short) 2359,
              (short) 288,
              (short) 291,
              (short) 302,
              (short) 2327,
              (short) 2325,
              (short) 2322,
              (short) 2354,
              (short) 2356,
              (short) 2355,
              (short) 2756,
              (short) 2352,
              (short) 2353,
              (short) 2350,
              (short) 4870,
              (short) 4477,
              (short) 4478,
              (short) 4479,
              (short) 5211
            };
            int[] numArray2 = new int[12]
            {
              ModContent.ItemType<PotionofOmniscience>(),
              ModContent.ItemType<BoundingPotion>(),
              ModContent.ItemType<CalciumPotion>(),
              ModContent.ItemType<CeaselessHungerPotion>(),
              ModContent.ItemType<GravityNormalizerPotion>(),
              ModContent.ItemType<PhotosynthesisPotion>(),
              ModContent.ItemType<ShadowPotion>(),
              ModContent.ItemType<SoaringPotion>(),
              ModContent.ItemType<SulphurskinPotion>(),
              ModContent.ItemType<TeslaPotion>(),
              ModContent.ItemType<ZenPotion>(),
              ModContent.ItemType<ZergPotion>()
            };
            foreach (short num in numArray1)
            {
                Recipe recipe = Recipe.Create((int)num, 1);
                recipe.AddIngredient((int)num, 1);
                recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 10);
                recipe.AddTile(355);
                recipe.Register();
            }
            foreach (int num in numArray2)
            {
                Recipe recipe = Recipe.Create(num, 1);
                recipe.AddIngredient(num, 1);
                recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 10);
                recipe.AddTile(355);
                recipe.Register();
            }
            Recipe recipe1 = Recipe.Create(ModContent.ItemType<RampartofDeities>(), 1);
            recipe1.AddIngredient(3997, 1);
            recipe1.AddIngredient(ModContent.ItemType<DeificAmulet>(), 1);
            recipe1.AddIngredient(ModContent.ItemType<AsgardianAegis>(), 1);
            recipe1.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            recipe1.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 5);
            recipe1.AddTile(ModContent.TileType<CosmicAnvil>());
            recipe1.Register();
            Recipe recipe2 = Recipe.Create(ModContent.ItemType<CosmicWorm>(), 1);
            recipe2.AddIngredient(ModContent.ItemType<ArmoredShell>(), 1);
            recipe2.AddIngredient(ModContent.ItemType<TwistingNether>(), 1);
            recipe2.AddIngredient(ModContent.ItemType<DarkPlasma>(), 1);
            recipe2.AddIngredient(ModContent.ItemType<RuinousSoul>(), 1);
            recipe2.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 20);
            recipe2.AddTile(412);
            recipe2.Register();
            Recipe recipe3 = Recipe.Create(ModContent.ItemType<ExoticPheromones>(), 1);
            recipe3.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 5);
            recipe3.AddIngredient(ModContent.ItemType<MeldConstruct>(), 20);
            recipe3.AddIngredient(ModContent.ItemType<LifeAlloy>(), 10);
            recipe3.AddTile(412);
            recipe3.Register();
            Recipe recipe4 = Recipe.Create(ModContent.ItemType<DeathWhistle>(), 1);
            recipe4.AddIngredient(2766, 15);
            recipe4.AddIngredient(ModContent.ItemType<LifeAlloy>(), 5);
            recipe4.AddTile(134);
            recipe4.Register();
            Recipe recipe5 = Recipe.Create(ModContent.ItemType<CeremonialUrn>(), 1);
            recipe5.AddIngredient(133, 30);
            recipe5.AddIngredient(ModContent.ItemType<UnholyCore>(), 5);
            recipe5.AddIngredient(ModContent.ItemType<AshesofCalamity>(), 30);
            recipe5.AddIngredient(ModContent.ItemType<DemonicBoneAsh>(), 20);
            recipe5.AddTile(ModContent.TileType<DraedonsForge>());
            recipe5.Register();
            Recipe recipe6 = Recipe.Create(ModContent.ItemType<YharonEgg>(), 1);
            recipe6.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 15);
            recipe6.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            recipe6.AddIngredient(ModContent.ItemType<UnholyEssence>(), 50);
            recipe6.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 5);
            recipe6.AddTile(ModContent.TileType<CosmicAnvil>());
            recipe6.Register();
            Recipe recipe7 = Recipe.Create(ModContent.ItemType<Abombination>(), 1);
            recipe7.AddIngredient(ModContent.ItemType<PlagueCellCanister>(), 20);
            recipe7.AddIngredient(2860, 30);
            recipe7.AddIngredient(ModContent.ItemType<DubiousPlating>(), 5);
            recipe7.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 5);
            recipe7.AddIngredient(ModContent.ItemType<OldPowerCell>(), 20);
            recipe7.AddTile(ModContent.TileType<CodebreakerTile>());
            recipe7.Register();
            Recipe recipe8 = Recipe.Create(ModContent.ItemType<Seafood>(), 1);
            recipe8.AddIngredient(2427, 5);
            recipe8.AddIngredient(ModContent.ItemType<MolluskHusk>(), 20);
            recipe8.AddIngredient(ModContent.ItemType<SulphuricScale>(), 20);
            recipe8.AddIngredient(ModContent.ItemType<PlantyMush>(), 30);
            recipe8.AddTile(96 /*0x60*/);
            recipe8.Register();
            Recipe recipe9 = Recipe.Create(ModContent.ItemType<OverloadedSludge>(), 1);
            recipe9.AddIngredient(23, 100);
            recipe9.AddIngredient(ModContent.ItemType<BlightedGel>(), 20);
            recipe9.AddIngredient(86, 30);
            recipe9.AddIngredient(ModContent.ItemType<BloodSample>(), 30);
            recipe9.AddTile(96 /*0x60*/);
            recipe9.Register();
            Recipe recipe10 = Recipe.Create(ModContent.ItemType<OverloadedSludge>(), 1);
            recipe10.AddIngredient(23, 100);
            recipe10.AddIngredient(ModContent.ItemType<BlightedGel>(), 20);
            recipe10.AddIngredient(1329, 30);
            recipe10.AddIngredient(ModContent.ItemType<RottenMatter>(), 30);
            recipe10.AddTile(96 /*0x60*/);
            recipe10.Register();
            Recipe recipe11 = Recipe.Create(1315, 1);
            recipe11.AddIngredient(3794, 10);
            recipe11.AddIngredient(1119, 2);
            recipe11.AddIngredient(73, 20);
            recipe11.AddTile(86);
            recipe11.AddTile(101);
            recipe11.Register();
            Recipe recipe12 = Recipe.Create(4672, 1);
            recipe12.AddIngredient(259, 10);
            recipe12.AddTile(86);
            recipe12.Register();
            if (!Terraria.ModLoader.ModLoader.HasMod("Remnants"))
                return;
            Recipe recipe13 = Recipe.Create(3467, 2);
            recipe13.AddIngredient(3460, 4);
            recipe13.AddIngredient(5349, 4);
            recipe13.AddTile(412);
            recipe13.Register();
        }
    }
}
