using CalamityMod.Items;

namespace InfernalEclipseAPI.Content.Items.Materials
{
    public class InfectedMothwingSpore : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 40;
            Item.rare = ItemRarityID.Blue;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.maxStack = 9999;
        }
    }
}
