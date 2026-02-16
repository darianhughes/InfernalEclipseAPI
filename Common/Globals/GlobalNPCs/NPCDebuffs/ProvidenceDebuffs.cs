using CalamityMod.NPCs.Providence;

namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs
{
    public class ProvidenceDebuffs : GlobalNPC
    {
        private Mod clamity
        {
            get
            {
                ModLoader.TryGetMod("Clamity", out Mod clam);
                return clam;
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (!npc.active || npc.type != ModContent.NPCType<Providence>()) return base.PreAI(npc);

            if (clamity != null)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player.active && !player.dead)
                    {
                        if (player.mount?.Type == clamity.Find<ModMount>("PlagueChairMount").Type)
                            player.mount.Dismount(player);
                    }
                }
            }

            return base.PreAI(npc);
        }
    }
}
