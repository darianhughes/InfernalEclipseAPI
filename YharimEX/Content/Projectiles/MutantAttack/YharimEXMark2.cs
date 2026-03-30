using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using YharimEX.Core.Systems;
using YharimEX.Content.Projectiles.FargoProjectile;

namespace YharimEX.Content.Projectiles
{
    public class YharimEXMark2 : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_226";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 900;
            Projectile.aiStyle = -1;
            CooldownSlot = 1;
            Projectile.hide = true;

            if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
            {
                SetupFargoProjectile SetupFargoProjectile = Projectile.GetGlobalProjectile<SetupFargoProjectile>();
                SetupFargoProjectile.DeletionImmuneRank = 1;
            }
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                SoundEngine.PlaySound(SoundID.Item84, Projectile.Center);
            }

            if (--Projectile.ai[0] == 0)
            {
                Projectile.netUpdate = true;
                Projectile.velocity = Vector2.Zero;
            }
            if (--Projectile.ai[1] == 0)
            {
                Projectile.netUpdate = true;
                Player target = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
                Projectile.velocity = Projectile.SafeDirectionTo(target.Center) * 15;
                SoundEngine.PlaySound(SoundID.Item84, Projectile.Center);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Mod FargoSouls = null;
            if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
            {
                FargoSouls = YharimEXCrossmodSystem.FargowiltasSouls.Mod;
            }
            target.AddBuff(BuffID.Poisoned, Main.rand.Next(60, 300));
            if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
            {
                if (YharimEXWorldFlags.EternityMode)
                {
                    target.AddBuff(FargoSouls.Find<ModBuff>("InfestedBuff").Type, Main.rand.Next(60, 300));
                    target.AddBuff(FargoSouls.Find<ModBuff>("IvyVenomBuff").Type, Main.rand.Next(60, 300));
                    target.AddBuff(FargoSouls.Find<ModBuff>("MutantFangBuff").Type, 180);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}