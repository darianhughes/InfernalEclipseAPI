using CalamityMod.NPCs.Yharon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs
{
    public class YharonDebuffs : GlobalNPC
    {
        public override bool PreAI(NPC npc)
        {
            if (!InfernalConfig.Instance.PreventBossCheese || !npc.active || npc.type != ModContent.NPCType<Yharon>()) return base.PreAI(npc);

            Player target = Main.player[npc.target];
            if (target.mount?.Type == MountID.GolfCartSomebodySaveMe)
                target.mount.Dismount(target);

            return base.PreAI(npc);
        }
    }
}
