using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.Donate;
using ThoriumMod.Utilities;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    [ExtendsFromMod("ThoriumMod")]
    public class PlasmaGenAstrageldonFix : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (ModLoader.TryGetMod("CatalystMod", out Mod catalyst))
            {
                if (NPC.AnyNPCs(catalyst.Find<ModNPC>("Astrageldon").Type))
                {
                    int[] generatorTypes =
                    {
                        ModContent.ItemType<PlasmaGenerator>(),
                        ModContent.ItemType <AsgardianAegis>(),
                    };

                    foreach (int genItem in generatorTypes)
                    {
                        if (item.type == genItem)
                        {
                            player.GetThoriumPlayer().accPlasmaGenerator = false;
                        }
                    }

                    if (ModLoader.TryGetMod("Clamity", out Mod clam))
                    {
                        if (item.type == clam.Find<ModItem>("SupremeBarrier").Type)
                            player.GetThoriumPlayer().accPlasmaGenerator = false;
                    }
                }
            }
        }
    }
}
