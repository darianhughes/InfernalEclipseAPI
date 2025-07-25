using Terraria;
using Terraria.ModLoader;
using YouBoss.Content.NPCs.Bosses.TerraBlade;
using YouBoss.Core.Graphics.Shaders.Screen;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;

namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs
{
    [ExtendsFromMod("YouBoss")]
    public class TerraBladeDebuff : GlobalNPC
    {
        public override void PostAI(NPC npc)
        {
            if (!npc.active || npc.type != ModContent.NPCType<TerraBladeBoss>() || !InfernumSaveSystem.InfernumModeEnabled)
                return;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && npc.Distance(player.Center) < 8000f)
                {
                    player.ClearBuff(ModContent.BuffType<GracedWings>());
                }
            }
        }
    }
}
