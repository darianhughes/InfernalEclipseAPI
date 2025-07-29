using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Items.Lore;
using Terraria.ModLoader;
using Terraria;
using YouBoss.Content.NPCs.Bosses.TerraBlade;
using CalamityMod.Items.Materials;
using CalamityMod;
using ThoriumMod.NPCs.Depths;

namespace InfernalEclipseAPI.Common.GlobalNPCs.LootAdjustments
{
    [ExtendsFromMod("ThoriumMod")]
    internal class ThoriumLootAdjustments : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == ModContent.NPCType<GigaClam>())
            {
                npcLoot.AddIf(() => Main.hardMode, ModContent.ItemType<MolluskHusk>(), 2, 1, 1);
            }

        }
    }
}
