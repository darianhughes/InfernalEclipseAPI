using InfernalEclipseAPI.Content.Tiles.Relics;
using InfernalEclipseAPI.Core.Configs;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics
{
    public class HypnosRelic : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<HypnosRelicTile>(), 0);
            Item.width = 30;
            Item.height = 40;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
        }

        public override string Texture => "InfernalEclipseAPI/Content/Items/Placeables/Relics/" + textureName();

        private string textureName()
        {
            return InfernalConfig.Instance.ColoredRelics ? "HypnosRelicColored" : nameof(HypnosRelic);
        }
    }
}
