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


namespace InfernalEclipseAPI.Common.Balance
{
    public class InfernumBossFix : BalancingManager
    {
        public const BalancePriority InfernumModeBasePriority = (BalancePriority)10;
        public const BalancePriority InfernumModeHigherPriority = (BalancePriority)10;
        public static readonly
#nullable disable
        Func<bool> InfernumModeCondition = (Func<bool>)(() => InfernumBossFix.CanUseCustomAIs && !BossRushEvent.BossRushActive);
        public static readonly Func<bool> InfernumModeBossRushCondition = (Func<bool>)(() => InfernumBossFix.CanUseCustomAIs && BossRushEvent.BossRushActive);
        public static readonly Func<bool> InfernumHardmodeCondition = (Func<bool>)(() => InfernumBossFix.CanUseCustomAIs && Main.hardMode && !BossRushEvent.BossRushActive);

        public static bool CanUseCustomAIs => WorldSaveSystem.InfernumModeEnabled;

        public static Func<bool> InfernumFirstMechCondition
        {
            get
            {
                return (Func<bool>)(() => InfernumBossFix.CanUseCustomAIs && InfernumBossFix.GetMechsDowned() == 0 && CalamityConfig.Instance.EarlyHardmodeProgressionRework && !BossRushEvent.BossRushActive);
            }
        }

        public static Func<bool> InfernumSecondMechCondition
        {
            get
            {
                return (Func<bool>)(() => InfernumBossFix.CanUseCustomAIs && InfernumBossFix.GetMechsDowned() == 1 && CalamityConfig.Instance.EarlyHardmodeProgressionRework && !BossRushEvent.BossRushActive);
            }
        }

