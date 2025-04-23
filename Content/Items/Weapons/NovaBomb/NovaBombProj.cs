using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace InfernalEclipseAPI.Content.Items.Weapons.NovaBomb
{
    public class NovaBombProj : SupernovaBomb
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override string Texture => "InfernalEclipseAPI/Content/Items/Weapons/NovaBomb/Empty";
        public new Color variedColor = new Color(178, 114, 255);   // Soft glowing purple
        public new Color mainColor = new Color(112, 40, 222);      // Deep violet core
        public new Color randomColor = new Color(245, 210, 255);   // Pale pinkish-white sparkles
        public new int colorTimer = 0;
        public new int time = 0;
        public new bool homing = false;
        public new bool returning = false;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 53;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            base.AI();
        }

        public override void OnKill(int timeLeft)
        {
           base.OnKill(timeLeft);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) // Add to regular plz
        {
            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, 5, targetHitbox);
        public override bool PreDraw(ref Color lightColor)
        {
            Color auraColor = Projectile.GetAlpha(Color.Lerp(Color.White, randomColor, 0.3f)) * 0.25f;
            for (int i = 0; i < 7; i++)
            {
                Texture2D centerTexture = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/NovaBomb/NovaBombProjectile").Value;
                Vector2 rotationalDrawOffset = (MathHelper.TwoPi * i / 7f + Main.GlobalTimeWrappedHourly * 8f).ToRotationVector2();
                rotationalDrawOffset *= MathHelper.Lerp(3f, 5.25f, (float)Math.Cos(Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f);
                Main.EntitySpriteDraw(centerTexture, Projectile.Center - Main.screenPosition + rotationalDrawOffset, null, auraColor, Projectile.rotation, centerTexture.Size() * 0.5f, Projectile.scale * 1.1f, SpriteEffects.None, 0f);
            }

            if (!homing)
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], Color.Lerp(Color.White, randomColor, 0.3f), 1);
            return true;
        }
    }
}
