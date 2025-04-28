using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;
using Consolaria.Content.NPCs.Bosses.Ocram;

namespace InfernalEclipseAPI.Content.DifficultyOverrides.OcramOverridess
{
    [JITWhenModsEnabled("Consolaria")]
    public class OcramBehavior : GlobalNPC
    {
        public override void AI(NPC npc)
        {
            if (!InfernumActive.InfernumActive)
                { return; }
            if (npc.type == ModContent.NPCType<Ocram>())
            {
                if (!Main.bloodMoon)
                {
                    Main.bloodMoon = true;
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.WorldData); // sync the blood moon
                }
            }
        }

        public override void OnKill(NPC npc)
        {
            if (!InfernumActive.InfernumActive)
            { return; }
            if (npc.type == ModContent.NPCType<Ocram>())
                DisableBloodMoon();
        }

        public override bool CheckDead(NPC npc)
        {
            if (InfernumActive.InfernumActive) 
            {
                if (npc.type == ModContent.NPCType<Ocram>())
                    DisableBloodMoon();
            }

            return base.CheckDead(npc);
        }

        private static void DisableBloodMoon()
        {
            if (Main.bloodMoon)
            {
                Main.bloodMoon = false;

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Resync after ending
            }
        }

        public override bool InstancePerEntity => true;
    }
}
