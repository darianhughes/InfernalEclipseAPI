using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.ProfanedGuardians;
using System.Collections.Generic;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges
{
    //Wardrobe Hummus
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    public class PlatinumDartBlacklistHook : ModSystem
    {
        public override void Load()
        {
            if (!ModLoader.TryGetMod(InfernalCrossmod.SOTS.Name, out var sots))
                return;

            var dartType = sots.Code.GetType("SOTS.Projectiles.Ores.PlatinumDart");
            if (dartType == null)
                return;

            var onHit = dartType.GetMethod(
                "OnHitNPC",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );

            if (onHit != null)
                MonoModHooks.Modify(onHit, IL_BlockBossLatch);
        }

        private static void IL_BlockBossLatch(ILContext il)
        {
            var c = new ILCursor(il);

            /*
             * We want to replace:
             *   latch = true;
             *
             * With:
             *   if (!CanLatch(target)) return;
             *   latch = true;
             */

            if (!c.TryGotoNext(
                i => i.OpCode == OpCodes.Ldc_I4_1,
                i => i.OpCode == OpCodes.Stfld &&
                     i.Operand is FieldReference fr &&
                     fr.Name == "latch"
            ))
                return;

            // Move cursor BEFORE ldc.i4.1
            c.Index--;

            // Load NPC target (arg1)
            c.Emit(OpCodes.Ldarg_1);

            // Call CanLatch via delegate (SAFE)
            c.EmitDelegate<Func<NPC, bool>>(npc =>
            {
                return CanLatch(npc);
            });

            // If false -> return from OnHitNPC
            c.Emit(OpCodes.Brtrue_S, c.Next); // allow latch
            c.Emit(OpCodes.Ret);
        }

        private static readonly HashSet<int> NPCBlacklist = new()
        {
            NPCID.EaterofWorldsHead,
            NPCID.EaterofWorldsBody,
            NPCID.EaterofWorldsTail,

            NPCID.TheDestroyer,
            NPCID.TheDestroyerBody,
            NPCID.TheDestroyerTail,

            // Perforators
            ModContent.NPCType<PerforatorHeadSmall>(),
            ModContent.NPCType<PerforatorBodySmall>(),
            ModContent.NPCType<PerforatorTailSmall>(),

            ModContent.NPCType<PerforatorHeadMedium>(),
            ModContent.NPCType<PerforatorBodyMedium>(),
            ModContent.NPCType<PerforatorTailMedium>(),

            ModContent.NPCType<PerforatorHeadLarge>(),
            ModContent.NPCType<PerforatorBodyLarge>(),
            ModContent.NPCType<PerforatorTailLarge>(),

            // Slime God
            ModContent.NPCType<CrimulanPaladin>(),
            ModContent.NPCType<EbonianPaladin>(),
            ModContent.NPCType<SplitCrimulanPaladin>(),
            ModContent.NPCType<SplitEbonianPaladin>(),

            // Profaned Guardians
            ModContent.NPCType<ProfanedGuardianDefender>(),
            ModContent.NPCType<ProfanedGuardianCommander>(),
            ModContent.NPCType<ProfanedGuardianHealer>(),
        };


        /// <summary>
        /// Determines whether Platinum Dart is allowed to latch onto the NPC.
        /// </summary>
        private static bool CanLatch(NPC npc)
        {
            if (npc == null)
                return false;

            // Vanilla + modded boss flag
            if (npc.boss)
                return false;

            // Explicit blacklist
            if (NPCBlacklist.Contains(npc.type))
                return false;

            return true;
        }
    }
}
