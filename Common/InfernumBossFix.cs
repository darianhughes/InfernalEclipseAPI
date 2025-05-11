using CalamityMod;
using CalamityMod.Events;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.CeaselessVoid;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.PrimordialWyrm;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.StormWeaver;
using CalamityMod.NPCs.SunkenSea;
using CalamityMod.NPCs.SupremeCalamitas;
using InfernumMode.Core.GlobalInstances.Systems;
using Luminance.Core.Balancing;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using tModPorter;


namespace InfernalEclipseAPI.Common
{
    public class InfernumBossFix : BalancingManager
    {
        public const BalancePriority InfernumModeBasePriority = (BalancePriority)10;
        public const BalancePriority InfernumModeHigherPriority = (BalancePriority)10;
        public static readonly
#nullable disable
        Func<bool> InfernumModeCondition = () => CanUseCustomAIs && !BossRushEvent.BossRushActive;
        public static readonly Func<bool> InfernumModeBossRushCondition = () => CanUseCustomAIs && BossRushEvent.BossRushActive;
        public static readonly Func<bool> InfernumHardmodeCondition = () => CanUseCustomAIs && Main.hardMode && !BossRushEvent.BossRushActive;

        public static bool CanUseCustomAIs => WorldSaveSystem.InfernumModeEnabled;

        public static Func<bool> InfernumFirstMechCondition
        {
            get
            {
                return () => CanUseCustomAIs && GetMechsDowned() == 0 && CalamityConfig.Instance.EarlyHardmodeProgressionRework && !BossRushEvent.BossRushActive;
            }
        }

        public static Func<bool> InfernumSecondMechCondition
        {
            get
            {
                return () => CanUseCustomAIs && GetMechsDowned() == 1 && CalamityConfig.Instance.EarlyHardmodeProgressionRework && !BossRushEvent.BossRushActive;
            }
        }

        public static Func<bool> InfernumFinalMechCondition
        {
            get
            {
                return () => CanUseCustomAIs && GetMechsDowned() >= 2 && !InfernumFirstMechCondition() && !BossRushEvent.BossRushActive;
            }
        }

        private static int GetMechsDowned()
        {
            int mechsDowned = 0;
            if (NPC.downedMechBoss1)
                ++mechsDowned;
            if (NPC.downedMechBoss2)
                ++mechsDowned;
            if (NPC.downedMechBoss3)
                ++mechsDowned;
            return mechsDowned;
        }

        private static int AccountForExpertHP1Point4(int hp)
        {
            return (int)(hp - hp * 0.28571429848670959);
        }

        private static int AccountForExpertHP1Point6(int hp) => (int)(hp - hp * 0.375);

        private static NPCHPBalancingChange CreateBaseChangeVanilla(int npcType, int hp)
        {
            return new NPCHPBalancingChange(npcType, AccountForExpertHP1Point4(hp), (BalancePriority)10, InfernumModeCondition);
        }

        private static NPCHPBalancingChange CreateBaseChangeModded(int npcType, int hp)
        {
            return new NPCHPBalancingChange(npcType, AccountForExpertHP1Point6(hp), (BalancePriority)10, InfernumModeCondition);
        }

        private static NPCHPBalancingChange CreateBossRushChange(int npcType, int hp)
        {
            return new NPCHPBalancingChange(npcType, AccountForExpertHP1Point6(hp), (BalancePriority)10, InfernumModeBossRushCondition);
        }

