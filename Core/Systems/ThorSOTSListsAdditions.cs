using System.Collections.Generic;
using System.Reflection;
using SOTS;
using ThoriumMod.Projectiles;

namespace InfernalEclipseAPI.Core.Systems
{
    [JITWhenModsEnabled("SOTS", "ThoriumMod")]
    [ExtendsFromMod("SOTS", "ThoriumMod")]
    public class ThorSOTSListsAdditions : ModSystem
    {
        private static readonly FieldInfo blacklistField =
            typeof(SOTSPlayer).GetField(
                nameof(SOTSPlayer.HomingProjectileBlacklist),
                BindingFlags.Public | BindingFlags.Static);

        private static void AddToBlacklist(IEnumerable<int> additions)
        {
            HashSet<int> current = (HashSet<int>)blacklistField.GetValue(null);

            current.UnionWith(additions);
        }

        public override void Load()
        {
            AddToBlacklist(new[]
            {
                ModContent.ProjectileType<DragonPulse>(),
                ModContent.ProjectileType<OmniBoom>(),
                ModContent.ProjectileType<OmniBurst>(),
                ModContent.ProjectileType<OmniBurstDamage>(),
            });

            if (InfernalCrossmod.ThoriumRework.Loaded)
            {
                Mod rework = InfernalCrossmod.ThoriumRework.Mod;

                List<int> reworkAdditions = new();

                if (rework.TryFind("TerrariumPulseRay", out ModProjectile pulseRay))
                {
                    reworkAdditions.Add(pulseRay.Type);
                }

                AddToBlacklist(reworkAdditions);
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
                FieldInfo blacklistField =
                    typeof(SOTSPlayer).GetField(
                        nameof(SOTSPlayer.HomingProjectileBlacklist),
                        BindingFlags.Public | BindingFlags.Static);

                HashSet<int> current = (HashSet<int>)blacklistField.GetValue(null);

                current.Add(calAmmo.Find<ModProjectile>("seaPrismShard").Type);
            }
        }
    }
}