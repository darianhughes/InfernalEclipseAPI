using CalamityMod.NPCs.Yharon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using YouBoss.Content.NPCs.Bosses.TerraBlade;

namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs
{
    public class YharonDebuffs : GlobalNPC
    {
        public override bool PreAI(NPC npc)
        {
            if (!InfernalConfig.Instance.PreventBossCheese || !npc.active || npc.type != ModContent.NPCType<Yharon>()) return base.PreAI(npc);

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead)
                {
                    if (player.mount?.Type == MountID.GolfCartSomebodySaveMe)
                        player.mount.Dismount(player);
                }
            }

            return base.PreAI(npc);
        }
    }
}
