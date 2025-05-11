using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using InfernalEclipseAPI.Content.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Placeables
{
    public class BossRushEncoreMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(ModContent.TileType<BossRushEncoreMusicBoxTile>(), 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Rock>(1).
                AddIngredient<ShadowspecBar>(3).
                AddIngredient(ItemID.MusicBox).
                AddTile(TileID.HeavyWorkBench).
                Register();
        }

    }
}
