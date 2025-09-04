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

        private Mod clamity
        {
            get
            {
                ModLoader.TryGetMod("Clamity", out Mod clam);
                return clam;
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
                    //if (recipe.HasResult(thorium.Find<ModItem>("SteamkeeperWatch")))
                    //{
                    //    recipe.AddIngredient(ItemID.SummonerEmblem);
                    //}

                    if (recipe.HasResult(ModContent.ItemType<StatisCurse>()))
                    {
                        //recipe.RemoveIngredient(ItemID.FragmentStardust);
                        //recipe.AddIngredient(thorium.Find<ModItem>("YumasPendant"));
                        //recipe.AddIngredient(ItemID.LunarBar, 8);
                    }

                    if (recipe.HasResult(ModContent.ItemType<Nucleogenesis>()))
                        if (!recipe.HasIngredient(thorium.Find<ModItem>("TerrariumCore"))) recipe.AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 3);

                    if (recipe.HasResult<StarTaintedGenerator>())
                    {
                        recipe.AddIngredient(thorium.Find<ModItem>("SteamkeeperWatch"));
                        recipe.AddIngredient<InfectedArmorPlating>(4);
                    }
                }

                if (sots != null)
                {
                    if (recipe.HasResult(ModContent.ItemType<StatisCurse>()))
                        recipe.AddIngredient(ModContent.ItemType<Necroplasm>(), 4);

                    if (recipe.HasResult(ModContent.ItemType<Nucleogenesis>()))
                        recipe.AddIngredient(sots.Find<ModItem>("FortressGenerator"));

                    if (recipe.HasResult(sots.Find<ModItem>("FortressGenerator")))
                    {
                        recipe.RemoveIngredient(ItemID.PaladinsShield);
                        recipe.RemoveIngredient(ItemID.SpectreBar);

                        if (clamity != null)
                        {
                            recipe.AddIngredient(clamity.Find<ModItem>("CyanPearl"));
                        }

                        recipe.AddIngredient<LifeAlloy>(2);
                    }
                }

                if (sots != null & thorium != null)
                {
                    //if (recipe.HasResult(sots.Find<ModItem>("PlatformGenerator")))
                    //{
                    //    recipe.AddIngredient(thorium.Find<ModItem>("ScryingGlass"));
                    //}

                    if (recipe.HasResult(sots.Find<ModItem>("FortressGenerator")) && recipe.HasIngredient(ItemID.PygmyNecklace))
                    {
                        recipe.RemoveIngredient(ItemID.PygmyNecklace);
                        if (!recipe.HasIngredient(thorium.Find<ModItem>("NecroticSkull"))) recipe.AddIngredient(thorium.Find<ModItem>("NecroticSkull"));
                    }

                    if (recipe.HasResult(ModContent.ItemType<StatisBlessing>()))
                    {
                        //recipe.RemoveIngredient(ItemID.PygmyNecklace);
                        //recipe.RemoveIngredient(ItemID.SummonerEmblem);
                        //recipe.RemoveIngredient(ModContent.ItemType<CoreofSunlight>());
                        recipe.RemoveIngredient(ModContent.ItemType<CoreofSunlight>());
                        //recipe.AddIngredient(sots.Find<ModItem>("FortressGenerator"));
                        recipe.AddIngredient(thorium.Find<ModItem>("CrystalScorpion"));
                        //recipe.AddIngredient(thorium.Find<ModItem>("SteamkeeperWatch"));
                        recipe.AddIngredient<CoreofCalamity>();
                    }
                }
                else if (thorium != null)
                {
                    if (recipe.HasResult(ModContent.ItemType<StatisBlessing>()))
                    {
                        recipe.RemoveIngredient(ItemID.PygmyNecklace);
                        //recipe.RemoveIngredient(ItemID.SummonerEmblem);
                        //recipe.RemoveIngredient(ModContent.ItemType<CoreofSunlight>());
                        //recipe.AddIngredient(ItemID.BeetleHusk, 3);
                        recipe.AddIngredient(thorium.Find<ModItem>("NecroticSkull"));
                        recipe.AddIngredient(thorium.Find<ModItem>("CrystalScorpion"));
                        //recipe.AddIngredient(thorium.Find<ModItem>("SteamkeeperWatch"));
                        //recipe.AddIngredient(thorium.Find<ModItem>("ScryingGlass"));
                    }

                    if (recipe.HasResult(ModContent.ItemType<Nucleogenesis>()))
                    {
                        if (clamity != null)
                        {
                            recipe.AddIngredient(clamity.Find<ModItem>("CyanPearl"));
                        }
                    }
                }

                if (recipe.HasResult(ModContent.ItemType<Nucleogenesis>()))
                {
                    recipe.RemoveIngredient(ItemID.LunarBar);
                    recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 8);
                }
            }
        }
    }
}
