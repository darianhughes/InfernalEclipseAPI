using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.RadianceCraftingTree
{
    public class RadianceRecipeChanges : ModSystem
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

                if (thorium != null)
                {
                    if (recipe.HasResult<HoneyDew>())
                        recipe.AddIngredient(thorium.Find<ModItem>("LivingLeaf").Type, 3);

                    if (recipe.HasResult<LivingDew>())
                        recipe.AddIngredient(thorium.Find<ModItem>("BioMatter").Type, 2);

                    if (recipe.HasResult<InfectedJewel>())
                        recipe.AddIngredient(thorium.Find<ModItem>("CrystalGeode").Type, 5);

                    if (recipe.HasResult<Radiance>())
                        recipe.AddIngredient(thorium.Find<ModItem>("SpiritsGrace").Type);
                }

                if (sots != null)
                {
                    if (recipe.HasResult<RadiantOoze>())
                        recipe.AddIngredient(sots.Find<ModItem>("CorrosiveGel").Type, 20);
                }
            }
        }
    }
}
