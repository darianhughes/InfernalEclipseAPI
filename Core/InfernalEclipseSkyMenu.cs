using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core
{
    public class InfernalEclipseSkyMenu : ModMenu
    {
        private const string MenuAssetPath = "InfernalEclipseAPI/Assets/Textures/Menu";
        private const int BaseWidth = 1920;
        private const int BaseHeight = 1080;

        // Texture assets
        private Asset<Texture2D> menuSky;
        private Asset<Texture2D> menuSkyTinted;
        private Asset<Texture2D> menuEclipseGlow;
        private Asset<Texture2D> menuEclipse;
        private Asset<Texture2D> menuEclipseGlare;
        private Asset<Texture2D> menuClouds;
        private Asset<Texture2D> menuCloudsTinted;
        private Asset<Texture2D> menuMountains;
        private Asset<Texture2D> menuMountainsTinted;
        private Asset<Texture2D> menuHills;
        private Asset<Texture2D> menuHillsTinted;
        private Asset<Texture2D> ieorAnkh;
        private Asset<Texture2D> ieorText;

        public override string DisplayName => "Infernal Eclipse Sky";

        public override void Load()
        {
            menuSky = ModContent.Request<Texture2D>($"{MenuAssetPath}/MenuSky");
            menuSkyTinted = ModContent.Request<Texture2D>($"{MenuAssetPath}/MenuSkyTinted");
            menuEclipseGlow = ModContent.Request<Texture2D>($"{MenuAssetPath}/MenuEclipseGlow");
            menuEclipse = ModContent.Request<Texture2D>($"{MenuAssetPath}/MenuEclipse");
            menuEclipseGlare = ModContent.Request<Texture2D>($"{MenuAssetPath}/MenuEclipseGlare");
            menuClouds = ModContent.Request<Texture2D>($"{MenuAssetPath}/MenuClouds");
            menuCloudsTinted = ModContent.Request<Texture2D>($"{MenuAssetPath}/MenuCloudsTinted");
            menuMountains = ModContent.Request<Texture2D>($"{MenuAssetPath}/MenuMountains");
            menuMountainsTinted = ModContent.Request<Texture2D>($"{MenuAssetPath}/MenuMountainsTinted");
            menuHills = ModContent.Request<Texture2D>($"{MenuAssetPath}/MenuHills");
            menuHillsTinted = ModContent.Request<Texture2D>($"{MenuAssetPath}/MenuHillsTinted");
            ieorAnkh = ModContent.Request<Texture2D>($"{MenuAssetPath}/IEoRAnhk");
            ieorText = ModContent.Request<Texture2D>($"{MenuAssetPath}/IEoRText");
        }

        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{MenuAssetPath}/Pixel");

        public override Asset<Texture2D> SunTexture => ModContent.Request<Texture2D>($"{MenuAssetPath}/Pixel");

        public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>($"{MenuAssetPath}/Pixel");

        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<NullSurfaceBackground>();

        public override int Music => MusicLoader.GetMusicSlot("CalamityModMusic/Sounds/Music/BrimstoneCrags");

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            // Calculate scale to fit screen (maintain aspect ratio)
            Vector2 drawOffset = Vector2.Zero;
            float xScale = (float)Main.screenWidth / BaseWidth;
            float yScale = (float)Main.screenHeight / BaseHeight;
            float scale = xScale;

            if (xScale != yScale)
            {
                if (yScale > xScale)
                {
                    scale = yScale;
                    drawOffset.X -= (BaseWidth * scale - Main.screenWidth) * 0.5f;
                }
                else
                {
                    drawOffset.Y -= (BaseHeight * scale - Main.screenHeight) * 0.5f;
                }
            }

            // Logo elements are scaled relative to base
            float logoScaleP = scale * 1f;

            // Force time to noon
            Main.time = 27000;
            Main.dayTime = true;
            drawColor = Color.White;

            // Draw sky background (static, full screen)
            DrawLayer(spriteBatch, menuSkyTinted.Value, drawOffset, scale, Color.White, BlendState.AlphaBlend);

            // Eclipse elements are also 1920x1080 canvases with pre-positioned content
            // Draw them with the same offset as sky, just different scales

            // Need to offset X to center the smaller canvas: shift by half the width difference
            Vector2 eclipseOffset = drawOffset + new Vector2(BaseWidth * (0.5f * scale) / 2f, 0f);

            // Draw eclipse glow (1920x1080 canvas, no additional scaling, additive blend)
            DrawLayer(spriteBatch, menuEclipseGlow.Value, eclipseOffset, scale, Color.White, BlendState.Additive);

            // Draw eclipse (1920x1080 canvas, scaled down 2x)
            DrawLayer(spriteBatch, menuEclipse.Value, eclipseOffset, scale * 0.5f, Color.White, BlendState.AlphaBlend);

            // Draw eclipse glare (1920x1080 canvas, no additional scaling, additive blend)
            DrawLayer(spriteBatch, menuEclipseGlare.Value, eclipseOffset, scale, Color.White, BlendState.Additive);

            // Draw parallax layers with horizontal tiling
            DrawParallaxLayer(spriteBatch, menuCloudsTinted.Value, scale, 0.15f, Color.White);
            DrawParallaxLayer(spriteBatch, menuMountainsTinted.Value, scale, 0.25f, Color.White);
            DrawParallaxLayer(spriteBatch, menuHillsTinted.Value, scale, 0.4f, Color.White);

            // End current batch to switch to NonPremultiplied for text
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

            //// Draw ankh logo
            //DrawLayer(spriteBatch, ieorAnkh.Value, drawOffset, logoScaleP, Color.White, null);

            //// Draw text logo
            //DrawLayer(spriteBatch, ieorText.Value, drawOffset, logoScaleP, Color.White, null);

            // Restart spritebatch with normal blend mode
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

            return false; // Don't draw the default logo
        }

        private void DrawLayer(SpriteBatch spriteBatch, Texture2D texture, Vector2 offset, float scale, Color color, BlendState blendState)
        {
            if (blendState != null)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, blendState, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            }

            spriteBatch.Draw(texture, offset, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            if (blendState != null)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            }
        }

        private void DrawParallaxLayer(SpriteBatch spriteBatch, Texture2D texture, float scale, float parallaxSpeed, Color color)
        {
            // Calculate horizontal parallax offset
            int xOffset = (int)(Main.screenPosition.X * parallaxSpeed);
            xOffset %= (int)(texture.Width * scale);

            float screenCenterX = Main.screenWidth / 2f;
            float screenCenterY = Main.screenHeight / 2f;

            // Draw multiple copies to ensure screen coverage with horizontal tiling
            for (int k = -1; k <= 1; k++)
            {
                Vector2 position = new Vector2(screenCenterX - xOffset + texture.Width * k * scale, screenCenterY);
                spriteBatch.Draw(texture, position, null, color, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, 0f);
            }
        }
    }
}
