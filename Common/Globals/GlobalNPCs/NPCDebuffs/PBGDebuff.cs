using CalamityMod.NPCs.PlaguebringerGoliath;
using InfernalEclipseAPI.Content.Buffs;

namespace InfernalEclipseAPI.Common.Globals.GlobalNPCs.NPCDebuffs
{
    public class PBGDebuff : GlobalNPC
    {
        public override bool PreAI(NPC npc)
        {
            if (!InfernalConfig.Instance.PreventBossCheese || !npc.active || npc.type != ModContent.NPCType<PlaguebringerGoliath>()) return base.PreAI(npc);

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && npc.Distance(player.Center) < 10000f)
                {
                    player.AddBuff(ModContent.BuffType<WarpJammed>(), 2);
                }
            }

            return base.PreAI(npc);
        }
    }
}
