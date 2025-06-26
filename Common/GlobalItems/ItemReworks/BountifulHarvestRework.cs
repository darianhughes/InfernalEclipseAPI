using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    [ExtendsFromMod("ThoriumMod")]
    public class BountifulHarvestRework : GlobalItem
    {
        private int useToggle = 0;

        public override bool InstancePerEntity => true;

        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<ThoriumMod.Items.HealerItems.BountifulHarvest>())
            {
                item.shootSpeed = 10f;
            }
        }

        public override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
                                   Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ModContent.ItemType<ThoriumMod.Items.HealerItems.BountifulHarvest>())
            {
                int projType = ModContent.ProjectileType<BountifulHarvestStinger>();

                Vector2 velocityWithSpeed = velocity.SafeNormalize(Vector2.UnitX) * item.shootSpeed;

                if (useToggle == 0)
                {
                    Projectile.NewProjectile(source, position, velocityWithSpeed, projType, damage, knockback, player.whoAmI);
                }
                else
                {
                    float angle = MathHelper.ToRadians(5f);
                    Vector2 vel1 = velocityWithSpeed.RotatedBy(angle);
                    Vector2 vel2 = velocityWithSpeed.RotatedBy(-angle);

                    Projectile.NewProjectile(source, position, vel1, projType, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, position, vel2, projType, damage, knockback, player.whoAmI);
                }

                SoundEngine.PlaySound(SoundID.Item17, position);

                useToggle = 1 - useToggle;

                return true;
            }

            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ModContent.ItemType<ThoriumMod.Items.HealerItems.BountifulHarvest>())
            {
                TooltipLine customTip = new TooltipLine(Mod, "RadiantStingerInfo",
                    "Fires piercing stingers in an alternating pattern");
                tooltips.Add(customTip);
            }
        }

    }
}
