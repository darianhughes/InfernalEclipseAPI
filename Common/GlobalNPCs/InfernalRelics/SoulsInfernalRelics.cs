using CalamityMod;
using FargowiltasSouls.Content.Bosses.AbomBoss;
using FargowiltasSouls.Content.Bosses.BanishedBaron;
using FargowiltasSouls.Content.Bosses.Champions.Cosmos;
using FargowiltasSouls.Content.Bosses.Champions.Earth;
using FargowiltasSouls.Content.Bosses.Champions.Life;
using FargowiltasSouls.Content.Bosses.Champions.Nature;
using FargowiltasSouls.Content.Bosses.Champions.Shadow;
using FargowiltasSouls.Content.Bosses.Champions.Spirit;
using FargowiltasSouls.Content.Bosses.Champions.Terra;
using FargowiltasSouls.Content.Bosses.Champions.Will;
using FargowiltasSouls.Content.Bosses.CursedCoffin;
using FargowiltasSouls.Content.Bosses.DeviBoss;
using FargowiltasSouls.Content.Bosses.MutantBoss;
using FargowiltasSouls.Content.Bosses.TrojanSquirrel;
using InfernalEclipseAPI.Content.Items.Accessories;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.FargosSouls;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;

namespace InfernalEclipseAPI.Common.GlobalNPCs.InfernalRelics
{
    [ExtendsFromMod("FargowiltasSouls")]
    public class SoulsInfernalRelics : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            bool isInfernum() => InfernumSaveSystem.InfernumModeEnabled;
            if (npc.type == ModContent.NPCType<AbomBoss>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<AbominationnRelic>());
            }
            if (npc.type == ModContent.NPCType<BanishedBaron>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<BanishedBaronRelic>());
            }
            if (npc.type == ModContent.NPCType<CursedCoffin>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<CursedCoffinRelic>());
            }
            if (npc.type == ModContent.NPCType<ShadowChampion>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<DeathChampionRelic>());
            }
            if (npc.type == ModContent.NPCType<DeviBoss>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<DevianttRelic>());
            }
            if (npc.type == ModContent.NPCType<EarthChampion>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<EarthChampionRelic>());
            }
            if (npc.type == ModContent.NPCType<CosmosChampion>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<EridanusRelic>());
            }
            if (npc.type == ModContent.NPCType<LifeChampion>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<LifeChampionRelic>());
            }
            if (npc.type == ModContent.NPCType<FargowiltasSouls.Content.Bosses.Lifelight.LifeChallenger>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<LifelightRelic>());
            }
            if (npc.type == ModContent.NPCType<MutantBoss>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<MutantRelic>());
            }
            if (npc.type == ModContent.NPCType<NatureChampion>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<NatureChampionRelic>());
            }
            if (npc.type == ModContent.NPCType<SpiritChampion>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<SpiritChampionRelic>());
            }
            if (npc.type == ModContent.NPCType<TerraChampion>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<TerraChampionRelic>());
            }
            if (npc.type == ModContent.NPCType<TrojanSquirrel>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<TrojanSquirrelRelic>());
            }
            if (npc.type == ModContent.NPCType<WillChampion>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<WillChampionRelic>());
            }

            if (ModLoader.TryGetMod("ssm", out Mod CSE))
            {
                if (npc.type == CSE.Find<ModNPC>("MutantEX").Type)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoltanBullyingSlip>()));
                }
            }
        }
    }
}
