using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ThoriumMod.Projectiles.Thrower;
using CalamityMod;

namespace InfernalEclipseAPI.Content.Projectiles
{
    [ExtendsFromMod("ThoriumMod")]
    public class TidalWaveWhirlpool : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_657"; // same water whirlpool texture used by TBR

        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 125;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6; // 20 ticks = can only hit each NPC every 1/10 second
        }

        public override void AI()
        {
            // Spin the whirlpool
            Projectile.ai[1] += 0.08f * Projectile.direction; // tweak the speed if you want

            // Try to find the nearest friendly shuriken owned by the projectile owner
            int shurikenType = ModContent.ProjectileType<TidalWavePro>();
            float closestDist = float.MaxValue;
            Projectile target = null;
            for (int i = 0; i < Main.maxProjectiles; ++i)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Projectile.owner && proj.type == shurikenType)
                {
                    float dist = Vector2.Distance(Projectile.Center, proj.Center);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        target = proj;
                    }
                }
            }

            if (target != null)
            {
                Projectile.Center = target.Center;
            }
            else
            {
                // If no shuriken found, follow the player
                Player owner = Main.player[Projectile.owner];
                Projectile.Center = owner.Center;
            }

            // FADE IN
            if (Projectile.alpha > 0 && Projectile.timeLeft > 45)
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }

            // FADE OUT
            if (Projectile.timeLeft <= 45)
            {
                Projectile.alpha += 6;
                if (Projectile.alpha > 255)
                    Projectile.alpha = 255;
            }

            // --- Vortex Pull Effect ---
            float pullRadius = Projectile.width * 1.25f + 150f;
            float pullStrength = 0.6f;
            Vector2 center = Projectile.Center;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && !npc.boss && !npc.dontTakeDamage && npc.lifeMax > 5 && !npc.immortal && npc.CanBeChasedBy(Projectile))
                {
                    float dist = Vector2.Distance(npc.Center, center);
                    if (dist < pullRadius)
                    {
                        Vector2 pull = center - npc.Center;
                        float strength = pullStrength * (1f - dist / pullRadius); // Linear falloff
                        pull.Normalize();
                        npc.velocity += pull * strength;
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            float scale = 3.25f;
            float rotation = Projectile.ai[1];
            // Ensure alpha fades out to invisible (alpha 255 is fully transparent in Terraria)
            float fade = 1f - Projectile.alpha / 255f;
            Color drawColor = Color.Cyan * fade;
            drawColor.A = (byte)(255 * fade);

            for (int i = 0; i < 3; i++)
            {
                float layerRotation = rotation + i * MathHelper.TwoPi / 3f;
                float layerScale = scale * (1f - i * 0.13f);
                Color layerColor = Color.Lerp(Color.Cyan, Color.White, i * 0.35f) * 0.75f * fade;
                layerColor.A = (byte)(255 * fade);

                Main.EntitySpriteDraw(
                    tex,
                    Projectile.Center - Main.screenPosition,
                    null,
                    layerColor,
                    layerRotation,
                    tex.Size() * 0.5f,
                    layerScale,
                    SpriteEffects.None,
                    0
                );
            }
            return false;
        }

        public override bool ShouldUpdatePosition() => false; // It is always positioned manually

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overWiresUI.Add(index); // Draw over wires/UI
        }
    }
}
