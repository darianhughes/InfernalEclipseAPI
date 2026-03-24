using CalamityMod.Systems.Collections;
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
                ModContent.ProjectileType<DragonPulse>()
            };

            foreach (int proj in noHomingWithGrapeBeer)
            {
                CalamityProjectileSets.DoesNotGetHomingWithGrapeBeer[proj] = true;
            }
        }
    }
}
