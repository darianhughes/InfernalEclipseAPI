using System.Linq;
using SOTS;
using ThoriumMod.Projectiles;

namespace InfernalEclipseAPI.Core.Systems
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class ThorSOTSListsAdditions : ModSystem
    {
        public override void Load()
        {
            int[] additions =
            {
                ModContent.ProjectileType<DragonPulse>(),
                ModContent.ProjectileType<OmniBoom>(),
                ModContent.ProjectileType<OmniBurst>(),
                ModContent.ProjectileType<OmniBurstDamage>(),
            };

            SOTSPlayer.typhonBlacklist = SOTSPlayer.typhonBlacklist
                .Concat(additions)
                .Distinct()
                .ToArray();

            if (InfernalCrossmod.ThoriumRework.Loaded)
            {
                Mod rework = InfernalCrossmod.ThoriumRework.Mod;

                int[] reworkAdditions =
                {
                    rework.Find<ModProjectile>("TerrariumPulseRay").Type
                };

                SOTSPlayer.typhonBlacklist = SOTSPlayer.typhonBlacklist
                    .Concat(reworkAdditions)
                    .Distinct()
                    .ToArray();
            }
        }
    }

}
