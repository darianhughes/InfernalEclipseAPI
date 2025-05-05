using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using InfernumMode.Content.BossIntroScreens;
using InfernumMode.Content.BossIntroScreens.InfernumScreens;
using Terraria.ModLoader;
using Terraria;
using Xna = Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;


namespace InfernalEclipseAPI.Common.InfernumScreens
{
    public class BossRushIntroScreen1 : BaseIntroScreen
    {
        public override TextColorData TextColor => Xna.Color.Red;
        public override bool TextShouldBeCentered => true;
        public override bool ShouldCoverScreen => false;
        public override bool CaresAboutBossEffectCondition => false;
        public override int AnimationTime => 210;
        public override LocalizedText TextToDisplay => Language.GetText("Mods.InfernalEclipseAPI.BossRushIntroText1");
        public override SoundStyle? SoundToPlayWithTextCreation => new SoundStyle("CalamityMod/Sounds/Custom/BossRush/BossRushTier2Transition");
        public override SoundStyle? SoundToPlayWithLetterAddition => SoundID.Item100;
        public override bool CanPlaySound => LetterDisplayCompletionRatio(AnimationTimer) >= 1f;
        public override bool ShouldBeActive()
        {
            Mod mod;
            bool flag = false;
            int num1 = 0, num2 = 0;

            if (ModLoader.TryGetMod("CalamityMod", out mod))
            {
                object result = mod.Call("GetDifficultyActive", "BossRush");
                if (result is bool b)
                {
                    flag = b;
                    num1 = 1;
                }
            }
            num2 = flag ? 1 : 0;
            if ((num1 & num2) != 0)
            {
                return true;
            }
            return false;
        }
        public override float LetterDisplayCompletionRatio(int animationTimer)
        {
            float completionRatio = Utils.GetLerpValue(TextDelayInterpolant, 0.92f, animationTimer / (float)AnimationTime, true);

            int startOfLargeTextIndex = TextToDisplay.Value.IndexOf('\n');
            int currentIndex = (int)(completionRatio * TextToDisplay.Value.Length);
            if (currentIndex >= startOfLargeTextIndex)
                completionRatio = 1f;

            return completionRatio;
        }
        public override void DoCompletionEffects()
        {
            AnimationTimer = 0;
        }
    }
}
