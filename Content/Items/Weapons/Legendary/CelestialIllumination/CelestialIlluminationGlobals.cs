using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.CelestialIllumination
{
    public class CelestialIlluminationPlayer : ModPlayer
    {
        public int CelestialStarCharge;
        public override void ResetEffects()
        {
            if (Player.HeldItem.type != ModContent.ItemType<CelestialIllumination>())
            {
                CelestialStarCharge = 0;
            }
        }
    }
}
