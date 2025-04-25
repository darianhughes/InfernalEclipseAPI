using System;
using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.CalPlayer;
using CalamityMod.Enums;
using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.CeaselessVoid;
using CalamityMod.NPCs.Crabulon;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.PrimordialWyrm;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs.Signus;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.StormWeaver;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Systems;
using InfernumMode;
using InfernumMode.Content.BehaviorOverrides.BossAIs.AstrumDeus;
using InfernumMode.Content.BehaviorOverrides.BossAIs.BoC;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Deerclops;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.Ares;
using InfernumMode.Content.BehaviorOverrides.BossAIs.DukeFishron;
using InfernumMode.Content.BehaviorOverrides.BossAIs.EyeOfCthulhu;
using InfernumMode.Content.BehaviorOverrides.BossAIs.KingSlime;
using InfernumMode.Content.BehaviorOverrides.BossAIs.PlaguebringerGoliath;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Polterghast;
using InfernumMode.Content.BehaviorOverrides.BossAIs.ProfanedGuardians;
using InfernumMode.Content.BehaviorOverrides.BossAIs.SlimeGod;
using InfernumMode.Content.BehaviorOverrides.BossAIs.SupremeCalamitas;
using InfernumMode.Core.GlobalInstances.Players;
using InfernumMode.Core.GlobalInstances.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static CalamityMod.Events.BossRushEvent;
namespace InfernalEclipseAPI.Core
{
    public class BossRushTeleports : ModSystem
    {
        internal static void BringPlayersBackToSpawn()
        {
            // Post-Wall of Flesh teleport back to spawn.
            foreach (var player in Main.ActivePlayers) 
            { 
                    player.Spawn(PlayerSpawnContext.RecallFromItem);
                    SoundEngine.PlaySound(TeleportSound with { Volume = 1.6f }, player.Center);
            }
        }

        public static Vector2 PyramidTeleport()
        {
            foreach (var player in Main.ActivePlayers)
            {
                for (int index1 = 0; index1 < Main.maxTilesX; ++index1)
                {
                    for (int index2 = 0; index2 < Main.maxTilesY; ++index2)
                    {
                        Tile tile = ((Tilemap)Main.tile)[index1, index2];
                        if ((int)((Tile)tile).TileType == (int)((ModBlockType)Terraria.ModLoader.ModLoader.GetMod("SOTS").Find<ModTile>("SarcophagusTile")).Type)
                        {
                            return new Vector2((index1 + 1) * 16f, index2 * 16f);
                        }
                    }
                }
            }
            return new Vector2(0, 0);
        }

        public static void HandleTeleports()
        {
            if (BossRushStage < 0 || BossRushStage > Bosses.Count - 1)
                return;

            foreach (var player in Main.ActivePlayers)
            {
                Vector2? teleportPosition = null;

                if (BossRushStage < Bosses.Count - 1 && !CalamityPlayer.areThereAnyDamnBosses)
                {
                    if (ModLoader.TryGetMod("SOTS", out Mod sots) && ModLoader.TryGetMod("RevengeancePlus", out Mod revenge))
                    {
                        int subspaceID = sots.Find<ModNPC>("SubspaceSerpentHead").Type;
                        int pharohID = sots.Find<ModNPC>("PharaohsCurse").Type;

                        if (CurrentlyFoughtBoss == subspaceID && !player.ZoneUnderworldHeight)
                            player.DemonConch();
                        if (CurrentlyFoughtBoss == pharohID)
                            teleportPosition = PyramidTeleport();
                    }

                    if (CurrentlyFoughtBoss == NPCID.WallofFlesh && !player.ZoneUnderworldHeight)
                        player.DemonConch();
                    if (CurrentlyFoughtBoss == ModContent.NPCType<ProfanedGuardianCommander>() && !player.Infernum_Biome().ZoneProfaned)
                        teleportPosition = WorldSaveSystem.ProvidenceArena.TopLeft() * 16f + new Vector2(WorldSaveSystem.ProvidenceArena.Width * 3.2f - 16f, 800f);
                    if (CurrentlyFoughtBoss == ModContent.NPCType<CeaselessVoid>() && !player.ZoneDungeon)
                        teleportPosition = WorldSaveSystem.ForbiddenArchiveCenter.ToWorldCoordinates() + Vector2.UnitY * 1032f;
                    if (CurrentlyFoughtBoss == ModContent.NPCType<Providence>() && !player.Infernum_Biome().ZoneProfaned)
                        teleportPosition = WorldSaveSystem.ProvidenceArena.TopRight() * 16f + new Vector2(WorldSaveSystem.ProvidenceArena.Width * -3.2f - 16f, 800f);
                }


                if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok) || ModLoader.TryGetMod("ThoriumRework", out Mod rework)))
                {
                    if (BossRushStage < Bosses.Count && CurrentlyFoughtBoss == thorium.Find<ModNPC>("BoreanStrider").Type) 
                    {
                        player.Spawn(PlayerSpawnContext.RecallFromItem);
                    }
                }
                else
                {
                    if (BossRushStage < Bosses.Count && CurrentlyFoughtBoss == ModContent.NPCType<PerforatorHive>())
                    {
                        player.Spawn(PlayerSpawnContext.RecallFromItem);
                    }
                }

                if (ModLoader.TryGetMod("SOTS", out Mod sots2) && ModLoader.TryGetMod("RevengeancePlus", out Mod revenge2)) {
                    if (BossRushStage < Bosses.Count && CurrentlyFoughtBoss == ModContent.NPCType<AquaticScourgeHead>())
                        player.Spawn(PlayerSpawnContext.RecallFromItem);
                }

                if (BossRushStage < Bosses.Count && CurrentlyFoughtBoss == ModContent.NPCType<StormWeaverHead>())
                    player.Spawn(PlayerSpawnContext.RecallFromItem);

                if (ModLoader.TryGetMod("Clamity", out Mod clam))
                {
                    if (BossRushStage < Bosses.Count && CurrentlyFoughtBoss == ModContent.NPCType<PrimordialWyrmHead>())
                        player.Spawn(PlayerSpawnContext.RecallFromItem);
                }

                // Check to make sure the teleport position is valid.
                bool fightingProfanedBoss = CurrentlyFoughtBoss == ModContent.NPCType<ProfanedGuardianCommander>() || CurrentlyFoughtBoss == ModContent.NPCType<Providence>();
                if (fightingProfanedBoss && WorldSaveSystem.ProvidenceArena.TopLeft() == Vector2.Zero)
                {
                    BossRushStage++;
                    return;
                }
                if (BossRushStage < Bosses.Count && CurrentlyFoughtBoss == ModContent.NPCType<CeaselessVoid>() && WorldSaveSystem.ForbiddenArchiveCenter == Point.Zero)
                {
                    BossRushStage++;
                    return;
                }

                // Teleport the player.
                if (teleportPosition.HasValue)
                {
                    if (BossRushStage < Bosses.Count && CurrentlyFoughtBoss != NPCID.SkeletronHead && WorldUtils.Find(teleportPosition.Value.ToTileCoordinates(), Searches.Chain(new Searches.Down(100), new Conditions.IsSolid()), out Point p))
                        teleportPosition = p.ToWorldCoordinates(8f, -32f);

                    player.Teleport(teleportPosition.Value, 0, 0);
                    SoundEngine.PlaySound(TeleportSound with { Volume = 1.6f }, player.Center);
                }
            }
        }
    }
}
