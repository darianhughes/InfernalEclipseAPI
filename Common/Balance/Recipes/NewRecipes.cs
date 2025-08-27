using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using InfernalEclipseAPI.Common.GlobalItems;
using SOTS;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Placeables;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace InfernalEclipseAPI.Common.Balance.Recipes
{
    public class NewRecipes : ModSystem
    {
        public static RecipeGroup EvilBarRecipeGroup;
        public override void Unload()
        {
            EvilBarRecipeGroup = null;
        }

        public override void AddRecipeGroups()
        {
            EvilBarRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CrimtaneBar)}", ItemID.DemoniteBar, ItemID.CrimtaneBar);
            RecipeGroup.RegisterGroup("LimitedResourcesRecipes:EvilBar", EvilBarRecipeGroup);
        }
        public override void AddRecipes()
        {
            Recipe.Create(ItemID.Terragrim)
            .AddIngredient(ItemID.EnchantedSword, 1)
            .AddIngredient(ItemID.JungleSpores, 5)
            .AddRecipeGroup(EvilBarRecipeGroup, 5)
            .AddIngredient(ItemID.Obsidian, 3)
            .AddIngredient(ItemID.FossilOre, 3)
            .AddTile(TileID.Anvils)
            .Register();

            Recipe recipe12 = Recipe.Create(4672, 1);
            recipe12.AddIngredient(ItemID.Leather, 8);
            recipe12.AddTile(TileID.Loom);
            recipe12.Register();

            if (ModLoader.TryGetMod("AlchemistNPCLite", out Mod AlchNPC))
            {
                int[] alchCombos =
                {
                    GetModItem(AlchNPC, "BattleCombination"),
                    GetModItem(AlchNPC, "BewitchingPotion"),
                    GetModItem(AlchNPC, "BuilderCombination"),
                    GetModItem(AlchNPC, "CalamityCombination"),
                    GetModItem(AlchNPC, "ExplorerCombination"),
                    GetModItem(AlchNPC, "FishingCombination"),
                    GetModItem(AlchNPC, "MageCombination"),
                    GetModItem(AlchNPC, "RangerCombination"),
                    GetModItem(AlchNPC, "SummonerCombination"),
                    GetModItem(AlchNPC, "VanTankCombination")
                };

                foreach (int potion in  alchCombos)
                {
                    Recipe newRecipe = Recipe.Create(potion, 2);

                    if (InfernalConfig.Instance.BloodOrbPotionDuplication)
                    {
                        newRecipe.AddIngredient(potion);
                    }
                    else
                    {
                        newRecipe.AddIngredient(ItemID.BottledWater);
                    }
                    
                    if (potion == GetModItem(AlchNPC, "BattleCombination"))
                        newRecipe.AddIngredient<BloodOrb>(60);
                    if (potion == GetModItem(AlchNPC, "BewitchingPotion"))
                        newRecipe.AddIngredient<BloodOrb>(10);
                    if (potion == GetModItem(AlchNPC, "BuilderCombination"))
                        newRecipe.AddIngredient<BloodOrb>(30);
                    if (potion == GetModItem(AlchNPC, "CalamityCombination"))
                        newRecipe.AddIngredient<BloodOrb>(40);
                    if (potion == GetModItem(AlchNPC, "ExplorerCombination"))
                        newRecipe.AddIngredient<BloodOrb>(90);
                    if (potion == GetModItem(AlchNPC, "MageCombination"))
                    {
                        newRecipe.AddIngredient<BloodOrb>(40);
                        newRecipe.AddIngredient(ItemID.FallenStar, 1);
                    }
                    if (potion == GetModItem(AlchNPC, "FishingCombination"))
                        newRecipe.AddIngredient<BloodOrb>(80);
                    if (potion == GetModItem(AlchNPC, "RangerCombination"))
                        newRecipe.AddIngredient<BloodOrb>(40);
                    if (potion == GetModItem(AlchNPC, "SummonerCombination"))
                        newRecipe.AddIngredient<BloodOrb>(30);
                    if (potion == GetModItem(AlchNPC, "FishingCombination"))
                        newRecipe.AddIngredient<BloodOrb>(80);
                    if (potion == GetModItem(AlchNPC, "VanTankCombination"))
                        newRecipe.AddIngredient<BloodOrb>(60);

                    newRecipe.AddTile(TileID.AlchemyTable);
                    newRecipe.Register();
                }

                if (ModLoader.TryGetMod("ThoriumMod", out _))
                {
                    int thorComboId = GetModItem(AlchNPC, "ThoriumCombination");
                    Recipe thorComboBloodOrb = Recipe.Create(thorComboId, 2);
                    if (InfernalConfig.Instance.BloodOrbPotionDuplication)
                    {
                        thorComboBloodOrb.AddIngredient(thorComboId);
                    }
                    else
                    {
                        thorComboBloodOrb.AddIngredient(ItemID.BottledWater);
                    }
                    thorComboBloodOrb.AddIngredient<BloodOrb>(90);

                    thorComboBloodOrb.AddTile(TileID.AlchemyTable);
                    thorComboBloodOrb.Register();
                }
            }

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                if (ModLoader.TryGetMod("ThoriumRework", out Mod thorRework) && !InfernalConfig.Instance.DisableBloodOrbPotions)
                {
                    if (!InfernalConfig.Instance.ThoriumBalanceChangess)
                    {
                        Recipe.Create(thorRework.Find<ModItem>("DeathsingerPotion").Type)
                            .AddIngredient(ItemID.BottledWater)
                            .AddIngredient<BloodOrb>(10)
                            .AddTile(TileID.AlchemyTable)
                            .Register();
                    }

                    thorium.TryFind("ManaBerry", out ModItem manaberry);
                    if (thorRework.TryFind("InspirationRegenerationPotion", out ModItem inspRegenPotion))
                    {
                        Recipe.Create(thorRework.Find<ModItem>("InspirationRegenerationPotion").Type)
                            .AddIngredient(ItemID.BottledWater)
                            .AddIngredient<BloodOrb>(10)
                            .AddIngredient(manaberry.Type)
                            .AddTile(TileID.AlchemyTable)
                            .Register();
                    }
                }

                if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
                {
                    Recipe.Create(thorium.Find<ModItem>("VoltHatchet").Type)
                        .AddIngredient(thorium.Find<ModItem>("AbyssalChitin").Type, 8)
                        .AddIngredient(ragnarok.Find<ModItem>("EmpoweredGranite").Type, 3)
                        .AddIngredient(thorium.Find<ModItem>("AquaiteBar").Type, 10)
                        .AddTile(TileID.Anvils)
                        .Register();
                }

                if (ModLoader.TryGetMod("SOTS", out Mod sots))
                {
                    /*
                        Recipe.Create(ModContent.ItemType<LiliesOfFinality>())
                            .AddIngredient(ItemID.ClayPot)
                            .AddIngredient<PlantyMush>()
                            .AddIngredient<AscendantSpiritEssence>(3)
                            .AddIngredient<YharonSoulFragment>(4)
                            .AddIngredient(ItemID.DarkShard)
                            .AddIngredient(ItemID.LightShard)
                            .AddTile<CosmicAnvil>()
                            .Register();
                    */

                    int[] sotsPotions =
                    {
                        GetModItem(sots, "AssassinationPotion"),
                        GetModItem(sots, "BluefirePotion"),
                        GetModItem(sots, "BrittlePotion"),
                        GetModItem(sots, "DoubleVisionPotion"),
                        GetModItem(sots, "HarmonyPotion"),
                        GetModItem(sots, "NightmarePotion"),
                        GetModItem(sots, "RipplePotion"),
                        GetModItem(sots, "RoughskinPotion"),
                        GetModItem(sots, "SoulAccessPotion"),
                        GetModItem(sots, "VibePotion")
                    };

                    foreach (int potion in sotsPotions)
                    {
                        Recipe newRecipe = Recipe.Create(potion, 2);

                        if (InfernalConfig.Instance.BloodOrbPotionDuplication)
                        {
                            newRecipe.AddIngredient(potion);
                        }
                        else
                        {
                            newRecipe.AddIngredient(ItemID.BottledWater);
                        }
                        newRecipe.AddIngredient<BloodOrb>(10);

                        if (potion == GetModItem(sots, "HarmonyPotion"))
                        {
                            newRecipe.AddIngredient(ItemID.SoulofLight);
                        }
                        else if (potion == GetModItem(sots, "NightmarePotion"))
                        {
                            newRecipe.AddIngredient(ItemID.SoulofNight);
                        }
                        newRecipe.AddTile(TileID.AlchemyTable);
                        newRecipe.Register();
                    }
                }
            }
        }

        private int GetModItem (Mod mod, string item)
        {
            return mod.Find<ModItem>(item).Type;
        }
    }
}
