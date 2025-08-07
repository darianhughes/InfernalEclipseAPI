using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Tiles;

namespace InfernalEclipseAPI.Common.Balance.Thorium
{
    [ExtendsFromMod("ThoriumMod")]
    public class ThoriumTileSafeguard : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int tile, ref bool blockDamaged)
        {
            if (tile == ModContent.TileType<BloodAltar>()) return false;

            if (InfernalConfig.Instance.BossKillCheckOnOres)
            {
                if (tile == ModContent.TileType<LeakyMarineBlock>() || tile == ModContent.TileType<LeakyMossyMarineBlock>()) return NPC.downedBoss2 || NPC.downedBoss3;
            }
            return base.CanKillTile(i, j, tile, ref blockDamaged);
        }
    }
}
