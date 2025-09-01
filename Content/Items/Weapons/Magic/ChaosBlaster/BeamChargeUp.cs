using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Core.DamageClasses.MythicClass;
using Luminance.Assets;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoxusBoss.Assets;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Weapons.Magic.ChaosBlaster
{
    [ExtendsFromMod("NoxusBoss")]
    public class BeamChargeUp : ModProjectile
    {
        private SlotId soundSlot;

        public Player Owner => Main.player[Projectile.owner];

        public ref float Time => ref Projectile.ai[0];
        public static int Lifetime => Utilities.SecondsToFrames(0.75f);
        public static float MaxGleamScaleFactor => 0.45f;
        public static Color LensFlareColor => new Color(byte.MaxValue, 174, 147);
        public static float MouseAimSpeedInterpolant => 0.15f;

        public override string Texture => "Terraria/Images/Projectile_" + 633.ToString();

        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = Lifetime;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.DamageType = MythicMagic.Instance;
            Projectile.ArmorPenetration = 500;
        }

        public override void AI()
        {
            // Despawn if owner invalid
            if (Projectile.owner < 0 || Projectile.owner >= byte.MaxValue || !Owner.active || Owner.dead)
            {
                Projectile.Kill();
                return;
            }

            // Despawn if player stopped channeling / can’t use item / is CCed
            if (!Owner.channel || Owner.noItems || Owner.CCed)
            {
                Projectile.Kill();
                return;
            }

            // Charge-up sound on tick 2 if near local player
            if (Time == 2 && Main.LocalPlayer.WithinRange(Projectile.Center, 3000f))
            {
                var charge = new SoundStyle("InfernalEclipseAPI/Assets/Sounds/SolynStarBeamChargeUp")
                {
                    Volume = 1.5f,
                    MaxInstances = 1
                };
                soundSlot = SoundEngine.PlaySound(charge, Projectile.Center);
            }

            // Anchor the projectile slightly in front of the player toward the mouse
            Projectile.Center = Owner.Center + Vector2.Normalize(Main.MouseWorld - Owner.Center) * 100f;

            // Scale & spin over the charge lifetime
            float x = Time / Lifetime;
            Projectile.scale = Utilities.InverseLerp(0f, 0.65f, x).Squared()
                             * Utilities.InverseLerp(1f, 0.9f, x);

            Projectile.rotation = MathF.Pow(MathHelper.SmoothStep(0f, 1f, x), 0.3f) * MathHelper.TwoPi * 2f;

            AimTowardsMouse();

            // Pretty dust ring that collapses inward
            Dust d = Dust.NewDustPerfect(
                Projectile.Center + Main.rand.NextVector2Circular(200f, 200f),
                264
            );
            d.velocity = d.position.SafeDirectionTo(Projectile.Center) * Main.rand.NextFloat(10f);
            d.noGravity = true;
            d.noLight = true;
            d.scale *= Main.rand.NextFloat(1f, 1.6f);
            d.color = Main.rand.NextBool() ? Color.Wheat : Color.HotPink;

            Time++;

            // When lifetime ends, spawn the actual beam
            if (Projectile.timeLeft <= 1)
                SpawnBeam();
        }

        public void AimTowardsMouse()
        {
            if (Main.myPlayer != Projectile.owner)
                return;

            Vector2 oldVel = Projectile.velocity;
            float targetRot = Projectile.AngleTo(Main.MouseWorld);

            Projectile.velocity = Utils.ToRotationVector2(
                Utils.AngleLerp(Projectile.velocity.ToRotation(), targetRot, MouseAimSpeedInterpolant * 3f)
            );

            if (Projectile.velocity != oldVel)
            {
                Projectile.netUpdate = true;
                Projectile.netSpam = 0;
            }
        }

        private void SpawnBeam()
        {
            if (Main.myPlayer != Projectile.owner)
                return;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                Projectile.velocity,
                ModContent.ProjectileType<ChaosBlasterBeam>(),
                Projectile.damage,
                Projectile.knockBack,
                Projectile.owner
            );
        }

        public override void OnKill(int timeLeft)
        {
            if (SoundEngine.TryGetActiveSound(soundSlot, out var active))
                active.Stop();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float gleamScale = Projectile.scale * BeamChargeUp.MaxGleamScaleFactor;
            Vector2 centerOnScreen = Projectile.Center - Main.screenPosition;

            Texture2D bloomSmall = GennedAssets.Textures.GreyscaleTextures.BloomCircleSmall.Value;
            Texture2D shineFlare = MiscTexturesRegistry.ShineFlareTexture.Value;

            // 3 rotating shine flares
            for (int i = 0; i < 3; i++)
            {
                Color c = BeamChargeUp.LensFlareColor; c.A = 0;
                Main.spriteBatch.Draw(
                    shineFlare,
                    centerOnScreen,
                    null,
                    c,
                    Projectile.rotation,
                    shineFlare.Size() * 0.5f,
                    gleamScale * 2f,
                    SpriteEffects.None,
                    0f
                );
            }

            // 2 small blooms
            for (int i = 0; i < 2; i++)
            {
                Color c = BeamChargeUp.LensFlareColor; c.A = 0;
                Main.spriteBatch.Draw(
                    bloomSmall,
                    centerOnScreen,
                    null,
                    c,
                    0f,
                    bloomSmall.Size() * 0.5f,
                    gleamScale * 2f,
                    SpriteEffects.None,
                    0f
                );
            }

            return false; // handled drawing
        }
    }
}
