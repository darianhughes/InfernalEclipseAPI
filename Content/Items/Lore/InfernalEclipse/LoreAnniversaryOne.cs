using CalamityMod.Items.LoreItems;
using InfernumMode.Content.Rarities.InfernumRarities;
using NoxusBoss.Core.GlobalInstances;

namespace InfernalEclipseAPI.Content.Items.Lore.InfernalEclipse
{
    public class LoreAnniversaryOne : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Item.ResearchUnlockCount = 1;
            PlayerDataManager.PostUpdateEvent += ProvideLuck;
        }

        private void ProvideLuck(PlayerDataManager p)
        {
            if (p.Player.HasItem(Type))
                p.Player.luck += 0.25f;
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
