using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using YharimEX.Content.NPCs.Bosses;
using YharimEX.Core.Systems;

namespace YharimEX.Content.Projectiles
{
    public class YharimEXSpazmarang : YharimEXRetirang
    {
        public override string Texture => "CalamityMod/Items/Weapons/Melee/TriactisTruePaladinianMageHammerofMightMelee";
        public override void SetStaticDefaults()
        {
             ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.CursedInferno, 120);
            if (YharimEXWorldFlags.EternityMode && YharimEXCrossmodSystem.FargowiltasSouls.Loaded) target.AddBuff(YharimEXCrossmodSystem.FargowiltasSouls.Mod.Find<ModBuff>("MutantFangBuff").Type, 180);
        }
    }
}