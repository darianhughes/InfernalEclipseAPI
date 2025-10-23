using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.Balance.Recipes
{
    internal sealed class DCURecipeTweak : ModSystem
    {
        public override void PostAddRecipes()
        {

            foreach (var recipe in Main.recipe)
            {
                if (recipe.HasResult(ItemID.DrillContainmentUnit))
                {
                    ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
                    bool hasThorium = thorium != null;
                    ModLoader.TryGetMod("SOTS", out Mod sots);
                    bool hasSOTS = sots != null;

                    //Uses our stack size instead of any other mods
                    recipe.RemoveIngredient(ItemID.LunarBar);
                    recipe.RemoveIngredient(ItemID.ShroomiteBar);
                    recipe.RemoveIngredient(ModContent.ItemType<AstralBar>());
                    recipe.RemoveIngredient(ModContent.ItemType<LifeAlloy>());
                    recipe.RemoveIngredient(ModContent.ItemType<AerialiteBar>());

                    recipe.AddIngredient(ItemID.LunarBar, 20);
                    recipe.AddIngredient<AstralBar>(20);
                    if (hasThorium)
                    {
                        //Included in Terrarium Core
                        recipe.RemoveIngredient(ItemID.ChlorophyteBar);
                        recipe.RemoveIngredient(ItemID.SpectreBar);
                        recipe.RemoveIngredient(ItemID.HellstoneBar);
                        recipe.RemoveIngredient(ItemID.MeteoriteBar);

                        //Use our stack size
                        recipe.RemoveIngredient(thorium.Find<ModItem>("TerrariumCore").Type);

                        recipe.AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 20);

                        //Included in Life Alloy
                        thorium.TryFind("TitanicBar", out ModItem titanBar);
                        recipe.RemoveIngredient(titanBar.Type);
                    }
                    if (hasSOTS) recipe.AddIngredient(sots.Find<ModItem>("PhaseBar"), 20);
                    recipe.AddIngredient<LifeAlloy>(20);
                    recipe.AddIngredient(ItemID.ShroomiteBar, 20);
                    if (thorium != null)
                    {
                        //Use our stack size
                        recipe.RemoveIngredient(thorium.Find<ModItem>("IllumiteIngot").Type);
                        recipe.RemoveIngredient(thorium.Find<ModItem>("LodeStoneIngot").Type);
                        recipe.RemoveIngredient(thorium.Find<ModItem>("ValadiumIngot").Type);
                        recipe.RemoveIngredient(thorium.Find<ModItem>("aDarksteelAlloy").Type);
                        recipe.RemoveIngredient(thorium.Find<ModItem>("AquaiteBar").Type);

                        recipe.AddIngredient(thorium.Find<ModItem>("IllumiteIngot"), 20);
                        recipe.AddIngredient(thorium.Find<ModItem>("LodeStoneIngot"), 20);
                        recipe.AddIngredient(thorium.Find<ModItem>("ValadiumIngot"), 20);
                        recipe.AddIngredient(thorium.Find<ModItem>("aDarksteelAlloy"), 20);
                        recipe.AddIngredient(thorium.Find<ModItem>("AquaiteBar"), 20);
                    }
                    recipe.AddIngredient<AerialiteBar>(20);
                    if (hasThorium)
                    {
                        //Use our stack size
                        recipe.RemoveIngredient(thorium.Find<ModItem>("SandstoneIngot").Type);

                        recipe.AddIngredient(thorium.Find<ModItem>("SandstoneIngot"), 20);
                    }
                    if (hasSOTS)
                    {
                        recipe.AddIngredient(sots.Find<ModItem>("FrigidBar"), 20);
                    }
                }
            }
        }
    }
}
