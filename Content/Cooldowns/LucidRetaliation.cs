using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Cooldowns
{
    [JITWhenModsEnabled("ThoriumRework")]
    [ExtendsFromMod("ThoriumRework")]
    public class LucidRetaliation : CooldownHandler
    {
        public static new string ID => "LucidRetaliation";

        public override bool ShouldDisplay => instance.player.HasBuff<ThoriumRework.Buffs.LucidRetaliation>();
        public override LocalizedText DisplayName => Language.GetOrRegister($"Mods.InfernalEclipseAPI.UI.Cooldowns.{ID}");
        public override string Texture => "InfernalEclipseAPI/Content/Cooldowns/TerminalLucidity";
        public override Color OutlineColor => Color.White;
        public override Color CooldownStartColor => Color.SkyBlue;
        public override Color CooldownEndColor => Color.Purple;
        public override SoundStyle? EndSound => SoundID.Shatter;
    }
}
