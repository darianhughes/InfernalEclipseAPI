using InfernalEclipseAPI.Core.Players;
using SOTSBardHealer.Items;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ItemReworks.Accessories
{
    [JITWhenModsEnabled("SOTSBardHealer")]
    [ExtendsFromMod("SOTSBardHealer")]
    public class RingofRestCooldown : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<RingofRest>();
        }

        public override bool InstancePerEntity => false;

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            var cdPlayer = player.GetModPlayer<InfernalPlayer>();

            if (cdPlayer.RingofRestCooldown > 0)
                return;

            cdPlayer.RingofRestCooldown = 60;

            player.GetModPlayer<SOTSBardHealer.SecretsOfThoriumPlayer>().CritInspirationsteal += Main.rand.Next(2) + 1;
        }
    }
}
