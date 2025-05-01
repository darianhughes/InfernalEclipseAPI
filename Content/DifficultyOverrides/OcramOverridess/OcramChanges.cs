using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;
namespace InfernalEclipseAPI.Content.DifficultyOverrides.OcramOverridess
{
    public class OcramBehavior : GlobalNPC
    {
        public static Mod console;
        public static bool ConsolariaActive
        {
            get
            {
                if (ModLoader.TryGetMod("Consolaria", out console))
                {
                    return true;
                }
                return false;
            }
        }
        public override void AI(NPC npc)
        {
            if (!InfernumActive.InfernumActive || !ConsolariaActive)
                { return; }

            if (npc.type ==  console.Find<ModNPC>("Ocram").Type)
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
            if (!InfernumActive.InfernumActive || !ConsolariaActive)
            { return; }
            if (npc.type == console.Find<ModNPC>("Ocram").Type)
                DisableBloodMoon();
        }

        public override bool CheckDead(NPC npc)
        {
            if (InfernumActive.InfernumActive && ConsolariaActive) 
            {
                if (npc.type == console.Find<ModNPC>("Ocram").Type)
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
