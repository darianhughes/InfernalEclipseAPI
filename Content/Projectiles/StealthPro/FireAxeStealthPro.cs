using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace InfernalEclipseAPI.Content.Projectiles.StealthPro
{
    //WH
    public class FireAxeStealthPro : ModProjectile
    {
        private int hitCount = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = 3;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        private const int HoverTime = 40;
        private bool initialized = false;
        private Vector2 hoverOffset = new Vector2(0, -240f);
        private NPC target;
        private float currentRotationSpeed = 0.3f;

        public override void AI()
        {
            if (!initialized)
            {
                if (Projectile.ai[1] > -1 && Projectile.ai[1] < Main.maxNPCs)
                {
                    NPC possibleTarget = Main.npc[(int)Projectile.ai[1]];
                    if (possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly)
                    {
                        target = possibleTarget;
                    }
                }
                if (target == null)
                {
                    target = FindTarget(); // fallback if initial target is invalid
                }
                if (target != null)
                {
                    Projectile.Center = target.Center + hoverOffset;
                }
                else
                {
                    Projectile.Kill();
                    return;
                }

                initialized = true;
            }

            if (Projectile.ai[0] < HoverTime)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.ai[0]++;

                if (target != null && target.active)
                {
                    Projectile.Center = target.Center + hoverOffset;
                }

                float progress = Projectile.ai[0] / HoverTime;
                Projectile.scale = MathHelper.Lerp(1f, 2.5f, progress * 1.2f);
                currentRotationSpeed = MathHelper.Lerp(0.1f, 1.2f, progress);
            }
            else
            {
                if (Projectile.ai[0] == HoverTime)
                {
                    Projectile.scale = 2.5f;
                    Projectile.penetrate = -1;
                    Projectile.tileCollide = true;
                    Projectile.velocity = new Vector2(0, 48f);
                }

                Projectile.velocity.Y = 48f;
            }

            Projectile.rotation += currentRotationSpeed * Projectile.direction;

            if (Main.rand.NextBool(2))
            {
                Rectangle dustArea = new Rectangle(
                    (int)(Projectile.Center.X - Projectile.width * Projectile.scale / 2f),
                    (int)(Projectile.Center.Y - Projectile.height * Projectile.scale / 2f),
                    (int)(Projectile.width * Projectile.scale),
                    (int)(Projectile.height * Projectile.scale)
                );

                Dust.NewDust(dustArea.TopLeft(), dustArea.Width, dustArea.Height, DustID.Torch, 0f, 0f, 100, default, 1.2f);
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.ai[0] >= HoverTime;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitCount++;
            if (hitCount >= 3)
            {
                Projectile.Kill();
                return;
            }

            target.AddBuff(BuffID.OnFire3, 300);

            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                if (calamity.TryFind("BurningBlood", out ModBuff burningBlood))
                    target.AddBuff(burningBlood.Type, 300);

                if (calamity.TryFind("BettyExplosion", out ModProjectile bettyExplosion))
                {
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        Vector2.Zero,
                        bettyExplosion.Type,
                        0,
                        0f,
                        Projectile.owner
                    );
                }
            }

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);

            for (int i = 0; i < 15; i++)
            {
                Rectangle dustArea = new Rectangle(
                    (int)(Projectile.Center.X - Projectile.width * Projectile.scale / 2f),
                    (int)(Projectile.Center.Y - Projectile.height * Projectile.scale / 2f),
                    (int)(Projectile.width * Projectile.scale),
                    (int)(Projectile.height * Projectile.scale)
                );

                Dust.NewDust(dustArea.TopLeft(), dustArea.Width, dustArea.Height, DustID.FireworkFountain_Yellow,
                    Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
            }
        }


        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) &&
                calamityMod.TryFind("BettyExplosion", out ModProjectile bettyExplosion))
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    bettyExplosion.Type,
                    0,
                    0f,
                    Projectile.owner
                );
            }

            for (int i = 0; i < 15; i++)
            {
                Rectangle dustArea = new Rectangle(
                    (int)(Projectile.Center.X - Projectile.width * Projectile.scale / 2f),
                    (int)(Projectile.Center.Y - Projectile.height * Projectile.scale / 2f),
                    (int)(Projectile.width * Projectile.scale),
                    (int)(Projectile.height * Projectile.scale)
                );

                Dust.NewDust(dustArea.TopLeft(), dustArea.Width, dustArea.Height, DustID.FireworkFountain_Yellow,
                    Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
            }
        }

        private NPC FindTarget()
        {
            NPC closest = null;
            float closestDist = 1000f;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(Projectile))
                {
                    float dist = Vector2.Distance(Projectile.Center, npc.Center);
                    if (dist < closestDist)
                    {
                        closest = npc;
                        closestDist = dist;
                    }
                }
            }

            return closest;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                Color color = lightColor * ((float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            }

            return false;
        }
    }
}
