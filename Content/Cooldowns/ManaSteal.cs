using CalamityMod;
using CalamityMod.Cooldowns;
using InfernalEclipseAPI.Core.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Cooldowns
{
    public class ManaSteal : CooldownHandler
    {
        public static new string ID => "ManaSteal";

        private float manaStealCap => Main.expertMode ? 40f : 50f;
        public override bool ShouldDisplay
        {
            get
            {
                float value = instance.player.GetModPlayer<InfernalPlayer>().manaSteal;
                return value < manaStealCap;
            }
        }
        public override LocalizedText DisplayName => Language.GetOrRegister("InfernalEclipseAPI.UI.Cooldowns.ManaSteal");
        public override string Texture => "CalamityMod/Cooldowns/Starburst";

        public override bool CanTickDown => false;
        public override Color OutlineColor => instance.player!.GetModPlayer<InfernalPlayer>().manaSteal < 0
            ? new Color(255, 142, 165)
            : new Color(255, 142, 165);
        public override Color CooldownStartColor => instance.player!.GetModPlayer<InfernalPlayer>().manaSteal < 0
            ? new Color(145, 59, 59)
            : new Color(255, 181, 181);
        public override Color CooldownEndColor => CooldownStartColor;
        private Color TextColor => Color.White;
        private Color TextBorderColor => new Color(40, 0, 0);
        public override void DrawExpanded(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
        {
            base.DrawExpanded(spriteBatch, position, opacity, scale);
            var value = instance.player.GetModPlayer<InfernalPlayer>().manaSteal;
            bool negate = value < 0;
            var valueToMeasure = Math.Abs(value);
            float Xoffset = valueToMeasure > 9 ? valueToMeasure > 99 ? -12.5f : -10f : -5;
            if (negate)
                Xoffset -= 8;
            CalamityUtils.DrawBorderStringEightWay(spriteBatch, FontAssets.MouseText.Value, (value).ToString("#"), position + new Vector2(Xoffset, 8) * scale, TextColor, TextBorderColor, scale);
        }

        public override void DrawCompact(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
        {
            base.DrawCompact(spriteBatch, position, opacity, scale);
            var value = instance.player.GetModPlayer<InfernalPlayer>().manaSteal;
            bool negate = value < 0;
            var valueToMeasure = Math.Abs(value);
            float Xoffset = valueToMeasure > 9 ? valueToMeasure > 99 ? -12.5f : -10f : -5;
            if (negate)
                Xoffset -= 8;
            CalamityUtils.DrawBorderStringEightWay(spriteBatch, FontAssets.MouseText.Value, (value).ToString("#"), position + new Vector2(Xoffset, 8) * scale, TextColor, TextBorderColor, scale);
        }
    }
}
