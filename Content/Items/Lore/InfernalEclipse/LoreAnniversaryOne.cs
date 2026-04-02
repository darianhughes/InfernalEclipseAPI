using CalamityMod.Items.LoreItems;
using InfernumMode.Content.Rarities.InfernumRarities;

namespace InfernalEclipseAPI.Content.Items.Lore.InfernalEclipse
{
    public class LoreAnniversaryOne : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Item.ResearchUnlockCount = 1;
        }

        public override void UpdateInventory(Player player)
        {
            if (player.HasItem(Type))
                player.luck += 0.25f;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 26;
            Item.rare = ModContent.RarityType<InfernumRedSparkRarity>();
            Item.consumable = false;
        }
    }
}