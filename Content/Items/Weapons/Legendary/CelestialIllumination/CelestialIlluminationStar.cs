using CalamityMod.Buffs.DamageOverTime;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.CelestialIllumination
{
    public class CelestialIlluminationStar : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 144;
            Projectile.height = 144;
            Projectile.alpha = 60;
            Projectile.penetrate = 20;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.DamageType = ModContent.GetInstance<LegendaryMagic>();
            Projectile.ignoreWater = true;
            Projectile.damage = 400;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.2f, 0.6f, 1.3f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var CelestialPlayer = Main.player[Projectile.owner].GetModPlayer<CelestialIlluminationPlayer>();
            if (CelestialPlayer.CelestialStarCharge < 20)
            {
                CelestialPlayer.CelestialStarCharge++;
            }
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 180);
        }
    }
}