using InfernalEclipseAPI.Core.Systems;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Earth;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    public class IlluminantAxeRework : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<SOTS.Items.Earth.Glowmoth.IlluminantAxe>();
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position + velocity, 0.2f * Utils.RotatedBy(velocity, (double)MathHelper.ToRadians(Utils.NextFloat(Main.rand, -12f, 12f)), new Vector2()), ModContent.ProjectileType<IlluminantBolt>(), (int)(damage * 0.60000002384185791), knockback * 0.2f, Main.myPlayer, Utils.NextFloat(Main.rand, 180f, 360f), 0.0f, 0.0f);
            Projectile.NewProjectile(source, position + velocity, 0.2f * Utils.RotatedBy(velocity, (double)MathHelper.ToRadians(Utils.NextFloat(Main.rand, -12f, 12f)), new Vector2()), ModContent.ProjectileType<IlluminantBolt>(), (int)(damage * 0.60000002384185791), knockback * 0.2f, Main.myPlayer, Utils.NextFloat(Main.rand, 180f, 360f), 0.0f, 0.0f);

            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
