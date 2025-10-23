using CalamityMod;
using CatalystMod.NPCs.Boss.Astrageldon;
using Clamity.Content.Bosses.Clamitas.NPCs;
using Clamity.Content.Bosses.Pyrogen.NPCs;
using Clamity.Content.Bosses.WoB.NPCs;
using InfernalEclipseAPI.Content.Items.Accessories;
using InfernalEclipseAPI.Content.Items.Materials;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons.Clamity;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons.WoTG;
using NoxusBoss.Content.Items;
using NoxusBoss.Content.NPCs.Bosses.Avatar.SecondPhaseForm;
using NoxusBoss.Content.NPCs.Bosses.NamelessDeity;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
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

    public class HuntInfernalRelics : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            bool isInfernum() => InfernumSaveSystem.InfernumModeEnabled;
            if (ModLoader.TryGetMod("CalamityHunt", out Mod hunt))
            {
                if (npc.type == hunt.Find<ModNPC>("Goozma").Type)
                {
                    npcLoot.AddIf(isInfernum, ModContent.ItemType<GoozmaRelic>());
                }
            }
        }
    }

    [ExtendsFromMod("NoxusBoss")]
    public class WrathInfernalRelics : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            bool isInfernum() => InfernumSaveSystem.InfernumModeEnabled;
            if (npc.type == ModContent.NPCType<AvatarOfEmptiness>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<AvatarOfEmptinessRelic>());
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<MetallicChunk>(), 1, 4, 9));
            }
            if (npc.type == ModContent.NPCType<NamelessDeityBoss>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<NamelessDeityRelic>());
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoltanBullyingSlip>(), 1));
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<PrimordialOrchid>(), 1, 10, 15));
            }
        }
    }

    [ExtendsFromMod("NoxusBoss")]
    public class WrathTreasureBags : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            Mod noxusBoss = ModLoader.GetMod("NoxusBoss");
            if (item.type == noxusBoss.Find<ModItem>("AvatarTreasureBag").Type)
                itemLoot.Add(ModContent.ItemType<MetallicChunk>(), 1, 4, 9);
            if (item.type == noxusBoss.Find<ModItem>("NamelessDeityTreasureBag").Type)
                itemLoot.Add(ModContent.ItemType<PrimordialOrchid>(), 1, 10, 15);
        }
    }

    public class NoxusInfernalRelic : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            bool isInfernum() => InfernumSaveSystem.InfernumModeEnabled;
            if (ModLoader.TryGetMod("NoxusPort", out Mod port))
            {
                if (npc.type == port.Find<ModNPC>("EntropicGod").Type)
                {
                    npcLoot.AddIf(isInfernum, ModContent.ItemType<NoxusRelic>());
                }
            }
        }
    }

    [ExtendsFromMod("Clamity")]
    public class ClamityInfernalRelic : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            bool isInfernum() => InfernumSaveSystem.InfernumModeEnabled;
            if (npc.type == ModContent.NPCType<PyrogenBoss>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<PyrogenRelic>());
            }
            if (npc.type == ModContent.NPCType<ClamitasBoss>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<ClamitasRelic>());
            }
            if (npc.type == ModContent.NPCType<WallOfBronze>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<WallofBronzeRelic>());
            }
        }
    }
}