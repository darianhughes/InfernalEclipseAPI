using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.Dusts;
using SOTS;
using SOTS.Helpers;

namespace InfernalEclipseAPI.Content.Projectiles
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class EvilGrowth : ModProjectile
    {
        public const int MaxTimeLeft = 150;

        private const int TrailLength = 10;
        private const float MaxBubbleSize = 420f;
        private const float MaxBubbleRadius = MaxBubbleSize / 2f + 4f;
        private const float MaxBubbleRadiusSquared = MaxBubbleRadius * MaxBubbleRadius;

        private bool runOnce = true;
        private bool runOnce2 = true;

        private readonly Vector2[] trailPos = new Vector2[TrailLength];

        private int stretchCounter;
        private float unwind = 720f;
        private float saveRotation;
        private float bubbleSize;

        public bool[] effected = new bool[Main.maxNPCs];

        private static Asset<Texture2D> GrowthTexture;
        private static Asset<Texture2D> AuraTexture;
        private static Asset<Texture2D> BorderTexture;

        public override string Texture => "SOTS/Projectiles/Evil/EvilGrowth";

        public override void Load()
        {
            GrowthTexture = ModContent.Request<Texture2D>("SOTS/Projectiles/Evil/EvilGrowth");
            AuraTexture = ModContent.Request<Texture2D>("SOTS/Gores/CircleAura");
            BorderTexture = ModContent.Request<Texture2D>("SOTS/Gores/CircleBorder");
        }

        public override void Unload()
        {
            GrowthTexture = null;
            AuraTexture = null;
            BorderTexture = null;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.timeLeft = MaxTimeLeft;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 0;
        }

        private void CataloguePos()
        {
            Vector2 previous = Projectile.Center - new Vector2(8f, -8f).RotatedBy(Projectile.rotation);

            for (int i = 0; i < TrailLength; i++)
            {
                Vector2 old = trailPos[i];
                trailPos[i] = previous;
                previous = old;
            }
        }

        public override bool PreAI()
        {
            if (runOnce)
            {
                Array.Clear(trailPos);
                runOnce = false;
            }

            CataloguePos();
            return true;
        }

        public override void AI()
        {
            if (runOnce2)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + PiOver4;
                saveRotation = Projectile.rotation;
                Projectile.spriteDirection = 1;
            }

            bool stopped = Projectile.velocity.LengthSquared() <= 0.01f;

            if (stopped)
            {
                if (runOnce2)
                {
                    Projectile.ai[0] = 0f;
                    runOnce2 = false;
                    Projectile.rotation = 0f;
                    Projectile.scale = 0f;
                    Projectile.netUpdate = true;

                    SpawnStartDust();
                    SOTSUtils.PlaySound(SoundID.Item116, (int)Projectile.Center.X, (int)Projectile.Center.Y, 2.3f, -0.5f);
                }

                UpdateGrowthExpansion();
            }

            UpdateAffectedNPCs();
        }

        private void UpdateGrowthExpansion()
        {
            if (Projectile.scale < 1f)
            {
                Projectile.scale += 0.05f;
                Projectile.scale *= 1.05f;
                Projectile.rotation = saveRotation + unwind * (1f - Projectile.scale);
            }
            else
            {
                Projectile.rotation = saveRotation;
                Projectile.scale = 1f;
            }

            if (stretchCounter < 120)
                stretchCounter += 5;
            else if (Projectile.ai[0] < 30f)
                Projectile.ai[0] += 3f;

            bubbleSize = MaxBubbleSize * Sin(ToRadians(stretchCounter - Projectile.ai[0] + 10f));
        }

        private void UpdateAffectedNPCs()
        {
            float radius = bubbleSize * 0.5f + 4f;
            float radiusSquared = radius * radius;

            Rectangle searchArea = Utils.CenteredRectangle(
                Projectile.Center,
                new Vector2(MaxBubbleRadius * 2f)
            );

            foreach (NPC npc in Main.ActiveNPCs)
            {
                int index = npc.whoAmI;

                if (npc.friendly || npc.dontTakeDamage)
                {
                    effected[index] = false;
                    continue;
                }

                if (!npc.Hitbox.Intersects(searchArea))
                    continue;

                if (Vector2.DistanceSquared(Projectile.Center, npc.Center) > radiusSquared)
                    continue;

                if (effected[index])
                    continue;

                if (Main.myPlayer == Projectile.owner)
                {
                    int damage = Projectile.damage;

                    if (!Main.hardMode)
                        damage = (int)(damage * 0.5f);

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        npc.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<EvilStrike>(),
                        damage,
                        0f,
                        Projectile.owner,
                        npc.whoAmI,
                        Projectile.whoAmI
                    );
                }

                effected[index] = true;
            }
        }

        private void SpawnStartDust()
        {
            for (int i = 0; i < 360; i += 15)
            {
                Vector2 circular = new Vector2(4f, 0f).RotatedBy(ToRadians(i));
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5f), 0, 0, ModContent.DustType<CopyDust4>());

                dust.velocity *= 0.33f;
                dust.velocity += circular;
                dust.scale *= 1.25f;
                dust.fadeIn = 0.2f;
                dust.color = ColorHelper.EvilColor;
                dust.alpha = 40;
                dust.noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 360; i += 10)
            {
                SpawnKillDust(i, 9f, 1.25f);
                SpawnKillDust(i, 6f, 1.75f);
            }
        }

        private void SpawnKillDust(int angleDegrees, float speed, float scaleMultiplier)
        {
            Vector2 outward = new Vector2(speed, 0f).RotatedBy(ToRadians(angleDegrees));
            Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5f), 0, 0, ModContent.DustType<CopyDust4>());

            dust.velocity *= 0.45f;
            dust.velocity += outward;
            dust.scale *= scaleMultiplier;
            dust.fadeIn = 0.2f;
            dust.color = ColorHelper.EvilColor;
            dust.alpha = 40;
            dust.noGravity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D growth = GrowthTexture.Value;
            Texture2D aura = AuraTexture.Value;
            Texture2D border = BorderTexture.Value;

            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float lifetimeRatio = Projectile.timeLeft / (float)MaxTimeLeft;
            float bubbleScale = bubbleSize / 600f;

            Main.spriteBatch.Draw(aura, drawPosition, null, new Color(200, 50, 0) * 0.2f * lifetimeRatio, 0f, new Vector2(300f), bubbleScale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(border, drawPosition, null, new Color(150, 30, 0) * 0.5f * lifetimeRatio, 0f, new Vector2(300f), bubbleScale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(growth, drawPosition, null, ColorHelper.EvilColor, Projectile.rotation, growth.Size() * 0.5f, Projectile.scale * lifetimeRatio + 0.3f, SpriteEffects.None, 0f);

            return false;
        }
    }
}