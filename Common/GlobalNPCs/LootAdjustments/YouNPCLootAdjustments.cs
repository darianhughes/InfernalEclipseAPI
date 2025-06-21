using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using InfernalEclipseAPI.Content.Items.Lore;
using Terraria;
using Terraria.ModLoader;
using YouBoss.Content.NPCs.Bosses.TerraBlade;
using YouBoss.Core;

namespace InfernalEclipseAPI.Common.GlobalNPCs.LootAdjustments
{
    [ExtendsFromMod("YouBoss")]
    public class YouNPCLootAdjustments : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == ModContent.NPCType<TerraBladeBoss>())
            {
                bool firstTerraBladeKill() => !WorldSaveSystem.HasDefeatedYourself;
                npcLoot.AddConditionalPerPlayer(firstTerraBladeKill, ModContent.ItemType<LoreMirror>(), desc: DropHelper.FirstKillText);
            }
           
        }
    }
}
