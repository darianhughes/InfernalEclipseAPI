namespace InfernalEclipseAPI.Core.Systems.Hooks.ILTileChanges
{
    [ExtendsFromMod("SOTS")]
    [JITWhenModsEnabled("SOTS")]
    public class SOTSChestGateSystem : ModSystem
    {
        private static int PermafrostPlatingCapsuleTileType = -1;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("SOTS", out Mod sots))
                return;

            PermafrostPlatingCapsuleTileType = sots.Find<ModTile>("PermafrostPlatingCapsuleTile").Type;
            On_Chest.Unlock += BlockPermafrostCapsuleUnlock;
        }

        public override void Unload()
        {
            On_Chest.Unlock -= BlockPermafrostCapsuleUnlock;
            PermafrostPlatingCapsuleTileType = -1;
        }

        private static bool BlockPermafrostCapsuleUnlock(On_Chest.orig_Unlock orig, int x, int y)
        {
            if (PermafrostPlatingCapsuleTileType <= 0)
                return orig(x, y);

            Tile tile = Framing.GetTileSafely(x, y);
            if (!tile.HasTile)
                return orig(x, y);

            // Normalize to chest top-left just in case.
            int left = x;
            int top = y;

            if (tile.TileFrameX % 36 != 0)
                left--;

            if (tile.TileFrameY != 0)
                top--;

            Tile topLeft = Framing.GetTileSafely(left, top);
            if (!topLeft.HasTile)
                return orig(x, y);

            if (topLeft.TileType == PermafrostPlatingCapsuleTileType && !NPC.downedDeerclops)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Main.NewText("This capsule is frozen shut by a winter beast from another world.", Microsoft.Xna.Framework.Color.LightBlue);
                }

                return false;
            }

            return orig(x, y);
        }
    }
}
