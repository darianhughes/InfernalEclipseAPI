﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static CalamityMod.Events.BossRushEvent;
using Microsoft.Xna.Framework.Graphics;

namespace InfernalEclipseAPI.Content.Projectiles
{
    public class BossRushTier6Animation : ModProjectile, ILocalizedModType
    {
        public Player Owner => Main.player[Projectile.owner];
        public const int FrameChangeRate = 4;
        public const int TotalFrames = 56;

        public override void SetDefaults()
        {
            Projectile.width = 76;
            Projectile.height = 180;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = FrameChangeRate * TotalFrames;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Projectile.Bottom = Owner.Top - Vector2.UnitY * Projectile.scale * 36f;
            Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter / FrameChangeRate;
            if (Projectile.frame >= TotalFrames)
                Projectile.frame = TotalFrames;

            // Play tier transition sounds on the first frame.
            if (Projectile.localAI[0] == 0f)
            {
                float volume = 2.8f;
                SoundEngine.PlaySound(Tier5TransitionSound with { Volume = volume }, Main.LocalPlayer.Center);
                Projectile.localAI[0] = 1f;
            }
        }

        public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.Opacity;

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>($"InfernalEclipseAPI/Content/Projectiles/BossRushTier6Animation").Value;
            Rectangle frame = texture.Frame(14, 4, Projectile.frame % 14, Projectile.frame / 14);
            Vector2 origin = frame.Size() * 0.5f;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), 0f, origin, Projectile.scale, 0, 0f);
            return false;
        }

    }
}
