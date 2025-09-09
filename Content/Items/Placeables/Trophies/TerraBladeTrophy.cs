using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Tiles.Trophies;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Placeables.Trophies
{
    public class TerraBladeTrophy : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<TerraBladeTrophyTile>(), 0);
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Blue;
        }
    }
}
