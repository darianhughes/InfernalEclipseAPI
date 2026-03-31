using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace YharimEX.Content.Projectiles.DLCAttack
{
    public class BloomLine : ModProjectile
    {
        public override string Texture => "InfernalEclipseAPI/YharimEX/Assets/Projectiles/BloomLine";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 2400;
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 1024;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.alpha = 255;

            Projectile.hide = true;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }

        public Color color = Color.White;

        public override bool? CanDamage()
        {
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(counter);
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            counter = reader.ReadInt32();
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }

        private int counter;
        private readonly int drawLayers = 1;
        public override void AI()
        {
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            float num6 = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 3000f, 16f * Projectile.scale, ref num6))
            {
                return true;
            }
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return color * Projectile.Opacity * (Main.mouseTextColor / 255f) * 0.9f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Rectangle rectangle = new(0, 0, Texture.Width, Texture.Height);
            Vector2 origin2 = rectangle.Size() / 2f;

            const int length = 3000;
            Vector2 offset = Projectile.rotation.ToRotationVector2() * length / 2f;
            Vector2 position = Projectile.Center - Main.screenLastPosition + new Vector2(0f, Projectile.gfxOffY) + offset;
            Rectangle destination = new((int)position.X, (int)position.Y, length, (int)(rectangle.Height * Projectile.scale));

            Color drawColor = Projectile.GetAlpha(lightColor);

            for (int j = 0; j < drawLayers; j++)
                Main.EntitySpriteDraw(new DrawData(Texture, destination, new Rectangle?(rectangle), drawColor, Projectile.rotation, origin2, SpriteEffects.None, 0));

            return false;
        }
    }
}
