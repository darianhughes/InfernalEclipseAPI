using InfernalEclipseAPI.Core.World;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.Yharon;
using InfernalEclipseAPI.Content.Items.Placeables.Paintings;
using Terraria.GameContent.ItemDropRules;
using InfernalEclipseAPI.Core.Systems;
using System.Collections.Generic;
using InfernumMode.Core.GlobalInstances.Systems;
using CalamityMod.World;
using CalamityMod.NPCs.Crags;
using CalamityMod.Items.Fishing.FishingRods;
using System.Linq;
using InfernalEclipseAPI.Content.Items.Materials;
using InfernalEclipseAPI.Core.Players;
using CalamityMod.Items.Placeables.Furniture.Paintings;
using Terraria.DataStructures;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.CalPlayer;
using CalamityMod;
using InfernalEclipseAPI.Core.Players.ThoriumPlayerOverrides.ThoriumMulticlassNerf;
using InfernalEclipseAPI.Core.Utils;
using CalamityMod.Buffs.StatDebuffs;
using InfernumMode.Content.BehaviorOverrides.BossAIs.ProfanedGuardians;
using Terraria;

namespace InfernalEclipseAPI.Common.GlobalNPCs
{
    public class InfernalGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(NPC entity)
        {
            if (InfernalCrossmod.Thorium.Loaded)
            {
                if (entity.buffImmune[BuffID.Confused] && entity.boss)
                {
                    entity.buffImmune[InfernalCrossmod.Thorium.Mod.Find<ModBuff>("Stunned").Type] = true;
                }
            }

