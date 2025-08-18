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
                    //Uses our stack size instead of any other mods
                    recipe.RemoveIngredient(ModContent.ItemType<AstralBar>());
                    recipe.RemoveIngredient(ModContent.ItemType<LifeAlloy>());
                    recipe.RemoveIngredient(ModContent.ItemType<AerialiteBar>());

                    recipe.AddIngredient<AstralBar>(40);
                    recipe.AddIngredient<LifeAlloy>(40);
                    recipe.AddIngredient<AerialiteBar>(40);

                    if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                    {
                        //Included in Terrarium Core
                        recipe.RemoveIngredient(ItemID.ChlorophyteBar);
                        recipe.RemoveIngredient(ItemID.SpectreBar);
                        recipe.RemoveIngredient(ItemID.HellstoneBar);
                        recipe.RemoveIngredient(ItemID.MeteoriteBar);

                        //Included in Life Alloy
                        thorium.TryFind("TitanicBar", out ModItem titanBar);
                        recipe.RemoveIngredient(titanBar.Type);

                        //Use our stack size
                        recipe.RemoveIngredient(thorium.Find<ModItem>("TerrariumCore").Type);
                        recipe.RemoveIngredient(thorium.Find<ModItem>("IllumiteIngot").Type);
                        recipe.RemoveIngredient(thorium.Find<ModItem>("LodeStoneIngot").Type);
                        recipe.RemoveIngredient(thorium.Find<ModItem>("ValadiumIngot").Type);
                        recipe.RemoveIngredient(thorium.Find<ModItem>("aDarksteelAlloy").Type);
                        recipe.RemoveIngredient(thorium.Find<ModItem>("AquaiteBar").Type);
                        recipe.RemoveIngredient(thorium.Find<ModItem>("SandstoneIngot").Type);

                        recipe.AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 40);
                        recipe.AddIngredient(thorium.Find<ModItem>("IllumiteIngot"), 40);
                        recipe.AddIngredient(thorium.Find<ModItem>("LodeStoneIngot"), 40);
                        recipe.AddIngredient(thorium.Find<ModItem>("ValadiumIngot"), 40);
                        recipe.AddIngredient(thorium.Find<ModItem>("aDarksteelAlloy"), 40);
                        recipe.AddIngredient(thorium.Find<ModItem>("AquaiteBar"), 40);
                        recipe.AddIngredient(thorium.Find<ModItem>("SandstoneIngot"), 40);
                    }

                    if (ModLoader.TryGetMod("SOTS", out Mod sots))
                    {
                        recipe.AddIngredient(sots.Find<ModItem>("PhaseBar"), 40);
                        recipe.AddIngredient(sots.Find<ModItem>("FrigidBar"), 40);
                    }
                }
            }
        }
    }
}
