namespace InfernalEclipseAPI.Content.Items.Materials
{
    public class TinkerersRepairBlueprints : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 8);
        }
    }
}
