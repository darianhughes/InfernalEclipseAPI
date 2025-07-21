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
    //public class SupremeCataclysmFixes : ModSystem
    //{
    //    private delegate void ReceiveExtraAIDelegate(SupremeCataclysm self, BinaryReader reader);
    //    private static Hook hook;
    //    private static ReceiveExtraAIDelegate orig;

    //    public override void Load()
    //    {
    //        MethodInfo method = typeof(SupremeCataclysm).GetMethod("ReceiveExtraAI", BindingFlags.Instance | BindingFlags.Public);
    //        orig = (ReceiveExtraAIDelegate)Delegate.CreateDelegate(typeof(ReceiveExtraAIDelegate), method);
    //        hook = new Hook(method, new ReceiveExtraAIDelegate(Detour));
    //    }

    //    public override void Unload()
    //    {
    //        hook?.Dispose();
    //        hook = null;
    //    }

    //    private static void Detour(SupremeCataclysm self, BinaryReader reader)
    //    {
    //        orig(self, reader);

    //        if (!self.NPC.active)
    //        {
    //            self.NPC.ai = new float[NPC.maxAI];
    //        }
    //    }
    //}

    public class SupremeCataclysmFixes : GlobalNPC
    {
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            if (npc.type == ModContent.NPCType<SupremeCataclysm>())
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
