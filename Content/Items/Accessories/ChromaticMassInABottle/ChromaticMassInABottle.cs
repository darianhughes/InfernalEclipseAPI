using Terraria.DataStructures;
using InfernumMode;
using CalamityMod.Rarities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace InfernalEclipseAPI.Content.Items.Accessories.ChromaticMassInABottle
{
    public class ChromaticMassInABottle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 5));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;

            Item.value = Item.buyPrice(copper: 0);
            Item.rare = ItemRarityID.Purple;
            Item.maxStack = 1;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.accessory = true;

            Item.Infernum_Tooltips().DeveloperItem = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Color color = CalamityUtils.ColorSwap(Color.OrangeRed, Color.DarkRed, 2f);

            if (Main.keyState.IsKeyDown(Keys.LeftShift))
            {
                TooltipLine line5 = new(Mod, "DedicatedItem", $"{Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.DedTo", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Dedicated.Yob"))}");
                line5.OverrideColor = color;
                tooltips.Add(line5);
            }
        }

        public override void AddRecipes()
        {
            if (ModLoader.TryGetMod("CalamityHunt", out Mod calamityHunt) && calamityHunt.TryFind("ChromaticMass", out ModItem ChormaticMass))
            {
                CreateRecipe()
                    .AddIngredient(ChormaticMass.Type, 1)
                    .AddIngredient(ItemID.Bottle, 1)
                    .Register();
            }
            else
            {
                CreateRecipe()
                    .AddIngredient(ItemID.Bottle, 1)
                    .AddIngredient(ItemID.LunarBar, 1)
                    .Register();
            }
        }
    }
}
