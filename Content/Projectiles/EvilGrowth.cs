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
        private bool runOnce = true;
        private readonly Vector2[] trailPos = new Vector2[10];
        private int stretchCounter;
        private float unwind = 720f;
        private bool runOnce2 = true;
        private float saveRotation;
        public bool[] effected = new bool[Main.maxNPCs];
        private float bubbleSize;

        public override string Texture => "SOTS/Projectiles/Evil/EvilGrowth";

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.timeLeft = 150;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 0;
        }

        public void CataloguePos()
        {
            Vector2 previous = Projectile.Center - new Vector2(8f, -8f).RotatedBy(Projectile.rotation);
            for (int i = 0; i < trailPos.Length; i++)
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
                for (int i = 0; i < trailPos.Length; i++)
                    trailPos[i] = Vector2.Zero;
                runOnce = false;
            }

            CataloguePos();
            return true;
        }

        public override void AI()
        {
            if (runOnce2)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
                saveRotation = Projectile.rotation;
                Projectile.spriteDirection = 1;
            }

            if (Projectile.velocity.Length() <= 0.1f)
            {
                if (runOnce2)
                {
                    Projectile.ai[0] = 0f;
                    runOnce2 = false;
                    Projectile.rotation = 0f;
                    Projectile.scale = 0f;
                    Projectile.netUpdate = true;

                    for (int i = 0; i < 360; i += 15)
                    {
                        Vector2 circular = new Vector2(4f, 0f).RotatedBy(MathHelper.ToRadians(i));
                        Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5f, 5f), 0, 0, ModContent.DustType<CopyDust4>());
                        dust.velocity *= 0.33f;
                        dust.velocity += circular;
                        dust.scale *= 1.25f;
                        dust.fadeIn = 0.2f;
                        dust.color = ColorHelper.EvilColor;
                        dust.alpha = 40;
                        dust.noGravity = true;
                    }

                    SOTSUtils.PlaySound(SoundID.Item116, (int)Projectile.Center.X, (int)Projectile.Center.Y, 2.3f, -0.5f);
                }

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

                bubbleSize = 420f * (float)Math.Sin(MathHelper.ToRadians(stretchCounter - Projectile.ai[0] + 10f));
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                Vector2 offset = Projectile.Center - npc.Center;

                if (offset.Length() <= bubbleSize / 2f + 4f && !npc.friendly && npc.active && !npc.dontTakeDamage)
                {
                    if (!effected[i])
                    {
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

                        effected[i] = true;
                    }
                }
                else if (npc.friendly || !npc.active)
                {
                    effected[i] = false;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 360; i += 10)
            {
                Vector2 outwardA = new Vector2(9f, 0f).RotatedBy(MathHelper.ToRadians(i));
                Dust dustA = Dust.NewDustDirect(Projectile.Center - new Vector2(5f, 5f), 0, 0, ModContent.DustType<CopyDust4>());
                dustA.velocity *= 0.45f;
                dustA.velocity += outwardA;
                dustA.scale *= 1.25f;
                dustA.fadeIn = 0.2f;
                dustA.color = ColorHelper.EvilColor;
                dustA.alpha = 40;
                dustA.noGravity = true;

                Vector2 outwardB = new Vector2(6f, 0f).RotatedBy(MathHelper.ToRadians(i));
                Dust dustB = Dust.NewDustDirect(Projectile.Center - new Vector2(5f, 5f), 0, 0, ModContent.DustType<CopyDust4>());
                dustB.velocity *= 0.45f;
                dustB.velocity += outwardB;
                dustB.scale *= 1.75f;
                dustB.fadeIn = 0.2f;
                dustB.color = ColorHelper.EvilColor;
                dustB.alpha = 40;
                dustB.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D growth = ModContent.Request<Texture2D>("SOTS/Projectiles/Evil/EvilGrowth", AssetRequestMode.ImmediateLoad).Value;
            Texture2D aura = ModContent.Request<Texture2D>("SOTS/Gores/CircleAura", AssetRequestMode.ImmediateLoad).Value;
            Texture2D border = ModContent.Request<Texture2D>("SOTS/Gores/CircleBorder", AssetRequestMode.ImmediateLoad).Value;

            Main.spriteBatch.Draw(aura, Projectile.Center - Main.screenPosition, null,
                new Color(200, 50, 0) * 0.2f * ((float)Projectile.timeLeft / 150f),
                0f, new Vector2(300f, 300f), bubbleSize / 600f, SpriteEffects.None, 0f);

            Main.spriteBatch.Draw(border, Projectile.Center - Main.screenPosition, null,
                new Color(150, 30, 0) * 0.5f * ((float)Projectile.timeLeft / 150f),
                0f, new Vector2(300f, 300f), bubbleSize / 600f, SpriteEffects.None, 0f);

            Main.spriteBatch.Draw(growth, Projectile.Center - Main.screenPosition, null,
                ColorHelper.EvilColor, Projectile.rotation, growth.Size() * 0.5f,
                Projectile.scale * ((float)Projectile.timeLeft / 150f) + 0.3f,
                SpriteEffects.None, 0f);

            return false;
        }
    }
}