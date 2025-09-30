using InfernalEclipseAPI.Content.Buffs;
using NoxusBoss.Content.NPCs.Bosses.Draedon;


namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs.ExoMechs
{
    [ExtendsFromMod("NoxusBoss")]
    public class ExoMarsDebuff : GlobalNPC
    {
        public override void PostAI(NPC npc)
        {
            if (!npc.active || npc.type != ModContent.NPCType<MarsBody>() || !InfernalConfig.Instance.PreventBossCheese)
                return;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && npc.Distance(player.Center) < 8000f)
                {
                    player.AddBuff(ModContent.BuffType<WarpJammed>(), 60);
                }
            }
        }
    }
}
