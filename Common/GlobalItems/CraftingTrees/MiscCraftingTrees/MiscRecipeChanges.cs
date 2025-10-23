using CalamityMod.Items.Accessories;
using InfernalEclipseAPI.Content.Items.Accessories.RingofTix;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.MiscCraftingTrees
{
    public class MiscRecipeChanges : ModSystem
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

        public override bool IsLoadingEnabled(Mod mod)
        {
            return InfernalConfig.Instance.MergeCraftingTrees;
        }

        public override void PostAddRecipes()
        {
            for (int index = 0; index < Recipe.numRecipes; ++index)
            {
                Recipe recipe = Main.recipe[index];

                if (thorium != null)
                {
                    if (recipe.HasResult<TheAbsorber>())
                    {
                        recipe.AddIngredient(thorium.Find<ModItem>("MonsterCharm").Type);
                    }

                    if (recipe.HasResult(thorium.Find<ModItem>("GreedyGoblet").Type) && sots != null)
                    {
                        recipe.RemoveIngredient(ItemID.GreedyRing);
                        recipe.AddIngredient(sots.Find<ModItem>("GreedierRing"));
                    }
                }

                if (ModLoader.HasMod("FargowiltasSouls"))
                {
                    if (recipe.HasResult(ModLoader.GetMod("FargowiltasSouls").Find<ModItem>("EternitySoul")))
                    {
                        recipe.AddIngredient<RingofTix>();
                    }
                }
            }
        }
    }
}
