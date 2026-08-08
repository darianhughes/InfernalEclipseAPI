using CalamityMod;
using CalamityMod.NPCs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;

namespace InfernalEclipseAPI.Content.Items.Weapons.Melee.Destinary
{
    [PierceResistException]
    public class DestinaryProjectile : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public const float ZeroChargeDamageRatio = 0.36f;
        public const float ToothDamageRatio = 0.3f;
        public const int ToothShootRate = 5;
        public const int ChargeUpTime = 150;
        public ref float Time => ref Projectile.ai[0];

        public ref float ToothDamage => ref Projectile.ai[1];
        public float ChargeUpPower => Clamp((float)Math.Pow(Time / ChargeUpTime, 1.6D), 0f, 1f);

        public override void SetDefaults()
        {
            Projectile.width = 198;
            Projectile.height = 102;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.noEnchantmentVisuals = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Texture2D glowmaskTexture = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Melee/Destinary/DestinaryGlowmask").Value; 
            Rectangle glowmaskRectangle = glowmaskTexture.Frame(1, 6, 0, Projectile.frame);
            Vector2 origin = texture.Size() * 0.5f;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            SpriteEffects direction = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(texture, drawPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, direction, 0);
            Main.EntitySpriteDraw(glowmaskTexture, drawPosition, glowmaskRectangle, Color.White, Projectile.rotation, origin, Projectile.scale, direction, 0);
            return false;
        }

        public override void AI()
        {
            Projectile.damage = Owner.HeldItem is null ? 0 : Owner.GetWeaponDamage(Owner.HeldItem);
            DetermineDamage();

            PlayChainsawSounds();

            Vector2 playerRotatedPosition = Owner.RotatedRelativePoint(Owner.MountedCenter);
            if (Main.myPlayer == Projectile.owner)
            {
                if ((!Owner.CantUseHoldout() && Projectile.ai[2] == 1) || (Projectile.ai[2] == 0 && Owner.Calamity().mouseRight && Owner.active && !Owner.dead))
                    HandleChannelMovement(playerRotatedPosition);
                else
                    Projectile.Kill();
            }

            DetermineVisuals(playerRotatedPosition);
            ManipulatePlayerValues();
            EmitPrettyDust();

            if (Time % ToothShootRate == ToothShootRate - 1f)
                ReleasePrismTeeth();

            Projectile.timeLeft = 2;

            Time++;
        }

