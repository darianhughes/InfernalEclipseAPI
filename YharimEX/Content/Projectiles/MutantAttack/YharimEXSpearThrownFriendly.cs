namespace YharimEX.Content.Projectiles
{
    public class YharimEXSpearThrownFriendly : YharimEXPenetratorThrown
    {
        public override string Texture => "YharimEX/Assets/Projectiles/YharimEXSpear";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.DamageType = Terraria.ModLoader.DamageClass.Default;
        }
    }
}