using CalamityMod.Projectiles.Melee;
using InfernalEclipseAPI.Core.DamageClasses.MythicClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace InfernalEclipseAPI.Content.Items.Weapons.Melee.TyrantYharimsUltisword
{
    public class TyrantYharimsUltiswordThrownBlade : DevilsDevastationThrownBlade
    {
        public override string Texture => "InfernalEclipseAPI/Content/Items/Weapons/Melee/TyrantYharimsUltisword/TyrantYharimsUltisword";

        public override void SetDefaults()
        {
            Projectile.width = 35;
            Projectile.height = 35;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Lifetime + ChargeupTime;
            Projectile.DamageType = ModContent.GetInstance<MythicMelee>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.noEnchantmentVisuals = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Melee/TyrantYharimsUltisword/TyrantYharimsUltisword").Value;
            Vector2 position = base.Projectile.Center - Main.screenPosition;
            Color color = Color.Lerp(Color.DarkRed, Color.Orange, 0.35f);
            float num = 1f - base.Projectile.Opacity + (stuckInTarget ? 0.55f : 0f);
            Color color2;
            for (int i = 0; i < 16; i++)
            {
                color2 = usedColor;
                color2.A = 0;
                Color color3 = color2 * 0.4f * num * base.Projectile.Opacity;
                Vector2 vector = (MathF.PI * 2f * (float)i / 16f).ToRotationVector2() * 9f * num + Main.rand.NextVector2Circular(3f, 3f);
                Main.EntitySpriteDraw(value, base.Projectile.Center - Main.screenPosition + vector, null, color3, base.Projectile.rotation, value.Size() * 0.5f, base.Projectile.scale, (base.Projectile.spriteDirection != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            }

            if (exitedTarget && !stuckInGround)
            {
                Asset<Texture2D> asset = ModContent.Request<Texture2D>("CalamityMod/Particles/CircularSmearFire3");
                Asset<Texture2D> asset2 = ModContent.Request<Texture2D>("CalamityMod/Particles/SemiCircularSmearSwipe");
                Texture2D value2 = asset2.Value;
                color2 = color;
                color2.A = 0;
                Main.EntitySpriteDraw(value2, position, null, color2 * 0.65f, base.Projectile.rotation * Main.rand.NextFloat(1.6f, 1.7f), asset2.Size() * 0.5f, 1.4f * Main.rand.NextFloat(0.8f, 1.15f), (base.Projectile.direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                Texture2D value3 = asset.Value;
                color2 = Color.Firebrick;
                color2.A = 0;
                Main.EntitySpriteDraw(value3, position, null, color2 * 0.75f, base.Projectile.rotation * Main.rand.NextFloat(1.2f, 1.3f), asset.Size() * 0.5f, 1.25f, (base.Projectile.direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            }

            Vector2 position2 = base.Projectile.Center - Main.screenPosition;
            color2 = Color.OrangeRed;
            color2.A = 0;
            Main.EntitySpriteDraw(value, position2, null, Color.Lerp(color2, lightColor, base.Projectile.Opacity) * base.Projectile.Opacity, base.Projectile.rotation, value.Size() * 0.5f, base.Projectile.scale, (base.Projectile.spriteDirection != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            return false;
        }
    }
}
