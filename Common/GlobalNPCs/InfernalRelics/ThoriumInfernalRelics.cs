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
using ThoriumMod.NPCs.BossBoreanStrider;
using ThoriumMod.NPCs.BossBuriedChampion;
using ThoriumMod.NPCs.BossFallenBeholder;
using ThoriumMod.NPCs.BossForgottenOne;
using ThoriumMod.NPCs.BossGraniteEnergyStorm;
using ThoriumMod.NPCs.BossLich;
using ThoriumMod.NPCs.BossMini;
using ThoriumMod.NPCs.BossQueenJellyfish;
using ThoriumMod.NPCs.BossStarScouter;
using ThoriumMod.NPCs.BossTheGrandThunderBird;
using ThoriumMod.NPCs.BossThePrimordials;
using ThoriumMod.NPCs.BossViscount;
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
            if (npc.type == ModContent.NPCType<PatchWerk>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<PatchWerkRelic>());
            }
            if (npc.type == ModContent.NPCType<QueenJellyfish>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<QueenJellyfishRelic>());
            }
            if (npc.type == ModContent.NPCType<Viscount>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<ViscountRelic>());
            }
            if (npc.type == ModContent.NPCType<CorpseBloom>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<CorpseBloomRelic>());
            }
            if (npc.type == ModContent.NPCType<GraniteEnergyStorm>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<GraniteEnergyStormRelic>());
            }
            if (npc.type == ModContent.NPCType<BuriedChampion>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<BurriedChampionRelic>());
            }
            if (npc.type == ModContent.NPCType<StarScouter>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<StarScouterRelic>());
            }
            if (npc.type == ModContent.NPCType<BoreanStriderPopped>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<BoreanStriderRelic>());
            }
            if (npc.type == ModContent.NPCType<FallenBeholder>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<FallenBeholderRelic>());
            }
            if (npc.type == ModContent.NPCType<LichHeadless>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<LichRelic>());
            }
            if (npc.type == ModContent.NPCType<ForgottenOneReleased>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<ForgottenOneRelic>());
            }
            if (npc.type == ModContent.NPCType<DreamEater>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<PrimordialsRelic>());
            }
        }
    }
}
