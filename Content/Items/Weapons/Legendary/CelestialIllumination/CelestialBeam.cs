using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Luminance.Core.Graphics;
using FargowiltasSouls.Assets.ExtraTextures;
using CalamityMod;
using InfernalEclipseAPI.Core.World;
using CalamityMod.Buffs.DamageOverTime;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.CelestialIllumination
{
    public class CelestialBeam : BaseLaserbeamProjectile
    {

        public override float Lifetime => 180f;
        public override float MaxScale => CalamityConditions.DownedProvidence.IsMet() ? 1.6f : 1f;
        private int Cooldown => CalamityConditions.DownedProvidence.IsMet() ? 8 : 6;
        public override float MaxLaserLength => 1200f;
        public override Texture2D LaserBeginTexture => ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Legendary/CelestialIllumination/CelestialBeam_Begin").Value;
        public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Legendary/CelestialIllumination/CelestialBeam_Middle").Value;
        public override Texture2D LaserEndTexture => ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Legendary/CelestialIllumination/CelestialBeam_End").Value;
        public override Color LightCastColor => Color.White;
        public override Color LaserOverlayColor => Color.WhiteSmoke * 0.85f;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<LegendaryMagic>();
            Projectile.friendly = true;
            Projectile.damage = InfernalDownedBossSystem.DownedSentinels() ? 400 : 300;
            Projectile.hostile = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Cooldown;
        }
        public override void AttachToSomething()
        {
            if (Projectile.owner != Main.myPlayer)
                return;
            float distance = CalamityConditions.DownedProvidence.IsMet() ? 75f : 60f;
            Player player = Main.player[Projectile.owner];
            if (player.dead || player.HeldItem.type != ModContent.ItemType<CelestialIllumination>())
            {
                Projectile.Kill();
                return;
            }
            Vector2 MousePos = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
            Vector2 spawnPosition = player.Center + MousePos * distance;
            Projectile.Center = spawnPosition;

            Vector2 direction = Main.MouseWorld - player.Center;
            if (direction != Vector2.Zero)
                Projectile.velocity = direction.SafeNormalize(Vector2.UnitY);
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