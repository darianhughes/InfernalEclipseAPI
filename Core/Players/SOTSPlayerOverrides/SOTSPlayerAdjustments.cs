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
