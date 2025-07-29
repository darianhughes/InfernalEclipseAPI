using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;
using CalamityMod;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.Consolaria;

namespace InfernalEclipseAPI.Common.GlobalNPCs.InfernalRelics
{
    public partial class ConsolariaInfernalRelics : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (ModLoader.TryGetMod("Consolaria", out Mod console))
            {
                bool isInfernum() => InfernumSaveSystem.InfernumModeEnabled;
                if (npc.type == console.Find<ModNPC>("Lepus").Type)
                {
                    npcLoot.AddIf(isInfernum, ModContent.ItemType<LepusRelic>());
                }
                if (npc.type == console.Find<ModNPC>("TurkortheUngrateful").Type)
                {
                    npcLoot.AddIf(isInfernum, ModContent.ItemType<TurkorTheUngratefulRelic>());
                }
                if (npc.type == console.Find<ModNPC>("Ocram").Type)
                {
                    npcLoot.AddIf(isInfernum, ModContent.ItemType<OcramRelic>());
                }
            }
        }
    }
}
