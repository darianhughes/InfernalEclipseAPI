using CalamityMod.Sounds;
using InfernalEclipseAPI.Common.GlobalProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;

namespace InfernalEclipseAPI.Content.Items.Weapons.Donor.The454CasullandTheJackal
{
    public class CasullandJackalProj : ModProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 24;
            Projectile.height = 24;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.manualDirectionChange = true;
        }

        public Player Player => Main.player[Projectile.owner];

        public bool IsRightClick => Projectile.ai[0] == 1f;

        public ref float ShotTime => ref Projectile.ai[1];

        public bool StillChanneling
        {
            get
            {
                if (Main.myPlayer != Projectile.owner)
                    return true;

                return IsRightClick ? Main.mouseRight : Main.mouseLeft;
            }
        }

        public ref float FrontRecoil => ref Projectile.localAI[0];
        public ref float BackRecoil => ref Projectile.localAI[1];

        public override void AI()
        {
            base.AI();

            if (Player.CCed || Player.dead || Player.HeldItem.type != ModContent.ItemType<The454CasullandTheJackal>())
            {
                Projectile.Kill();
                return;
            }

            int timePerShot = (int)(Player.GetAttackSpeed(DamageClass.Ranged) * Player.HeldItem.useTime * (IsRightClick ? 0.25f : 1f));

            if (ShotTime % timePerShot == 0 && Main.myPlayer == Projectile.owner)
            {
                Projectile.velocity = Player.MountedCenter.DirectionTo(Main.MouseWorld).SafeNormalize(Vector2.Zero).RotatedByRandom(0.01f) * 5f;
                Projectile.netUpdate = true;
            }

            Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
            var spriteDir = Projectile.direction * (int)Player.gravDir;
            Projectile.spriteDirection = spriteDir;
            Projectile.rotation = Projectile.velocity.ToRotation();
            var rotationDir = (Projectile.velocity - Vector2.UnitY * Player.gravDir).SafeNormalize(Vector2.Zero);
            Player.itemRotation = (float)Math.Atan2(rotationDir.Y * Projectile.direction, rotationDir.X * Projectile.direction);

            float handRotation = Projectile.rotation - MathHelper.PiOver2;
            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.ThreeQuarters, handRotation);
            Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, handRotation);

            var handPosition = Player.GetFrontHandPosition(Player.CompositeArmStretchAmount.ThreeQuarters, handRotation);
            Projectile.Center = handPosition - Projectile.velocity;

            FrontRecoil *= 0.66f;
            BackRecoil *= 0.66f;

            bool dryFire = false;

            if (StillChanneling && ShotTime % timePerShot == 0)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.velocity = Player.MountedCenter.DirectionTo(Main.MouseWorld).SafeNormalize(Vector2.Zero).RotatedByRandom(0.01f) * 5f;
                    Projectile.netUpdate = true;
                }

                Vector2 gunEndOffset = new Vector2(34, -4 * Projectile.direction).RotatedBy(Projectile.rotation) * Projectile.scale;

                bool useRightGun = IsRightClick;
                Vector2 gunEnd = gunEndOffset + (useRightGun ? Projectile.Center + new Vector2(10, 0).RotatedBy(Projectile.rotation) * Projectile.scale : Projectile.Center);

                bool successfullyPickedAmmo = false;
                int ammoProj = 0;
                float ammoSpeed = 0f;
                int ammoDamage = 0;
                float ammoKnockback = 0f;
                int usedAmmoItemId = 0;

                if (Main.myPlayer == Projectile.owner)
                {
                    successfullyPickedAmmo = Player.PickAmmo(Player.HeldItem, out ammoProj, out ammoSpeed, out ammoDamage, out ammoKnockback, out usedAmmoItemId);
                }
                else
                {
                    successfullyPickedAmmo = Player.HasAmmo(Player.HeldItem);
                }

                if (ammoProj == ProjectileID.Bullet)
                    ammoProj = (IsRightClick ? ProjectileID.MeteorShot : ProjectileID.BulletHighVelocity);

                dryFire = !successfullyPickedAmmo;

                float volume = 0.2f;
                SoundEngine.PlaySound(useRightGun ? SoundID.Item41 : CommonCalamitySounds.LargeWeaponFireSound with { MaxInstances = 0, Volume = useRightGun ? volume : 0.5f, PitchVariance = 0.1f }, Projectile.Center);

                if (successfullyPickedAmmo)
                {
                    float pitch = IsRightClick ? -0.2f : 0.2f;
                    SoundEngine.PlaySound(useRightGun ? SoundID.Item41 : CommonCalamitySounds.LargeWeaponFireSound with { MaxInstances = 0, Volume = useRightGun ? 0.5f : 0.125f, Pitch = pitch, PitchVariance = 0.01f }, Projectile.Center);

                    for (int i = 0; i < 8; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(
                            gunEnd + Main.rand.NextVector2Circular(8, 8),
                            DustID.BatScepter,
                            Player.velocity + Projectile.velocity * i / 8f,
                            0,
                            Scale: Main.rand.NextFloat(1f, 2f)
                        );
                        dust.noGravity = true;
                        dust.alpha = 200;
                    }

                    if (Main.myPlayer == Projectile.owner)
                    {
                        Vector2 boltVelocity =
                            Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedByRandom(0.04f) *
                            ammoSpeed +
                            Player.velocity * 0.1f;

                        Projectile bolt = Projectile.NewProjectileDirect(
                            Projectile.GetSource_FromThis(),
                            gunEnd,
                            boltVelocity,
                            ammoProj,
                            ammoDamage,
                            ammoKnockback,
                            Player.whoAmI
                        );

                        bolt.scale *= Main.rand.NextFloat(0.7f, 1.2f);
                        bolt.GetGlobalProjectile<InfernalGlobalProjectile>().casulJackalBullet = true;
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(
                            gunEnd + Main.rand.NextVector2Circular(5, 5),
                            DustID.Smoke,
                            Player.velocity.RotatedByRandom(0.3f) * 0.2f + Projectile.velocity * i / 8f,
                            0,
                            Scale: Main.rand.NextFloat()
                        );
                        dust.noGravity = true;

                        if (Main.rand.NextBool(3))
                        {
                            Dust fireDust = Dust.NewDustPerfect(
                                gunEnd + Main.rand.NextVector2Circular(8, 8),
                                DustID.BatScepter,
                                Player.velocity + Projectile.velocity * i / 8f,
                                0,
                                Scale: Main.rand.NextFloat(1f, 2f)
                            );
                            fireDust.noGravity = true;
                            fireDust.alpha = 200;
                        }
                    }
                }

                float downAmount = dryFire ? 0.05f : 0.35f;
                if (Player.direction < 0 ? !IsRightClick : IsRightClick)
                    BackRecoil -= downAmount;
                else
                    FrontRecoil -= downAmount;
            }

            if (StillChanneling && ShotTime % timePerShot < 5 && ShotTime % timePerShot > 0)
            {
                float amount = dryFire ? 0.1f : ShotTime % timePerShot / 6f;

                if (Player.direction < 0 ? !IsRightClick : IsRightClick)
                {
                    BackRecoil += amount;
                }
                else
                {
                    FrontRecoil += amount;
                }
            }

            Player.heldProj = Projectile.whoAmI;
            Player.ChangeDir(Projectile.direction);

            if (StillChanneling)
            {
                Player.SetDummyItemTime(10);
                Projectile.timeLeft = 10;
                ShotTime++;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            int front = Player.direction < 0 ? 1 : 0;
            int back = 1 - front;

            Vector2 backOffset = new Vector2(0, Projectile.spriteDirection).RotatedBy(Projectile.rotation) * Projectile.scale;
            Vector2 frontOffset = new Vector2(0, Projectile.spriteDirection).RotatedBy(Projectile.rotation) * Projectile.scale;

            Vector2 backSquish = new Vector2(1f - MathF.Pow(BackRecoil, 2) * 0.5f, 1f);
            Vector2 frontSquish = new Vector2(1f - MathF.Pow(FrontRecoil, 2) * 0.5f, 1f);

            Vector2 backPosition = backOffset + Player.GetBackHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            Vector2 frontPosition = frontOffset + Projectile.Center;

            Rectangle backFrame = texture.Frame(1, 2, 0, back);
            //Rectangle backFrameGlow = texture.Frame(2, 2, 1, back);
            Vector2 gunOrigin = new Vector2(backFrame.Width / 2 - 18, backFrame.Height / 2 + 7 * Projectile.spriteDirection);
            var spriteEffects = Projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : 0;
            float backRotation = Projectile.rotation - BackRecoil * Projectile.spriteDirection;

            Rectangle frontFrame = texture.Frame(1, 2, 0, front);
            //Rectangle frontFrameGlow = texture.Frame(2, 2, 1, front);
            float frontRotation = Projectile.rotation - FrontRecoil * Projectile.spriteDirection;

            // so i made the sprite wrong so they are backwards... lmao

            // Back
            Main.EntitySpriteDraw(texture, backPosition - Main.screenPosition, frontFrame, lightColor, backRotation, gunOrigin, backSquish * Projectile.scale / 1.5f, spriteEffects, 0);
            //Main.EntitySpriteDraw(texture, backPosition - Main.screenPosition, backFrameGlow, Color.White with { A = 200 }, backRotation, gunOrigin, backSquish * Projectile.scale, spriteEffects, 0);

            // Front
            Main.EntitySpriteDraw(texture, frontPosition - Main.screenPosition, backFrame, lightColor, frontRotation, gunOrigin, frontSquish * Projectile.scale / 1.5f, spriteEffects, 0);
            //Main.EntitySpriteDraw(texture, frontPosition - Main.screenPosition, frontFrameGlow, Color.White with { A = 200 }, frontRotation, gunOrigin, frontSquish * Projectile.scale, spriteEffects, 0);

            return false;
        }
    }
}
