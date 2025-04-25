using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using InfernumMode.Core.GlobalInstances.Systems;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;

namespace InfernalEclipseAPI.Common.Players
{
    public class InfernalPlayer : ModPlayer
    {
        public override void OnEnterWorld()
        {
            Main.NewText("Welcome to the Infernal Eclipse of Ragnarok Mod Pack!", 95, 06, 06);

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                //This message should always popup upon entering a world if they are playing the mod pack.
                if (ModLoader.TryGetMod("ThoriumRework", out Mod rework))
                {
                    Main.NewText("NOTICE: It is detected that you have Thorium Bosses Reworked enabled! Make sure the Health, Damage, & Speed multipliers are set to 1 in the compatability config (unless you want a harder experience) as this mod automatically adjusts the Thorium bosses when the Infernum difficulty is active.", 255, 255, 0);
                }
                else
                {
                    Main.NewText("WARNING: It is detected that you do not have Thorium Bosses Reworked enabled! This mod is heavily recommended as it rebalances Thorium Bosses to have mechanics more in-line with Infernum mode and Calamity in general.", 255, 0, 0);
                }

                if (!(ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok)))
                {
                    Main.NewText("WARNING: It is detected that you do not have Ragnarok enabled! Without this mod, Thorium & Calamity may not work well together. It is heavily recommended you enable this mod for a more complete & balanced experience", 255, 0, 0);
                }
            }

            //These messages should not appear if they are plaing the mod pack.
            if (!(ModLoader.TryGetMod("RevengeancePlus", out Mod revenge)))
            {
                Main.NewText("WARNING: It is detected that you do not have Revengeance Plus enabled! Without this mod, any bosses that aren't from Vanilla, Calamity, Infernum, or Thorium will not have Infernum mechanics or intros, and may not be included in Boss Rush. It is heavily recommended you enable this mod for a more complete & balanced experience", 255, 0, 0);
            }

            if (!(ModLoader.TryGetMod("EventTrophies", out Mod eventTrophy)))
            {
                Main.NewText("NOTICE: It is detected that you do not have More Trophies and Relics enabled! Without this mod, any bosses that aren't from Vanilla, Calamity, or Infernum may not have Calamity lore or Infernum intros. It is heavily recommended you enable this mod for a more complete & balanced experience", 255, 255, 0);
            }

            //Alerts the player if they have Fargo's Souls enabled.
            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSouls)) 
            {
                Main.NewText("WARNING: It is detected that you have the Fargo's Souls Mod enabled! Please note that Infernum Mode is not compatiable with Eternity Mode and you may expirence major bugs if you have both enabled.", 255, 0, 0);
            }

            //Alertes the player about WotG Mutliplayer incompatability
            if (ModLoader.TryGetMod("NoxusBoss", out Mod wotg))
            {
                Main.NewText("NOTICE: Calamity: Wrath of the Gods is currently multiplayer incompatiable. If you are wanting to play this world in multiplayer, please disable Wrath of the Gods first.", 255, 255, 0);
            }

            if (ModLoader.TryGetMod("CalamityMinus", out Mod calMinus))
            {
                Main.NewText("NOTICE: It is detected that you have the Calamity Minus Mod enabled! This mod already includes all of the mods features while you have Calamaity Balance Changes on in the config.", 255, 255, 06);
            }

            if (ModLoader.TryGetMod("CalBalChange", out Mod calBal))
            {
                Main.NewText("NOTICE: It is detected that you have the Calamity Balance Changes Mod enabled! This mod already includes most of the mods features that are listed on their Steam Workshop page, while you have Calamaity Balance Changes on in the config. Calamity Balance Changes also adds a bunch of unlisted changes which might make your playthough worse than intended.", 255, 255, 06);
            }

            if (InfernumActive.InfernumActive)
            {
                Main.NewText("The prodigy has returned to face the Infernal Eclipse...", 95, 06, 06);
                //SoundEngine.PlaySound(InfernumMode.Assets.Sounds.InfernumSoundRegistry.ModeToggleLaugh, this.Player.Center);
            }
            else if (InfernalConfig.Instance.InfernumModeForced)
            {
                Main.NewText("Infernal energy has been infused into this world...", 95, 06, 06);
                SoundEngine.PlaySound(InfernumMode.Assets.Sounds.InfernumSoundRegistry.ModeToggleLaugh, this.Player.Center);
                WorldSaveSystem.InfernumModeEnabled = true;
            }

            DownedBossSystem.startedBossRushAtLeastOnce = false;
        }
    }
}
