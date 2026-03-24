using CalamityMod;
using CalamityMod.Buffs.Alcohol;
using InfernalEclipseAPI.Core.Systems;
using SOTS;
using Terraria.Localization;

namespace InfernalEclipseAPI.Core.Players.SOTSPlayerOverrides
{
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    public class SOTSPlayerAdjustments : ModPlayer
    {
        public string bossMessage = "";
        public bool royalJelly;
        public bool glowSpores;
        public bool sandwich;
        public bool glowJelly;
        public bool alchemistsCharm;


        public override void ResetEffects()
        {
            SOTSPlayer sotsPlayer = SOTSPlayer.ModPlayer(Player);

            if (sotsPlayer.VigorDashes > 25)
            {
                sotsPlayer.VigorDashes = 25;
            }

            if (Player.GetModPlayer<InfernalPlayer>().singularityCore)
            {
                Player.VoidPlayer().voidRegenSpeed += 0.1f;
            }

            royalJelly = sandwich = glowJelly = alchemistsCharm = false;
        }

        public override void UpdateEquips()
        {
            SOTSPlayer sotsPlayer = SOTSPlayer.ModPlayer(Player);

            if (alchemistsCharm)
            {
                sotsPlayer.additionalHeal += 100;
                sotsPlayer.additionalPotionMana += 100;
            }
            else if (glowJelly)
            {
                sotsPlayer.additionalHeal += 40;
                sotsPlayer.additionalPotionMana += 40;
            }
            else if (glowSpores)
            {
                sotsPlayer.additionalPotionMana += 40;
            }

            if (sandwich)
            {
                sotsPlayer.additionalHeal += 40;
            }
            else if (royalJelly && !glowJelly && !alchemistsCharm)
            {
                sotsPlayer.additionalHeal += 40;
            }
        }

        public override void PostUpdateEquips()
        {
            SOTSPlayer sotsPlayer = SOTSPlayer.ModPlayer(Player);

            if (sotsPlayer.InverseDiamondRing)
            {
                Player.ClearBuff(ModContent.BuffType<GrapeBeerBuff>());
                Player.Calamity().grapeBeer = false;
            }

            if (Player.Calamity().grapeBeer)
            {
                sotsPlayer.CritBonusDamage = (int)(sotsPlayer.CritBonusDamage * 0.25f);
                sotsPlayer.CritBonusMultiplier *= 0.75f;
                sotsPlayer.CritCurseFire = false;
                sotsPlayer.CritFire = false;
                sotsPlayer.CritFrost = false;
            }
        }

        public override void PostUpdate()
        {
            if (Player.GetModPlayer<InfernalPlayer>().voidMagePrevention > 0)
                Player.SOTSPlayer().VMincubator = false;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            VoidMagePreventedOnHit();
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            VoidMagePreventedOnHit();
        }

        private void VoidMagePreventedOnHit()
        {
            InfernalPlayer mp = Player.GetModPlayer<InfernalPlayer>();
            if (mp.voidMagePrevention > 0 && mp.incubatorTextTime == 0)
            {
                Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.TimeFreezePrevention." + bossMessage));
                mp.incubatorTextTime = 60 * 60;
            }
        }
    }
}
