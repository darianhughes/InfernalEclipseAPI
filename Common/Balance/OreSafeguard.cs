using System.Security.Policy;
using CalamityMod.Tiles.Ores;
using InfernalEclipseAPI.Core.Systems;
using Terraria;
using Terraria.ObjectData;

namespace InfernalEclipseAPI.Common.Balance
{
    public class OreSafeguard : GlobalTile
    {
        public override void SetStaticDefaults()
        {
            if (InfernalConfig.Instance.BossKillCheckOnOres)
            {
                TileID.Sets.CanBeClearedDuringOreRunner[TileID.Meteorite] = true;
                TileID.Sets.CanBeClearedDuringOreRunner[ModContent.TileType<ExodiumOre>()] = true;
            }
        }

        public override bool CanKillTile(int i, int j, int tile, ref bool blockDamaged)
        {
            if (InfernalConfig.Instance.BossKillCheckOnOres)
            {
                if (IsBelowSpecialTable(i, j) && !NPC.downedBoss3)
                {
                    blockDamaged = false;
                    return false;
                }

                if (IsBelowStrangeKeystone(i, j) && !NPC.downedBoss2)
                {
                    blockDamaged = false;
                    return false;
                }

                if (InfernalCrossmod.SOTS.Loaded)
                {
                    if (tile == InfernalCrossmod.SOTS.Mod.Find<ModTile>("StrangeKeystoneTile").Type)
                    {
                        if (!MeetsPickRequirement(Main.LocalPlayer, 66))
                        {
                            blockDamaged = false;
                            return false;
                        }
                    }
                }

                switch (tile)
                {
                    case 37:
                        return NPC.downedBoss2;
                    case 58:
                        return NPC.downedBoss2;
                    case 408:
                        return NPC.downedMoonlord;
                    case 659:
                        return NPC.downedBoss2;
                    case TileID.AlchemyTable:
                    case TileID.BewitchingTable:
                        return NPC.downedBoss3;
                    default:
                        if (tile == ModContent.TileType<ExodiumOre>()) { return NPC.downedMoonlord; }
                        return base.CanKillTile(i, j, tile, ref blockDamaged);
                }
            }
            return base.CanKillTile(i, j, tile, ref blockDamaged);
        }

        public override bool CanExplode(int i, int j, int type)
        {
            if (InfernalConfig.Instance.BossKillCheckOnOres)
            {
                if (IsBelowSpecialTable(i, j))
                    return NPC.downedBoss3;

                if (IsBelowStrangeKeystone(i, j))
                {
                    return NPC.downedBoss2;
                }

                if (InfernalCrossmod.SOTS.Loaded)
                {
                    if (type == InfernalCrossmod.SOTS.Mod.Find<ModTile>("StrangeKeystoneTile").Type)
                    {
                        return NPC.downedBoss2;
                    }
                }

                switch (type)
                {
                    case 37:
                        return NPC.downedBoss2;
                    case 58:
                        return NPC.downedBoss2;
                    case 408:
                        return NPC.downedMoonlord;
                    case 659:
                        return NPC.downedBoss2;
                    case TileID.AlchemyTable:
                    case TileID.BewitchingTable:
                        return NPC.downedBoss3;
                    default:
                        if (type == ModContent.TileType<ExodiumOre>()) { return NPC.downedMoonlord; }
                        return base.CanExplode(i, j, type);
                }
            }
            return base.CanExplode(i, j, type);
        }

        private static bool IsBelowSpecialTable(int x, int y)
        {
            int aboveY = y - 1;
            if (aboveY < 0 || x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
                return false;

            Tile above = Framing.GetTileSafely(x, aboveY);
            if (!above.HasTile)
                return false;

            int aboveType = above.TileType;
            if (aboveType != TileID.BewitchingTable && aboveType != TileID.AlchemyTable)
                return false;

            TileObjectData data = TileObjectData.GetTileData(aboveType, 0);
            if (data == null)
            {
                int frameYMod = above.TileFrameY % 54;
                return frameYMod >= 36;
            }

            int row = (above.TileFrameY / 18) % data.Height;
            bool isBottomRow = row == data.Height - 1;

            return isBottomRow;
        }

        private static bool IsBelowStrangeKeystone(int x, int y)
        {
            if (!InfernalCrossmod.SOTS.Loaded)
                return false;

            int aboveY = y - 1;
            if (aboveY < 0 || x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
                return false;

            Tile above = Framing.GetTileSafely(x, aboveY);
            if (!above.HasTile)
                return false;

            int aboveType = above.TileType;
            if (aboveType != InfernalCrossmod.SOTS.Mod.Find<ModTile>("StrangeKeystoneTile").Type)
                return false;

            TileObjectData data = TileObjectData.GetTileData(aboveType, 0);
            if (data != null)
            {
                int row = (above.TileFrameY / 18) % data.Height;
                return row == data.Height - 1;
            }

            const int tilePixel = 18;
            const int heightTiles = 4;
            int styleBlockHeightPx = heightTiles * tilePixel;

            int frameYMod = above.TileFrameY % styleBlockHeightPx;
            return frameYMod >= (styleBlockHeightPx - tilePixel);
        }

        private static bool MeetsPickRequirement(Player player, int requiredPickPower)
        {
            return player.HeldItem.pick >= requiredPickPower;
        }
    }
}
