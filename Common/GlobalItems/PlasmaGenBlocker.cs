using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Accessories;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.Donate;
using ThoriumMod.Utilities;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    [ExtendsFromMod("ThoriumMod")]
    public class PlasmaGenBlocker : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            int[] generatorTypes =
            {
                ModContent.ItemType<PlasmaGenerator>(),
                ModContent.ItemType<AsgardianAegis>(),
            };

            int[] blockedBosses =
            {
                ModContent.NPCType<Apollo>(),
                ModContent.NPCType<Artemis>(),
                ModContent.NPCType<AresBody>(),
                ModContent.NPCType<ThanatosHead>()
            };

            foreach (int boss in blockedBosses)
            {
                if (NPC.AnyNPCs(boss))
                {
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

            if (ModLoader.TryGetMod("CatalystMod", out Mod catalyst))
            {
                if (NPC.AnyNPCs(catalyst.Find<ModNPC>("Astrageldon").Type))
                {
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

            if (ModLoader.TryGetMod("CalamityHunt", out Mod calHunt))
            {
                if (NPC.AnyNPCs(calHunt.Find<ModNPC>("Goozma").Type))
                {
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

            if (ModLoader.TryGetMod("NoxusBoss", out Mod wotg))
            {
                if (NPC.AnyNPCs(wotg.Find<ModNPC>("NamelessDeityBoss").Type))
                {
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
