using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader.IO;

namespace InfernalEclipseAPI.YharimEX.Core.Systems
{
    public class YharimEXWorldFlags : ModSystem
    {
        internal static bool downedYharimEX;
        internal static bool angryYharimEX = false;
        internal static int skipYharimEXP1;

        public static int SkipYharimEXP1 { get => skipYharimEXP1; set => skipYharimEXP1 = value; }
        public static bool DownedYharimEX { get => downedYharimEX; set => downedYharimEX = value; }
        public static bool AngryYharimEX { get => angryYharimEX; set => angryYharimEX = value; }

        private static void ResetFlags()
        {
            DownedYharimEX = false;
            AngryYharimEX = false;
            SkipYharimEXP1 = 0;
        }

        public override void OnWorldLoad() => ResetFlags();

        public override void OnWorldUnload() => ResetFlags();

        public override void SaveWorldData(TagCompound tag)
        {
            List<string> downed = [];
            if (DownedYharimEX)
                downed.Add("downedYharimEX");
            if (AngryYharimEX)
                downed.Add("AngryYharimEX");

            tag.Add("downed", downed);
            tag.Add("YharimEXP1", SkipYharimEXP1);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            IList<string> downed = tag.GetList<string>("downed");
            DownedYharimEX = downed.Contains("downedYharimEX");
            AngryYharimEX = downed.Contains("AngryYharimEX");

            if (tag.ContainsKey("YharimEXP1"))
                SkipYharimEXP1 = tag.GetAsInt("YharimEXP1");
        }

        public override void NetReceive(BinaryReader reader)
        {
            SkipYharimEXP1 = reader.ReadInt32();

            BitsByte flags = reader.ReadByte();
            DownedYharimEX = flags[5];
            AngryYharimEX = flags[6];
            flags = reader.ReadByte();
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(SkipYharimEXP1);

            writer.Write(new BitsByte
            {
                [1] = DownedYharimEX,
                [2] = AngryYharimEX,
            });
        }
    }
}
