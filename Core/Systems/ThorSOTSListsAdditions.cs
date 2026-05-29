using System.Collections.Generic;
using System.Linq;
using SOTS;
using ThoriumMod.Projectiles;

namespace InfernalEclipseAPI.Core.Systems
{
    [JITWhenModsEnabled("SOTS", "ThoriumMod")]
    [ExtendsFromMod("SOTS", "ThoriumMod")]
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

                List<int> reworkAdditions = new();

                if (rework.TryFind("TerrariumPulseRay", out ModProjectile pulseRay))
                {
                    reworkAdditions.Add(pulseRay.Type);
                }

                SOTSPlayer.typhonBlacklist = SOTSPlayer.typhonBlacklist
                    .Concat(reworkAdditions)
                    .Distinct()
                    .ToArray();
            }
        }
    }

    [JITWhenModsEnabled("SOTS", "CalamityAmmo")]
    [ExtendsFromMod("SOTS", "CalamityAmmo")]
    public class AmmoSOTSListsAdditions : ModSystem
    {
        public override void Load()
        {
            if (ModLoader.TryGetMod("CalamityAmmo", out Mod calAmmo))
            {
                int[] additions =
                {
                    calAmmo.Find<ModProjectile>("seaPrismShard").Type
                };

                SOTSPlayer.typhonBlacklist = SOTSPlayer.typhonBlacklist
                    .Concat(additions)
                    .Distinct()
                    .ToArray();
            }
        }
    }
}
