using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.HypersonicTunerCraftingTree
{
    public class TunnerRecipeChanges : ModSystem
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

                if (ThoriumRework != null)
                {
                    if (ThoriumRework.TryFind("FanDonations", out ModItem fanDOnations) && (InfernalConfig.Instance.MergeCraftingTrees || InfernalConfig.Instance.ThoriumBalanceChangess))
                    {
                        if (recipe.HasResult(fanDOnations))
                        {
                            recipe.RemoveTile(TileID.WorkBenches);
                            recipe.AddTile(TileID.LunarCraftingStation);
                            recipe.RemoveTile(TileID.TinkerersWorkbench);
                            recipe.RemoveIngredient(thorium.Find<ModItem>("BloomWeave").Type);
                            recipe.AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 5);
                        }
                    }
                }

                if (!InfernalConfig.Instance.MergeCraftingTrees)
                    return;

                if (SOTSBardHealer != null)
                {
                    if (recipe.HasResult(SOTSBardHealer.Find<ModItem>("HypersonicTuner")))
                    {
                        recipe.RemoveIngredient(SOTSBardHealer.Find<ModItem>("SubsonicTuner").Type);

                        recipe.AddIngredient(SOTSBardHealer, "InfrasonicTuner");
                        recipe.AddIngredient(thorium, "ShootingStarFragment", 6);
                        recipe.AddIngredient<EffulgentFeather>(5);
                    }
                }
            }
        }
    }
}
