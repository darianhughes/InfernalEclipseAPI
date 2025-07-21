using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixSystems.SpawnBlockerFixes
{
    public class SpawnBlockerFixes : GlobalItem
    {
        public override void UpdateInventory(Item item, Player player)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                if (item.type == ModContent.ItemType<VoodooDemonVoodooDoll>())
                {
                    player.Calamity().disableVoodooSpawns = true;
                }
                
                if (item.type == ModContent.ItemType<BrokenWaterFilter>())
                {
                    player.Calamity().noStupidNaturalARSpawns = true;
                }

                if (item.type == ModContent.ItemType<AntiCystOintment>())
                {
                    player.Calamity().disablePerfCystSpawns = true;
                }

                if (item.type == ModContent.ItemType<AntiTumorOintment>())
                {
                    player.Calamity().disableHiveCystSpawns = true;
                }

                if (item.type == ModContent.ItemType<BleachBall>())
                {
                    player.Calamity().disableNaturalScourgeSpawns = true;
                }

                if (item.type == ModContent.ItemType<SirenproofEarMuffs>())
                {
                    player.Calamity().disableAnahitaSpawns = true;
                }
            }
        }
    }
}
