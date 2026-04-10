using CalamityMod.Systems.Collections;
using InfernalEclipseAPI.Core.Systems;
using ThoriumMod.Projectiles;

namespace InfernalEclipseAPI.Core.Utils.CalamitySetsAdditions
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class CalThorSets : ModSystem
    {
        public override void PostSetupContent()
        {
            int[] noHomingWithGrapeBeer =
            {
                ModContent.ProjectileType<DragonPulse>(),
                ModContent.ProjectileType<OmniBoom>(),
                ModContent.ProjectileType<OmniBurst>(),
                ModContent.ProjectileType<OmniBurstDamage>(),
            };

            foreach (int proj in noHomingWithGrapeBeer)
            {
                CalamityProjectileSets.DoesNotGetHomingWithGrapeBeer[proj] = true;
            }

            if (InfernalCrossmod.ThoriumRework.Loaded)
            {
                Mod rework = InfernalCrossmod.ThoriumRework.Mod;

                int[] noHomingWithGrapeBeerRework =
                {
                    rework.Find<ModProjectile>("TerrariumPulseRay").Type
                };

                foreach (int proj in noHomingWithGrapeBeerRework)
                {
                    CalamityProjectileSets.DoesNotGetHomingWithGrapeBeer[proj] = true;
                }
            }
        }
    }
}
