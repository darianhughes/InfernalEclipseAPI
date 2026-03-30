using Microsoft.Xna.Framework;
using Terraria;
using YharimEX.Core.Systems;
using YharimEX.Content.Projectiles.FargoProjectile;

namespace YharimEX.Content.Projectiles
{
    public class YharimEXBombSmall : YharimEXBomb
    {
        public override string Texture => $"Terraria/Images/Projectile_687";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 275;
            Projectile.height = 275;
            Projectile.scale = 0.75f;
            if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
            {
                SetupFargoProjectile SetupFargoProjectile = Projectile.GetGlobalProjectile<SetupFargoProjectile>();
                SetupFargoProjectile.TimeFreezeImmune = false;
            }
        }

        public override bool? CanDamage()
        {
            if (Projectile.frame > 2 && Projectile.frame <= 4)
            {
                if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
                {
                    SetupFargoProjectile.SetGrazeCD(Projectile);
                    return false;
                }
            }
            return true;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);

            }

            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame--;
                    Projectile.Kill();
                }
            }
        }
    }
}