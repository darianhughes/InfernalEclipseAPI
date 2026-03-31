using InfernalEclipseAPI.YharimEX.Core.Systems;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.YharimEX.Content.Projectiles.MutantAttack
{
    public class YharimEXFragment : ModProjectile
    {
        public override string Texture => "InfernalEclipseAPI/YharimEX/Assets/Projectiles/NovaSlime";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.scale = 1.25f;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
        }

        private int ritualID = -1;

        public override void AI()
        {
            Projectile.velocity *= 0.985f;
            Projectile.rotation += Projectile.velocity.X / 30f;
            Projectile.frame = (int)Projectile.ai[0];
            if (Main.rand.NextBool(15))
            {
                var type = (int)Projectile.ai[0] switch
                {
                    0 => 242,
                    1 => 127,
                    2 => 229,
                    _ => 135,
                };
                Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type, 0f, 0f, 0, new Color(), 1f)];
                dust.velocity *= 4f;
                dust.fadeIn = 1f;
                dust.scale = 1f + Main.rand.NextFloat() + Main.rand.Next(4) * 0.3f;
                dust.noGravity = true;
            }

            if (ritualID == -1) //identify the ritual CLIENT SIDE
            {
                ritualID = -2; //if cant find it, give up and dont try every tick

                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<YharimEXRitual>())
                    {
                        ritualID = i;
                        break;
                    }
                }
            }

            Projectile ritual = YharimEXUtils.ProjectileExists(ritualID, ModContent.ProjectileType<YharimEXRitual>());
            if (ritual != null && Projectile.Distance(ritual.Center) > 1200f) //despawn faster
                Projectile.timeLeft = 0;
        }

        public override void OnKill(int timeLeft)
        {
            var type = (int)Projectile.ai[0] switch
            {
                0 => 242,
                1 => 127,
                2 => 229,
                _ => 135,
            };
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type, 0f, 0f, 0, new Color(), 1f)];
                dust.velocity *= 6f;
                dust.fadeIn = 1f;
                dust.scale = 1f + Main.rand.NextFloat() + Main.rand.Next(4) * 0.3f;
                dust.noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            Color color = Projectile.GetAlpha(lightColor);

            Main.spriteBatch.UseBlendState(BlendState.Additive);
            for (int j = 0; j < 12; j++)
            {
                Vector2 afterimageOffset = (MathHelper.TwoPi * j / 12f).ToRotationVector2() * 3f;
                Color glowColor = Color.White;

                Main.EntitySpriteDraw(texture, drawPosition + afterimageOffset, rectangle, glowColor, Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.ResetToDefault();

            Main.EntitySpriteDraw(texture, drawPosition, rectangle, color, Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}