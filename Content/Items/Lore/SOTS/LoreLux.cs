using System.Collections.Generic;
using CalamityMod.Items.LoreItems;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Items.Lore.SOTS
{
    public class LoreLux : LoreItem
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.TryGetMod("SOTS", out _);

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 26;
            Item.rare = ItemRarityID.LightRed;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            ModLoader.TryGetMod("SOTS", out Mod sots);

            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(sots.Find<ModItem>("SpiritSymphony").Type)
                .AddTile(TileID.Bookcases)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(sots.Find<ModItem>("LuxRelic").Type)
                .AddTile(TileID.Bookcases)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(sots.Find<ModItem>("LuxBag").Type)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}
