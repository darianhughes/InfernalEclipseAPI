using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems.AdvisorSpawnChanges
{
    public class AdvisorSpawnBlocker : ModSystem
    {
        private Hook randomUpdateHook;

        public override void Load()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) // Only hook on server/singleplayer
                return;

            if (!ModLoader.TryGetMod("SOTS", out Mod sots))
                return;

            var tileType = sots.Code.GetType("SOTS.Items.Planetarium.AvaritianGatewayTile");
            if (tileType == null) return;

            var randomUpdate = tileType.GetMethod("RandomUpdate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (randomUpdate == null) return;

            randomUpdateHook = new Hook(randomUpdate, (Action<object, int, int>)((self, i, j) => {
                // Only run base RandomUpdate (which just calls base class, does not spawn Advisor)
                // Do nothing to block SOTS spawn logic
            }));
        }

        public override void Unload()
        {
            randomUpdateHook?.Dispose();
        }
    }
}
