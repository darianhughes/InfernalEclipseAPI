using ThoriumMod.Rarities;

namespace InfernalEclipseAPI.Content.Items.Materials
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class DreamEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 34;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 1, 35, 0);
            Item.rare = ModContent.RarityType<BloodOrangeRarity>();
            Item.expert = true;
        }
    }
}
