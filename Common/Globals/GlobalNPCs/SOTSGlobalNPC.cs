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
using SOTS;
using InfernalEclipseAPI.Content.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.Helpers;
using CalamityMod.NPCs.SunkenSea;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.HiveMind;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Perforators;
using CalamityMod.Projectiles.Boss.BrainOfCthulhu;
using InfernumMode.Content.BehaviorOverrides.BossAIs.BoC;
using InfernumMode.Content.BehaviorOverrides.BossAIs.HiveMind;
using SOTS.Common.GlobalNPCs;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Deerclops;
using Terraria.DataStructures;
using SOTS.NPCs.Town;
using SOTS.Items.ChestItems;
using SOTS.Buffs.Debuffs;

namespace InfernalEclipseAPI.Common.Globals.GlobalNPCs
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class SOTSGlobalNPC : GlobalNPC
    {
        public bool canDoVoidDamage = false;
        public bool strongVoidDamge = false;
        public bool isFlowered;

        private const int MaxAnomalyCurseStacks = 30;

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

            int[] dealsVoidDamge =
            {
                ModContent.NPCType<PerforatorHive>(),
                ModContent.NPCType<PerforatorHeadLarge>(),
                ModContent.NPCType<PerforatorHeadMedium>(),
                ModContent.NPCType<PerforatorHeadSmall>(),
                ModContent.NPCType<PerforatorBodyLarge>(),
                ModContent.NPCType<PerforatorBodyMedium>(),
                ModContent.NPCType<PerforatorBodySmall>(),
                ModContent.NPCType<PerforatorTailLarge>(),
                ModContent.NPCType<PerforatorTailMedium>(),
                ModContent.NPCType<PerforatorTailSmall>(),
                ModContent.NPCType<HiveMind>(),
                ModContent.NPCType<DarkHeart>(),
                ModContent.NPCType<HiveBlob>()
            };

            if (curseImmune.Contains(entity.type) && WorldSaveSystem.InfernumModeEnabled)
            {
                entity.buffImmune[ModContent.BuffType<CurseVision>()] = true;
                entity.buffImmune[ModContent.BuffType<PharaohsCurse>()] = true;
            }

            if (dealsVoidDamge.Contains(entity.type))
            {
                canDoVoidDamage = true;
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

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.boss)
            {
                foreach (Player player in Main.player)
                {
                    if (player.active && !player.dead)
                    {
                        player.ClearBuff(ModContent.BuffType<Embattle>());
                    }
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

        public override void PostAI(NPC npc)
        {
            if (!npc.active)
                return;

            DebuffNPC debuffNPC = npc.GetGlobalNPC<DebuffNPC>();

            if (debuffNPC.AnomalyCurse > MaxAnomalyCurseStacks)
                debuffNPC.AnomalyCurse = MaxAnomalyCurseStacks;

            if (npc.immortal || npc.realLife != -1)
                return;

            float num8 = 0f;
            bool flag3 = false;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (!projectile.active || projectile.timeLeft >= 8998)
                    continue;

                if (projectile.type != ModContent.ProjectileType<EvilGrowth>())
                    continue;

                if (projectile.ModProjectile is not EvilGrowth growth)
                    continue;

                bool flag5 = growth.effected[npc.whoAmI];
                bool flag6 = false;
                int num14 = -1;

                if (flag5 && !npc.immortal && npc.realLife == -1)
                {
                    flag3 = true;

                    if (num8 <= 1f)
                    {
                        SOTSPlayer sotsPlayer = SOTSPlayer.ModPlayer(Main.player[projectile.owner]);
                        if (sotsPlayer.halfLifeRegen < 3)
                            sotsPlayer.halfLifeRegen += 3;
                        sotsPlayer.halfLifeRegen++;
                        if (npc.boss)
                            sotsPlayer.halfLifeRegen++;
                    }

                    if (num14 == npc.whoAmI)
                    {
                        if (flag6)
                            num8++;
                    }
                    else
                    {
                        float num15 = 0.5f;
                        float num16 = 0.025f;
                        if (!flag6 && !npc.boss)
                            num16 = 0.04f;

                        Vector2 vector2_23 =
                            new Vector2(projectile.Center.X, projectile.position.Y - 8f) -
                            new Vector2(npc.Center.X, npc.position.Y + npc.height);

                        float num19 = vector2_23.Length() * num16 * (npc.boss ? 0.01f : 1f);

                        if (!flag6)
                            num15 = 0.65f;

                        Vector2 vector2_25 = vector2_23.SafeNormalize(Vector2.Zero) * (num15 + num19);

                        int[] immuneNPCs =
                        {
                            ModContent.NPCType<PerforatorHeadLarge>(),
                            ModContent.NPCType<PerforatorHeadMedium>(),
                            ModContent.NPCType<PerforatorHeadSmall>(),
                            ModContent.NPCType<PerforatorBodyLarge>(),
                            ModContent.NPCType<PerforatorBodyMedium>(),
                            ModContent.NPCType<PerforatorBodySmall>(),
                            ModContent.NPCType<PerforatorTailLarge>(),
                            ModContent.NPCType<PerforatorTailMedium>(),
                            ModContent.NPCType<PerforatorTailSmall>(),
                            ModContent.NPCType<DarkHeart>(),
                            ModContent.NPCType<GiantClam>(),
                            ModContent.NPCType<LightSnuffingHand>(),
                            ModContent.NPCType<PutridPinky1>()
                        };

                        // The only behavioral change:
                        if (!npc.boss && npc.type != NPCID.EaterofWorldsHead && npc.type != NPCID.EaterofWorldsBody && npc.type != NPCID.EaterofWorldsTail && !immuneNPCs.Contains(npc.type))
                            npc.position += vector2_25;
                    }
                }
            }

            isFlowered = num8 >= 1f;

            float num26 = 1f;
            if (flag3)
            {
                if (!npc.boss && npc.type != NPCID.EaterofWorldsHead && npc.type != NPCID.EaterofWorldsBody && npc.type != NPCID.EaterofWorldsTail && npc.type != ModContent.NPCType<GiantClam>())
                    num26 *= 0.2f;
                else
                    num26 *= 0.875f;
            }

            npc.position -= npc.velocity * (1f - num26);
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (isFlowered)
                npc.lifeRegen -= 8;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo info)
        {
            if (canDoVoidDamage)
            {
                int damage = 1 + npc.damage / (strongVoidDamge ? 3 : 6);
                VoidPlayer.VoidDamage(Mod, target, damage);
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            NerfDendroChain(npc, ref modifiers);
            NerfBlazingCurse(npc, ref modifiers);
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            NerfDendroChain(npc, ref modifiers);
            NerfBlazingCurse(npc, ref modifiers);
        }

        private static void NerfBlazingCurse(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (!InfernalConfig.Instance.SOTSBalanceChanges) return;

            DebuffNPC debuffNPC = npc.GetGlobalNPC<DebuffNPC>();

            if (debuffNPC.BlazingCurse > 0)
            {
                float orig = 1f + 0.03f * debuffNPC.BlazingCurse + 0.005f * debuffNPC.BlazingCurse;
                float nerfed = 1f + 0.01f * debuffNPC.BlazingCurse + 0.005f * debuffNPC.BlazingCurse;

                modifiers.SourceDamage /= orig;
                modifiers.SourceDamage *= nerfed;
            }
        }

        private static void NerfDendroChain(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (!InfernalConfig.Instance.SOTSBalanceChanges || npc.immortal) return;

            if (!npc.HasBuff(ModContent.BuffType<Shattered>()))
            {
                if (npc.HasBuff<DendroChain>()) 
                {
                    modifiers.Defense.Flat += 15;
                }
            }
        }

        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            if (npc.type == ModContent.NPCType<Archaeologist>() && !NPC.downedDeerclops)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null && !items[i].IsAir &&
                        items[i].type == ModContent.ItemType<GlazeBow>())
                    {
                        items[i].TurnToAir();
                    }
                }
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

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (!projectile.active || projectile.timeLeft >= 8998)
                    continue;

                if (projectile.type != ModContent.ProjectileType<EvilGrowth>())
                    continue;

                if (projectile.ModProjectile is not EvilGrowth growth)
                    continue;

                bool flag = growth.effected[npc.whoAmI];
                if (!flag || npc.realLife != -1)
                    continue;

                Texture2D armTexture = ModContent.Request<Texture2D>("SOTS/Projectiles/Evil/EvilArm", AssetRequestMode.ImmediateLoad).Value;
                Texture2D handTexture = ModContent.Request<Texture2D>("SOTS/Projectiles/Evil/EvilHand", AssetRequestMode.ImmediateLoad).Value;

                Color armColor = ColorHelper.EvilColor;
                float scale = projectile.scale;
                float drawScale = scale * ((float)projectile.timeLeft / 150f);

                Vector2 toNpc = npc.Center - projectile.Center;
                float segmentCount = toNpc.Length() / (armTexture.Width * drawScale);

                for (int j = 0; j < segmentCount; j++)
                {
                    Vector2 drawPos = npc.Center + (-toNpc * (j / segmentCount)) - screenPos;

                    if (j == 0)
                    {
                        Main.spriteBatch.Draw(
                            handTexture,
                            drawPos,
                            null,
                            armColor,
                            toNpc.ToRotation() + MathHelper.PiOver2,
                            new Vector2(handTexture.Width / 2f, handTexture.Height / 2f),
                            drawScale * 1.4f,
                            SpriteEffects.None,
                            0f
                        );
                    }
                    else
                    {
                        Main.spriteBatch.Draw(
                            armTexture,
                            drawPos,
                            null,
                            armColor,
                            toNpc.ToRotation(),
                            new Vector2(armTexture.Width / 2f, armTexture.Height / 2f),
                            drawScale,
                            SpriteEffects.None,
                            0f
                        );
                    }
                }
            }

            return true;
        }
    }

    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class VoidDamageProjectile : GlobalProjectile
    {
        public bool canDoVoidDamage = false;
        public bool strongVoidDamge = false;
        public bool strongerVoidDamage = false;

        public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile entity)
        {
            int[] dealsVoidDamge =
            {
                ModContent.ProjectileType<IchorShot>(),
                ModContent.ProjectileType<IchorBlob>(),
                ModContent.ProjectileType<IchorBlast>(),
                ModContent.ProjectileType<IchorBolt>(),
                ModContent.ProjectileType<IchorShower>(),
                ModContent.ProjectileType<IchorSpit>(),
                ModContent.ProjectileType<Crimera>(),
                ModContent.ProjectileType<ShaderainHostile>(),
                ModContent.ProjectileType<VileClot>(),
                ModContent.ProjectileType<EaterOfSouls>(),
                ModContent.ProjectileType<InfernumMode.Content.BehaviorOverrides.BossAIs.HiveMind.ShadeFire>(),
            };
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (projectile.type == ModContent.ProjectileType<SupremeCataclysmFist>() || projectile.type == ModContent.ProjectileType<SupremeCatastropheSlash>() || projectile.type == ModContent.ProjectileType<SupremeCataclysmFistOld>() || projectile.type == ModContent.ProjectileType<CatastropheSlash>()
                || canDoVoidDamage)
            {
                int damage = 1 + projectile.damage / (strongerVoidDamage ? 2 : strongVoidDamge ? 3 : 6);
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