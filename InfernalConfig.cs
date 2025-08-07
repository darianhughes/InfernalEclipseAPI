using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.CalPlayer.DrawLayers;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.Config;

namespace InfernalEclipseAPI
{
    internal class InfernalConfig : ModConfig
    {
        public static InfernalConfig Instance;

        public override ConfigScope Mode => ConfigScope.ServerSide;

        #region Balance Changess
        [Header("BalanceChanges")]

        [DefaultValue(true)]
        public bool BossKillCheckOnOres { get; set; }

        [DefaultValue(false)]
        [ReloadRequired]
        public bool PreventBossCheese { get; set; }

        [DefaultValue(true)]
        public bool AdditonalVanillaBossAdjustments { get; set; }

        [DefaultValue(true)]
        public bool VanillaBalanceChanges { get; set; }

        [DefaultValue(true)]
        [ReloadRequired]
        public bool CalamityBalanceChanges { get; set; }

        [DefaultValue(true)]
        [ReloadRequired]
        public bool ThoriumBalanceChangess { get; set; }

        [DefaultValue(true)]
        [ReloadRequired]
        public bool SOTSBalanceChanges { get; set; }

        [DefaultValue(true)]
        [ReloadRequired]
        public bool ConsolariaBalanceChanges { get; set; }

        [DefaultValue(false)]
        [ReloadRequired]
        public bool CalamityRecipeTweaks { get; set; }

        [DefaultValue(true)]
        [ReloadRequired]
        public bool BloodOrbPotionDuplication { get; set; }

        [DefaultValue(false)]
        [ReloadRequired]
        public bool DisableBloodOrbPotions { get; set; }

        [DefaultValue(true)]
        [ReloadRequired]
        public bool InfernumRecipeTweaks { get; set; }

        [DefaultValue(true)]
        public bool SOTSThrowerToRogue { get; set; }

        [DefaultValue(true)]
        [ReloadRequired]
        public bool MergeCraftingTrees { get; set; }

        [DefaultValue(true)]
        public bool ChanageWeaponClasses { get; set; }

        #endregion

        #region Boss Rush Changes
        [Header("BossRushChanges")]

        [DefaultValue(true)]
        [ReloadRequired]
        public bool DreadnautillusInBossRush { get; set; }

        [DefaultValue(false)]
        [ReloadRequired]
        public bool BetsyInBossRush { get; set; }

        [DefaultValue(true)]
        [ReloadRequired]
        public bool TerraBladeBossInBossRush { get; set;}

        [DefaultValue(true)]
        [ReloadRequired]
        public bool WrathoftheGodsBossesInBossRush { get; set; }

        [DefaultValue(false)]
        public bool ForceFullXerocDialogue { get; set; }


        #endregion

        #region Miscellaneous
        [Header ("Miscellaneous")]
        [DefaultValue(false)]
        public bool InfernumModeForced { get; set; }

        [DefaultValue(true)]
        [ReloadRequired]
        public bool DisableUnfinisedContent { get; set; }

        [DefaultValue(true)]
        [ReloadRequired]
        public bool DisableDuplicateContent { get; set; }

        //[DefaultValue(true)]
        //public bool PlayInfernumExoMechThemeWithWoTM {  get; set; }

        [DefaultValue(true)]
        public bool DisplayWorldEntryMessages { get; set; }

        [DefaultValue(true)]
        [ReloadRequired]
        public bool AdjustAdvisorSpawnConditions { get; set; }

        [DefaultValue(true)]
        [ReloadRequired]
        public bool UseAprilFoolsMutant { get; set; }

        [DefaultValue(true)]
        public bool EnableInterlude4 { get; set; }

        #endregion
    }
}
