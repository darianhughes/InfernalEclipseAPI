using CalamityMod.Events;
using CalamityMod.Systems.Mechanic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ObjectData;
using ThoriumMod.NPCs.BossViscount;
using ThoriumMod.Tiles;

namespace InfernalEclipseAPI.Content.DifficultyOverrides.Thorium.ViscountOverrides
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public sealed class ViscountArenaGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private ArenaWallSystem.Box arenaBox;
        private bool arenaCreated;

        private const int ArenaWidthTiles = 25;
        private const int ArenaHeightTiles = 77;
        private float ArenaVerticalOffset = -170f;

        private const float ArenaWidth = ArenaWidthTiles * 16f;
        private const float ArenaHeight = ArenaHeightTiles * 16f;

        private static readonly Vector4 ArenaDimensions = new(
            ArenaWidth * 0.5f,
            ArenaHeight * 0.5f,
            ArenaWidth * 0.5f,
            ArenaHeight * 0.5f
        );

        private const float BossRangeToEnforceArena = 2200f;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            bool applies = entity.type == ModContent.NPCType<Viscount>();

            if (applies)
                Main.NewText($"ViscountArenaGlobalNPC attached to {entity.FullName}");

            return applies;
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            //Main.NewText("ViscountArenaGlobalNPC OnSpawn fired");

            if (arenaCreated)
                return;

            CreateArena(npc);
            arenaCreated = true;
        }

        public override void AI(NPC npc)
        {
            if (!npc.active)
            {
                arenaCreated = false;
                arenaBox = null;
                return;
            }

            if (arenaBox is not null)
                PushPlayersIntoArena(npc, arenaBox);
        }

        private void CreateArena(NPC npc)
        {
            //skip arena in boss rush
            if (BossRushEvent.BossRushActive)
                return;

            Vector2 anchor = npc.Center;

            if (TryFindNearestBloodAltarAnchor(npc.Center, out Vector2 altarAnchor))
                anchor = altarAnchor;

            // Move arena up.
            anchor.Y += ArenaVerticalOffset;

            arenaBox = new ArenaWallSystem.Box
            {
                position = anchor,
                boxDimensions = ArenaDimensions,
                NewDimensions = ArenaDimensions,
                borderThickness = 2000f,
                RemovalCondition = () => !npc.active || Main.npc[npc.whoAmI].type != npc.type,
                UpdateBox = UpdateArena,
                DrawBox = DrawArena,
                DespawnAction = (box) => true
            };

            arenaBox.borderColor = Color.DarkRed;
            ArenaWallSystem.ActiveBoxes.Add(arenaBox);
        }

        private static void UpdateArena(ArenaWallSystem.Box box)
        {
            box.NewDimensions = ArenaDimensions;

            if (box.oldData is not null)
                box.oldData.borderColor = box.borderColor;
        }

        private static void DrawArena(ArenaWallSystem.Box box)
        {
            box.DrawBoxWithOffset(box.borderThickness * 0.5f, box.borderThickness, Color.Black * 0.72f);
            box.DrawBoxWithOffset(4f, 8f, box.borderColor);

            float amount = 4f;
            float totalDistance = 64f;
            for (float i = Main.GlobalTimeWrappedHourly % 1f; i < amount; i++)
                box.DrawBoxWithOffset(totalDistance * (i / amount) + 4f, 4f, box.borderColor * (1f - i / amount));

            box.DrawBoxWithOffset(box.borderThickness - 4f, 4f, box.borderColor);
        }

        private static void PushPlayersIntoArena(NPC boss, ArenaWallSystem.Box box)
        {
            Rectangle arenaRect = new(
                (int)box.TopLeft.X,
                (int)box.TopLeft.Y,
                (int)box.Size.X,
                (int)box.Size.Y
            );

            const float pushStrength = 1.15f;
            const float maxPushSpeed = 16f;
            const float snapMargin = 12f;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (!player.active || player.dead)
                    continue;

                if (Vector2.Distance(player.Center, boss.Center) > BossRangeToEnforceArena)
                    continue;

                if (arenaRect.Intersects(player.Hitbox))
                    continue;

                float clampedX = MathHelper.Clamp(player.Center.X, arenaRect.Left + 16f, arenaRect.Right - 16f);
                float clampedY = MathHelper.Clamp(player.Center.Y, arenaRect.Top + 16f, arenaRect.Bottom - 16f);

                Vector2 targetPoint = new(clampedX, clampedY);
                Vector2 pushDirection = targetPoint - player.Center;

                if (pushDirection.LengthSquared() <= 0.001f)
                    continue;

                pushDirection.Normalize();
                player.velocity += pushDirection * pushStrength;

                if (player.velocity.Length() > maxPushSpeed)
                    player.velocity = Vector2.Normalize(player.velocity) * maxPushSpeed;

                if (Vector2.Distance(player.Center, targetPoint) <= snapMargin)
                    player.position = targetPoint - player.Size * 0.5f;

                if (player.Center.X < arenaRect.Left)
                    player.velocity.X = Math.Max(player.velocity.X, 0f);
                else if (player.Center.X > arenaRect.Right)
                    player.velocity.X = Math.Min(player.velocity.X, 0f);

                if (player.Center.Y < arenaRect.Top)
                    player.velocity.Y = Math.Max(player.velocity.Y, 0f);
                else if (player.Center.Y > arenaRect.Bottom)
                    player.velocity.Y = Math.Min(player.velocity.Y, 0f);
            }
        }

        private static bool TryFindNearestBloodAltarAnchor(Vector2 searchFromWorld, out Vector2 altarAnchor)
        {
            altarAnchor = Vector2.Zero;

            int altarTileType = ModContent.TileType<BloodAltar>();
            Point searchTile = searchFromWorld.ToTileCoordinates();
            const int searchRadius = 140;

            float closestDistSq = float.MaxValue;
            bool found = false;

            for (int x = Math.Max(10, searchTile.X - searchRadius); x < Math.Min(Main.maxTilesX - 10, searchTile.X + searchRadius); x++)
            {
                for (int y = Math.Max(10, searchTile.Y - searchRadius); y < Math.Min(Main.maxTilesY - 10, searchTile.Y + searchRadius); y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (!tile.HasTile || tile.TileType != altarTileType)
                        continue;

                    Vector2 anchor = GetMultiTileCenterOneTileBelow(x, y, altarTileType);
                    float distSq = Vector2.DistanceSquared(searchFromWorld, anchor);

                    if (distSq < closestDistSq)
                    {
                        closestDistSq = distSq;
                        altarAnchor = anchor;
                        found = true;
                    }
                }
            }

            return found;
        }

        private static Vector2 GetMultiTileCenterOneTileBelow(int x, int y, int tileType)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            TileObjectData data = TileObjectData.GetTileData(tileType, 0);

            if (data is null)
                return new Vector2((x + 0.5f) * 16f, (y + 1.5f) * 16f);

            int frameX = tile.TileFrameX / 18;
            int frameY = tile.TileFrameY / 18;

            int left = x - (frameX % data.Width);
            int top = y - (frameY % data.Height);

            float centerX = (left + data.Width * 0.5f) * 16f;
            float centerY = (top + data.Height * 0.5f) * 16f;

            // One tile below the altar's actual center.
            centerY += 16f;

            return new Vector2(centerX, centerY);
        }
    }
}