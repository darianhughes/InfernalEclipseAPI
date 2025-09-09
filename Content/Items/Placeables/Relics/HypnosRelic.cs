using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using InfernalEclipseAPI.Content.Tiles.Relics;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics
{
    public class HypnosRelic : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<HypnosRelicTile>(), 0);
            Item.width = 30;
            Item.height = 40;
            Item.rare = -13;
            Item.master = true;
        }

        public override string Texture
        {
            get => "InfernalEclipseAPI/Content/Items/Placeables/Relics/" + textureName();
        }

        private string textureName()
        {
            return InfernalConfig.Instance.ColoredRelics ? "HypnosRelicColored" : nameof(HypnosRelic);
        }
    }
}
