using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Mounts;
using CatalystMod.NPCs.Boss.Astrageldon;
using SOTS.NPCs.Boss;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs
{
    [ExtendsFromMod("SOTS")]
    public class SubspaceSerpantDebuffs : GlobalNPC
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
            if (!InfernalConfig.Instance.SOTSBalanceChanges || !npc.active || npc.type != ModContent.NPCType<SubspaceSerpentHead>()) return base.PreAI(npc);

            Player target = Main.player[npc.target];
            if (target.mount?.Type == ModContent.MountType<DraedonGamerChairMount>())
                target.mount.Dismount(target);
            if (clamity != null)
            {
                if (target.mount?.Type == clamity.Find<ModMount>("PlagueChairMount").Type)
                    target.mount.Dismount(target);
            }

            return base.PreAI(npc);
        }
    }
}