            switch (entity.type)
            {
                case NPCID.DetonatingBubble:
                    if (WorldSaveSystem.InfernumModeEnabled && Main.masterMode && CalamityWorld.revenge && NPC.AnyNPCs(NPCID.DukeFishron))
                        entity.dontTakeDamage = false;
                    break;
                default:
                    break;
            }
        }

        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            if (npc.type == NPCID.GoblinTinkerer && InfernalConfig.Instance.BossKillCheckOnOres)
            {
                // Replace Tinkerer's Workshop by filtering the Entries list
                for (int i = 0; i < items.Length; i++)
                {
                    var item = items[i];
                    if (item != null && !item.IsAir && item.type == ItemID.TinkerersWorkshop)
                    {
                        // Replace workshop with blueprint
                        items[i] = new Item(ModContent.ItemType<TinkerersRepairBlueprints>());
                    }
                }

                bool someoneHasOwnedWorkshop = false;

                static bool HasWorkshop(Item[] arr)
                {
                    if (arr is null) return false;
                    foreach (var it in arr)
                        if (!it.IsAir && it.type == ItemID.TinkerersWorkshop)
                            return true;
                    return false;
                }

                foreach (var player in Main.ActivePlayers)
                {
                    if (HasWorkshop(player.inventory) || HasWorkshop(player.armor) ||
                        HasWorkshop(player.bank?.item) || HasWorkshop(player.bank2?.item) ||
                        HasWorkshop(player.bank3?.item) || HasWorkshop(player.bank4?.item))
                    {
                        player.GetModPlayer<InfernalPlayer>().workshopHasBeenOwned = true;
                    }

                    if (player.GetModPlayer<InfernalPlayer>().workshopHasBeenOwned)
                    {
                        someoneHasOwnedWorkshop = true;
                        break;
                    }
                }

                if (someoneHasOwnedWorkshop || InfernalWorld.craftedWorkshop || Main.hardMode) // start selling it again after its been obtained at least once; and will always sell it again in hardmode
                {
                    // Find first empty slot
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i] == null || items[i].IsAir)
                        {
                            items[i] = new Item(ItemID.TinkerersWorkshop);
                            break;
                        }
                    }
                }
            }

            if (ModLoader.TryGetMod("CalamityAmmo", out Mod calamityAmmo) && InfernalConfig.Instance.CalamityBalanceChanges)
            {
                int hydroArrow = calamityAmmo.Find<ModItem>("HydrothermicArrow").Type;
                int hydroBullet = calamityAmmo.Find<ModItem>("HydrothermicBullet").Type;

                // Remove matching entries
                for (int i = 0; i < items.Length; i++)
                {
                    Item item = items[i];

                    if (item == null || item.IsAir)
                        continue;

                    if (item.type == hydroArrow || item.type == hydroBullet)
                        item.TurnToAir();
                }
            }
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (InfernalWorld.RagnarokModeEnabled && npc.boss)
            {
                foreach (Player player in Main.player)
                {
                    if (player.active && !player.dead)
                    {
                        player.ClearBuff(ModContent.BuffType<RageMode>());
                        player.ClearBuff(ModContent.BuffType<AdrenalineMode>());

                        CalamityPlayer mp = player.Calamity();
                        mp.rage = 0;
                        mp.rageModeActive = false;
                        mp.adrenaline = 0;
                        mp.adrenalineModeActive = false;

                        if (InfernalCrossmod.Thorium.Loaded)
                        {
                            if (InfernalCrossmod.RagnarokMod.Loaded)
                            {
                                RagnarokRiffSystemInteraction.ClearRiffs(player);
                            }

                            // clear locally
                            ThoriumHelpers.ClearAllEmpowerments(player);

                            // and tell others
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                ModPacket p = Mod.GetPacket();
                                p.Write((byte)InfernalEclipseMessageType.ThoriumEmpowerment);
                                p.Write((byte)ThoriumEmpowermentMsg.ClearEmpowerments);
                                p.Write((byte)player.whoAmI);
                                p.Send();
                            }
                        }
                    }
                }
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (!npc.active) return base.PreAI(npc);

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.dead || !player.active || !npc.WithinRange(player.Center, 10000f))
                    continue;

                if (InfernalConfig.Instance.VanillaBalanceChanges && npc.type == NPCID.HallowBoss)
                {
                    if (InfernalCrossmod.Clamity.Loaded)
                    {
                        if (player.mount?.Type == InfernalCrossmod.Clamity.Mod.Find<ModMount>("PlagueChairMount").Type)
                            player.mount.Dismount(player);
                    }
                }

                if (InfernalWorld.RagnarokModeEnabled && npc.type == NPCID.Golem)
                {
                    player.AddBuff(ModContent.BuffType<WeakPetrification>(), 2);
                }

                if (InfernalConfig.Instance.PreventBossCheese && npc.type == ModContent.NPCType<HealerShieldCrystal>())
                {
                    player.ClearBuff(ModContent.BuffType<RageMode>());
                    player.ClearBuff(ModContent.BuffType<AdrenalineMode>());

                    CalamityPlayer mp = player.Calamity();
                    mp.rage = 0;
                    mp.rageModeActive = false;
                    mp.adrenaline = 0;
                    mp.adrenalineModeActive = false;
                }
            }

            return base.PreAI(npc);
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.boss && npc.type != NPCID.TorchGod) //anything that is considered a boss will have a 1/100 chance to drop our dev painting directly
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<InfernalTwilight>(), ThankYouPainting.DropInt));
            }

            if (npc.type == ModContent.NPCType<SoulSlurper>() && InfernalConfig.Instance.BossKillCheckOnOres)
            {
                int slurperPole = ModContent.ItemType<SlurperPole>();

                npcLoot.RemoveWhere(rule =>
                    rule is CommonDrop cd && cd.itemId == slurperPole ||
                    rule is ItemDropWithConditionRule iwc && iwc.itemId == slurperPole);

                foreach (var rule in npcLoot.Get())
                    PruneFromChains(rule, slurperPole);

                npcLoot.Add(ItemDropRule.ByCondition(new EvilBossDownedCondition(), slurperPole, 30));
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.Player;
            if (InfernalCrossmod.SOTS.Loaded)
            {
                if (player.ZoneHallow && Main.hardMode && !TwoMechsDowned())
                {
                    pool.Remove(InfernalCrossmod.SOTS.Mod.Find<ModNPC>("HallowTreasureSlime").Type);
                }
            }
        }

        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.TheDestroyer)
            {
                InfernalWorld.dreadonDestroyerDialoguePlayed = false;
                InfernalWorld.dreadonDestroyer2DialoguePlayed = false;
            }
            if (npc.type == NPCID.Plantera)
            {
                InfernalWorld.jungleSubshockPlanteraDialoguePlayed = false;
                InfernalWorld.jungleSlagspitterPlateraDiaglougePlayer = false;
            }
            if (npc.type == ModContent.NPCType<BrimstoneElemental>() || npc.type == ModContent.NPCType<AquaticScourgeHead>())
            {
                InfernalWorld.sulfurScourgeDialoguePlayed = false;
                InfernalWorld.brimstoneDialoguePlayed = false;
            }
            if (npc.type == ModContent.NPCType<Yharon>())
            {
                InfernalWorld.yharonDischarge = false;
                InfernalWorld.yharonSmasher = false;
            }
        }

        public override bool CheckDead(NPC npc)
        {
            if (npc.type == NPCID.TheDestroyer)
            {
                InfernalWorld.dreadonDestroyerDialoguePlayed = false;
                InfernalWorld.dreadonDestroyer2DialoguePlayed = false;
            }
            if (npc.type == NPCID.Plantera)
            {
                InfernalWorld.jungleSubshockPlanteraDialoguePlayed = false;
                InfernalWorld.jungleSlagspitterPlateraDiaglougePlayer = false;
            }
            if (npc.type == ModContent.NPCType<BrimstoneElemental>() || npc.type == ModContent.NPCType<AquaticScourgeHead>())
            {
                InfernalWorld.sulfurScourgeDialoguePlayed = false;
                InfernalWorld.brimstoneDialoguePlayed = false;
            }

            return base.CheckDead(npc);
        }

        public static bool TwoMechsDowned()
        {
            return (NPC.downedMechBoss1 && NPC.downedMechBoss2) || (NPC.downedMechBoss1 && NPC.downedMechBoss3) || (NPC.downedMechBoss2 && NPC.downedMechBoss3);
        }

        private static void PruneFromChains(IItemDropRule rule, int itemId)
        {
            if (rule.ChainedRules is null || rule.ChainedRules.Count == 0)
                return;

            rule.ChainedRules.RemoveAll(c =>
                c.RuleToChain is CommonDrop cd && cd.itemId == itemId ||
                c.RuleToChain is ItemDropWithConditionRule iwc && iwc.itemId == itemId);

            foreach (var chain in rule.ChainedRules.ToList())
                PruneFromChains(chain.RuleToChain, itemId);
        }

        private sealed class EvilBossDownedCondition : IItemDropRuleCondition
        {
            public bool CanDrop(DropAttemptInfo info) => NPC.downedBoss2;
            public bool CanShowItemDropInUI() => true;
            public string GetConditionDescription() => "Downed Evil Boss";
        }
    }

}
