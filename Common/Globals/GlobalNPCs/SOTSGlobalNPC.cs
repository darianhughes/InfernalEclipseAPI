using System.Linq;
using CalamityMod;
using CalamityMod.Items.Mounts;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.Projectiles.Boss;
using InfernalEclipseAPI.Content.Items.Placeables.Relics.SOTS;
using InfernalEclipseAPI.Core.Systems;
using InfernumMode;
using InfernumMode.Content.BehaviorOverrides.BossAIs.SupremeCalamitas;
using InfernumMode.Core.GlobalInstances.Systems;
using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.NPCs.Boss;
using SOTS.NPCs.Boss.Advisor;
using SOTS.NPCs.Boss.Glowmoth;
using SOTS.NPCs.Boss.Lux;
using SOTS.NPCs.Boss.Polaris.NewPolaris;
using SOTS.NPCs.Boss.Polaris;
using SOTS.Void;
using ThoriumMod.NPCs.BossGraniteEnergyStorm;
using InfernalEclipseAPI.Content.Items.Lore.SOTS;
using SOTS.Items.Fragments;
using SOTS.NPCs.TreasureSlimes;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.Items.Materials;
using CalamityMod.Items.TreasureBags;

namespace InfernalEclipseAPI.Common.Globals.GlobalNPCs
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class SOTSGlobalNPC : GlobalNPC
    {
        public bool canDoVoidDamage = false;
        public bool strongVoidDamge = false;
        public override bool InstancePerEntity => true;

        public override void SetDefaults(NPC entity)
        {
            int[] curseImmune =
            {
                NPCID.WallofFlesh,
                NPCID.WallofFleshEye,
                NPCID.Retinazer,
                NPCID.Spazmatism,
                NPCID.SkeletronPrime,
                NPCID.PrimeCannon,
                NPCID.PrimeLaser,
                NPCID.PrimeSaw,
                NPCID.PrimeVice,
                ModContent.NPCType<CrimulanPaladin>(),
                ModContent.NPCType<EbonianPaladin>(),
                ModContent.NPCType<SlimeGodCore>(),
                NPCID.TheDestroyer,
                NPCID.TheDestroyerBody,
                NPCID.TheDestroyerTail,
                NPCID.Probe

            };

            if (curseImmune.Contains(entity.type) && WorldSaveSystem.InfernumModeEnabled)
            {
                entity.buffImmune[ModContent.BuffType<CurseVision>()] = true;
                entity.buffImmune[ModContent.BuffType<PharaohsCurse>()] = true;
            }

            if (InfernalCrossmod.Thorium.Loaded)
            {
                if (CurseImmuneThoriumBosses.curseImmune.Contains(entity.type) && WorldSaveSystem.InfernumModeEnabled)
                {
                    entity.buffImmune[ModContent.BuffType<CurseVision>()] = true;
                    entity.buffImmune[ModContent.BuffType<PharaohsCurse>()] = true;
                }
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (!InfernalConfig.Instance.SOTSBalanceChanges || !npc.active || (npc.type != ModContent.NPCType<SubspaceSerpentHead>() && npc.type != ModContent.NPCType<Lux>())) return base.PreAI(npc);

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.dead || !player.active || !npc.WithinRange(player.Center, 10000f))
                    continue;

                if (player.mount?.Type == ModContent.MountType<DraedonGamerChairMount>())
                    player.mount.Dismount(player);
                if (InfernalCrossmod.Clamity.Loaded)
                {
                    if (player.mount?.Type == InfernalCrossmod.Clamity.Mod.Find<ModMount>("PlagueChairMount").Type)
                        player.mount.Dismount(player);
                }

                player.DoInfiniteFlightCheck(Color.LimeGreen);
            }

            return base.PreAI(npc);
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo info)
        {
            if (canDoVoidDamage)
            {
                int damage = 1 + npc.damage / (strongVoidDamge ? 3 : 6);
                VoidPlayer.VoidDamage(Mod, target, damage);
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == ModContent.NPCType<CrimsonTreasureSlime>() || npc.type == ModContent.NPCType<CorruptionTreasureSlime>())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BlightedGel>(), 1, 7, 13));
            }

            if (npc.type == ModContent.NPCType<Cryogen>())
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<FragmentOfPermafrost>(), 1, 12, 18));
            }

            #region Lore Items
            if (npc.type == ModContent.NPCType<TheAdvisorHead>())
            {
                bool firstAdvisorKill() => !SOTS.SOTSWorld.downedAdvisor;
                npcLoot.AddConditionalPerPlayer(firstAdvisorKill, ModContent.ItemType<LoreAdvisor>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<Glowmoth>())
            {
                bool firstGlowmothKill() => !SOTS.SOTSWorld.downedGlowmoth;
                npcLoot.AddConditionalPerPlayer(firstGlowmothKill, ModContent.ItemType<LoreGlowmoth>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<Lux>())
            {
                bool firstLuxKill() => !SOTS.SOTSWorld.downedLux;
                npcLoot.AddConditionalPerPlayer(firstLuxKill, ModContent.ItemType<LoreLux>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<SOTS.NPCs.Boss.Curse.PharaohsCurse>())
            {
                bool firstCurseKill() => !SOTS.SOTSWorld.downedCurse;
                npcLoot.AddConditionalPerPlayer(firstCurseKill, ModContent.ItemType<LorePharaoh>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<Polaris>() || npc.type == ModContent.NPCType<NewPolaris>())
            {
                bool firstPolarisKill() => !SOTS.SOTSWorld.downedAmalgamation;
                npcLoot.AddConditionalPerPlayer(firstPolarisKill, ModContent.ItemType<LorePolaris>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<PutridPinkyPhase2>())
            {
                bool firstPutridKill() => !SOTS.SOTSWorld.downedPinky;
                npcLoot.AddConditionalPerPlayer(firstPutridKill, ModContent.ItemType<LorePutrid>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<SubspaceSerpentHead>())
            {
                bool firstSupspaceKill() => !SOTS.SOTSWorld.downedSubspace;
                npcLoot.AddConditionalPerPlayer(firstSupspaceKill, ModContent.ItemType<LoreSerpent>(), desc: DropHelper.FirstKillText);
            }
            #endregion

            #region Infernal Relics
            static bool isInfernum() => WorldSaveSystem.InfernumModeEnabled;
            Mod sots = ModLoader.GetMod("SOTS");
            if (npc.type == ModContent.NPCType<Glowmoth>())
            {
                npcLoot.AddIf(isInfernum, ModContent.ItemType<GlowmothRelic>());
            }
            if (npc.type == ModContent.NPCType<SOTS.NPCs.Boss.Curse.PharaohsCurse>())
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
            #endregion
        }
    }

    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class VoidDamageProjectile : GlobalProjectile
    {
        public bool canDoVoidDamage = false;
        public bool strongVoidDamge = false;
        public override bool InstancePerEntity => true;

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (projectile.type == ModContent.ProjectileType<SupremeCataclysmFist>() || projectile.type == ModContent.ProjectileType<SupremeCatastropheSlash>() || projectile.type == ModContent.ProjectileType<SupremeCataclysmFistOld>() || projectile.type == ModContent.ProjectileType<CatastropheSlash>()
                || canDoVoidDamage)
            {
                int damage = 1 + projectile.damage / (strongVoidDamge ? 3 : 6);
                VoidPlayer.VoidDamage(Mod, target, damage);
            }
        }
    }

    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class SOTSBossBagChanges : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ModContent.ItemType<CryogenBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfPermafrost>(), 1, 15, 21));
            }
        }
    }

    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public static class CurseImmuneThoriumBosses
    {
        public static int[] curseImmune =
        {
            ModContent.NPCType<GraniteEnergyStorm>()
        };
    }
}
