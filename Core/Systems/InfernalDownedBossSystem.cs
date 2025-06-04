using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace InfernalEclipseAPI.Core.Systems
{
    public class InfernalDownedBossSystem : ModSystem
    {
        private static bool _downedDreadNautilus;

        public static bool downedDreadNautilus
        {
            get => _downedDreadNautilus;
            set
            {
                if (!value)
                    _downedDreadNautilus = false;
                else
                    NPC.SetEventFlagCleared(ref _downedDreadNautilus, -1);
            }
        }

        private static void ReseetAllFlags()
        {
            downedDreadNautilus = false;
        }

        public override void OnWorldLoad()
        {
            ReseetAllFlags();
        }

        public override void OnWorldUnload()
        {
            ReseetAllFlags();
        }

        public override void ClearWorld()
        {
            downedDreadNautilus = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (downedDreadNautilus)
                tag["downedDreadNautilus"] = true;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedDreadNautilus = tag.ContainsKey("downedDreadNautilus");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte bitsByte1 = new BitsByte();
            bitsByte1[0] = downedDreadNautilus;
            writer.Write(bitsByte1);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedDreadNautilus = flags[0];
        }
    }
}
