using SOTS;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILTileChanges
{
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    public class SOTSHydroponicsHooks : ModSystem
    {
        public override void PostSetupContent()
        {
            // Example registrations:
            /* 
            RegisterHydroponicsHerb(
                ModContent.ItemType<YourModdedHerbItem>(), 
                ModContent.TileType<YourModdedHerbTile>(), 
                tileOffset (should be 0),
                DustID.GrassBlades,
                red light,
                green light,
                blue light
            );
            */
            RegisterHydroponicsHerb(
                ModContent.ItemType<ThoriumMod.Items.Depths.MarineKelp>(), 
                ModContent.TileType<ThoriumMod.Tiles.MarineKelp>(), 
                0, 
                DustID.GrassBlades,
                0f, 
                0f, 
                0f    
            );
        }

        private static void RegisterHydroponicsHerb(int itemType, int tileType, int tileOffset = 0, int harvestDust = 0, float r = 0f, float g = 0f, float b = 0f)
        {
            InfernalCrossmod.SOTS.Mod?.Call(
                "AddHydroponicsHerb",
                itemType,
                tileType,
                tileOffset,
                harvestDust,
                r,
                g,
                b
            );
        }
    }
}