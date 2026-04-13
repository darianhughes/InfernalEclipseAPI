using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Cooldowns
{
    [JITWhenModsEnabled("ThoriumRework")]
    [ExtendsFromMod("ThoriumRework")]
    public class TerminalLucidity : CooldownHandler
    {
        public static new string ID => "TerminalLucidity";

        public override bool ShouldDisplay => instance.player.HasBuff<ThoriumRework.Buffs.TerminalLucidity>();
        public override LocalizedText DisplayName => Language.GetOrRegister($"Mods.InfernalEclipseAPI.UI.Cooldowns.{ID}");
        public override string Texture => "InfernalEclipseAPI/Content/Cooldowns/TerminalLucidity";
        public override Color OutlineColor => Color.White;
        public override Color CooldownStartColor => Color.Purple;
        public override Color CooldownEndColor => Color.Red;
        public override SoundStyle? EndSound => SoundID.DD2_BetsyFlameBreath;
    }
}
