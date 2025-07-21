using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.NPCs.SupremeCalamitas;
using InfernumMode;
using MonoMod.RuntimeDetour;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using InfernumMode.Content.Credits;
using Terraria.ModLoader.IO;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.SupCal
{
    //public class SupremeCalamitasFixes : ModSystem
    //{
    //    private delegate void ReceiveExtraAIDelegate(SupremeCalamitas self, BinaryReader reader);
    //    private static Hook hook;
    //    private static ReceiveExtraAIDelegate orig;

    //    public override void Load()
    //    {
    //        MethodInfo target = typeof(SupremeCalamitas).GetMethod("ReceiveExtraAI", BindingFlags.Instance | BindingFlags.Public);
    //        orig = (ReceiveExtraAIDelegate)Delegate.CreateDelegate(typeof(ReceiveExtraAIDelegate), target);
    //        hook = new Hook(target, new ReceiveExtraAIDelegate(Patched_ReceiveExtraAI));
    //    }

    //    public override void Unload()
    //    {
    //        hook?.Dispose();
    //        hook = null;
    //    }

    //    private static void Patched_ReceiveExtraAI(SupremeCalamitas self, BinaryReader reader)
    //    {
    //        orig(self, reader); // Run original first, then inject logic afterward

    //        float attackType = self.NPC.ai[0];
    //        float attackState = self.NPC.Infernum().ExtraAI[4];

    //        if (Main.netMode == NetmodeID.MultiplayerClient && !self.NPC.active && attackType == 13 && attackState == 4f)
    //        {
    //            self.NPC.NPCLoot();
    //            if (DownedBossSystem.downedExoMechs)
    //            {
    //                CreditManager.BeginCredits();
    //            }
    //        }
    //    }
    //}

    public class SupremeCalamitasFixes : GlobalNPC
    {
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {

        }

        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            if (npc.type == ModContent.NPCType<SupremeCalamitas>())
            {
                base.ReceiveExtraAI(npc, bitReader, binaryReader);

                float attackType = npc.ai[0];
                float attackState = npc.Infernum().ExtraAI[4];

                if (Main.netMode == NetmodeID.MultiplayerClient && !npc.active && attackType == 13 && attackState == 4f)
                {
                    npc.NPCLoot();
                    if (DownedBossSystem.downedExoMechs)
                    {
                        CreditManager.BeginCredits();
                    }
                }
            }
        }
    }
}
