using CalamityMod.Events;
using CalamityMod.Systems.Mechanic;
using Microsoft.Xna.Framework;
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

        public ArenaWallSystem.Box? ArenaBox = null;
        private bool arenaCreated = false;

        private const int ArenaWidthTiles = 25;
        private const int ArenaHeightTiles = 77;
        private const float ArenaVerticalOffset = -170f;

        private const float ArenaWidth = ArenaWidthTiles * 16f;
        private const float ArenaHeight = ArenaHeightTiles * 16f;

        private static readonly Vector4 ArenaDimensions = new
        (
            ArenaWidth * 0.5f,
            ArenaHeight * 0.5f,
            ArenaWidth * 0.5f,
            ArenaHeight * 0.5f
        );

        void UpdateArena(ArenaWallSystem.Box box)
        {
            if (box.borderColor == Color.Gray || box.oldData.borderColor == Color.Gray)
                return;
            for (var i2 = 0; i2 < box.Size.Y / 400f; i2++)
            {
                var p = Vector2.Lerp(box.BottomRight, box.TopRight, Main.rand.NextFloat());
                Dust.NewDustPerfect(p, DustID.Clentaminator_Red, p.DirectionFrom(box.Center) * Main.rand.NextFloat(0, 5), Scale: Main.rand.NextFloat(0.1f, 1f), newColor: box.borderColor);

                p = Vector2.Lerp(box.TopLeft, box.BottomLeft, Main.rand.NextFloat());
                Dust.NewDustPerfect(p, DustID.Clentaminator_Red, p.DirectionFrom(box.Center) * Main.rand.NextFloat(0, 5), Scale: Main.rand.NextFloat(0.1f, 1f), newColor: box.borderColor);

            }
            for (var i2 = 0; i2 < box.Size.X / 400f; i2++)
            {
                var p = Vector2.Lerp(box.TopLeft, box.TopRight, Main.rand.NextFloat());
                Dust.NewDustPerfect(p, DustID.Clentaminator_Red, p.DirectionFrom(box.Center) * Main.rand.NextFloat(0, 5), Scale: Main.rand.NextFloat(0.1f, 1f), newColor: box.borderColor);
                p = Vector2.Lerp(box.BottomRight, box.BottomLeft, Main.rand.NextFloat());
                Dust.NewDustPerfect(p, DustID.Clentaminator_Red, p.DirectionFrom(box.Center) * Main.rand.NextFloat(0, 5), Scale: Main.rand.NextFloat(0.1f, 1f), newColor: box.borderColor);
            }
        }

        void DrawArena(ArenaWallSystem.Box box)
        {
            var color = Color.Black * 0.75f;
            //Inside Fill
            box.DrawBoxWithOffset(box.borderThickness * 0.5f, box.borderThickness, Color.Black * 0.75f);
            //Inner Border
            box.DrawBoxWithOffset(4, 8, box.borderColor);
            //Inner Border Clones
            float amount = 4;
            float totalDistance = 64f;
            for (var i = Main.GlobalTimeWrappedHourly % 1; i < amount; i++)
            {
                box.DrawBoxWithOffset(totalDistance * (i / amount) + 4, 4, box.borderColor * (1 - i / amount));
            }
            //Outer Border
            box.DrawBoxWithOffset(box.borderThickness - 4, 4, box.borderColor);
        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            bool applies = entity.type == ModContent.NPCType<Viscount>();

            if (applies)
                Main.NewText($"ViscountArenaGlobalNPC attached to {entity.FullName}");

            return applies;
        }

        public override bool PreAI(NPC npc)
        {
            #region Arena Box
            if (!arenaCreated)
            {
                if (ArenaBox is not null)
                    ArenaBox = null;

                arenaCreated = true;
            }

            if (ArenaBox is null)
            {
                int npcIndex = npc.whoAmI;
                int npcType = npc.type;

                Vector2 anchor = npc.Center;

                if (TryFindNearestBloodAltarAnchor(npc.Center, out Vector2 altarAnchor))
                {
                    // Move arena up.
                    altarAnchor.Y += ArenaVerticalOffset;
                    anchor = altarAnchor;
                }

                ArenaBox = new ArenaWallSystem.Box
                {
                    position = anchor,
                    boxDimensions = ArenaDimensions * 2f,
                    borderThickness = 2000f,
                    borderColor = Color.DarkRed,
                    RemovalCondition = () => !Main.npc.IndexInRange(npcIndex) || !Main.npc[npcIndex].active || Main.npc[npcIndex].type != npcType,
                    UpdateBox = UpdateArena,
                    DrawBox = DrawArena,
                    DespawnAction = (box) =>
                    {
                        box.boxDimensions += new Vector4(64);
                        if (box.Size.X > 5000)
                            return true;
                        return false;
                    }
                };
                ArenaWallSystem.ActiveBoxes.Add(ArenaBox);
            }

            ArenaBox.NewDimensions = Vector4.Lerp(ArenaBox.boxDimensions, ArenaDimensions, 0.1f);
            #endregion

            return base.PreAI(npc);
        }

        public override void AI(NPC npc)
        {
            if (!npc.active)
            {
                arenaCreated = false;
                ArenaBox = null;
                return;
            }
        }

        private static bool TryFindNearestBloodAltarAnchor(Vector2 searchFromWorld, out Vector2 altarAnchor)
        {
            if (BossRushEvent.BossRushActive)
            {
                altarAnchor = searchFromWorld;
                return false;
            }

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