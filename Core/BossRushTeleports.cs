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

        public static void PyramidTeleport()
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
                            player.Teleport(new Vector2((index1 + 1) * 16f, index2 * 16f), 0, 0);
                            return;
                        }
                    }
                }
            }
        }

        public static void HandleTeleports()
        {
            if (BossRushStage < 0 || BossRushStage > Bosses.Count - 1)
                return;

            foreach (var player in Main.ActivePlayers)
            {
                Vector2? teleportPosition = null;

                // Teleport the player to the garden for the guardians fight in boss rush.
                if (BossRushStage < Bosses.Count - 1 && !CalamityPlayer.areThereAnyDamnBosses)
                {
                    if (ModLoader.TryGetMod("SOTS", out Mod sots) && ModLoader.TryGetMod("RevengeancePlus", out Mod revenge))
                    {
                        int subspaceID = sots.Find<ModNPC>("SubspaceSerpentHead").Type;

                        if (CurrentlyFoughtBoss == subspaceID && !player.ZoneUnderworldHeight)
                            teleportPosition = CalamityPlayer.GetUnderworldPosition(player);
                    }

                    if (CurrentlyFoughtBoss == NPCID.WallofFlesh && !player.ZoneUnderworldHeight)
                        teleportPosition = CalamityPlayer.GetUnderworldPosition(player);
                    if (CurrentlyFoughtBoss == ModContent.NPCType<ProfanedGuardianCommander>() && !player.Infernum_Biome().ZoneProfaned)
                        teleportPosition = WorldSaveSystem.ProvidenceArena.TopLeft() * 16f + new Vector2(WorldSaveSystem.ProvidenceArena.Width * 3.2f - 16f, 800f);
                    if (CurrentlyFoughtBoss == ModContent.NPCType<CeaselessVoid>() && !player.ZoneDungeon)
                        teleportPosition = WorldSaveSystem.ForbiddenArchiveCenter.ToWorldCoordinates() + Vector2.UnitY * 1032f;
                    if (CurrentlyFoughtBoss == ModContent.NPCType<Providence>() && !player.Infernum_Biome().ZoneProfaned)
                        teleportPosition = WorldSaveSystem.ProvidenceArena.TopRight() * 16f + new Vector2(WorldSaveSystem.ProvidenceArena.Width * -3.2f - 16f, 800f);
                }

                if (BossRushStage < Bosses.Count && CurrentlyFoughtBoss == NPCID.SkeletronHead && player.ZoneUnderworldHeight)
                    player.Spawn(PlayerSpawnContext.RecallFromItem);

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

                    CalamityPlayer.ModTeleport(player, teleportPosition.Value, playSound: false, 7);
                    SoundEngine.PlaySound(TeleportSound with { Volume = 1.6f }, player.Center);
                }
            }
        }
    }
}
