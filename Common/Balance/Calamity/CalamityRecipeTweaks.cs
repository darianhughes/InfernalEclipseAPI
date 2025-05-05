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
using Terraria.ID;

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
                if (recipe.HasResult(29))
                    recipe.DisableRecipe();
                if (recipe.HasResult(1291))
                    recipe.DisableRecipe();
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
            }
        }

        public override void AddRecipes()
        {
            if (!InfernalConfig.Instance.CalamityRecipeTweaks) return;
            Recipe recipe1 = Recipe.Create(ModContent.ItemType<RampartofDeities>(), 1);
            recipe1.AddIngredient(ItemID.FrozenShield, 1);
            recipe1.AddIngredient(ModContent.ItemType<DeificAmulet>(), 1);
            recipe1.AddIngredient(ModContent.ItemType<AsgardianAegis>(), 1);
            recipe1.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            recipe1.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 5);
            recipe1.AddTile(ModContent.TileType<CosmicAnvil>());
            recipe1.Register();
        }
    }
}
