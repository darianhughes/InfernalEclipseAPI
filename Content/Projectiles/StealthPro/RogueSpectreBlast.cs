using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Core.DamageClasses.MergedRogueClass;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalamityMod;

namespace InfernalEclipseAPI.Content.Projectiles.StealthPro
{
    //Wardrobe Hummus
    public class RogueSpectreBlast : ModProjectile
    {
        private bool? useCyanDust;
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.IceBolt}";
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = ModContent.GetInstance<StealthDamageClass>();
            Projectile.penetrate = 3;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
        }

        public override void OnSpawn(IEntitySource source)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    useCyanDust = true;
                    break;
                case 1:
                    useCyanDust = false;
                    break;
                default:
                    useCyanDust = null;
                    break;
            }
        }

        public override void AI()
        {
            NPC target = null;
            float closestDist = 700f;

            // Find nearest valid NPC
            for (int i = 0; i < Main.maxNPCs; ++i)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(null, false) && !npc.friendly)
                {
                    float dist = Vector2.Distance(Projectile.Center, npc.Center);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        target = npc;
                    }
                }
            }

            // Homing logic
            if (target != null)
            {
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize();
                float speed = 12f;
                Projectile.velocity = (Projectile.velocity * 20f + direction * speed) / 21f;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            else
            {
                Projectile.velocity *= 0.98f;
            }

            // Dust
            for (int i = 0; i < 5; ++i)
            {
                int dustIndex = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.DungeonSpirit,
                    Projectile.velocity.X * 0.1f + Main.rand.NextFloat(-0.5f, 0.5f),
                    Projectile.velocity.Y * 0.1f + Main.rand.NextFloat(-0.5f, 0.5f),
                    0,
                    default,
                    Main.rand.NextFloat(0.8f, 1.2f)
                );

                Dust dust = Main.dust[dustIndex];
                dust.velocity *= 0.05f;
                dust.fadeIn = 1.2f;
                dust.noGravity = true;

                if (useCyanDust.GetValueOrDefault())
                {
                    dust.color = new Color(80, 255, 255);
                }
                else if (useCyanDust.HasValue && !useCyanDust.Value)
                {
                    dust.color = new Color(255, 200, 80);
                }
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[297].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Rectangle frame = texture.Frame();
            Vector2 origin = frame.Size() / 2f;

            Color drawColor;
            if (useCyanDust.GetValueOrDefault())
            {
                drawColor = new Color(80, 255, 255);
            }
            else if (useCyanDust.HasValue && !useCyanDust.Value)
            {
                drawColor = new Color(255, 200, 80);
            }
            else
            {
                drawColor = Color.White;
            }

            // Switch to additive blending for the special effect
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive,
                SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone,
                null, Main.GameViewMatrix.TransformationMatrix);

            Main.spriteBatch.Draw(
                texture,
                drawPos,
                frame,
                drawColor * 1.3f,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            // Restore to normal alpha blending
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone,
                null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
