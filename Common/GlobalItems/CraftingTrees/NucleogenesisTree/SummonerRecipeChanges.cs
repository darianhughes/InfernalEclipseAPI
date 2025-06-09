using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using SOTS.Items.Celestial;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.NucleogenesisTree
{
    public class SummonerRecipeChanges : ModSystem
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

                if (!InfernalConfig.Instance.MergeCraftingTrees)
                    return;

                if (thorium != null)
                {
                    if (recipe.HasResult(thorium.Find<ModItem>("SteamkeeperWatch")))
                    {
                        recipe.AddIngredient(ItemID.SummonerEmblem);
                    }

                    if (recipe.HasResult(ModContent.ItemType<StatisCurse>()))
                    {
                        recipe.RemoveIngredient(ItemID.FragmentStardust);
                        recipe.AddIngredient(thorium.Find<ModItem>("YumasPendant"));
                        recipe.AddIngredient(ItemID.LunarBar, 8);
                    }
                }

                if (sots != null & thorium != null)
                {
                    if (recipe.HasResult(sots.Find<ModItem>("PlatformGenerator")))
                    {
                        recipe.AddIngredient(thorium.Find<ModItem>("ScryingGlass"));
                    }

                    if (recipe.HasResult(sots.Find<ModItem>("FortressGenerator")) && recipe.HasIngredient(ItemID.PygmyNecklace))
                    {
                        recipe.DisableRecipe();
                    }

                    if (recipe.HasResult(ModContent.ItemType<StatisBlessing>()))
                    {
                        recipe.RemoveIngredient(ItemID.PygmyNecklace);
                        recipe.RemoveIngredient(ItemID.SummonerEmblem);
                        recipe.RemoveIngredient(ModContent.ItemType<CoreofSunlight>());
                        recipe.AddIngredient(ItemID.BeetleHusk, 3);
                        recipe.AddIngredient(sots.Find<ModItem>("FortressGenerator"));
                        recipe.AddIngredient(thorium.Find<ModItem>("CrystalScorpion"));
                        recipe.AddIngredient(thorium.Find<ModItem>("SteamkeeperWatch"));
                    }
                }
                else if (thorium != null)
                {
                    if (recipe.HasResult(ModContent.ItemType<StatisBlessing>()))
                    {
                        recipe.RemoveIngredient(ItemID.PygmyNecklace);
                        recipe.RemoveIngredient(ItemID.SummonerEmblem);
                        recipe.RemoveIngredient(ModContent.ItemType<CoreofSunlight>());
                        recipe.AddIngredient(ItemID.BeetleHusk, 3);
                        recipe.AddIngredient(thorium.Find<ModItem>("NecroticSkull"));
                        recipe.AddIngredient(thorium.Find<ModItem>("CrystalScorpion"));
                        recipe.AddIngredient(thorium.Find<ModItem>("SteamkeeperWatch"));
                        recipe.AddIngredient(thorium.Find<ModItem>("ScryingGlass"));
                    }
                }
                else if (sots != null)
                {
                    if (recipe.HasResult(ModContent.ItemType<StatisBlessing>()))
                    {
                        recipe.RemoveIngredient(ItemID.PygmyNecklace);
                        recipe.RemoveIngredient(ModContent.ItemType<CoreofSunlight>());
                        recipe.AddIngredient(sots.Find<ModItem>("FortressGenerator"));
                        recipe.AddIngredient(ItemID.BeetleHusk, 3);
                    }
                }

                if (recipe.HasResult(ModContent.ItemType<Nucleogenesis>()))
                {
                    recipe.RemoveIngredient(ItemID.LunarBar);
                    recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
                }
            }
        }

        public override void AddRecipes()
        {
            if (sots != null & thorium != null)
            {
                Recipe.Create(sots.Find<ModItem>("FortressGenerator").Type)
                    .AddIngredient(sots.Find<ModItem>("PlatformGenerator"))
                    .AddIngredient(ItemID.PaladinsShield)
                    .AddIngredient(thorium.Find<ModItem>("NecroticSkull"))
                    .AddIngredient(ItemID.SpectreBar, 10)
                    .AddIngredient(sots.Find<ModItem>("DissolvingDeluge"))
                    .AddTile(TileID.TinkerersWorkbench)
                    .Register();
            }
        }
    }
}
