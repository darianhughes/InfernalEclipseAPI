﻿namespace InfernalEclipseAPI.Core
{
    public class MenuBGStyle : ModSurfaceBackgroundStyle
    {
        //yeah idk what im doing here ngl...
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

        public override int ChooseFarTexture() => BackgroundTextureLoader.GetBackgroundSlot("InfernalEclipseAPI/Assets/Textures/Menu/MenuClouds");

        public override int ChooseMiddleTexture() => BackgroundTextureLoader.GetBackgroundSlot("InfernalEclipseAPI/Assets/Textures/Menu/MenuMountains");

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) => BackgroundTextureLoader.GetBackgroundSlot("InfernalEclipseAPI/Assets/Textures/Menu/MenuHills");
    }
}
