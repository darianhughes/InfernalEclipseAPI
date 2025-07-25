using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Mounts;
using CatalystMod.NPCs.Boss.Astrageldon;
using Clamity.Content.Items.Mounts;
using InfernalEclipseAPI.Content.Buffs;
using Terraria;
using Terraria.ModLoader;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;


namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs
{
    [ExtendsFromMod("CatalystMod")]
    public class AstrageldonDebuff : GlobalNPC
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
            if (!InfernalConfig.Instance.CalamityBalanceChanges || !npc.active || npc.type != ModContent.NPCType<Astrageldon>()) return base.PreAI(npc);

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

        public override void PostAI(NPC npc)
        {
            if (!npc.active || npc.type != ModContent.NPCType<Astrageldon>() || !InfernalConfig.Instance.PreventBossCheese)
                return;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && npc.Distance(player.Center) < 8000f)
                {
                    player.AddBuff(ModContent.BuffType<StarboundHorrification>(), 60);
                }
            }
        }
    }
}
