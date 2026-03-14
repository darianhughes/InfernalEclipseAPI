using CalamityMod;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.CelestialIllumination
{
    public class CelestialIlluminationBeam : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public override string Texture => "InfernalEclipseAPI/Content/Items/Weapons/Legendary/CelestialIllumination/CelestialIllumination";
        public override void SetDefaults()
        {
            Projectile.DamageType = LegendaryMagic.Instance;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }
        public override void AI()
        {
            if (Projectile.owner < 0 || Projectile.owner >= byte.MaxValue || !Owner.active || Owner.dead || !Owner.Calamity().mouseRight || Owner.noItems || Owner.CCed)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = Owner.Center + Vector2.Normalize(Main.MouseWorld - Owner.Center).SafeNormalize(Vector2.UnitX) * 40f;
        }
    }
}