        public void PlayChainsawSounds()
        {
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item22, Projectile.Center);
                Projectile.soundDelay = (int)MathHelper.Lerp(30f, 12f, ChargeUpPower);
            }
        }

        public void DetermineDamage()
        {
            // Set the initial tooth damage the instant the projectile is created.
            if (Main.myPlayer == Projectile.owner && ToothDamage == 0f)
            {
                ToothDamage = ToothDamageRatio * Projectile.damage;
                Projectile.netUpdate = true;
            }

            if (ToothDamage != 0f)
            {
                float fullMult = ToothDamageRatio;
                float zeroMult = ZeroChargeDamageRatio * ToothDamageRatio;
                ToothDamage = (int)MathHelper.SmoothStep(Projectile.damage * zeroMult, Projectile.damage * fullMult, ChargeUpPower);
            }
        }

        public void DetermineVisuals(Vector2 playerRotatedPosition)
        {
            float directionAngle = Projectile.velocity.ToRotation();
            Projectile.rotation = directionAngle;

            int oldDirection = Projectile.spriteDirection;
            if (oldDirection == -1)
                Projectile.rotation += MathHelper.Pi;

            Projectile.direction = Projectile.spriteDirection = (Math.Cos(directionAngle) > 0).ToDirectionInt();

            if (Projectile.spriteDirection != oldDirection)
                Projectile.rotation -= MathHelper.Pi;

            float forwardOffset = 30f;
            float rightOffset = 16f;
            float downOffset = 10f;

            Vector2 aim = directionAngle.ToRotationVector2();

            Vector2 right = aim.RotatedBy(MathHelper.PiOver2);

            Vector2 finalOffset = aim * (forwardOffset + rightOffset) + new Vector2(0f, downOffset);

            Projectile.position = playerRotatedPosition - Projectile.Size * 0.5f + finalOffset;

            Projectile.position += Main.rand.NextVector2Circular(1.4f, 1.4f);

            Projectile.frameCounter += (int)MathHelper.SmoothStep(12f, 33f, ChargeUpPower);
            if (Projectile.frameCounter >= 32)
            {
                Projectile.frame = (Projectile.frame + 1) % 6;
                Projectile.frameCounter = 0;
            }
        }

        public void HandleChannelMovement(Vector2 playerRotatedPosition)
        {
            Vector2 idealAimDirection = (Main.MouseWorld - playerRotatedPosition).SafeNormalize(Vector2.UnitX * Owner.direction);

            float angularAimVelocity = 0.15f;
            float directionAngularDisparity = Projectile.velocity.AngleBetween(idealAimDirection) / MathHelper.Pi;

            angularAimVelocity += MathHelper.Lerp(0f, 0.25f, Utils.GetLerpValue(0.28f, 0.08f, directionAngularDisparity, true));

            if (directionAngularDisparity > 0.02f)
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, idealAimDirection, angularAimVelocity);
            else
                Projectile.velocity = idealAimDirection;

            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX * Owner.direction);
        }

        public void ManipulatePlayerValues()
        {
            Owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.ChangeDir(Projectile.direction);
        }

        public void EmitPrettyDust()
        {
            if (Main.dedServ)
                return;

            for (int i = 0; i < 2; i++)
            {
                Vector2 spawnPosition = Projectile.Center + Projectile.velocity * 35f;

                spawnPosition += Main.rand.NextVector2CircularEdge(9f, 35f).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver2);

                Dust rainbowSpark = Dust.NewDustPerfect(spawnPosition, DustID.AncientLight);
                rainbowSpark.velocity = Projectile.velocity * 3f + Main.rand.NextVector2CircularEdge(1.5f, 1.5f);
                rainbowSpark.noGravity = true;
                rainbowSpark.color = Main.hslToRgb((Time / 40f + Main.rand.NextFloat(-0.1f, 0.1f)) % 1f, 0.95f, 0.8f);
                rainbowSpark.scale = Main.rand.NextFloat(0.9f, 1.25f);
            }
        }

        public void ReleasePrismTeeth()
        {
            SoundEngine.PlaySound(SoundID.Item101, Projectile.Center);

            if (Main.myPlayer != Projectile.owner)
                return;

            float shootReach = MathHelper.SmoothStep(Projectile.width * 1.8f, Projectile.width * 5.3f + 16f, ChargeUpPower);

            shootReach *= Owner.HeldItem.shootSpeed;

            float distanceFromMouse = Owner.Distance(Main.MouseWorld);

            if (distanceFromMouse < shootReach)
            {
                if (distanceFromMouse > 40f)
                    shootReach = distanceFromMouse + 32f;
                else
                    shootReach = 72f;
            }

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Owner.Center, Projectile.velocity, ModContent.ProjectileType<ExoDisk>(), (int)ToothDamage, 0f, Projectile.owner, shootReach, Projectile.whoAmI, Projectile.ai[2]);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = 0f;
            float width = Projectile.scale * 36f;
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + Projectile.velocity * 70f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, width, ref _);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 500);
            SoundStyle fire = new("CalamityMod/Sounds/Item/WulfrumKnifeTileHit", 2);
            SoundEngine.PlaySound(fire with { Volume = 0.7f, Pitch = -0.1f }, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {
                Particle spark2 = new SparkParticle(target.Center, ((Owner.Center - Owner.Calamity().mouseWorld).SafeNormalize(Vector2.UnitY) * -40).RotatedByRandom(0.55) * Main.rand.NextFloat(0.3f, 1f), false, 20, Main.rand.NextFloat(0.3f, 1.2f), Main.hslToRgb((Time / 40f + Main.rand.NextFloat(-0.1f, 0.1f)) % 1f, 0.95f, 0.8f));
                GeneralParticleHandler.SpawnParticle(spark2);
            }
            for (int i = 0; i < 3; i++)
            {
                Particle blastRing = new CustomPulse(target.Center, Vector2.Zero, Main.hslToRgb((Time / 40f + Main.rand.NextFloat(-0.1f, 0.1f)) % 1f, 0.95f, 0.8f), "CalamityMod/Particles/BloomCircle", Vector2.One, Main.rand.NextFloat(-10, 10), 0.4f, 0.9f * Main.rand.NextFloat(0.9f, 1.1f), 12, true);
                GeneralParticleHandler.SpawnParticle(blastRing);
            }
        }
    }
}
