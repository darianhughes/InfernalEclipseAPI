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

        private static int? _gelatinTherapyType;

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            Mod mod;
            ModLoader.TryGetMod("CalamityBardHealer", out mod);
            if (mod == null && !ModLoader.TryGetMod("Catalyst", out _)) return false;

            _gelatinTherapyType = mod.Find<ModItem>("GelatinTherapy").Type;

            return item.type == _gelatinTherapyType;
        }

        public override void SetDefaults(Item item)
        {
            if (_gelatinTherapyType != null && item.type == _gelatinTherapyType)
            {
                item.autoReuse = true;
            }
        }

        public override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (_gelatinTherapyType != null && item.type == _gelatinTherapyType)
            {
                // Fire a spread of 3 projectiles at varying angles/speeds
                int numberProjectiles = 3;

                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(25f));
                    float scale = 0.7f + Main.rand.NextFloat() * 0.5f;
                    perturbedSpeed *= scale;

                    Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                }

                // Prevent vanilla projectile from firing
                return false;
            }

            // Let vanilla behavior happen otherwise
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
