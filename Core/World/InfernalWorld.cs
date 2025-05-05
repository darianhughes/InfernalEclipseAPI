using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;

namespace InfernalEclipseAPI.Core.World
{
    public class InfernalWorld : ModSystem
    {
        public static bool dreadonDestroyerDialoguePlayed = false;

        public static void ResetFlags()
        {
            dreadonDestroyerDialoguePlayed = false;
        }

        public override void OnWorldLoad()
        {
            ResetFlags();
        }

        public override void OnWorldUnload()
        {
            ResetFlags();
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["dreadonDestroyerDialoguePlayed"] = dreadonDestroyerDialoguePlayed;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            GetData(ref dreadonDestroyerDialoguePlayed, "dreadonDestroyerDialoguePlayed", tag);
        }

        public static void GetData(ref bool baseVar, string path, TagCompound tag)
        {
            if (tag.ContainsKey(path)) { baseVar = tag.Get<bool>(path); }
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(dreadonDestroyerDialoguePlayed);
        }

        public override void NetReceive(BinaryReader reader)
        {
            dreadonDestroyerDialoguePlayed = reader.ReadBoolean();
        }
    }
}
