using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace InfernalEclipseAPI.Core.Graphics.Automators
{
    public class SolynTexturesRegistry : ILoadable
    {
        public void Load(Mod mod)
        {
            GreyscaleTextures.BloomCircleSmall = mod.Assets.Request<Texture2D>("Assets/Textures/Extra/GreyscaleTextures/BloomCircleSmall", (AssetRequestMode)2);
            Noise.DendriticNoiseZoomedOut = mod.Assets.Request<Texture2D>("Assets/Textures/Extra/Noise/DendriticNoiseZoomedOut", (AssetRequestMode)2);
            Noise.FireNoiseA = mod.Assets.Request<Texture2D>("Assets/Textures/Extra/Noise/FireNoiseA", (AssetRequestMode)2);
            Noise.CrackedNoiseA = mod.Assets.Request<Texture2D>("Assets/Textures/Extra/Noise/CrackedNoiseA", (AssetRequestMode)2);
            Noise.SharpNoise = mod.Assets.Request<Texture2D>("Assets/Textures/Extra/Noise/SharpNoise", (AssetRequestMode)2);
            Noise.PerlinNoise = mod.Assets.Request<Texture2D>("Assets/Textures/Extra/Noise/PerlinNoise", (AssetRequestMode)2);
            Extra.Cosmos = mod.Assets.Request<Texture2D>("Assets/Textures/Extra/Cosmos", (AssetRequestMode)2);
            Extra.Void = mod.Assets.Request<Texture2D>("Assets/Textures/Extra/Void", (AssetRequestMode)2);
            Extra.CosmicLightCircle = mod.Assets.Request<Texture2D>("Assets/Textures/Extra/CosmicLightCircle", (AssetRequestMode)2);
            Extra.CosmicLightCircleCenter = mod.Assets.Request<Texture2D>("Assets/Textures/Extra/CosmicLightCircleCenter", (AssetRequestMode)2);
            Extra.CosmicLightCircleRing = mod.Assets.Request<Texture2D>("Assets/Textures/Extra/CosmicLightCircleRing", (AssetRequestMode)2);
            UI.SolynPanel = mod.Assets.Request<Texture2D>("Assets/Images/UI/SolynPanel", (AssetRequestMode)2);
            UI.CosmicIcon = mod.Assets.Request<Texture2D>("Assets/Images/UI/CosmicIcon", (AssetRequestMode)2);
            UI.CosmicPulse = mod.Assets.Request<Texture2D>("Assets/Images/UI/CosmicPulse", (AssetRequestMode)2);
        }

        public void Unload()
        {
            GreyscaleTextures.BloomCircleSmall = (Asset<Texture2D>)null;
            Noise.DendriticNoiseZoomedOut = (Asset<Texture2D>)null;
            Noise.FireNoiseA = (Asset<Texture2D>)null;
            Noise.CrackedNoiseA = (Asset<Texture2D>)null;
            Noise.SharpNoise = (Asset<Texture2D>)null;
            Noise.PerlinNoise = (Asset<Texture2D>)null;
            Extra.Cosmos = (Asset<Texture2D>)null;
            Extra.Void = (Asset<Texture2D>)null;
            Extra.CosmicLightCircle = (Asset<Texture2D>)null;
            Extra.CosmicLightCircleCenter = (Asset<Texture2D>)null;
            Extra.CosmicLightCircleRing = (Asset<Texture2D>)null;
            UI.SolynPanel = (Asset<Texture2D>)null;
            UI.CosmicIcon = (Asset<Texture2D>)null;
            UI.CosmicPulse = (Asset<Texture2D>)null;
        }

        public static class GreyscaleTextures
        {
            public static Asset<Texture2D> BloomCircleSmall;
        }

        public static class Noise
        {
            public static Asset<Texture2D> DendriticNoiseZoomedOut;
            public static Asset<Texture2D> FireNoiseA;
            public static Asset<Texture2D> CrackedNoiseA;
            public static Asset<Texture2D> SharpNoise;
            public static Asset<Texture2D> PerlinNoise;
        }

        public static class Extra
        {
            public static Asset<Texture2D> Cosmos;
            public static Asset<Texture2D> Void;
            public static Asset<Texture2D> CosmicLightCircle;
            public static Asset<Texture2D> CosmicLightCircleCenter;
            public static Asset<Texture2D> CosmicLightCircleRing;
        }

        public static class UI
        {
            public static Asset<Texture2D> SolynPanel;
            public static Asset<Texture2D> CosmicIcon;
            public static Asset<Texture2D> CosmicPulse;
        }
    }
}
