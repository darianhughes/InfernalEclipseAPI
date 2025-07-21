using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System;
using CalamityMod;
using CalamityMod.NPCs;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using System.Reflection;
using InfernumMode.Content.Subworlds;
using System.Linq;
using InfernumMode;
using System.Numerics;
using CalamityMod.NPCs.CalClone;
using ReLogic.Content.Sources;
using Terraria.Audio;
using InfernumMode.Assets.Sounds;
using InfernumMode.Core.GlobalInstances.Systems;
using System.IO;
using InfernumMode.Content.Achievements.InfernumAchievements;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Artemis;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.Ares;
using static InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.Ares.AresBodyBehaviorOverride;
using static InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ArtemisAndApollo.ApolloBehaviorOverride;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon;
using MonoMod.Cil;
using static Mono.Cecil.Cil.OpCodes;
using Mono.Cecil.Cil;
using CalamityMod.CalPlayer;
using log4net.Repository.Hierarchy;
using CalamityMod.Balancing;
using Terraria.GameContent.Events;
using CalamityMod.Items.Accessories;
using InfernumMode.Core.GlobalInstances;
using CalamityMod.Events;
using CalamityMod.NPCs.CeaselessVoid;
using CalamityMod.Items;
using InfernalEclipseAPI.Core.Systems.MultiplayerFixes.ExoMechs;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixSystems
{
    public class InfernumMultiplayerMassPatches : ModSystem
    {
        private Mod Infernum => ModLoader.GetMod("InfernumMode");
        private Mod Calamity => ModLoader.GetMod("CalamityMod");
        public override void Load()
        {
            if (Calamity == null)
            {
                throw new NullReferenceException();
            }
            if (Infernum != null)
            {
                On.InfernumMode.Content.BehaviorOverrides.BossAIs.Signus.SignusBehaviorOverride.PreAI += SignusBehaviorOverride_PreAI;
                On.InfernumMode.Content.BehaviorOverrides.BossAIs.SupremeCalamitas.SupremeCalamitasBehaviorOverride.PreAI += SupremeCalamitasBehaviorOverride_PreAI;
                MethodInfo seekerMethod = typeof(InfernumMode.Content.BehaviorOverrides.BossAIs.SupremeCalamitas.SupremeCalamitasBehaviorOverride).GetMethod("DoBehavior_SummonSeekers", BindingFlags.Static | BindingFlags.Public);
                MonoModHooks.Add(seekerMethod, SupremeCalamitasBehaviorOverride_DoBehavior_SummonSeekers);
                //IL.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.Ares.AresCannonBehaviorOverride.PreAI += AresCannonBehaviorOverride_PreAI;
                //IL.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.Ares.AresPulseCannon.AI += AresPulseCannon_AI;
                //IL.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.Ares.AresPlasmaFireball.OnKill += AresPlasmaFireball_OnKill;
                //IL.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.Ares.AresEnergyKatana.DoBehavior_EnergyBladeSlices += AresEnergyKatana_DoBehavior_EnergyBladeSlices;
                //On.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.Ares.AresBodyBehaviorOverride.PreAI += AresBodyBehaviorOverride_PreAI;
                //IL.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ArtemisAndApollo.ApolloBehaviorOverride.DoBehavior_FireCharge += ApolloBehaviorOverride_DoBehavior_FireCharge;
                //IL.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ArtemisAndApollo.ApolloBehaviorOverride.DoBehavior_ApolloPlasmaCharges += ApolloBehaviorOverride_DoBehavior_ApolloPlasmaCharges;
                //IL.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ArtemisAndApollo.ApolloBehaviorOverride.DoBehavior_GatlingLaserAndPlasmaFlames += ApolloBehaviorOverride_DoBehavior_GatlingLaserAndPlasmaFlames;
                //IL.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ArtemisAndApollo.ApolloBehaviorOverride.DoBehavior_SlowLaserRayAndPlasmaBlasts += ApolloBehaviorOverride_DoBehavior_SlowLaserRayAndPlasmaBlasts;
                //IL.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ComboAttacks.ExoMechComboAttackContent.DoBehavior_AresTwins_CircleAttack += ExoMechComboAttackContent_DoBehavior_AresTwins_CircleAttack;
                //IL.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ComboAttacks.ExoMechComboAttackContent.DoBehavior_AresTwins_DualLaserCharges += ExoMechComboAttackContent_DoBehavior_AresTwins_DualLaserCharges;
                //IL.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ComboAttacks.ExoMechComboAttackContent.DoBehavior_TwinsThanatos_AlternatingTwinsBursts += ExoMechComboAttackContent_DoBehavior_TwinsThanatos_AlternatingTwinsBursts;
                //IL.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ComboAttacks.ExoMechComboAttackContent.DoBehavior_TwinsThanatos_ThermoplasmaDashes += ExoMechComboAttackContent_DoBehavior_TwinsThanatos_ThermoplasmaDashes;
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        //Change projectiles that are fired during phases where the npc firing them is positionially locked to be clientside
        #region MPCheck replacements

        void ReplaceMPCheck(ILCursor c)
        {
            if (!c.TryGotoNext(
                i => i.MatchLdsfld<Main>("netMode"),
                i => i.MatchLdcI4(1)
                )) return;
            c.Index++;
            c.Index++;
            c.EmitPop();
            c.Emit(Ldc_I4, 2);
        }
        private void ExoMechComboAttackContent_DoBehavior_TwinsThanatos_ThermoplasmaDashes(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ReplaceMPCheck(c);
        }

        private void ExoMechComboAttackContent_DoBehavior_TwinsThanatos_AlternatingTwinsBursts(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ReplaceMPCheck(c);
            ReplaceMPCheck(c);
            ReplaceMPCheck(c);
            ReplaceMPCheck(c);
        }
        private void ApolloBehaviorOverride_DoBehavior_SlowLaserRayAndPlasmaBlasts(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ReplaceMPCheck(c);
            ReplaceMPCheck(c);
            ReplaceMPCheck(c);
        }
        private void ExoMechComboAttackContent_DoBehavior_AresTwins_DualLaserCharges(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ReplaceMPCheck(c);
            ReplaceMPCheck(c);
        }

        private void ExoMechComboAttackContent_DoBehavior_AresTwins_CircleAttack(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ReplaceMPCheck(c);
            ReplaceMPCheck(c);
        }
        private void ApolloBehaviorOverride_DoBehavior_GatlingLaserAndPlasmaFlames(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ReplaceMPCheck(c);
            ReplaceMPCheck(c);
        }


        private void ApolloBehaviorOverride_DoBehavior_ApolloPlasmaCharges(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext(MoveType.After, i => i.MatchLdsfld<Main>("netMode"));
            ReplaceMPCheck(c);
        }

        private void ApolloBehaviorOverride_DoBehavior_FireCharge(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ReplaceMPCheck(c);
            ReplaceMPCheck(c);
        }

        private void AresEnergyKatana_DoBehavior_EnergyBladeSlices(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ReplaceMPCheck(c);
        }
        private void AresPlasmaFireball_OnKill(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ReplaceMPCheck(c);
        }

        private void AresPulseCannon_AI(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ReplaceMPCheck(c);
        }
        private void AresCannonBehaviorOverride_PreAI(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ReplaceMPCheck(c);
        }
        #endregion
        //private object AresBodyBehaviorOverride_PreAI(On.InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.Ares.AresBodyBehaviorOverride.orig_PreAI orig, AresBodyBehaviorOverride self, object npc)
        //{
        //    //Make the red spinny laser clientside.
        //    if (Main.netMode == NetmodeID.MultiplayerClient) ((NPC)npc).Infernum().ExtraAI[2] = ((NPC)npc).GetGlobalNPC<ExoMechsJitterFix>().NewLocals[2];
        //    return orig(self, npc);
        //}

        //If the client begins running SummonSeekers at the same time at the server (as it usually does), it will attempt to summon seekers with the viiglance
        //however, the vigilance must be created serverside and sent to the client, meaning that for the first frame or two before the netUpdate arrives it is vigilanceless
        //this causes a crash as it has no VigilanceProj ModProjectile.
        public delegate void orig_DoBehavior_SummonSeekers(NPC npc, Player target, Vector2 handPosition, ref float frameType, ref float frameChangeSpeed, ref float attackTimer);
        public static void SupremeCalamitasBehaviorOverride_DoBehavior_SummonSeekers(orig_DoBehavior_SummonSeekers orig, NPC npc, Player target, Vector2 handPosition, ref float frameType, ref float frameChangeSpeed, ref float attackTimer)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                bool seekersCouldFire = npc.Infernum().ExtraAI[1] == 1;
                orig(npc, target, handPosition, ref frameType, ref frameChangeSpeed, ref attackTimer);
                if (!seekersCouldFire && npc.Infernum().ExtraAI[1] == 1 && Main.netMode == NetmodeID.Server)
                {
                    npc.netUpdate = true;
                }
                return;
            }
            var staffs = Utilities.AllProjectilesByID(ModContent.ProjectileType<InfernumMode.Content.BehaviorOverrides.BossAIs.SupremeCalamitas.VigilanceProj>());
            if (staffs.Any())
            {
                npc.Infernum().ExtraAI[0] = staffs.First().whoAmI;
                orig(npc, target, handPosition, ref frameType, ref frameChangeSpeed, ref attackTimer);
                return;
            }
            //if no staves, run the essential (non-graphical) clientside effects, and skip the rest of the method
            npc.dontTakeDamage = true;
            npc.damage = 0;
            npc.velocity *= 0.98f;

        }

        private object SupremeCalamitasBehaviorOverride_PreAI(On.InfernumMode.Content.BehaviorOverrides.BossAIs.SupremeCalamitas.SupremeCalamitasBehaviorOverride.orig_PreAI orig, InfernumMode.Content.BehaviorOverrides.BossAIs.SupremeCalamitas.SupremeCalamitasBehaviorOverride self, object npc)
        {//since the win effects happen on netupdate instead, they must be prevented from running normally.
            if (Main.netMode == NetmodeID.MultiplayerClient && ((NPC)npc).ai[0] == 13)
            {
                if (((NPC)npc).ai[1] > 779)
                    ((NPC)npc).ai[1] = 779;
            }
            return orig(self, npc);
        }

        //due to latency, Signus can spawn more than once as RoK spawns from clientside as long as it is unaware of any signuses that already exist.
        private object SignusBehaviorOverride_PreAI(On.InfernumMode.Content.BehaviorOverrides.BossAIs.Signus.SignusBehaviorOverride.orig_PreAI orig, InfernumMode.Content.BehaviorOverrides.BossAIs.Signus.SignusBehaviorOverride self, object _npc)
        {
            NPC npc = (NPC)_npc;
            if (Main.netMode == NetmodeID.Server && npc.ai[2] > 5 && CalamityGlobalNPC.signus != npc.whoAmI)
            {
                npc.active = false;
                npc.netUpdate = true;
                return false;
            }
            return orig(self, _npc);
        }
    }
}