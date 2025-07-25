using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CatalystMod.NPCs.Boss.Astrageldon;
using InfernalEclipseAPI.Content.Items.Placeables.Relics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.NPCs.BossTheGrandThunderBird;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;

namespace InfernalEclipseAPI.Common.GlobalNPCs.InfernalRelics
{
    [ExtendsFromMod("CatalystMod")]
    public class CatalystInfernalRelics : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            bool isInfernum() => InfernumSaveSystem.InfernumModeEnabled;
            if (npc.type == ModContent.NPCType<Astrageldon>() && !ModLoader.TryGetMod("CnI", out _))
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<AstrageldonRelic>());
            }
        }
    }
}