using Microsoft.Xna.Framework;
using Terraria.Audio;
using ThoriumMod;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ItemReworks.Weapons.Healer.Scythes
{
    [ExtendsFromMod("ThoriumMod")]
    public class AquaiteScytheProjectiles : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<ThoriumMod.Items.HealerItems.AquaiteScythe>())
            {
                item.shootSpeed = 10f;
            }
        }

        public override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ModContent.ItemType<ThoriumMod.Items.HealerItems.AquaiteScythe>())
            {
                if (!ModLoader.TryGetMod("CalamityMod", out Mod calamity))
                    return false;

                int projType = calamity.Find<ModProjectile>("AquaSigilWaterdroplet").Type;

                Vector2 spawnPosition = player.Center;

                for (int i = 0; i < 5; i++)
                {
                    float randomSpeed = Main.rand.NextFloat(7.5f, 12.5f);

                    // 180 degree spread centered upward
                    float randomAngle = Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);

                    Vector2 baseDirection = -Vector2.UnitY;
                    Vector2 randomVelocity = baseDirection.RotatedBy(randomAngle) * randomSpeed;

                    Projectile proj = Projectile.NewProjectileDirect(
                        source,
                        spawnPosition,
                        randomVelocity,
                        projType,
                        item.damage,
                        knockback,
                        player.whoAmI
                    );

                    proj.damage = item.damage;
                    proj.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
                    proj.friendly = true;
                    proj.ai[1] = 7777f;
                    proj.penetrate = 2;
                    proj.usesLocalNPCImmunity = true;
                    proj.localNPCHitCooldown = 40;
                    proj.usesIDStaticNPCImmunity = false;
                }

                SoundEngine.PlaySound(SoundID.Item21, player.Center);

                return true;
            }

            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }

    [ExtendsFromMod("ThoriumMod")]
    public class AquaSigilWaterdropletGlobal : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!ModLoader.TryGetMod("CalamityMod", out Mod calamity))
                return;

            int dropletType = calamity.Find<ModProjectile>("AquaSigilWaterdroplet").Type;

            if (projectile.type == dropletType && projectile.ai[1] == 7777f)
            {
                int riptideType = calamity.Find<ModBuff>("RiptideDebuff")?.Type ?? -1;

                if (riptideType != -1)
                {
                    target.AddBuff(riptideType, 120);
                }
            }
        }
    }
}
