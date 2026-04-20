using CalamityMod.Systems.Collections;

namespace InfernalEclipseAPI.Core.Utils.CalamitySetsAdditions
{
    public class CalMiscSets : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("CalamityAmmo", out Mod calAmmo))
            {
                int[] noHomingWithGrapeBeerAmmo =
                {
                    calAmmo.Find<ModProjectile>("seaPrismShard").Type
                };

                foreach (int proj in noHomingWithGrapeBeerAmmo)
                {
                    CalamityProjectileSets.DoesNotGetHomingWithGrapeBeer[proj] = true;
                }
            }
        }
    }
}
