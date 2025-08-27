using System;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using InfernalEclipseAPI.Content.Projectiles;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    [ExtendsFromMod("RagnarokMod")]
    public class LamentationRework : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            try
            {
                if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarokMod))
                {
                    // Try to find the item type safely by internal name
                    if (ragnarokMod.TryFind("Lamentation", out ModItem calamityBell))
                    {
                        if (item.type == calamityBell.Type)
                        {
                            int projType = ModContent.ProjectileType<FriendlyBrimstoneGigablast>();
                            Projectile.NewProjectile(source, position, velocity, projType, damage, knockback, player.whoAmI);
                            return false;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
