using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Accessories;
using Terraria.ID;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.RogueCraftingTrees
{
    public class RogueRecipeChanges : ModSystem
    {
        private Mod thorium
        {
            get
            {
                ModLoader.TryGetMod("ThoriumMod", out Mod thor);
                return thor;
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
                    //ROGUE 101 CHANGES
                    if (recipe.HasResult(thorium.Find<ModItem>("ThrowingGuideVolume2")))
                        recipe.AddIngredient<PerennialBar>(3);

                    if (recipe.HasResult(thorium.Find<ModItem>("ThrowingGuideVolume3")))
                        recipe.AddIngredient(thorium.Find<ModItem>("WhiteDwarfFragment"), 6);

                    //VAMPIRIC TALISMAN CHANGES
                    if (recipe.HasResult<VampiricTalisman>())
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<RogueEmblem>());
                        recipe.AddIngredient(ItemID.AvengerEmblem);
                    }

                    if (clamity != null)
                    {
                        if (recipe.HasResult(clamity.Find<ModItem>("DraculasCharm")))
                        {
                            recipe.AddIngredient<MeldBlob>(3);
                            recipe.AddIngredient(thorium.Find<ModItem>("WhiteDwarfFragment"), 3);
                        }
                    }

                    //NANOTECH CHANGES
                    if (recipe.HasResult<FeatherCrown>())
                        recipe.AddIngredient(thorium.Find<ModItem>("AquaiteBar"), 6);

                    if (recipe.HasResult<MoonstoneCrown>())
                        recipe.AddIngredient<TitanHeart>();

                    if (recipe.HasResult<FilthyGlove>() || recipe.HasResult<BloodstainedGlove>())
                        recipe.AddIngredient(thorium.Find<ModItem>("BoneGrip"));

                    if (recipe.HasResult(thorium.Find<ModItem>("MagnetoGrip")) && recipe.HasIngredient(thorium.Find<ModItem>("BoneGrip")))
                    {
                        recipe.DisableRecipe();
                    }

                    if (recipe.HasResult<ElectriciansGlove>() && recipe.HasIngredient<BloodstainedGlove>())
                        recipe.DisableRecipe();

                    if (recipe.HasResult<ElectriciansGlove>())
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<FilthyGlove>());
                        recipe.RemoveIngredient(ModContent.ItemType<BloodstainedGlove>());
                        recipe.AddIngredient(ItemID.HallowedBar, 3);
                        recipe.AddIngredient(thorium.Find<ModItem>("MagnetoGrip"));
                    }

                    if (recipe.HasResult<Nanotech>())
                    {
                        recipe.RemoveIngredient(ItemID.LunarBar);
                        recipe.AddIngredient<CosmiliteBar>(8);
                        recipe.AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 6);
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees)
                return;

            if (thorium != null)
            {
                Recipe corruptMaganeto = Recipe.Create(thorium.Find<ModItem>("MagnetoGrip").Type)
                                               .AddIngredient<FilthyGlove>()
                                               .AddIngredient(ItemID.Wire, 25)
                                               .AddIngredient<DubiousPlating>(8)
                                               .AddTile(TileID.TinkerersWorkbench)
                                               .Register();

                Recipe crimsonMaganeto = Recipe.Create(thorium.Find<ModItem>("MagnetoGrip").Type)
                                               .AddIngredient<BloodstainedGlove>()
                                               .AddIngredient(ItemID.Wire, 25)
                                               .AddIngredient<DubiousPlating>(8)
                                               .AddTile(TileID.TinkerersWorkbench)
                                               .Register();
            }
        }
    }
}
