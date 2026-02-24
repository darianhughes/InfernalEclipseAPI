using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;
namespace InfernalEclipseAPI.Content.DifficultyOverrides.OcramOverridess
{
    public class OcramBehavior : GlobalNPC
    {
        private static Mod console;
        private static bool bloodmoonStartedByOcram = false;
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
                    bloodmoonStartedByOcram = true;
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
            if (Main.bloodMoon && bloodmoonStartedByOcram)
            {
                Main.bloodMoon = false;

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Resync after ending
            }
        }

        public override bool InstancePerEntity => true;
    }
}
