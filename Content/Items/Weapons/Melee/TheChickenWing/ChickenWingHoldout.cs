using CalamityMod;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace InfernalEclipseAPI.Content.Items.Weapons.Melee.TheChickenWing
{
    public class ChickenWingHoldout : BaseCustomUseStyleProjectile
    {
        public override string Texture => "InfernalEclipseAPI/Content/Items/Weapons/Melee/TheChickenWing/TheChickenWing";
        public override int AssignedItemID => ModContent.ItemType<TheChickenWing>();

        public override Vector2 HitboxSize => new Vector2(250f, 250f);
        public override float HitboxOutset => 60f;
        public override float HitboxRotationOffset => ToRadians(-45f);
        public override Vector2 SpriteOrigin => new Vector2(0f, 150f);

        // Swing state flags
        private bool doSwing = true;
        private bool postSwing;
        private bool playedSound;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void WhenSpawned()
        {
            var mp = Owner.GetModPlayer<ChickenWingPlayer>();
            Projectile.ai[1] = mp.swingParity == 0 ? 1f : -1f;
            FlipAsSword = Owner.direction == -1;
        }
        public override void UseStyle()
        {
            Projectile.Center = Owner.MountedCenter;
            Projectile.velocity = Vector2.Zero;

            float useAnimMax = Owner.itemAnimationMax;
            AnimationProgress = Animation % useAnimMax;
            float progress = AnimationProgress / useAnimMax;

            // Determine direction based on ai[1]
            //FlipAsSword = Projectile.ai[1] < 0;

            // Reset swing at the beginning of a new cycle
            if (progress < 0.33f && !doSwing)
            {
                doSwing = true;
                postSwing = false;
                playedSound = false;
                Projectile.ai[1] = -Projectile.ai[1];

                for (int i = 0; i < Main.maxNPCs; i++)
                    Projectile.localNPCImmunity[i] = 0;

                Projectile.numHits = 0;
                CanHit = false;
            }

            // Basher-style swing rotation
            float eased = CalamityUtils.ExpInOutEasing(progress, 1);
            float startRot = 150f * Projectile.ai[1] * Owner.direction;
            float endRot = 120f * -Projectile.ai[1] * Owner.direction;
            RotationOffset = Lerp(RotationOffset, ToRadians(Lerp(startRot, endRot, eased)), 0.2f);

            // Smooth rotation towards mouse
            Projectile.rotation = Utils.AngleLerp(
                Projectile.rotation,
                Owner.AngleTo(Main.MouseWorld) + ToRadians(45f),
                0.15f
            );

            // Activate hitboxes during main swing window
            if (progress > 0.25f && progress < 0.75f)
            {
                CanHit = true;
                postSwing = true;
            }
            else
            {
                CanHit = false;
            }

            bool swingingDown = Projectile.ai[1] * Owner.direction > 0;

            if (progress >= 0.5f && !playedSound)
            {
                playedSound = true;
                SoundEngine.PlaySound(SoundID.Item95, Projectile.position);
            }

            // Mark swing as finished
            if (progress >= 0.75f)
            {
                doSwing = false;
            }

            ArmRotationOffset = ToRadians(-140f);
            ArmRotationOffsetBack = ToRadians(-140f);
        }

        public override void ResetStyle()
        {
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact);

            target.AddBuff(BuffID.Oiled, 600);

            if (hit.Damage >= target.life)
            {
                Main.player[Projectile.owner].AddBuff(BuffID.HeartyMeal, 60 * 7);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact);

            target.AddBuff(BuffID.Oiled, 600);

            if (info.Damage >= target.statLife)
            {
                Main.player[Projectile.owner].AddBuff(BuffID.HeartyMeal, 60 * 7);
            }
        }
    }

    public class ChickenWingPlayer : ModPlayer
    {
        public int swingParity;

        public override void ResetEffects()
        {
        }
    }
}
