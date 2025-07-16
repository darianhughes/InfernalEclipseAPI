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
using Terraria.Localization;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;


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

        public override void AddRecipes()
        {
            if (!InfernalConfig.Instance.MergeCraftingTrees || calamity == null || thorium == null)
                return;

            RecipeGroup group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.ManaFlower)}", new int[]
            {
                ItemID.ManaFlower,
                ItemID.ArcaneFlower,
                ItemID.MagnetFlower,
                ItemID.ManaCloak,
            });
            int AnyManaFlower = RecipeGroup.RegisterGroup("AnyManaFlowerAccessory", group);

            Recipe.Create(ModContent.ItemType<EtherealTalisman>())
               .AddIngredient<SigilofCalamitas>().
                AddRecipeGroup("AnyManaFlowerAccessory"). //Any mana flower accessory
                AddIngredient<CosmiliteBar>(8).
                AddIngredient<GalacticaSingularity>(4).
                AddIngredient<AscendantSpiritEssence>(4).
                AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 2).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<EtherealTalisman>())
               .AddIngredient<SigilofCalamitas>().
                AddIngredient(thorium.Find<ModItem>("HungeringBlossom")).
                AddIngredient<CosmiliteBar>(8).
                AddIngredient<GalacticaSingularity>(4).
                AddIngredient<AscendantSpiritEssence>(4).
                AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 2).
                AddTile<CosmicAnvil>().
                Register();
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

                if (recipe.HasResult(ModContent.ItemType<EtherealTalisman>()) && recipe.HasIngredient(ItemID.LunarBar))
                {
                    recipe.DisableRecipe();
                }
            }
        }
    }
}
