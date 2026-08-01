using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace InfernalEclipseAPI.Content.Items.Weapons.Donor.BlixerCore
{
    public class BlixerCannonPro : ModProjectile
    {
        private Vector2[] trailPos = new Vector2[8];
        private bool runOnce = true;
        private float acceleration = 0.3f;
        private int counter;
        private bool AllowTrailToEnd;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = 1;
            Projectile.width = 12;
            Projectile.height = 14;
            Projectile.timeLeft = 1060;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
        }

        public override bool ShouldUpdatePosition() => false;

        public override bool PreAI()
        {
            if (Projectile.ai[0] == -1f)
            {
                Projectile.ai[0]--;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (runOnce)
            {
                acceleration = 0.4f;

                // Move slightly forward immediately after spawning.
                Projectile.position += Projectile.velocity * 2f;

                SoundEngine.PlaySound(SoundID.Item11 with { Volume = 0.8f, Pitch = 0.1f }, Projectile.Center);

                for (int i = 0; i < trailPos.Length; i++)
                    trailPos[i] = Vector2.Zero;

                runOnce = false;
            }

            CheckPosition();

            if ((counter > 10 || Projectile.ai[0] == -3f) && !AllowTrailToEnd)
            {
                Vector2 attemptedMovement = Projectile.velocity * acceleration;

                Vector2 collisionMovement = Collision.TileCollision(Projectile.Center - new Vector2(10f), attemptedMovement, 20, 20, true, true);
            }

            Projectile.position += Projectile.velocity * acceleration;

            if (Projectile.ai[0] != -3f)
            {
                counter++;
                acceleration += 0.15f;
            }
            else
            {
                acceleration += 0.12f;

                if (Projectile.timeLeft > 1000)
                    Projectile.timeLeft -= 2;
            }

            return false;
        }

        private void CheckPosition()
        {
            int matchingPositions = 0;
            Vector2 center = Projectile.Center;

            for (int i = 0; i < trailPos.Length; i++)
            {
                if (center == trailPos[i])
                    matchingPositions++;
            }

            // Once every trail point reaches the stopped projectile,
            // the residual trail has finished retracting.
            if (matchingPositions >= trailPos.Length)
                Projectile.Kill();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.tileCollide);
            writer.Write(Projectile.friendly);
            writer.Write(AllowTrailToEnd);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.tileCollide = reader.ReadBoolean();
            Projectile.friendly = reader.ReadBoolean();
            AllowTrailToEnd = reader.ReadBoolean();
        }
    }
}
