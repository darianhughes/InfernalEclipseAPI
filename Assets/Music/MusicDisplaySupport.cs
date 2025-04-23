using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using InfernalEclipseAPI;

namespace InfernalEclipseAPI.Assets.Music
{
    public class MusicDisplaySupport : ModSystem
    {
        public override void PostSetupContent()
        {
          MusicDisplaySetup();
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
        }
    }
}
