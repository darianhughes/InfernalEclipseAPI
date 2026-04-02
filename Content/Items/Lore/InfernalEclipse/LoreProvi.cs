using CalamityMod.Items.LoreItems;
using CalamityMod.Rarities;

namespace InfernalEclipseAPI.Content.Items.Lore.InfernalEclipse
{
    public class LoreProvi : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.consumable = false;
            //Item.Calamity().devItem = true;
        }
    }
}
