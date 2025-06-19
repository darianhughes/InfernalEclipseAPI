﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Items.Materials;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.ProgressionRework
{
    public class BossDropChanges : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (ModLoader.TryGetMod("SOTS", out Mod sots))
            {
                if (npc.type == sots.Find<ModNPC>("Glowmoth").Type) 
                {
                    npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<InfectedMothwingSpore>()));
                }
            }
        }
    }

    public class BossBagChanges : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (ModLoader.TryGetMod("SOTS", out Mod sots))
            {
                if (item.type == sots.Find<ModItem>("GlowmothBag").Type)
                {
                    itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<InfectedMothwingSpore>()));
                }
            }
        }
    }
}
