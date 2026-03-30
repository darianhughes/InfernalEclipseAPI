using Terraria.Graphics.Effects;

namespace InfernalEclipseAPI.YharimEX
{
    public class YharimEXSystem : ModSystem
    {
        public static YharimEXSystem Instance;

        public YharimEXSystem() => Instance = this;

        public override void Load()
        {
            SkyManager.Instance["InfernalEclipseAPI:YharimEXBoss"] = new YharimEXSky();
        }
    }
}
