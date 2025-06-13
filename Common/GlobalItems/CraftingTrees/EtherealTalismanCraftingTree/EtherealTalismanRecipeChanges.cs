using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Accessories;
using ThoriumMod;
using CalamityMod;
using Steamworks;


namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.EtherealTalismanCraftingTree
{
    public class EtherealTalismanRecipeChanges : ModSystem
    {
        private Mod calamity
        {
            get
            {
                ModLoader.TryGetMod("CalamityMod", out Mod cal);
                return cal;
            }
        }
        private Mod thorium
        {
            get
            {
                ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
                return thorium;
            }
        }

        public override void PostAddRecipes()
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees || calamity == null || thorium == null)
                return;

            for (int index = 0; index < Recipe.numRecipes; ++index)
            {
                Recipe recipe = Main.recipe[index];

                if (recipe.HasResult(ModContent.ItemType<SigilofCalamitas>()))
                {
                    recipe.AddIngredient(thorium.Find<ModItem>("MurkyCatalyst"), 1);
                }

                if (recipe.HasResult(ModContent.ItemType<EtherealTalisman>()))
                {
                    recipe.AddIngredient(thorium.Find<ModItem>("HungeringBlossom"), 1);
                }
            }
        }
    }
}
