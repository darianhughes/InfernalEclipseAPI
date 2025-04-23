using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Placeables
{
    public class BossRushTier6MusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(ModContent.TileType<Tiles.BossRushTier6MusicBoxTile>(), 0);
        }

    }
}
