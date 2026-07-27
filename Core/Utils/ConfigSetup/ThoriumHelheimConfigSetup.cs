using ThoriumRework;

namespace InfernalEclipseAPI.Core.Utils.ConfigSetup
{
    [JITWhenModsEnabled("ThoriumRework")]
    [ExtendsFromMod("ThoriumRework")]
    public static class ThoriumHelheimConfigSetup
    {
        public static void DisableItemReworks()
        {
            ModContent.GetInstance<ItemReworksConfig>().ChampionsRebuttal = false;
        }
    }
}
