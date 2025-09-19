namespace InfernalEclipseAPI.Content.NPCs.LittleCat
{
    public class DemonicRune : ModProjectile
    {
        // Little rune that will orbit around the Demonic Little Grey Cat
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
            Projectile.alpha = 190;
            Projectile.penetrate = 0;
            Projectile.scale = 0.1f;
        }
        public override void AI()
        {
            NPC Owner = Main.npc[(int)Projectile.ai[0]];
            if (Owner.active)
            {
                Projectile.Center = Owner.Center;
                Projectile.timeLeft = 2;
            }
            else
                Projectile.Kill();
        }
    }
}