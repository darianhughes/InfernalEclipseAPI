using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoxusBoss.Core.CrossCompatibility.Inbound.BossChecklist;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems
{
    public class ModIntegrationsSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            MusicDisplaySetup();
            BossChecklistSetup();
        }
        private void MusicDisplaySetup()
        {
            ModLoader.TryGetMod("MusicDisplay", out Mod musicDisplay);
            if (musicDisplay is null)
            {
              return;
            }

            musicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot("InfernalEclipseAPI/Assets/Music/tier6"), "Descent Of Divinities", "psykomatic", "Infernal Eclipse of Ragnarok");
            musicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot("InfernalEclipseAPI/Assets/Music/tier5"), "Omiscience Of Gods", "TheTrester", "Infernal Eclipse of Ragnarok");
            musicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot("InfernalEclipseAPI/Assets/Music/TWISTEDGARDENRemix"), "TWISTED GARDEN [Remix]", "Kuudray", "Infernal Eclipse of Ragnarok");
        }

        private void BossChecklistSetup()
        {
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
                return;

            Func<bool> downedDreadnautilus = () => InfernalDownedBossSystem.downedDreadNautilus;

            bossChecklist.Call(
                "AddBoss",                                      
                7.9f,                                           
                downedDreadnautilus,                            
                "Use a Bloody Tear at night during a Blood Moon.", 
                "The Dreadnautilus retreats beneath the crimson waves...",
                NPCID.BloodNautilus
            );
        }
    }
}
