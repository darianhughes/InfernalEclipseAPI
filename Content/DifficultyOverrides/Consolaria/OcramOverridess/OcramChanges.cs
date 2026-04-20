using CalamityMod.Events;
using InfernalEclipseAPI.Core.Systems;
using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;
namespace InfernalEclipseAPI.Content.DifficultyOverrides.Consolaria.OcramOverridess
{
    public class OcramBehavior : GlobalNPC
    {
        private static bool bloodmoonStartedByOcram = false;
        private static bool bloodmoonStartedByDreadnaut = false;

        public override bool PreAI(NPC npc)
        {
            if (InfernumActive.InfernumActive && npc.type == NPCID.BloodNautilus && !Main.bloodMoon)
            {
                Main.bloodMoon = true;
                bloodmoonStartedByDreadnaut = true;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // sync the blood moon
            }

            return base.PreAI(npc);
        }

        public override void AI(NPC npc)
        {
            if (!InfernumActive.InfernumActive)
                { return; }

            if (InfernalCrossmod.Consolaria.Loaded) 
            {
                if (npc.type == InfernalCrossmod.Consolaria.Mod.Find<ModNPC>("Ocram").Type)
                {
                    if (!Main.bloodMoon && BossRushEvent.BossRushActive)
                    {
                        Main.bloodMoon = true;
                        bloodmoonStartedByOcram = true;
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendData(MessageID.WorldData); // sync the blood moon
                    }
                }
            }
        }

        public override void OnKill(NPC npc)
        {
            if (!InfernumActive.InfernumActive)
                return;

            if (npc.type == NPCID.BloodNautilus && bloodmoonStartedByDreadnaut)
            {
                DisableBloodMoon();
            }

            if (InfernalCrossmod.Consolaria.Loaded && bloodmoonStartedByOcram)
            {
                if (npc.type == InfernalCrossmod.Consolaria.Mod.Find<ModNPC>("Ocram").Type)
                    DisableBloodMoon();
            }
        }

        public override bool CheckDead(NPC npc)
        {
            if (InfernumActive.InfernumActive) 
            {
                if (npc.type == NPCID.BloodNautilus && bloodmoonStartedByDreadnaut)
                {
                    DisableBloodMoon();
                }

                if (InfernalCrossmod.Consolaria.Loaded && bloodmoonStartedByOcram)
                {
                    if (npc.type == InfernalCrossmod.Consolaria.Mod.Find<ModNPC>("Ocram").Type)
                        DisableBloodMoon();
                }
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
