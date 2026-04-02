using System.Collections.Generic;
using CalamityMod.Items;
using InfernumMode.Content.Rarities.InfernumRarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria.Graphics.Shaders;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Items.Dyes
{
    public class AngelDyeItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Item.ResearchUnlockCount = 3;

            if (Main.dedServ)
            {
                return;
            }

            GameShaders.Armor.BindShader(Type, new ArmorShaderData(Mod.Assets.Request<Effect>("Assets/Effects/AngelDye"), "AngelDyePass"));
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.rare = ModContent.RarityType<InfernumVassalRarity>();
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Color lerpedColor = Color.Lerp(Color.White, new Color(30, 144, 255), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));

            if (Main.keyState.IsKeyDown(Keys.LeftShift))
            {
                TooltipLine line5 = new(Mod, "DedicatedItem", $"{Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.DedTo", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Dedicated.Arkangel"))}\n{Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Playtester")}");
                line5.OverrideColor = lerpedColor;
                tooltips.Add(line5);
            }
            else
            {
                TooltipLine line5 = new(Mod, "DedicatedItem", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Playtester"));
                line5.OverrideColor = lerpedColor;
                tooltips.Add(line5);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cloud, 10)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }
}