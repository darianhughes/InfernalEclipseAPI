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

            //This message should always popup upon entering a world if they are playing the mod pack.
            if (ModLoader.TryGetMod("ThoriumRework", out Mod rework))
            {
                Main.NewText("NOTICE: It is detected that you have Thorium Bosses Reworked Installed! Make sure the Health, Damage, & Speed multipliers are set to 1 in the compatability config (unless you want a harder experience) as this mod automatically adjusts the Thorium bosses when the Infernum difficulty is active.", 255, 255, 0);
            }

            //Alerts the player if they have Fargo's Souls enabled.
            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSouls)) 
            {
                Main.NewText("WARNING: It is detected that you have the Fargo's Souls Mod Installed! Please note that Infernum Mode is not compatiable with Eternity Mode and you may expirence major bugs if you have both enabled.", 255, 0, 0);
            }

            if (ModLoader.TryGetMod("CalamityMinus", out Mod calMinus))
            {
                Main.NewText("NOTICE: It is detected that you have the Calamity Minus Mod Installed! This mod already includes all of the mods features while you have Calamaity Balance Changes on in the config.", 255, 255, 06);
            }

            if (ModLoader.TryGetMod("CalBalChange", out Mod calBal))
            {
                Main.NewText("NOTICE: It is detected that you have the Calamity Balance Changes Mod Installed! This mod already includes most of the mods features that are listed on their Steam Workshop page, while you have Calamaity Balance Changes on in the config. Calamity Balance Changes also adds a bunch of unlisted changes which might make your playthough worse than intended.", 255, 255, 06);
            }

            if (InfernumActive.InfernumActive)
            {
                Main.NewText("The prodigy has returned to face the Infernal Eclipse...", 95, 06, 06);
                //SoundEngine.PlaySound(InfernumMode.Assets.Sounds.InfernumSoundRegistry.ModeToggleLaugh, this.Player.Center);
            }
            else if (InfernalConfig.Instance.InfernumModeForced);
            {
                Main.NewText("Infernal energy has been infused into this world...", 95, 06, 06);
                SoundEngine.PlaySound(InfernumMode.Assets.Sounds.InfernumSoundRegistry.ModeToggleLaugh, this.Player.Center);
                WorldSaveSystem.InfernumModeEnabled = true;
            }

            DownedBossSystem.startedBossRushAtLeastOnce = false;
        }
    }
}
