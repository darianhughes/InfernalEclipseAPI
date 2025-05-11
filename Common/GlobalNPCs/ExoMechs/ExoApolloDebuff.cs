using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Artemis;
using InfernalEclipseAPI.Content.Buffs;
using Terraria;
using Terraria.ModLoader;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;


namespace InfernalEclipseAPI.Common.GlobalNPCs.ExoMechs
{
    public class ExoApolloDebuff : GlobalNPC
    {
        public override void PostAI(NPC npc)
        {
            if (!npc.active || npc.type != ModContent.NPCType<Apollo>() || !InfernalConfig.Instance.PreventBossCheese)
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
