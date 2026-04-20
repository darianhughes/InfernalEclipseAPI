using InfernalEclipseAPI.Content.Tiles.Relics;

namespace InfernalEclipseAPI.Content.Items.Placeables.Relics
{
    public class DreadnautilusRelic : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<DreadnautilusRelicTile>(), 0);
            Item.width = 30;
            Item.height = 40;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
        }

        public override string Texture => "InfernalEclipseAPI/Content/Items/Placeables/Relics/" + textureName();

        private string textureName()
        {
            return InfernalConfig.Instance.ColoredRelics ? "DreadnautilusRelicColored" : nameof(DreadnautilusRelic);
        }
    }
}
