using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace InfernalEclipseAPI.Core.Configs
{
    [JITWhenModsEnabled("ContinentOfJourney")]
    [ExtendsFromMod("ContinentOfJourney")]
    public class HomewardConfig : ModConfig
    {
        public static HomewardConfig Instance;

        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Main")]
        [DefaultValue(true)]
        public bool DisplayHomewardWorldEntryMessages { get; set; }

        [Header("BossRushChanges")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool HomewardInBossRush { get; set; }
    }
}
