using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace InfernalEclipseAPI.Content.UI.Notificatons
{
    public class HomewardJourneyNotification : IInGameNotification
    {
        public bool ShouldBeRemoved => timeLeft <= 0;

        private int timeLeft = 5 * 60;

        private Asset<Texture2D> iconTexture = ModLoader.HasMod("HomewardRagnarok") ? ModContent.Request<Texture2D>("HomewardRagnarok/icon_small") : ModContent.Request<Texture2D>("InfernalEclipseAPI/icon_small");

        private float Scale
        {
            get
            {
                if (timeLeft < 30)
                {
                    return Lerp(0f, 1f, timeLeft / 30f);
                }

                if (timeLeft > 285)
                {
                    return Lerp(1f, 0f, (timeLeft - 285) / 15f);
                }

                return 1f;
            }
        }

        private float Opacity
        {
            get
            {
                if (Scale <= 0.5f)
                {
                    return 0f;
                }

                return (Scale - 0.5f) / 0.5f;
            }
        }

        public void Update()
        {
            if (timeLeft <= 30 || timeLeft > 200)
                timeLeft--;

            if (timeLeft < 0)
            {
                timeLeft = 0;
            }
        }

        public void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition)
        {
            if (Opacity <= 0f)
            {
                return;
            }

            string title = Language.GetTextValue(ModLoader.HasMod("HomewardRagnarok") ? "Mods.InfernalEclipseAPI.UI.NoticeHomewardRagnarok" : "Mods.InfernalEclipseAPI.UI.NoticeHomewardJourney");

            float effectiveScale = Scale * 1.1f;
            Vector2 size = (FontAssets.ItemStack.Value.MeasureString(title) + new Vector2(70f, 100f)) * effectiveScale;
            Rectangle panelSize = Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, (0f - size.Y) * 0.5f), size);

            // Check if the mouse is hovering over the notification.
            bool hovering = panelSize.Contains(Main.MouseScreen.ToPoint());

            Utils.DrawInvBG(spriteBatch, panelSize, new Color(64, 109, 164) * (hovering ? 0.75f : 0.5f));
            float iconScale = effectiveScale * 0.7f;
            Vector2 vector = panelSize.Right() - Vector2.UnitX * effectiveScale * (12f + iconScale * iconTexture.Width());
            spriteBatch.Draw(iconTexture.Value, vector, null, Color.White * Opacity, 0f, new Vector2(0f, iconTexture.Width() / 2f), iconScale, SpriteEffects.None, 0f);
            Utils.DrawBorderString(color: new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor / 5, Main.mouseTextColor) * Opacity, sb: spriteBatch, text: title, pos: vector - Vector2.UnitX * 10f, scale: effectiveScale * 0.9f, anchorx: 1f, anchory: 0.4f);

            if (hovering)
            {
                OnMouseOver();
            }
        }

        private void OnMouseOver()
        {
            if (PlayerInput.IgnoreMouseInterface)
            {
                return;
            }

            Main.LocalPlayer.mouseInterface = true;

            if (!Main.mouseLeft || !Main.mouseLeftRelease)
            {
                return;
            }

            Main.mouseLeftRelease = false;

            if (timeLeft > 30 && timeLeft < 255)
            {
                timeLeft = 30;
            }
        }

        public void PushAnchor(ref Vector2 positionAnchorBottom)
        {
            positionAnchorBottom.Y -= 180f * Opacity;
        }
    }
}
