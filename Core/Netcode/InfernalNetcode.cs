using System.Collections.Generic;
using System.IO;

namespace InfernalEclipseAPI.Core.Netcode
{
    public class InfernalNetcode : ModSystem
    {
        private static List<InfernalPacket> _PacketHandlers = [];

        internal static ushort RegisterHandler(InfernalPacket handler)
        {
            var id = (ushort)_PacketHandlers.Count;
            _PacketHandlers.Add(handler);
            return id;
        }

        internal static void WriteHandlerNetID(BinaryWriter packet, ushort netID)
        {
            if (_PacketHandlers.Count > 256)
                packet.Write(netID);
            else
                packet.Write((byte)netID);
        }

        internal static ushort ReadHandlerNetID(BinaryReader packet)
        {
            if (_PacketHandlers.Count > 256)
                return packet.ReadUInt16();
            else
                return packet.ReadByte();
        }

        public override void OnModUnload()
        {
            _PacketHandlers = null;
        }

        public static void HandlePacket(Mod mod, BinaryReader reader, int whoAmI)
        {
            try
            {
                var netID = ReadHandlerNetID(reader);
                var packetHandler = _PacketHandlers[netID];
                if (packetHandler is not null)
                {
                    packetHandler.HandlePacket(reader, whoAmI);
                }
                else
                {
                    //
                    // Default case: with no idea how long the packet is, we can't safely read data.
                    // Throw an exception now instead of allowing the network stream to corrupt.
                    //
                    throw new Exception("Failed to parse Infernal packet: Invalid Calamity packet ID.");
                }
            }
            catch (Exception e)
            {
                throw; // this either will crash the game or be caught by TML's packet policing
            }
        }

        public static void SyncWorld()
        {
            if (Main.dedServ)
                NetMessage.SendData(MessageID.WorldData);
        }

        /// <summary>
        /// Shorthand Method for SyncNPC
        /// <code>
        /// This Equals to:
        /// 
        /// if (Main.dedServ and npc != null)
        ///     NetMessage.SendData(MessageID.SyncNPC, ...)
        /// </code>
        /// </summary>
        public static void SyncNPC(NPC npcToSync, int toClient = -1, int ignoreClient = -1)
        {
            if (!Main.dedServ)
                return;

            if (npcToSync is null)
                return;

            var npcWhoAmI = npcToSync.whoAmI;
            if (npcWhoAmI < 0 || npcWhoAmI >= Main.maxNPCs)
                return;

            NetMessage.SendData(MessageID.SyncNPC, toClient, ignoreClient, null, npcWhoAmI);
        }

        /// <summary>
        /// Shorthand Method for SyncNPC
        /// <code>
        /// This Equals to:
        /// 
        /// if (Main.dedServ and npcWhoAmI in valid range)
        ///     NetMessage.SendData(MessageID.SyncNPC, ...)
        /// </code>
        /// </summary>
        public static void SyncNPC(int npcWhoAmI, int toClient = -1, int ignoreClient = -1)
        {
            if (!Main.dedServ)
                return;

            if (npcWhoAmI < 0 || npcWhoAmI >= Main.maxNPCs)
                return;

            NetMessage.SendData(MessageID.SyncNPC, toClient, ignoreClient, null, npcWhoAmI);
        }
    }
}
