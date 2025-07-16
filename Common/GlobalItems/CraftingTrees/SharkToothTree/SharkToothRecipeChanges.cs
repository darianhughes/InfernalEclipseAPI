using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.SharkToothTree
{
    internal class SharkToothRecipeChanges : ModSystem
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
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;

            for (int index = 0; index < Recipe.numRecipes; ++index)
            {
                Recipe recipe = Main.recipe[index];

                if (sots != null && thorium != null)
                {
                    if (recipe.HasResult<ReaperToothNecklace>())
                    {
                        recipe.AddIngredient(thorium.Find<ModItem>("OceanEssence"), 5);
                        recipe.RemoveTile(TileID.TinkerersWorkbench);
                        recipe.AddTile<CosmicAnvil>();
                    }

                    if (recipe.HasResult(sots.Find<ModItem>("MidnightPrism")))
                    {
                        recipe.AddIngredient<MeldConstruct>(3);
                    }
                }
                else if (sots != null)
                {
                    if (recipe.HasResult(ModContent.ItemType<SandSharkToothNecklace>()))
                    {
                        recipe.RemoveIngredient(ItemID.SharkToothNecklace);
                        recipe.AddIngredient(sots.Find<ModItem>("PrismarineNecklace").Type);
                        recipe.AddIngredient(ItemID.FragmentSolar, 3);
                    }

                    if (recipe.HasResult(sots.Find<ModItem>("MidnightPrism")))
                    {
                        recipe.RemoveIngredient(sots.Find<ModItem>("PrismarineNecklace").Type);
                        recipe.AddIngredient<SandSharkToothNecklace>();
                        recipe.AddIngredient<Necroplasm>(3);
                        recipe.RemoveTile(TileID.TinkerersWorkbench);
                        recipe.AddTile(TileID.LunarCraftingStation);
                    }

                    if (recipe.HasResult<ReaperToothNecklace>())
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<SandSharkToothNecklace>());
                    }
                }

                if (thorium != null)
                {
                    if (recipe.HasResult(ModContent.ItemType<SandSharkToothNecklace>()))
                    {
                        recipe.RemoveIngredient(ItemID.SharkToothNecklace);
                        recipe.AddIngredient(thorium.Find<ModItem>("DragonTalonNecklace").Type);
                        recipe.AddIngredient(ItemID.FragmentSolar, 3);
                        recipe.RemoveTile(TileID.TinkerersWorkbench);
                        recipe.AddTile(TileID.LunarCraftingStation);
                    }
                }

                if (sots != null)
                {
                    if (recipe.HasResult(sots.Find<ModItem>("PrismarineNecklace").Type))
                    {
                        recipe.AddIngredient<SeaPrism>(4);
                    }

                    if (recipe.HasResult(sots.Find<ModItem>("WitchHeart").Type))
                    {
                        if (ModLoader.TryGetMod("Consolaria", out Mod console))
                            recipe.AddIngredient(console.Find<ModItem>("SoulofBlight").Type, 3);
                        else
                            recipe.AddIngredient(ItemID.BeetleHusk, 3);
                    }

                    if (recipe.HasResult<ReaperToothNecklace>())
                    {
                        recipe.AddIngredient(sots.Find<ModItem>("MidnightPrism"));
                    }
                }
            }
        }
    }
}
