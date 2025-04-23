using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.Tiles.Ores;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Common.Balance
{
    public class OreSafeguard : GlobalTile
    {
        public override void SetStaticDefaults()
        {
            if (InfernalConfig.Instance.BossKillCheckOnOres)
            {

                TileID.Sets.CanBeClearedDuringOreRunner[37] = true;
                TileID.Sets.CanBeClearedDuringOreRunner[ModContent.TileType<ExodiumOre>()] = true;
            }
        }

        public override bool CanKillTile(int i, int j, int tile, ref bool blockDamaged)
        {
            if (InfernalConfig.Instance.BossKillCheckOnOres)
            {
                switch (tile)
                {
                    case 37:
                        return NPC.downedBoss2;
                    case 58:
                        return NPC.downedBoss3;
                    case 408:
                        return NPC.downedMoonlord;
                    case 659:
                        return NPC.downedBoss2;
                    default:
                        if (tile == ModContent.TileType<ExodiumOre>()) { return NPC.downedMoonlord; }

                        //putting this here because i dont wanna make a new class
                        else if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                        {
                            if (thorium.TryFind<ModTile>("", out ModTile bloodAlter))
                            {
                                return false;
                            }
                        }

                        return true; // Default fallback if no match
                }
            }
            return true;
        }
    }
}
