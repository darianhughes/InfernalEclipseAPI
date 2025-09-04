using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Mounts;
using CalamityMod.NPCs.DevourerofGods;
using Terraria;
using Terraria.ModLoader;

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

            Player target = Main.player[npc.target];
            if (clamity != null)
            {
                if (target.mount?.Type == clamity.Find<ModMount>("PlagueChairMount").Type)
                    target.mount.Dismount(target);
            }

            return base.PreAI(npc);
        }
    }
}
