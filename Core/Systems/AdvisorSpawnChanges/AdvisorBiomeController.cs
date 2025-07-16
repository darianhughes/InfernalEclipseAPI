using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace InfernalEclipseAPI.Core.Systems.AdvisorSpawnChanges
{
    [ExtendsFromMod("SOTS")]
    public class AdvisorBiomeController : ModPlayer
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return InfernalConfig.Instance.AdjustAdvisorSpawnConditions;
        }

        private bool wasInPlanetarium = false;
        private MethodInfo spawnAdvisorMethod;
        private int gatewayTileType;

        public override void OnEnterWorld()
        {
            // Cache method and tile type for reflection
            if (spawnAdvisorMethod == null && ModLoader.TryGetMod("SOTS", out Mod sots))
            {
                var tileType = sots.Code.GetType("SOTS.Items.Planetarium.AvaritianGatewayTile");
                spawnAdvisorMethod = tileType?.GetMethod("SpawnAdvisor", BindingFlags.Static | BindingFlags.Public);
                gatewayTileType = ModContent.TileType<SOTS.Items.Planetarium.AvaritianGatewayTile>();
            }
        }

        public override void PreUpdate()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) // Only run on server
                return;

            bool isInPlanetarium = Player.InModBiome(ModContent.GetInstance<SOTS.Biomes.PlanetariumBiome>());

            if (isInPlanetarium && !wasInPlanetarium && spawnAdvisorMethod != null)
            {
                // Find all gateway tiles in world (could be optimized, but for simplicity scan)
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for (int j = 0; j < Main.maxTilesY; j++)
                    {
                        if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType == gatewayTileType)
                        {
                            spawnAdvisorMethod.Invoke(null, new object[] { i, j });
                        }
                    }
                }
            }
            wasInPlanetarium = isInPlanetarium;
        }
    }
}
