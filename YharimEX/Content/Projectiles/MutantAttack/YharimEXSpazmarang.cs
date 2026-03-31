
namespace InfernalEclipseAPI.YharimEX.Content.Projectiles.MutantAttack
{
    public class YharimEXSpazmarang : YharimEXRetirang
    {
        public override string Texture => "CalamityMod/Items/Weapons/Melee/TriactisTruePaladinianMageHammerofMight";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.CursedInferno, 120);
        }
    }
}