using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.InsigniaCraftingTree
{
    public class InsigniaRecipeChanges : ModSystem
    {
        private Mod calamity
        {
            get
            {
                ModLoader.TryGetMod("CalamityMod", out Mod cal);
                return cal;
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

                if (sots != null)
                {
                    if (recipe.HasResult(ModContent.ItemType<AscendantInsignia>()))
                    {
                        recipe.RemoveIngredient(ItemID.EmpressFlightBooster);
                        recipe.AddIngredient(sots.Find<ModItem>("SpiritInsignia"), 1);
                    }

                    if (recipe.HasResult(sots.Find<ModItem>("GildedBladeWings")))
                    {
                        recipe.RemoveIngredient(sots.Find<ModItem>("SpiritInsignia").Type);
                        recipe.AddIngredient<AscendantInsignia>(1);
                        recipe.AddIngredient<AscendantSpiritEssence>(3);
                    }
                }
            }
        }
    }
}