        public static Func<bool> InfernumFinalMechCondition
        {
            get
            {
                return (Func<bool>)(() => InfernumBossFix.CanUseCustomAIs && InfernumBossFix.GetMechsDowned() >= 2 && !InfernumBossFix.InfernumFirstMechCondition() && !BossRushEvent.BossRushActive);
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
            return (int)((double)hp - (double)hp * 0.28571429848670959);
        }

        private static int AccountForExpertHP1Point6(int hp) => (int)((double)hp - (double)hp * 0.375);

        private static NPCHPBalancingChange CreateBaseChangeVanilla(int npcType, int hp)
        {
            return new NPCHPBalancingChange(npcType, InfernumBossFix.AccountForExpertHP1Point4(hp), (BalancePriority)10, InfernumBossFix.InfernumModeCondition);
        }

        private static NPCHPBalancingChange CreateBaseChangeModded(int npcType, int hp)
        {
            return new NPCHPBalancingChange(npcType, InfernumBossFix.AccountForExpertHP1Point6(hp), (BalancePriority)10, InfernumBossFix.InfernumModeCondition);
        }

        private static NPCHPBalancingChange CreateBossRushChange(int npcType, int hp)
        {
            return new NPCHPBalancingChange(npcType, InfernumBossFix.AccountForExpertHP1Point6(hp), (BalancePriority)10, InfernumBossFix.InfernumModeBossRushCondition);
        }

        public override IEnumerable<NPCHPBalancingChange> GetNPCHPBalancingChanges()
        {
            int Boost = 1;
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<KingSlimeJewelRuby>(), 2000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<DesertScourgeHead>(), 7200 * Boost);
            yield return new NPCHPBalancingChange(ModContent.NPCType<GiantClam>(), InfernumBossFix.AccountForExpertHP1Point6(4100 * Boost), (BalancePriority)10, (Func<bool>)(() => InfernumBossFix.CanUseCustomAIs && !Main.hardMode));
            yield return new NPCHPBalancingChange(ModContent.NPCType<GiantClam>(), InfernumBossFix.AccountForExpertHP1Point6(16200 * Boost), (BalancePriority)10, (Func<bool>)(() => InfernumBossFix.CanUseCustomAIs && Main.hardMode));
            yield return InfernumBossFix.CreateBaseChangeVanilla(50, 4200 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(4, 6100 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(266, 9389 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.Crabulon.Crabulon>(), 10600 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(13, 4000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(14, 4000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(15, 4000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(564, 5000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.HiveMind.HiveMind>(), 8100 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<PerforatorHive>(), 9176 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<PerforatorHeadSmall>(), 2000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<PerforatorHeadMedium>(), 2735 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<PerforatorHeadLarge>(), 3960 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(222, 9669 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(668, 22844 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(35, 9860 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<SlimeGodCore>(), 3275 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CrimulanPaladin>(), 7464 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CrimulanPaladin>(), 7464 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(114, 3232 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(113, 10476 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<ThiccWaifu>(), 18000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(576, 15100 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(657, 30000 * Boost);
            yield return new NPCHPBalancingChange(126, InfernumBossFix.AccountForExpertHP1Point4((int)(23960.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(126, InfernumBossFix.AccountForExpertHP1Point4((int)(26955.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(126, InfernumBossFix.AccountForExpertHP1Point4(29950 * Boost), (BalancePriority)10, InfernumBossFix.InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(125, InfernumBossFix.AccountForExpertHP1Point4((int)(23960.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(125, InfernumBossFix.AccountForExpertHP1Point4((int)(26955.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(125, InfernumBossFix.AccountForExpertHP1Point4(29950 * Boost), (BalancePriority)10, InfernumBossFix.InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange((int)sbyte.MaxValue, InfernumBossFix.AccountForExpertHP1Point4((int)(22400.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange((int)sbyte.MaxValue, InfernumBossFix.AccountForExpertHP1Point4((int)(25200.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange((int)sbyte.MaxValue, InfernumBossFix.AccountForExpertHP1Point4(28000 * Boost), (BalancePriority)10, InfernumBossFix.InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(130, InfernumBossFix.AccountForExpertHP1Point4((int)(17600.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(130, InfernumBossFix.AccountForExpertHP1Point4((int)(19800.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(130, InfernumBossFix.AccountForExpertHP1Point4(22000), (BalancePriority)10, InfernumBossFix.InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(129, InfernumBossFix.AccountForExpertHP1Point4((int)(17600.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(129, InfernumBossFix.AccountForExpertHP1Point4((int)(19800.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(129, InfernumBossFix.AccountForExpertHP1Point4(22000), (BalancePriority)10, InfernumBossFix.InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(128 /*0x80*/, InfernumBossFix.AccountForExpertHP1Point4((int)(17600.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(128 /*0x80*/, InfernumBossFix.AccountForExpertHP1Point4((int)(19800.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(128 /*0x80*/, InfernumBossFix.AccountForExpertHP1Point4(22000), (BalancePriority)10, InfernumBossFix.InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(131, InfernumBossFix.AccountForExpertHP1Point4((int)(17600.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(131, InfernumBossFix.AccountForExpertHP1Point4((int)(19800.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(131, InfernumBossFix.AccountForExpertHP1Point4(22000), (BalancePriority)10, InfernumBossFix.InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(134, InfernumBossFix.AccountForExpertHP1Point4((int)(88800.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(134, InfernumBossFix.AccountForExpertHP1Point4((int)(99900.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(134, InfernumBossFix.AccountForExpertHP1Point4(111000 * Boost), (BalancePriority)10, InfernumBossFix.InfernumFinalMechCondition);
            yield return new NPCHPBalancingChange(139, InfernumBossFix.AccountForExpertHP1Point4((int)(560.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumFirstMechCondition);
            yield return new NPCHPBalancingChange(139, InfernumBossFix.AccountForExpertHP1Point4((int)(630.0 * (double)Boost)), (BalancePriority)10, InfernumBossFix.InfernumSecondMechCondition);
            yield return new NPCHPBalancingChange(139, InfernumBossFix.AccountForExpertHP1Point4(700 * Boost), (BalancePriority)10, InfernumBossFix.InfernumFinalMechCondition);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.BrimstoneElemental.BrimstoneElemental>(), 85515 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamitasClone>(), 76250 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<Cataclysm>(), 20600 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<Catastrophe>(), 13000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<SoulSeeker>(), 2100 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(262, 110500 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.Leviathan.Leviathan>(), 102097 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<AquaticAberration>(), 900 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<Anahita>(), 71000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<AureusSpawn>(), 2500 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.AstrumAureus.AstrumAureus>(), 144074 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(565, 24500 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(551, 66500 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(245, 198700 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(246, 198700 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(249, 198700 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(247, 198700 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(248, 198700 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.PlaguebringerGoliath.PlaguebringerGoliath>(), 136031 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.GreatSandShark.GreatSandShark>(), 107400 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(370, 100250 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(636, 220056 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<RavagerHead>(), 18000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<DevilFish>(), 5000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<Eidolist>(), 20000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(439, 104000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(521, 9020 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(454, 36500 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<AstrumDeusHead>(), 287000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(397, 50000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(396, 61000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeVanilla(398, 135000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<ProfanedGuardianCommander>(), 132000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<ProfanedGuardianDefender>(), 80000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<ProfanedGuardianHealer>(), 80000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<Bumblefuck>(), 256000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<Bumblefuck2>(), 14300 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<ProfanedRocks>(), 2300 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.Providence.Providence>(), 900000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<StormWeaverHead>(), 646400 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.CeaselessVoid.CeaselessVoid>(), 455525 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<DarkEnergy>(), 5000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.Signus.Signus>(), 546102 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.Polterghast.Polterghast>(), 544440 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.OldDuke.OldDuke>(), 936000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<DevourerofGodsHead>(), 1776500 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.Yharon.Yharon>(), 968420 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<PrimordialWyrmHead>(), 1260750 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<ThanatosHead>(), 2400000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<AresBody>(), 2560000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Artemis.Artemis>(), 2400000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Apollo.Apollo>(), 2400000 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<SupremeCataclysm>(), 537200 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<SupremeCatastrophe>(), 537200 * Boost);
            yield return InfernumBossFix.CreateBaseChangeModded(ModContent.NPCType<CalamityMod.NPCs.SupremeCalamitas.SupremeCalamitas>(), 3141592 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<KingSlimeJewelRuby>(), 1176000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<DesertScourgeHead>(), 1185000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(50, 420000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(4, 770000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(266, 689000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.Crabulon.Crabulon>(), 1776000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(13, 376810 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(14, 376810 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(15, 376810 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.HiveMind.HiveMind>(), 606007 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<PerforatorHive>(), 420419 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<PerforatorHeadSmall>(), 239000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<PerforatorHeadMedium>(), 330000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<PerforatorHeadLarge>(), 296500 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(222, 611100 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(668, 927000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(35, 2508105 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<SlimeGodCore>(), 486500 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CrimulanPaladin>(), 213720 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CrimulanPaladin>(), 213720 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(114, 140800 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(113, 854000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(657, 840000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(126, 833760 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(125, 840885 * Boost);
            yield return InfernumBossFix.CreateBossRushChange((int)sbyte.MaxValue, 989515 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(130, 346000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(129, 346000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(128 /*0x80*/, 346000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(131, 346000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(134, 1110580 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(139, 30000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.BrimstoneElemental.BrimstoneElemental>(), 1105000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CalamitasClone>(), 985000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<Cataclysm>(), 193380 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<Catastrophe>(), 176085 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<SoulSeeker>(), 24000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(262, 575576 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.Leviathan.Leviathan>(), 1200000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<AquaticAberration>(), 900 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<Anahita>(), 450000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.AstrumAureus.AstrumAureus>(), 1230680 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(245, 1250000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(246, 1250000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(249, 1250000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(247, 1250000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(248, 1250000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.PlaguebringerGoliath.PlaguebringerGoliath>(), 666666 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(370, 1330000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(636, 2960000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<RavagerHead>(), 999 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(439, 727272);
            yield return InfernumBossFix.CreateBossRushChange(521, 999 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(454, 999 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<AstrumDeusHead>(), 930000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(397, 400000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(396, 661110 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(398, 1600000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<ProfanedGuardianCommander>(), 720000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<ProfanedGuardianDefender>(), 205000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<ProfanedGuardianHealer>(), 205000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<Bumblefuck>(), 860000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<Bumblefuck2>(), 999 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<ProfanedRocks>(), 7500 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.Providence.Providence>(), 3900000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<StormWeaverHead>(), 1232100 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.CeaselessVoid.CeaselessVoid>(), 1040000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<DarkEnergy>(), 19000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.Signus.Signus>(), 848210 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.Polterghast.Polterghast>(), 1575910 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.OldDuke.OldDuke>(), 1600000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<DevourerofGodsHead>(), 2960000 * Boost);
            yield return InfernumBossFix.CreateBossRushChange(ModContent.NPCType<CalamityMod.NPCs.Yharon.Yharon>(), 1618950 * Boost);
        }
    }
}
