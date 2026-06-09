using InfernalEclipseAPI.Content.Tiles.Relics;
using InfernalEclipseAPI.Core.Configs;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics
{
    public class TerraBladeRelic : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<TerraBladeRelicTile>(), 0);
            Item.width = 30;
            Item.height = 40;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
        }

        public override string Texture  => "InfernalEclipseAPI/Content/Items/Placeables/Relics/" + textureName();

        private string textureName()
        {
            return InfernalConfig.Instance.ColoredRelics ? "TerraBladeRelicColored" : nameof(TerraBladeRelic);
        }
    }
}
