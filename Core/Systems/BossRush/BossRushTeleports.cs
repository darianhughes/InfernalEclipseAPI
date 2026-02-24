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
using static ThoriumMod.Tiles.Statues;
namespace InfernalEclipseAPI.Core.Systems.BossRush
{
    public class BossRushTeleports : ModSystem
    {
        public static void BringPlayersBackToSpawn()
        {
            // Post-Wall of Flesh teleport back to spawn.
            foreach (Player player in Main.ActivePlayers)
            {
                if (player.active && !player.dead)
                {
                    player.Spawn(PlayerSpawnContext.RecallFromItem);
                    SoundEngine.PlaySound(TeleportSound with { Volume = 1.6f }, player.Center);
                }
            }
        }

        public static void PyramidTeleportAll()
        {
            Vector2? target = null;

            // Find the sarcophagus tile once
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    if (tile.HasTile && tile.TileType == ModLoader.GetMod("SOTS").Find<ModTile>("SarcophagusTile").Type)
                    {
                        target = new Vector2((x + 3f) * 16f, (y + 1.5f) * 16f);
                        break;
                    }
                }
                if (target.HasValue)
                    break;
            }

            if (!target.HasValue)
                return;

            // Teleport every active player
            foreach (Player player in Main.ActivePlayers)
            {
                if (player.active && !player.dead)
                {
                    player.Teleport(target.Value, 1);
                    NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, target.Value.X, target.Value.Y, 1);
                    SoundEngine.PlaySound(TeleportSound with { Volume = 1.6f }, player.Center);
                }
            }
        }

        public static void OceanTeleportAll()
        {
            foreach (Player player in Main.ActivePlayers)
            {
                if (player.active && !player.dead)
                {
                    player.MagicConch();
                    SoundEngine.PlaySound(TeleportSound with { Volume = 1.6f }, player.Center);
                }
            }
        }

        private static uint _lastSearchFrame = UInt32.MaxValue;
        private static Vector2? _cachedHellSpotServer;

        public static void HellTeleportAll()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (_lastSearchFrame != Main.GameUpdateCount)
                {
                    _cachedHellSpotServer = GetHellCenterLandingFast();
                    _lastSearchFrame = Main.GameUpdateCount;
                }

                if (!_cachedHellSpotServer.HasValue)
                    return;

                Vector2 worldPos = _cachedHellSpotServer.Value;

                foreach (var player in Main.ActivePlayers)
                {
                    if (!player.active || player.dead)
                        continue;

                    player.Teleport(worldPos, TeleportationStyleID.RodOfDiscord);
                    SoundEngine.PlaySound(SoundID.Item6, worldPos);

                    NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, worldPos.X, worldPos.Y, TeleportationStyleID.RodOfDiscord);
                }
            }
        }

        private static Vector2? GetHellCenterLandingFast()
        {
            int centerX = Main.maxTilesX / 2;
            int startY = Main.UnderworldLayer + 8;
            const int maxSearchDown = 400;
            const int maxRadius = 500;
            const int bodyW = 2;
            const int bodyH = 4;

            for (int r = 0; r <= maxRadius; r += 2)
            {
                for (int side = 0; side < 2; side++)
                {
                    int x = side == 0 ? centerX + r : centerX - r;
                    if (x < 10 || x > Main.maxTilesX - 10)
                        continue;

                    if (WorldUtils.Find(new Point(x, startY),
                        Searches.Chain(new Searches.Down(maxSearchDown), new Conditions.IsSolid()),
                        out Point ground))
                    {
                        int standX = ground.X;
                        int standY = ground.Y - 1;

                        if (!IsSafeFloor(ground.X, ground.Y))
                            continue;

                        int tlX = standX;
                        int tlY = standY - (bodyH - 1);

                        if (!WorldGen.InWorld(tlX, tlY, 10) || !WorldGen.InWorld(tlX + bodyW - 1, tlY + bodyH - 1, 10))
                            continue;

                        // Fast empty-body check
                        bool blocked = Collision.SolidTiles(tlX, tlX + bodyW - 1, tlY, tlY + bodyH - 1);
                        if (blocked)
                            continue;

                        // Quick liquid probe (middle of the body)
                        var mid = Main.tile[tlX, tlY + (bodyH / 2)];
                        if (mid != null && mid.LiquidAmount > 128)
                            continue;

                        return new Point(standX, standY).ToWorldCoordinates(8f, 0f);
                    }
                }
            }
            return null;
        }

        private static bool HasClearance(int topLeftX, int topLeftY, int w, int h)
        {
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    int tx = topLeftX + i;
                    int ty = topLeftY + j;
                    if (!WorldGen.InWorld(tx, ty, 10))
                        return false;

                    Tile t = Main.tile[tx, ty];
                    if (t == null)
                        return false;

                    // No solids or half-bricks in the body space
                    if (t.HasTile && Main.tileSolid[t.TileType] && !Main.tileSolidTop[t.TileType])
                        return false;

                    // Avoid deep liquid in the body space
                    if (t.LiquidAmount > 128) // ~50% liquid
                        return false;
                }
            }
            return true;
        }

        // Require the floor tile to be solid (or a platform) and not fully submerged in lava.
        private static bool IsSafeFloor(int x, int y)
        {
            if (!WorldGen.InWorld(x, y, 10))
                return false;

            Tile floor = Main.tile[x, y];
            if (floor == null)
                return false;

            bool solid = floor.HasTile && (Main.tileSolid[floor.TileType] || Main.tileSolidTop[floor.TileType]);
            if (!solid)
                return false;

            // Avoid standing on tiles covered by lava
            if (floor.LiquidType == LiquidID.Lava && floor.LiquidAmount > 0)
                return false;

            return true;
        }

        public static void GaurdiansTeleportAll()
        {
            Vector2 teleportPosition =
            WorldSaveSystem.ProvidenceArena.TopLeft() * 16f +
            new Vector2(WorldSaveSystem.ProvidenceArena.Width * 3.2f - 16f, 800f);

            foreach (Player player in Main.ActivePlayers)
            {
                if (player.active && !player.dead)
                {
                    player.Teleport(teleportPosition, 1);
                    NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, teleportPosition.X, teleportPosition.Y, 1);
                    SoundEngine.PlaySound(TeleportSound with { Volume = 1.6f }, player.Center);
                }
            }
        }

        public static void TeleportToForbiddenArchive()
        {
            // Compute teleport position
            Vector2 teleportPosition =
                WorldSaveSystem.ForbiddenArchiveCenter.ToWorldCoordinates() +
                Vector2.UnitY * 1032f;

            // Teleport all active, living players
            foreach (Player player in Main.ActivePlayers)
            {
                if (player.active && !player.dead)
                {
                    player.Teleport(teleportPosition, 1);
                    NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, teleportPosition.X, teleportPosition.Y, 1);
                    SoundEngine.PlaySound(TeleportSound with { Volume = 1.6f }, player.Center);
                }
            }
        }

        public static void TeleportToProvidence()
        {
            // Compute teleport position
            Vector2 teleportPosition =
                WorldSaveSystem.ProvidenceArena.TopRight() * 16f +
                new Vector2(WorldSaveSystem.ProvidenceArena.Width * -3.2f - 16f, 800f);

            // Teleport all active, living players
            foreach (Player player in Main.ActivePlayers)
            {
                if (player.active && !player.dead)
                {
                    player.Teleport(teleportPosition, 1);
                    NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, teleportPosition.X, teleportPosition.Y, 1);
                    SoundEngine.PlaySound(TeleportSound with { Volume = 1.6f }, player.Center);
                }
            }
        }

        /*
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
                        //if (CurrentlyFoughtBoss == pharohID)
                        //    teleportPosition = PyramidTeleport();
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
                    if (BossRushStage < Bosses.Count && CurrentlyFoughtBoss == thorium.Find<ModNPC>("BoreanStriderPopped").Type) 
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
        */
    }
}
