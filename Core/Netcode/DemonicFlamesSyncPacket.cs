using System.IO;
using CalamityMod;

namespace InfernalEclipseAPI.Core.Netcode
{
    internal sealed class DemonicFlamesSyncPacket : InfernalPacket
    {
        public static DemonicFlamesSyncPacket Instance { get; private set; }

        public static void Send(NPC npc, int toClient = -1, int ignoreClient = -1)
        {
            if (npc is null)
                return;

            var packet = Instance.CreateBasePacket();
            packet.WriteWhoAmI(npc);
            packet.Write(npc.Calamity().demonicFlamesBonusDamage);
            packet.Send(toClient, ignoreClient);
        }

        public override void HandlePacket(BinaryReader packet, int sender)
        {
            var npc = packet.ReadNPC();
            var damage = packet.ReadInt32();

            if (npc is null)
                return;

            npc.Calamity().demonicFlamesBonusDamage = damage;
        }
    }
}
