using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Tiles.Relics;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics
{
    public class TerraBladeRelic : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<TerraBladeRelicTile>(), 0);
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
            return InfernalConfig.Instance.ColoredRelics ? "TerraBladeRelicColored" : nameof(TerraBladeRelic);
        }
    }
}
