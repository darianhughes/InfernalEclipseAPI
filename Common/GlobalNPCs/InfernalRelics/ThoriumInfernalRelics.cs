using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.Thorium;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using ThoriumMod.NPCs.BossBuriedChampion;
using ThoriumMod.NPCs.BossMini;
using ThoriumMod.NPCs.BossQueenJellyfish;
using ThoriumMod.NPCs.BossTheGrandThunderBird;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;

namespace InfernalEclipseAPI.Common.GlobalNPCs.InfernalRelics
{
    [ExtendsFromMod("ThoriumMod")]
    public partial class ThoriumInfernalRelics : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            bool isInfernum() => InfernumSaveSystem.InfernumModeEnabled;
            if (npc.type == ModContent.NPCType<TheGrandThunderBird>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<GrandThunderBirdRelic>());
            }
            if (npc.type == ModContent.NPCType<QueenJellyfish>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<QueenJellyfishRelic>());
            }
            if (npc.type == ModContent.NPCType<CorpseBloom>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<CorpseBloomRelic>());
            }
            if (npc.type == ModContent.NPCType<BuriedChampion>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<BurriedChampionRelic>());
            }
        }
    }
}
