using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;
using SOTS.NPCs.Boss.Advisor;
using SOTS.NPCs.Boss;
using CalamityMod;
using SOTS.NPCs.Boss.Glowmoth;
using SOTS.NPCs.Boss.Polaris;
using SOTS.NPCs.Boss.Polaris.NewPolaris;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.SOTS;
using SOTS.NPCs.Boss.Curse;
using SOTS.NPCs.Boss.Lux;

namespace InfernalEclipseAPI.Common.GlobalNPCs.InfernalRelics
{
    [ExtendsFromMod("SOTS")]
    public partial class SOTSInfernalRelics : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            bool isInfernum() => InfernumSaveSystem.InfernumModeEnabled;
            Mod sots = ModLoader.GetMod("SOTS");
            if (npc.type == ModContent.NPCType<Glowmoth>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<GlowmothRelic>());
            }
            if (npc.type == ModContent.NPCType<PharaohsCurse>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<PharohsCurseRelic>());
            }
            if (npc.type == ModContent.NPCType<PutridPinkyPhase2>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<PutridPinkyRelic>());
            }
            if (npc.type == ModContent.NPCType<PutridPinkyPhase2>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<PutridPinkyRelic>());
            }
            if (npc.type == sots.Find<ModNPC>("Excavator").Type)
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<ExcavatorRelic>());
            }
            if (npc.type == ModContent.NPCType<TheAdvisorHead>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<AdvisorRelic>());
            }
            if (npc.type == ModContent.NPCType<Polaris>() || npc.type == ModContent.NPCType<NewPolaris>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<PolarisRelic>());
            }
            if (npc.type == ModContent.NPCType<Lux>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<LuxRelic>());
            }
            if (npc.type == ModContent.NPCType<SubspaceSerpentHead>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<SubspaceSerpentRelic>());
            }
        }
    }
}
