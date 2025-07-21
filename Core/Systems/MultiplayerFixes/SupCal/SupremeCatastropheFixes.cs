using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.NPCs.SupremeCalamitas;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.SupCal
{
    //public class SupremeCatastropheFixes : ModSystem
    //{
    //    public static class SupremeCatastropheReceiveExtraAIHook
    //    {
    //        private delegate void ReceiveExtraAIDelegate(SupremeCatastrophe self, BinaryReader reader);
    //        private static Hook hook;
    //        private static ReceiveExtraAIDelegate orig;

    //        public static void Load()
    //        {
    //            MethodInfo method = typeof(SupremeCatastrophe).GetMethod("ReceiveExtraAI", BindingFlags.Instance | BindingFlags.Public);
    //            orig = (ReceiveExtraAIDelegate)Delegate.CreateDelegate(typeof(ReceiveExtraAIDelegate), method);
    //            hook = new Hook(method, new ReceiveExtraAIDelegate(Detour));
    //        }

    //        public static void Unload()
    //        {
    //            hook?.Dispose();
    //            hook = null;
    //        }

    //        private static void Detour(SupremeCatastrophe self, BinaryReader reader)
    //        {
    //            orig(self, reader);

    //            if (!self.NPC.active)
    //            {
    //                self.NPC.ai = new float[NPC.maxAI];
    //            }
    //        }
    //    }
    //}

    public class SupremeCatastropheFixes : GlobalNPC
    {
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            if (npc.type == ModContent.NPCType<SupremeCatastrophe>())
            {
                base.ReceiveExtraAI(npc, bitReader, binaryReader);

                if (!npc.active)
                {
                    npc.ai = new float[NPC.maxAI];
                }
            }
        }
    }
}