        public override IEnumerable<NPCHPBalancingChange> GetNPCHPBalancingChanges()
        {
            int Boost = 1;
            yield return CreateBaseChangeModded(ModContent.NPCType<KingSlimeJewelRuby>(), 2000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<DesertScourgeHead>(), 7200 * Boost);
            yield return new NPCHPBalancingChange(ModContent.NPCType<GiantClam>(), AccountForExpertHP1Point6(4100 * Boost), (BalancePriority)10, () => CanUseCustomAIs && !Main.hardMode);
            yield return new NPCHPBalancingChange(ModContent.NPCType<GiantClam>(), AccountForExpertHP1Point6(16200 * Boost), (BalancePriority)10, () => CanUseCustomAIs && Main.hardMode);
            yield return CreateBaseChangeVanilla(50, 4200 * Boost);
            yield return CreateBaseChangeVanilla(4, 6100 * Boost);
            yield return CreateBaseChangeVanilla(266, 9389 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.Crabulon.Crabulon>(), 10600 * Boost);
            yield return CreateBaseChangeVanilla(13, 4000 * Boost);
            yield return CreateBaseChangeVanilla(14, 4000 * Boost);
            yield return CreateBaseChangeVanilla(15, 4000 * Boost);
            yield return CreateBaseChangeVanilla(564, 5000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.HiveMind.HiveMind>(), 8100 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<PerforatorHive>(), 9176 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<PerforatorHeadSmall>(), 2000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<PerforatorHeadMedium>(), 2735 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<PerforatorHeadLarge>(), 3960 * Boost);
            yield return CreateBaseChangeVanilla(222, 9669 * Boost);
            yield return CreateBaseChangeVanilla(668, 22844 * Boost);
            yield return CreateBaseChangeVanilla(35, 9860 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<SlimeGodCore>(), 3275 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CrimulanPaladin>(), 7464 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CrimulanPaladin>(), 7464 * Boost);
            yield return CreateBaseChangeVanilla(114, 3232 * Boost);
            yield return CreateBaseChangeVanilla(113, 10476 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<ThiccWaifu>(), 18000 * Boost);
            yield return CreateBaseChangeModded(576, 15100 * Boost);
            yield return CreateBaseChangeModded(657, 30000 * Boost);
            yield return new NPCHPBalancingChange(126, AccountForExpertHP1Point4((int)(23960.0 * Boost)), (BalancePriority)10, InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(126, AccountForExpertHP1Point4((int)(26955.0 * Boost)), (BalancePriority)10, InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(126, AccountForExpertHP1Point4(29950 * Boost), (BalancePriority)10, InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(125, AccountForExpertHP1Point4((int)(23960.0 * Boost)), (BalancePriority)10, InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(125, AccountForExpertHP1Point4((int)(26955.0 * Boost)), (BalancePriority)10, InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(125, AccountForExpertHP1Point4(29950 * Boost), (BalancePriority)10, InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(sbyte.MaxValue, AccountForExpertHP1Point4((int)(22400.0 * Boost)), (BalancePriority)10, InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(sbyte.MaxValue, AccountForExpertHP1Point4((int)(25200.0 * Boost)), (BalancePriority)10, InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(sbyte.MaxValue, AccountForExpertHP1Point4(28000 * Boost), (BalancePriority)10, InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(130, AccountForExpertHP1Point4((int)(17600.0 * Boost)), (BalancePriority)10, InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(130, AccountForExpertHP1Point4((int)(19800.0 * Boost)), (BalancePriority)10, InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(130, AccountForExpertHP1Point4(22000), (BalancePriority)10, InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(129, AccountForExpertHP1Point4((int)(17600.0 * Boost)), (BalancePriority)10, InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(129, AccountForExpertHP1Point4((int)(19800.0 * Boost)), (BalancePriority)10, InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(129, AccountForExpertHP1Point4(22000), (BalancePriority)10, InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(128 /*0x80*/, AccountForExpertHP1Point4((int)(17600.0 * Boost)), (BalancePriority)10, InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(128 /*0x80*/, AccountForExpertHP1Point4((int)(19800.0 * Boost)), (BalancePriority)10, InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(128 /*0x80*/, AccountForExpertHP1Point4(22000), (BalancePriority)10, InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(131, AccountForExpertHP1Point4((int)(17600.0 * Boost)), (BalancePriority)10, InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(131, AccountForExpertHP1Point4((int)(19800.0 * Boost)), (BalancePriority)10, InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(131, AccountForExpertHP1Point4(22000), (BalancePriority)10, InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(134, AccountForExpertHP1Point4((int)(88800.0 * Boost)), (BalancePriority)10, InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(134, AccountForExpertHP1Point4((int)(99900.0 * Boost)), (BalancePriority)10, InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(134, AccountForExpertHP1Point4(111000 * Boost), (BalancePriority)10, InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(139, AccountForExpertHP1Point4((int)(560.0 * Boost)), (BalancePriority)10, InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(139, AccountForExpertHP1Point4((int)(630.0 * Boost)), (BalancePriority)10, InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(139, AccountForExpertHP1Point4(700 * Boost), (BalancePriority)10, InfernumFinalMechCondition);
            yield return CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.BrimstoneElemental.BrimstoneElemental>(), 85515 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CalamitasClone>(), 76250 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<Cataclysm>(), 20600 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<Catastrophe>(), 13000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<SoulSeeker>(), 2100 * Boost);
            yield return CreateBaseChangeVanilla(262, 110500 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<Leviathan>(), 102097 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<AquaticAberration>(), 900 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<Anahita>(), 71000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<AureusSpawn>(), 2500 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<AstrumAureus>(), 144074 * Boost);
            yield return CreateBaseChangeVanilla(565, 24500 * Boost);
            yield return CreateBaseChangeVanilla(551, 66500 * Boost);
            yield return CreateBaseChangeVanilla(245, 198700 * Boost);
            yield return CreateBaseChangeVanilla(246, 198700 * Boost);
            yield return CreateBaseChangeVanilla(249, 198700 * Boost);
            yield return CreateBaseChangeVanilla(247, 198700 * Boost);
            yield return CreateBaseChangeVanilla(248, 198700 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.PlaguebringerGoliath.PlaguebringerGoliath>(), 136031 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.GreatSandShark.GreatSandShark>(), 107400 * Boost);
            yield return CreateBaseChangeVanilla(370, 100250 * Boost);
            yield return CreateBaseChangeVanilla(636, 220056 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<RavagerHead>(), 18000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<DevilFish>(), 5000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<Eidolist>(), 20000 * Boost);
            yield return CreateBaseChangeVanilla(439, 104000 * Boost);
            yield return CreateBaseChangeVanilla(521, 9020 * Boost);
            yield return CreateBaseChangeVanilla(454, 36500 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<AstrumDeusHead>(), 287000 * Boost);
            yield return CreateBaseChangeVanilla(397, 50000 * Boost);
            yield return CreateBaseChangeVanilla(396, 61000 * Boost);
            yield return CreateBaseChangeVanilla(398, 135000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<ProfanedGuardianCommander>(), 132000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<ProfanedGuardianDefender>(), 80000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<ProfanedGuardianHealer>(), 80000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<Bumblefuck>(), 256000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<Bumblefuck2>(), 14300 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<ProfanedRocks>(), 2300 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.Providence.Providence>(), 900000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<StormWeaverHead>(), 646400 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CeaselessVoid>(), 455525 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<DarkEnergy>(), 5000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.Signus.Signus>(), 546102 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.Polterghast.Polterghast>(), 544440 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.OldDuke.OldDuke>(), 936000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<DevourerofGodsHead>(), 1776500 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.Yharon.Yharon>(), 968420 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<PrimordialWyrmHead>(), 1260750 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<ThanatosHead>(), 2400000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<AresBody>(), 2560000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Artemis.Artemis>(), 2400000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Apollo.Apollo>(), 2400000 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<SupremeCataclysm>(), 537200 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<SupremeCatastrophe>(), 537200 * Boost);
            yield return CreateBaseChangeModded(ModContent.NPCType<SupremeCalamitas>(), 3141592 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<KingSlimeJewelRuby>(), 1176000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<DesertScourgeHead>(), 1185000 * Boost);
            yield return CreateBossRushChange(50, 420000 * Boost);
            yield return CreateBossRushChange(4, 770000 * Boost);
            yield return CreateBossRushChange(266, 689000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.Crabulon.Crabulon>(), 1776000 * Boost);
            yield return CreateBossRushChange(13, 376810 * Boost);
            yield return CreateBossRushChange(14, 376810 * Boost);
            yield return CreateBossRushChange(15, 376810 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.HiveMind.HiveMind>(), 606007 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<PerforatorHive>(), 420419 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<PerforatorHeadSmall>(), 239000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<PerforatorHeadMedium>(), 330000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<PerforatorHeadLarge>(), 296500 * Boost);
            yield return CreateBossRushChange(222, 611100 * Boost);
            yield return CreateBossRushChange(668, 927000 * Boost);
            yield return CreateBossRushChange(35, 2508105 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<SlimeGodCore>(), 486500 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<CrimulanPaladin>(), 213720 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<CrimulanPaladin>(), 213720 * Boost);
            yield return CreateBossRushChange(114, 140800 * Boost);
            yield return CreateBossRushChange(113, 854000 * Boost);
            yield return CreateBossRushChange(657, 840000 * Boost);
            yield return CreateBossRushChange(126, 833760 * Boost);
            yield return CreateBossRushChange(125, 840885 * Boost);
            yield return CreateBossRushChange(sbyte.MaxValue, 989515 * Boost);
            yield return CreateBossRushChange(130, 346000 * Boost);
            yield return CreateBossRushChange(129, 346000 * Boost);
            yield return CreateBossRushChange(128 /*0x80*/, 346000 * Boost);
            yield return CreateBossRushChange(131, 346000 * Boost);
            yield return CreateBossRushChange(134, 1110580 * Boost);
            yield return CreateBossRushChange(139, 30000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.BrimstoneElemental.BrimstoneElemental>(), 1105000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<CalamitasClone>(), 985000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<Cataclysm>(), 193380 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<Catastrophe>(), 176085 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<SoulSeeker>(), 24000 * Boost);
            yield return CreateBossRushChange(262, 575576 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<Leviathan>(), 1200000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<AquaticAberration>(), 900 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<Anahita>(), 450000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<AstrumAureus>(), 1230680 * Boost);
            yield return CreateBossRushChange(245, 1250000 * Boost);
            yield return CreateBossRushChange(246, 1250000 * Boost);
            yield return CreateBossRushChange(249, 1250000 * Boost);
            yield return CreateBossRushChange(247, 1250000 * Boost);
            yield return CreateBossRushChange(248, 1250000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.PlaguebringerGoliath.PlaguebringerGoliath>(), 666666 * Boost);
            yield return CreateBossRushChange(370, 1330000 * Boost);
            yield return CreateBossRushChange(636, 2960000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<RavagerHead>(), 999 * Boost);
            yield return CreateBossRushChange(439, 727272);
            yield return CreateBossRushChange(521, 999 * Boost);
            yield return CreateBossRushChange(454, 999 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<AstrumDeusHead>(), 930000 * Boost);
            yield return CreateBossRushChange(397, 400000 * Boost);
            yield return CreateBossRushChange(396, 661110 * Boost);
            yield return CreateBossRushChange(398, 1600000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<ProfanedGuardianCommander>(), 720000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<ProfanedGuardianDefender>(), 205000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<ProfanedGuardianHealer>(), 205000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<Bumblefuck>(), 860000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<Bumblefuck2>(), 999 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<ProfanedRocks>(), 7500 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.Providence.Providence>(), 3900000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<StormWeaverHead>(), 1232100 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<CeaselessVoid>(), 1040000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<DarkEnergy>(), 19000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.Signus.Signus>(), 848210 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.Polterghast.Polterghast>(), 1575910 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.OldDuke.OldDuke>(), 1600000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<DevourerofGodsHead>(), 2960000 * Boost);
            yield return CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.Yharon.Yharon>(), 1618950 * Boost);
        }
    }
}
