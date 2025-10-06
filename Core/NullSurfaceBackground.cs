using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core
{
    // Disables vanilla parallax background rendering so we can draw everything manually
    internal class NullSurfaceBackground : ModSurfaceBackgroundStyle
    {
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                        fades[i] = 1f;
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                        fades[i] = 0f;
                }
            }
        }

        // Return blank pixel for all background layers
        private static readonly string BlankPath = "InfernalEclipseAPI/Assets/Textures/Pixel";

        // Return 0 (vanilla background) - won't render anyway due to PreDrawCloseBackground
        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) => 0;

        public override int ChooseFarTexture() => 0;

        public override int ChooseMiddleTexture() => 0;

        public override bool PreDrawCloseBackground(SpriteBatch spriteBatch) => false;
    }
}
