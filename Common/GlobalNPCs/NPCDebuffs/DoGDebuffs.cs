using CalamityMod.NPCs.DevourerofGods;
using InfernalEclipseAPI.Core.Systems;

namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs
{
    public class DoGDebuffs : GlobalNPC
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
            if (!InfernalConfig.Instance.CalamityBalanceChanges || !npc.active || npc.type != ModContent.NPCType<DevourerofGodsHead>()) return base.PreAI(npc);

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead)
                {
                    if (clamity != null)
                    {
                        if (player.mount?.Type == clamity.Find<ModMount>("PlagueChairMount").Type)
                            player.mount.Dismount(player);
                    }
                    if (InfernalCrossmod.Thorium.Loaded)
                    {
                        if (InfernalCrossmod.Thorium.Mod.TryFind("SuperAnvilMount", out ModMount supAnvil))
                            if (player.mount?.Type == supAnvil.Type)
                                player.mount.Dismount(player);
                    }
                }
            }

            return base.PreAI(npc);
        }
    }
}
