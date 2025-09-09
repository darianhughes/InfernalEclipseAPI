using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace InfernalEclipseAPI.Core
{
    public class InfernalEclipseMenuTheme : ModMenu
    {
        public override string DisplayName => "Infernal Eclipse of Ragnarok";

        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("InfernalEclipseAPI/Assets/Textures/Menu/IEoRLogoFull", AssetRequestMode.ImmediateLoad);

        public override int Music => MusicLoader.GetMusicSlot("CalamityModMusic/Sounds/Music/BrimstoneCrags");

        public override void OnSelected()
        {
            //Main.eclipse = true;
        }

        public override void OnDeselected()
        {
            //Main.eclipse = false;
        }

        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("CalamityMod/AstralDesertSurfaceBGStyle");

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            logoScale *= 0.55f; // 75% size
            return true;        // continue with default logo draw using modified scale
        }
    }
}
