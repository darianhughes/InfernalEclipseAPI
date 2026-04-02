using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using CalamityMod.Items.LoreItems;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Items.Lore.SOTS
{
    public class LoreAdvisor : LoreItem
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
            Item.rare = ItemRarityID.Green;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            ModLoader.TryGetMod("SOTS", out Mod sots);

            CreateRecipe()
                .AddIngredient(sots.Find<ModItem>("AdvisorTrophy").Type)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}
