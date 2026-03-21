using InfernalEclipseAPI.Core.Systems;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ItemReworks.Weapons.Healer
{
    public class RecuperateChanges : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return !InfernalCrossmod.Hummus.Loaded;
        }

        public override bool InstancePerEntity => false;
        public static Mod thorium
        {
            get
            {
                ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
                return thorium;
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!IsRecuperate(item)) return base.Shoot(item, player, source, position, velocity, type, damage, knockback);

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && thorium.TryFind("Mortality", out ModBuff mortality))
            {
                player.AddBuff(mortality.Type, 600);
            }

            return true;
        }

        public override void HoldItem(Item item, Player player)
        {
            if (!IsRecuperate(item))
                return;

            if (!player.channel)
                return;

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                thorium.TryFind("Mortality", out ModBuff mortality))
            {
                // refresh debuff while channeling
                player.AddBuff(mortality.Type, 2);
            }
        }

        private bool IsRecuperate(Item item)
        {
            return ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                   thorium.TryFind("Recuperate", out ModItem recuperate) &&
                   item.type == recuperate.Type;
        }
    }
}
