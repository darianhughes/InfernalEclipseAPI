using CalamityMod;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.Platinum;
using SOTS.NPCs.Boss;

namespace InfernalEclipseAPI.Common.Globals.GlobalNPCs.InfernalRelics
{
    [ExtendsFromMod("SOTS")]
    [JITWhenModsEnabled("SOTS")]
    public class SOTSInfernalRelics : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == ModContent.NPCType<SubspaceSerpentHead>())
            {
                npcLoot.AddIf(() => !NPC.downedMoonlord, ModContent.ItemType<SubspaceSerpentRelicPlatinum>());
            }
        }
    }
}