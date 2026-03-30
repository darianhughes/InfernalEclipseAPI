using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace InfernalEclipseAPI.YharimEX.Core.Players
{
    public class YharimEXPlayer : ModPlayer
    {
        public int MaxLifeReduction;
        int LifeReductionUpdateTimer;
        public int CurrentLifeReduction;
        public bool noDodge;
        public bool YharimPresence;
        public bool YharimPresenceBuffer;
        public int PresenseTogglerTimer;
        public bool HadYharimPresence;
        public bool YharimDesperation;
        public bool YharimFang;
        public int YharimEXNoUsingItems;

        public override void UpdateDead()
        {
            MaxLifeReduction = 0;
            CurrentLifeReduction = 0;
            YharimEXNoUsingItems = 0;
        }

        public override void ResetEffects()
        {
            noDodge = false;
            if (!YharimPresenceBuffer)
            {
                if (YharimPresence == false) PresenseTogglerTimer = 0;
                YharimPresence = YharimPresence && Player.HasBuff(ModContent.BuffType<TyrantPresenceBuff>());
            }
            YharimPresenceBuffer = false;
            HadYharimPresence = YharimPresence;
            YharimDesperation = false;
            YharimFang = false;

            if (YharimEXNoUsingItems > 0)
                YharimEXNoUsingItems--;
        }

        public void ManageLifeReduction()
        {
            if (LifeReductionUpdateTimer > 0)
            {
                const int threshold = 30;
                if (LifeReductionUpdateTimer++ > threshold)
                {
                    LifeReductionUpdateTimer = 1;

                    CurrentLifeReduction -= 5;
                    if (MaxLifeReduction > CurrentLifeReduction)
                        MaxLifeReduction = CurrentLifeReduction;
                    CombatText.NewText(Player.Hitbox, Color.DarkGreen, Language.GetTextValue($"Mods.{Mod.Name}.LifeUp"));
                }
            }

            if (CurrentLifeReduction > 0)
            {
                if (CurrentLifeReduction > Player.statLifeMax2 - 100)
                    CurrentLifeReduction = Player.statLifeMax2 - 100;
                Player.statLifeMax2 -= CurrentLifeReduction;
            }
        }

        public override void PostUpdateEquips()
        {
            if (noDodge)
            {
                Player.onHitDodge = false;
                Player.shadowDodge = false;
                Player.blackBelt = false;
                Player.brainOfConfusionItem = null;
            }
        }

        public int GetHealMultiplier(int heal)
        {
            float multiplier = 1f;
            if (YharimPresence)
                multiplier *= 0.5f;

            heal = (int)(heal * multiplier);

            return heal;
        }

        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            healValue = GetHealMultiplier(healValue);
        }

        public override void UpdateBadLifeRegen()
        {
            if (YharimPresence)
            {
                if (Player.lifeRegen > 5)
                    Player.lifeRegen = 5;
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (YharimPresence)
            {
                Player.statDefense /= 2;
                Player.endurance /= 2;
                Player.shinyStone = false;
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (YharimDesperation)
                modifiers.SourceDamage *= 2f;
        }
        public Rectangle GetPrecisionHurtbox()
        {
            Rectangle hurtbox = Player.Hitbox;
            hurtbox.X += hurtbox.Width / 2;
            hurtbox.Y += hurtbox.Height / 2;
            hurtbox.Width = Math.Min(hurtbox.Width, hurtbox.Height);
            hurtbox.Height = Math.Min(hurtbox.Width, hurtbox.Height);
            hurtbox.X -= hurtbox.Width / 2;
            hurtbox.Y -= hurtbox.Height / 2;
            return hurtbox;
        }
    }
}
