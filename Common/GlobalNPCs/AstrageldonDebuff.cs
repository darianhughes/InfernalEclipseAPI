using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalystMod.NPCs.Boss.Astrageldon;
using InfernalEclipseAPI.Content.Buffs;
using Terraria;
using Terraria.ModLoader;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;


namespace InfernalEclipseAPI.Common.GlobalNPCs
{
    [ExtendsFromMod("CatalystMod")]
    public class AstrageldonDebuff : GlobalNPC
    {
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
