using System.IO;
using Microsoft.Xna.Framework;

namespace YharimEX.Content.Projectiles.DLCAttack
{
    public class DLCBloomLine : BloomLine
    {
        public override string Texture => "InfernalEclipseAPI/YharimEX/Assets/Projectiles/BloomLine";

        private int counter;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(counter);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            counter = reader.ReadInt32();
            base.ReceiveExtraAI(reader);
        }

        public override void AI()
        {
            int maxTime = 60;
            float alphaModifier = 3;

            switch ((int)Projectile.ai[0])
            {
                case 1: //mutant scal line
                    {
                        Projectile.position -= Projectile.velocity;
                        Projectile.rotation = Projectile.velocity.ToRotation();
                        color = Color.Red;
                        alphaModifier = 1;
                        Projectile.scale = 1f;
                        maxTime = (int)Projectile.ai[2];
                    }
                    break;
                default:
                    Main.NewText("bloom line: you shouldnt be seeing this text");
                    break;
            }

            if (++counter > maxTime)
            {
                Projectile.Kill();
                return;
            }

            if (alphaModifier >= 0)
            {
                Projectile.alpha = 255 - (int)(255 * Math.Sin(Math.PI / maxTime * counter) * alphaModifier);
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }

            color.A = 0;
        }
    }
}
