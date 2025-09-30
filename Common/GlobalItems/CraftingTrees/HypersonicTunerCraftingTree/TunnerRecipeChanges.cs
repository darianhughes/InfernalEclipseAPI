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
                    }

                    if (ModLoader.TryGetMod("ssm", out Mod CSE))
                    {
                        if (recipe.HasResult(CSE.Find<ModItem>("BardSoul").Type))
                        {
                            if (SOTSBardHealer.TryFind("TesseractTuner", out ModItem tuner))
                            {
                                recipe.RemoveIngredient(ThoriumRework.Find<ModItem>("FanDonations").Type);
                                recipe.AddIngredient(tuner.Type);
                            }
                        }
                    }
                }
            }
        }
    }
}
