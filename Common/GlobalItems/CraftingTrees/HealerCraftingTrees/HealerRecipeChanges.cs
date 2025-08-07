using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.HealerCraftingTrees
{
    public class HealerRecipeChanges : ModSystem
    {
        private Mod Ragnarok
        {
            get
            {
                ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok);
                return ragnarok;
            }
        }

        private Mod CalBardHealer
        {
            get
            {
                ModLoader.TryGetMod("CalamityBardHealer", out Mod calbh);
                return calbh;
            }
        }

        private Mod SOTSBardHealer
        {
            get
            {
                ModLoader.TryGetMod("SOTSBardHealer", out Mod sotsbh);
                return sotsbh;
            }
        }

        private Mod ThoriumRework
        {
            get
            {
                ModLoader.TryGetMod("ThoriumRework", out Mod thorRe);
                return thorRe;
            }
        }
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

                if (CalBardHealer != null)
                {
                    if (!InfernalConfig.Instance.MergeCraftingTrees)
                        return;

                    if (recipe.HasResult(CalBardHealer.Find<ModItem>("BloomingSaintessStatue")))
                    {
                        recipe.AddIngredient<LifeAlloy>(3);
                    }

                    if (recipe.HasResult(CalBardHealer.Find<ModItem>("ElementalBloom")))
                    {
                        recipe.RemoveIngredient(ItemID.LunarBar);
                        recipe.AddIngredient<CosmiliteBar>(8);
                        recipe.AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 2);
                        recipe.AddIngredient(thorium.Find<ModItem>("SoulGuard"));
                    }
                }

                if (ThoriumRework != null)
                {
                    if (ThoriumRework.TryFind("ExecutionersContract", out ModItem contract) && (InfernalConfig.Instance.MergeCraftingTrees || InfernalConfig.Instance.ThoriumBalanceChangess))
                    {
                        if (recipe.HasResult(contract))
                        {
                            recipe.RemoveTile(TileID.Loom);
                            recipe.AddTile(TileID.LunarCraftingStation);
                            recipe.AddIngredient(thorium.Find<ModItem>("CelestialFragment"), 5);
                        }
                    }

                    if (!InfernalConfig.Instance.MergeCraftingTrees)
                        return;

                    if (ThoriumRework.TryFind("SealedContract", out ModItem sealedContract))
                    {
                        if (recipe.HasResult(sealedContract))
                        {
                            recipe.RemoveIngredient(thorium.Find<ModItem>("LifeGem").Type);

                            if (SOTSBardHealer != null)
                            {
                                recipe.AddIngredient(SOTSBardHealer.Find<ModItem>("SerpentsTongue"));
                            }

                            recipe.AddIngredient(thorium.Find<ModItem>("InfernoEssence"), 3);
                            recipe.RemoveTile(TileID.MythrilAnvil);
                            recipe.AddTile(ModContent.TileType<CosmicAnvil>());
                        }
                    }
                }
            }
        }
    }
}
