using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    //WH
    public class GelatinTherapyRework : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            Mod mod;
            ModLoader.TryGetMod("CalamityBardHealer", out mod);
            if (mod == null) return false;

            int targetType = mod.Find<ModItem>("GelatinTherapy")?.Type ?? -1;
            return item.type == targetType;
        }

        public override void SetDefaults(Item item)
        {

            item.autoReuse = true;
        }

        public override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Fire a spread of 3 projectiles at varying angles/speeds
            int numberProjectiles = 3;

            for (int i = 0; i < numberProjectiles; i++)
            {
                // Random rotation within 20 degrees (~0.35 radians)
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(25f));

                // Random scale between 0.7x and 1.2x speed
                float scale = 0.7f + Main.rand.NextFloat() * 0.5f;
                perturbedSpeed *= scale;

                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
            }

            // Prevent vanilla projectile from firing
            return false;
        }
    }
}
