using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace YharimEX.Assets.ExtraTextures
{
    public static class YharimEXTextureRegistry
    {
        public static Asset<Texture2D> WavyNoise => ModContent.Request<Texture2D>("InfernalEclipseAPI/YharimEX/Assets/ExtraTextures/Noise/WavyNoise");
        public static Asset<Texture2D> YharimEXStreak => ModContent.Request<Texture2D>("InfernalEclipseAPI/YharimEX/Assets/ExtraTextures/Trails/YharimEXStreak");
        public static Asset<Texture2D> FadedStreak => ModContent.Request<Texture2D>("InfernalEclipseAPI/YharimEX/Assets/ExtraTextures/Trails/FadedStreak");
        public static Asset<Texture2D> ColorNoiseMap => ModContent.Request<Texture2D>("InfernalEclipseAPI/YharimEX/Assets/ExtraTextures/Noise/ColorNoiseMap");
        public static Asset<Texture2D> BloomTexture => ModContent.Request<Texture2D>("InfernalEclipseAPI/YharimEX/Assets/ExtraTextures/AdditiveTextures/Bloom");

    }
}
