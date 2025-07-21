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
                    //ROGUE 101 CHANGES
                    if (recipe.HasResult(thorium.Find<ModItem>("ThrowingGuideVolume2")))
                        recipe.AddIngredient<PerennialBar>(3);

                    if (recipe.HasResult(thorium.Find<ModItem>("ThrowingGuideVolume3")))
                        recipe.AddIngredient(thorium.Find<ModItem>("WhiteDwarfFragment"), 6);

                    //VAMPIRIC TALISMAN CHANGES
                    if (recipe.HasResult<VampiricTalisman>())
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<RogueEmblem>());
                        recipe.AddIngredient(thorium.Find<ModItem>("CursedCloth"), 3);
                        if (ModLoader.TryGetMod("Consolaria", out Mod console))
                        {
                            recipe.AddIngredient(console.Find<ModItem>("SoulofBlight"), 3);
                        }
                        else
                        {
                            recipe.AddIngredient(ItemID.SpookyWood, 20);
                        }
                        recipe.AddIngredient(thorium.Find<ModItem>("ShinobiSigil"));
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

                    if (recipe.HasResult(thorium.Find<ModItem>("BoneGrip")))
                    {
                        recipe.AddIngredient<ScuttlersJewel>();
                        recipe.AddIngredient<AncientBoneDust>(2);
                    }

                    if (recipe.HasResult<FilthyGlove>() || recipe.HasResult<BloodstainedGlove>())
                    {
                        recipe.AddIngredient(thorium.Find<ModItem>("UnholyShards"), 5);
                        recipe.AddIngredient(thorium.Find<ModItem>("BoneGrip"));
                    }

                    if (recipe.HasResult(thorium.Find<ModItem>("MagnetoGrip")) && recipe.HasIngredient(thorium.Find<ModItem>("BoneGrip")))
                    {
                        recipe.DisableRecipe();
                    }

                    if (recipe.HasResult<ElectriciansGlove>() && (recipe.HasIngredient<BloodstainedGlove>() || recipe.HasIngredient<FilthyGlove>()))
                        recipe.DisableRecipe();

                    if (recipe.HasResult<Nanotech>())
                    {
                        recipe.RemoveIngredient(ItemID.LunarBar);
                        recipe.AddIngredient<CosmiliteBar>(8);
                        recipe.AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 2);
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
                                               .AddIngredient(ItemID.Wire, 15)
                                               .AddIngredient<DubiousPlating>(6)
                                               .AddIngredient<PurifiedGel>(3)
                                               .AddTile(TileID.TinkerersWorkbench)
                                               .Register();

                Recipe crimsonMaganeto = Recipe.Create(thorium.Find<ModItem>("MagnetoGrip").Type)
                                               .AddIngredient<BloodstainedGlove>()
                                               .AddIngredient(ItemID.Wire, 15)
                                               .AddIngredient<DubiousPlating>(6)
                                               .AddIngredient<PurifiedGel>(3)
                                               .AddTile(TileID.TinkerersWorkbench)
                                               .Register();

                Recipe electriciansGlove = Recipe.Create(ModContent.ItemType<ElectriciansGlove>());
                electriciansGlove.AddIngredient(thorium.Find<ModItem>("MagnetoGrip"));

                if (sots != null)
                {
                    electriciansGlove.AddIngredient(sots.Find<ModItem>("AbsoluteBar"), 5);
                    electriciansGlove.AddIngredient(sots.Find<ModItem>("TwilightShard"), 12);
                }
                else
                {
                    electriciansGlove.AddIngredient(ItemID.Wire, 100);
                    electriciansGlove.AddIngredient(ItemID.HallowedBar, 5);
                }
                electriciansGlove.Register();
            }
        }
    }
}
