using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.NPCs.SupremeCalamitas;
using SOTS.NPCs.Boss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs
{
    public class SupremeCalamitasDebuffs : GlobalNPC
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
            Player target = Main.player[npc.target];
            if (clamity != null)
            {
                if (target.mount?.Type == clamity.Find<ModMount>("PlagueChairMount").Type)
                    target.mount.Dismount(target);
            }

            if (!InfernalConfig.Instance.CalamityBalanceChanges || !npc.active || npc.type != ModContent.NPCType<SupremeCalamitas>()) return base.PreAI(npc);
           
            if (target.mount?.Type == MountID.PogoStick)
                target.mount.Dismount(target);

            return base.PreAI(npc);
        }
    }
}
