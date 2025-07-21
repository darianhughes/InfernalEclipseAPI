using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Artemis;
using InfernumMode;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.Ares;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ComboAttacks;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.Ares.AresBodyBehaviorOverride;
using static InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ArtemisAndApollo.ApolloBehaviorOverride;
using static InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ComboAttacks.ExoMechComboAttackContent;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.ExoMechs
{
    public class ExoMechsJitterFix : GlobalNPC
    {
        //public override bool InstancePerEntity => true;
        //public float[] NewLocals = new float[4];

        //public Vector2 RootPosition;
        //public override void SetDefaults(NPC npc)
        //{
        //    NewLocals = new float[4];
        //    RootPosition = npc.Center;
        //}
        //static bool IsAresCannon(NPC npc) =>
        //        npc.type == ModContent.NPCType<AresLaserCannon>() ||
        //        npc.type == ModContent.NPCType<AresPlasmaFlamethrower>() ||
        //        npc.type == ModContent.NPCType<AresPulseCannon>() ||
        //        npc.type == ModContent.NPCType<AresTeslaCannon>() ||
        //        npc.type == ModContent.NPCType<AresEnergyKatana>();
        //static int AresIndex => CalamityMod.NPCs.CalamityGlobalNPC.draedonExoMechPrime;
        //static int ArtemisIndex => CalamityMod.NPCs.CalamityGlobalNPC.draedonExoMechTwinRed;
        //public static bool ShouldRootPosition(NPC npc)
        //{
        //    if (Main.netMode != NetmodeID.MultiplayerClient) return false;
        //    if (npc.type == ModContent.NPCType<AresBody>())
        //    {
        //        AresBodyAttackType attackType = (AresBodyAttackType)npc.ai[0];
        //        float attackSubstate = npc.Infernum().ExtraAI[9];
        //        bool IdleHoverRoot = attackType == AresBodyAttackType.IdleHover;
        //        bool PrecisionBlastRoot = attackType == AresBodyAttackType.PrecisionBlasts && attackSubstate == 1;
        //        bool TwinsAndAresCircleAttackRoot = (ExoMechComboAttackType)attackType == ExoMechComboAttackType.AresTwins_CircleAttack;
        //        bool LaserSpinRoot = attackType == AresBodyAttackType.LaserSpinBursts;
        //        bool EnergyBladeSliceRoot = attackType == AresBodyAttackType.EnergyBladeSlices;
        //        bool EnergySlashThanatosRoot = (ExoMechComboAttackType)attackType == ExoMechComboAttackType.ThanatosAres_EnergySlashesAndCharges;
        //        return IdleHoverRoot || PrecisionBlastRoot || TwinsAndAresCircleAttackRoot || LaserSpinRoot || EnergyBladeSliceRoot || EnergySlashThanatosRoot;
        //    }
        //    if (npc.type == ModContent.NPCType<Apollo>() || npc.type == ModContent.NPCType<Artemis>())
        //    {
        //        bool IsApollo = npc.type == ModContent.NPCType<Apollo>();
        //        TwinsAttackType attackType = (TwinsAttackType)npc.ai[0];
        //        int attackSubstate = (int)npc.Infernum().ExtraAI[0];
        //        bool FireChargeRoot = attackType == TwinsAttackType.FireCharge;
        //        bool ApolloPlasmaChargeRoot = IsApollo && attackType == TwinsAttackType.ApolloPlasmaCharges;
        //        bool GatlingLaserRoot = attackType == TwinsAttackType.GatlingLaserAndPlasmaFlames && attackSubstate != 0;
        //        bool LaserRayPlasmaBlastRoot = attackType == TwinsAttackType.SlowLaserRayAndPlasmaBlasts;
        //        bool AresCircleAttackRoot = (ExoMechComboAttackType)attackType == ExoMechComboAttackType.AresTwins_CircleAttack;
        //        bool AresLaserAttackRoot = (ExoMechComboAttackType)attackType == ExoMechComboAttackType.AresTwins_DualLaserCharges;
        //        return FireChargeRoot || ApolloPlasmaChargeRoot || GatlingLaserRoot || LaserRayPlasmaBlastRoot || AresCircleAttackRoot || AresLaserAttackRoot;
        //    }

        //    if (IsAresCannon(npc))
        //    {
        //        if (Main.npc.IndexInRange(AresIndex)) return ShouldRootPosition(Main.npc[AresIndex]);
        //        else return false;
        //    }
        //    return false;

        //}
        //private void RootNPC(NPC npc)
        //{
        //    if (RootPosition.Distance(npc.Center) > 2500) return;
        //    npc.netOffset = Vector2.Zero;
        //    npc.Center = RootPosition;
        //    if (IsAresCannon(npc)) //if ares cannons should be rooted, their AI should also be rooted
        //    {
        //        for (int i = 0; i < 4; i++)
        //        {
        //            npc.ai[i] = NewLocals[i];
        //        }
        //    }
        //    //If Attackstate or attackSubstate of ares is changed by netupdate instead of SelectNextAttack(), it will miss any deletion of projectiles
        //    if (npc.whoAmI == AresIndex)
        //    {
        //        if (npc.ai[0] != NewLocals[0] || npc.ai[1] != NewLocals[1])
        //        {
        //            int PrevAttackType = (int)NewLocals[0];
        //            if (PrevAttackType == (int)AresBodyAttackType.LaserSpinBursts)
        //            {
        //                if (npc.ai[0] != NewLocals[0])
        //                    Utilities.DeleteAllProjectiles(true, ModContent.ProjectileType<AresDeathBeamTelegraph>(), ModContent.ProjectileType<AresSpinningDeathBeam>(),
        //                    ModContent.ProjectileType<ExoburstSpark>(), ModContent.ProjectileType<AresBeamExplosion>());
        //            }
        //            if (PrevAttackType == (int)AresBodyAttackType.PrecisionBlasts && PrevAttackType == npc.ai[0])
        //            {
        //                int AttackSubstate = (int)NewLocals[1];
        //                if (AttackSubstate == 1) Utilities.DeleteAllProjectiles(false, ModContent.ProjectileType<HotMetal>());
        //                if (AttackSubstate == 3) Utilities.DeleteAllProjectiles(true, ModContent.ProjectileType<HotMetal>(), ModContent.ProjectileType<AresTeslaSpark>());
        //            }
        //            if (PrevAttackType == (int)ExoMechComboAttackType.AresTwins_DualLaserCharges && PrevAttackType != npc.ai[0])
        //            {
        //                ExoMechManagement.ClearAwayTransitionProjectiles();

        //            }
        //            if (PrevAttackType == (int)ExoMechComboAttackType.AresTwins_CircleAttack && PrevAttackType != npc.ai[0])
        //            {
        //                ExoMechManagement.ClearAwayTransitionProjectiles();
        //            }
        //        }
        //        npc.Infernum().ExtraAI[2] = NewLocals[2];

        //    }
        //    else if (npc.whoAmI == ArtemisIndex)
        //    {
        //        npc.Infernum().ExtraAI[2] = NewLocals[0];
        //    }
        //}

        //public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        //{
        //}

        //public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        //{
        //    if (!npc.active)
        //    {
        //        return;
        //    }
        //    if (ShouldRootPosition(npc))
        //    {
        //        RootNPC(npc);
        //    }
        //}
        //public override void PostAI(NPC npc)
        //{
        //    if (Main.netMode != NetmodeID.MultiplayerClient) return;
        //    RootPosition = npc.Center;
        //    if (IsAresCannon(npc))
        //    {
        //        for (int i = 0; i < 4; i++)
        //        {
        //            NewLocals[i] = npc.ai[i];
        //        }
        //        return;
        //    }
        //    else if (npc.whoAmI == AresIndex)
        //    {

        //        NewLocals[0] = npc.ai[0]; // previous attack type
        //        NewLocals[1] = npc.ai[1]; // previous attack state
        //        NewLocals[2] = npc.Infernum().ExtraAI[2];
        //    }
        //    else if (npc.whoAmI == ArtemisIndex)
        //    {
        //        NewLocals[0] = npc.Infernum().ExtraAI[2];
        //    }
        //}
    }
}
