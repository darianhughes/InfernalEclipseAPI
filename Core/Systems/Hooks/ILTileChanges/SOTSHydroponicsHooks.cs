using SOTS;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILTileChanges
{
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    public class SOTSHydroponicsHooks : ModSystem
    {
        public override void PostSetupContent()
        {
            //Example registration: InfernalCrossmod.SOTS.Mod?.Call("AddHydroponicsHerb", ModContent.ItemType<ThoriumMod.Items.Depths.MarineKelp>(), ModContent.TileType<MarineKelp>(), 0, DustID.GrassBlades, 0f, 0f, 0f);
        }
    }
}