using InfernalEclipseAPI.Content.Items.SpawnItems;
using SOTS.Items.Celestial;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    [ExtendsFromMod("SOTS")]
    public class CatalystBombDropper : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<CatalystBomb>();
        }

        public override bool? UseItem(Item item, Player player)
        {
            Item.NewItem(item.GetSource_Loot(), player.Hitbox, ModContent.ItemType<CatalyzedCrystal>());
            return base.UseItem(item, player);
        }
    }
}
