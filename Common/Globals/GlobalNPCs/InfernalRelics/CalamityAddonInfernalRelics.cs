using CalamityMod;
using CalamityMod.Items.Mounts;
using CatalystMod.NPCs.Boss.Astrageldon;
using Clamity.Content.Bosses.Clamitas.NPCs;
using Clamity.Content.Bosses.Pyrogen.NPCs;
using Clamity.Content.Bosses.WoB.NPCs;
using InfernalEclipseAPI.Content.Items.Accessories;
using InfernalEclipseAPI.Content.Items.Materials;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons.Clamity;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.CalamityAddons.WoTG;
using InfernalEclipseAPI.Core.Systems;
using NoxusBoss.Assets;
using NoxusBoss.Content.Items;
using NoxusBoss.Content.NPCs.Bosses.Avatar.SecondPhaseForm;
using NoxusBoss.Content.NPCs.Bosses.NamelessDeity;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
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
        public override bool PreAI(NPC npc)
        {
            if (!InfernalConfig.Instance.CalamityBalanceChanges || !npc.active || npc.type != ModContent.NPCType<NamelessDeityBoss>()) return base.PreAI(npc);

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead)
                {
                    if (player.mount?.Type == ModContent.MountType<DraedonGamerChairMount>())
                    {
                        player.mount.Dismount(player);
                        SoundEngine.PlaySound(GennedAssets.Sounds.NamelessDeity.Chuckle, player.Center);
                    }
                }
            }
            if (InfernalCrossmod.Clamity.Loaded)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player.active && !player.dead)
                    {
                        if (player.mount?.Type == InfernalCrossmod.Clamity.Mod.Find<ModMount>("PlagueChairMount").Type)
                        {
                            player.mount.Dismount(player);
                            SoundEngine.PlaySound(GennedAssets.Sounds.NamelessDeity.Chuckle, player.Center);
                        }
                    }
                }
            }

            return base.PreAI(npc);
        }

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