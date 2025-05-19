using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.SummonItems;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.ProgressionRework
{
    public class ProvidenceRuneOfKosFix : GlobalNPC
    {
        public override bool InstancePerEntity => false;
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            //Ensures Providence drops the Rune of Kos
            if (npc.type == ModContent.NPCType<CalamityMod.NPCs.Providence.Providence>())
            {
                bool hasRuneDrop = false;
                foreach (var rule in npcLoot.Get(false))
                {
                    if (rule is CommonDrop commonDrop && commonDrop.itemId == ModContent.ItemType<RuneofKos>())
                    {
                        hasRuneDrop = true;
                        break;
                    }
                }

                if (!hasRuneDrop)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RuneofKos>(), 1));
                }
            }
        }
    }
}
