using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThoriumMod;
using Terraria.Audio;
using CalamityMod.Buffs.DamageOverTime;
using ThoriumMod.Projectiles.Bard;
namespace InfernalEclipseAPI.Content.Projectiles
{
    [ExtendsFromMod("ThoriumMod")]
    public class FriendlyBrimstoneGigablast : BardProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Boss/SCalBrimstoneGigablast";

        public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6; // 6-frame animation like SCal's
        }

        public override void SetBardDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 80;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance; // Or change if you're not using Thorium
        }

        public override void AI()
        {
            // Animate
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // 5 ticks per frame
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }


            // Rotation
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // Opacity fade in
            Projectile.Opacity = MathHelper.Clamp(1f - (Projectile.timeLeft - 60f) / 20f, 0f, 1f);

            // Lighting
            Lighting.AddLight(Projectile.Center, 0.9f * Projectile.Opacity, 0f, 0f);

            // Dust effect
            if (Main.rand.NextBool())
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, Main.rand.NextBool(3) ? 60 : 114);
                d.noGravity = true;
                d.velocity = Main.rand.NextVector2Circular(4f, 4f) * Main.rand.NextFloat(0.5f, 1.3f);
                d.scale = Main.rand.NextFloat(0.7f, 1.8f);
            }

            // Homing
            float inertia = 90f;
            float homeSpeed = 20f;
            float minDist = 40f;

            NPC target = FindClosestEnemy(800f);
            if (target != null && Projectile.Center.Distance(target.Center) > minDist)
            {
                Vector2 direction = SafeDirectionTo(Projectile.Center, target.Center, Vector2.UnitY);
                Projectile.velocity = (Projectile.velocity * (inertia - 1f) + direction * homeSpeed) / inertia;
            }

            // Slow down gradually like the original projectile
            Projectile.velocity *= 0.95f;
        }

        private Vector2 SafeDirectionTo(Vector2 from, Vector2 to, Vector2 fallback)
        {
            Vector2 direction = to - from;
            if (direction == Vector2.Zero)
                return fallback;
            return Vector2.Normalize(direction);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Pull texture directly from Calamity
            Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Boss/SCalBrimstoneGigablast").Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int drawStart = frameHeight * Projectile.frame;

            Color drawColor = Projectile.GetAlpha(lightColor);
            Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);

            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                new Rectangle(0, drawStart, texture.Width, frameHeight),
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            // Spawn explosion dust and play sound as before
            for (int i = 0; i < 14; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(12f, 12f);
                Dust d = Dust.NewDustPerfect(Projectile.Center + velocity, Main.rand.NextBool(3) ? 60 : 114);
                d.noGravity = true;
                d.velocity = velocity * Main.rand.NextFloat(0.5f, 1.3f);
                d.scale = Main.rand.NextFloat(0.75f, 1.3f);
            }

            SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/SCalSounds/BrimstoneFireblastImpact", (SoundType)0));

            // Spawn smaller projectiles in a circle
            int totalProjectiles = 14; // Change number as you want
            float radians = MathHelper.TwoPi / totalProjectiles;
            int smallProjectileType = ModContent.ProjectileType<FriendlyBrimstoneBarrage>();
            float speed = 8f;

            if (Projectile.owner == Main.myPlayer)
            {
                for (int k = 0; k < totalProjectiles; k++)
                {
                    Vector2 velocity = new Vector2(0, -speed).RotatedBy(radians * k);
                    int projIndex = Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        velocity,
                        smallProjectileType,
                        (int)(Projectile.damage * 0.25f),
                        0f,
                        Projectile.owner,
                        1f
                    );

                    // Make sure the projectile exists before setting rotation
                    if (Main.projectile.IndexInRange(projIndex))
                    {
                        Projectile spawned = Main.projectile[projIndex];
                        spawned.rotation = velocity.ToRotation();
                        spawned.ai[0] = 1; // <-- trying to set here
                    }
                }
            }
        }

        // Find the closest enemy for homing
        private NPC FindClosestEnemy(float maxDetectDistance)
        {
            NPC closestNPC = null;
            float closestDistance = maxDetectDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(this) && !npc.friendly)
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNPC = npc;
                    }
                }
            }

            return closestNPC;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
        }
    }
}
