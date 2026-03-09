using Terraria.GameContent.ItemDropRules;
using CalamityMod.Items.TreasureBags.MiscGrabBags;
using CalamityMod.Items.SummonItems;
using InfernalEclipseAPI.Content.Items.Lore.InfernalEclipse;
using InfernalEclipseAPI.Core.Players;
using InfernalEclipseAPI.Core.World;
using Terraria.DataStructures;
using InfernalEclipseAPI.Content.Items.Placeables.Paintings;
using InfernalEclipseAPI.Content.Items.Placeables.MusicBoxes;
using InfernalEclipseAPI.Core.Systems;
using InfernalEclipseAPI.Content.Items.Lore.Thorium;
using InfernalEclipseAPI.Content.Items.Armor.Vanity;
using System.Collections.Generic;
using InfernalEclipseAPI.Core.Utils;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Potions.Alcohol;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ModSpecific
{
    public class InfernalGlobalItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<Moonshine>() && InfernalConfig.Instance.CalamityBalanceChanges)
            {
                item.value = Item.buyPrice(0, 1, 0, 0);
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (InfernalCrossmod.FargosMutant.Loaded)
            {
                if (item.type == InfernalCrossmod.FargosMutant.Mod.Find<ModItem>("SuspiciousSkull").Type && !NPC.downedBoss3)
                    return false;
            }

            return base.CanUseItem(item, player);
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (ModLoader.TryGetMod("YouBoss", out Mod you))
            {
                if (you.TryFind("FirstFractal", out ModItem firstFractal))
                {
                    if (item.type == firstFractal.Type)
                    {
                        if (player.mount.Active)
                        {
                            player.mount.Dismount(player);
                        }
                        player.RemoveAllGrapplingHooks();
                    }
                }
            }

            return base.UseItem(item, player);
        }

        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ModContent.ItemType<StarterBag>())
            {
                if (ModLoader.TryGetMod("CalamityAmmo", out Mod calAmmo))
                {
                    itemLoot.Remove(ItemDropRule.Common(calAmmo.Find<ModItem>("HardTackChest").Type));
                }

                if (ModLoader.TryGetMod("ThoriumMod", out var thoriumMod) && !ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                {
                    if (thoriumMod.TryFind("Tambourine", out ModItem tambourineItem))
                    {
                        itemLoot.Add(ItemDropRule.Common(tambourineItem.Type));
                    }

                    if (thoriumMod.TryFind("Pill", out ModItem pillsItem))
                    {
                        itemLoot.Add(ItemDropRule.Common(pillsItem.Type, 1, 200, 200));
                    }
                }

                if (ModLoader.HasMod("SOTS"))
                {
                    itemLoot.Add(ItemDropRule.Common(InfernalCrossmod.SOTS.Mod.Find<ModItem>("WorldgenScanner").Type));
                }

                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MenuMusicBox>()));

                itemLoot.Add(ItemDropRule.ByCondition(new ProviPlayerCondition(), ModContent.ItemType<LoreProvi>()));
                itemLoot.Add(ItemDropRule.ByCondition(new ProviPlayerCondition(), ModContent.ItemType<MysteriousDiary>()));

                itemLoot.Add(ItemDropRule.ByCondition(new SoltanPlayerCondition(), ModContent.ItemType<LoreDylan>()));

                itemLoot.Add(ItemDropRule.ByCondition(new CheesePlayerCondition(), ModContent.ItemType<DeathWhistle>()));

                itemLoot.Add(ItemDropRule.ByCondition(new devListPlayerCondition(), ModContent.ItemType<InfernalTwilight>()));

                itemLoot.Add(ItemDropRule.ByCondition(new AkiraPlayerCondition(), ModContent.ItemType<PhantomMask>()));
                itemLoot.Add(ItemDropRule.ByCondition(new AkiraPlayerCondition(), ModContent.ItemType<PhantomSuitCoat>()));
                itemLoot.Add(ItemDropRule.ByCondition(new AkiraPlayerCondition(), ModContent.ItemType<PhantomSuitPants>()));
            }

            /*
            if (InfernalConfig.Instance.CalamityExpertAccessories) 
            {
                if (item.type == ModContent.ItemType<AquaticScourgeBag>())
                {
                    itemLoot.RemoveWhere(aqua => aqua is CommonDrop commonDrop1 && commonDrop1.itemId == ModContent.ItemType<AquaticEmblem>(), true);
                    itemLoot.RemoveWhere(spine => spine is CommonDrop commonDrop2 && commonDrop2.itemId == ModContent.ItemType<CorrosiveSpine>(), true);
                    ((ILoot)(object)itemLoot).Add(ModContent.ItemType<CorrosiveSpine>(), 1, 1, 1);
                    ((ILoot)(object)itemLoot).Add(ModContent.ItemType<AquaticEmblem>(), DropHelper.BagWeaponDropRateFraction, 1, 1);
                }
                if (item.type == ModContent.ItemType<CryogenBag>())
                {
                    itemLoot.RemoveWhere(flare => flare is CommonDrop commonDrop3 && commonDrop3.itemId == ModContent.ItemType<FrostFlare>(), true);
                    itemLoot.RemoveWhere(soul => soul is CommonDrop commonDrop4 && commonDrop4.itemId == ModContent.ItemType<SoulofCryogen>(), true);
                    ((ILoot)(object)itemLoot).Add(ModContent.ItemType<FrostFlare>(), 1, 1, 1);
                    ((ILoot)(object)itemLoot).Add(ModContent.ItemType<SoulofCryogen>(), DropHelper.BagWeaponDropRateFraction, 1, 1);
                }
                if (item.type == ModContent.ItemType<DesertScourgeBag>())
                {
                    itemLoot.RemoveWhere(cloak => cloak is CommonDrop commonDrop5 && commonDrop5.itemId == ModContent.ItemType<SandCloak>(), true);
                    itemLoot.RemoveWhere(crest => crest is CommonDrop commonDrop6 && commonDrop6.itemId == ModContent.ItemType<OceanCrest>(), true);
                    ((ILoot)(object)itemLoot).Add(ModContent.ItemType<SandCloak>(), 1, 1, 1);
                    ((ILoot)(object)itemLoot).Add(ModContent.ItemType<OceanCrest>(), DropHelper.BagWeaponDropRateFraction, 1, 1);
                }
                if (item.type == ModContent.ItemType<RavagerBag>())
                {
                    itemLoot.RemoveWhere(totem => totem is CommonDrop commonDrop7 && commonDrop7.itemId == ModContent.ItemType<FleshTotem>(), true);
                    itemLoot.RemoveWhere(core => core is CommonDrop commonDrop8 && commonDrop8.itemId == ModContent.ItemType<BloodflareCore>(), true);
                    ((ILoot)(object)itemLoot).Add(ModContent.ItemType<FleshTotem>(), 1, 1, 1);
                    ((ILoot)(object)itemLoot).Add(ModContent.ItemType<BloodflareCore>(), DropHelper.BagWeaponDropRateFraction, 1, 1);
                }
                if (item.type == ItemID.BossBagBetsy)
                {
                    itemLoot.RemoveWhere(wing => wing is CommonDrop commonDrop && commonDrop.itemId == ItemID.BetsyWings, true);
                    ((ILoot)(object)itemLoot).Add(3883, 1, 1, 1);
                }
                if (item.type == ModContent.ItemType<LeviathanBag>())
                {
                    itemLoot.RemoveWhere(Community => Community is CommonDrop commonDrop && commonDrop.itemId == ModContent.ItemType<TheCommunity>(), true);
                    ((ILoot)(object)itemLoot).DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<TheCommunity>(), 10, 1, 1, false);
                }
                if (item.type == ModContent.ItemType<CalamitasCloneBag>())
                {
                    itemLoot.RemoveWhere(regen => regen is CommonDrop commonDrop && commonDrop.itemId == ModContent.ItemType<Regenator>(), true);
                    ((ILoot)(object)itemLoot).DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<Regenator>(), 10, 1, 1, false);
                }
                if (item.type == ItemID.QueenBeeBossBag)
                {
                    itemLoot.RemoveWhere(Bee => Bee is CommonDrop commonDrop9 && commonDrop9.itemId == ModContent.ItemType<TheBee>(), true);
                    ((ILoot)(object)itemLoot).DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<TheBee>(), 10, 1, 1, false);
                }

                if (ModLoader.TryGetMod("CalamityHunt", out Mod calHunt))
                {
                    if (item.type == calHunt.Find<ModItem>("TreasureTrunk").Type)
                    {
                        itemLoot.RemoveWhere(Tend => Tend is CommonDrop commonDrop && commonDrop.itemId == calHunt.Find<ModItem>("TendrilCursorAttachment").Type, true);
                        ((ILoot)(object)itemLoot).DefineConditionalDropSet(DropHelper.RevAndMaster).Add(calHunt.Find<ModItem>("TendrilCursorAttachment").Type, 10, 1, 1, false);
                    }
                }

                if (InfernalCrossmod.Clamity.Loaded)
                {
                    if (item.type == InfernalCrossmod.Clamity.Mod.Find<ModItem>("PyrogenBag").Type)
                    {
                        itemLoot.RemoveWhere(flare => flare is CommonDrop commonDrop3 && commonDrop3.itemId == InfernalCrossmod.Clamity.Mod.Find<ModItem>("HellFlare").Type, true);
                        ((ILoot)(object)itemLoot).Add(InfernalCrossmod.Clamity.Mod.Find<ModItem>("HellFlare").Type);
                    }
                }
            }
            */
        }

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (item.type == ItemID.TinkerersWorkshop)
            {
                InfernalWorld.craftedWorkshop = true;
            }

            if (InfernalCrossmod.Thorium.Loaded)
            {
                if (item.type == InfernalCrossmod.Thorium.Mod.Find<ModItem>("Mjolnir").Type)
                {
                    Player p = Main.LocalPlayer;

                    if (Main.netMode == NetmodeID.Server)
                    {
                        int hammerLore = Item.NewItem(p.GetSource_Misc("IEoR_MjolnirCrafted"), (int)p.position.X, (int)p.position.Y, p.width, p.height, ModContent.ItemType<LoreMjolnir>());
                        NetMessage.SendData(MessageID.InstancedItem, Main.myPlayer, -1, null, hammerLore);
                    }
                }
            }

            base.OnCreated(item, context);
        }

        public override bool OnPickup(Item item, Player player)
        {
            if (item.type == ItemID.TinkerersWorkshop)
            {
                player.GetModPlayer<InfernalPlayer>().workshopHasBeenOwned = true;
                InfernalWorld.craftedWorkshop = true;
            }

            return base.OnPickup(item, player);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if ((item.type == ModContent.ItemType<GrapeBeer>() || item.type == ModContent.ItemType<Moonshine>()) && InfernalConfig.Instance.CalamityBalanceChanges)
                InfernalUtilities.AddTooltip(tooltips, Language.GetTextValue(Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.TwoAlchs")), Color.Lerp(Color.White, new Color(255, 80, 0), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)));

            if (InfernalCrossmod.FargosMutant.Loaded)
            {
                if (item.type == InfernalCrossmod.FargosMutant.Mod.Find<ModItem>("SuspiciousSkull").Type && !NPC.downedBoss3)
                    InfernalUtilities.AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MutantSkeletron"), Color.Lerp(Color.White, new Color(255, 80, 0), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)));
            }
        }
    }

    public class devListPlayerCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            // Loop through all players in the world
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                foreach (string name in InfernalTwilight.devList)
                {
                    if (player.active && player.name.ToLower().Contains(name))
                        return true;
                }
                if (player.active && (player.name.ToLower().Contains("nuggets") || player.name.ToLower().Contains("hummus")))
                    return true;
            }
            return false;
        }

        public bool CanShowItemDropInUI() => false;
        public string GetConditionDescription() => "A certain person must be present...";
    }

    public class ProviPlayerCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            // Loop through all players in the world
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && (player.name == "Galactica" || player.name.ToLower().Contains("radiant")))
                    return true;
            }
            return false;
        }

        public bool CanShowItemDropInUI() => false;
        public string GetConditionDescription() => "A certain person must be present...";
    }

    public class SoltanPlayerCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            // Loop through all players in the world
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && player.name == "Bloxxer")
                    return true;
            }
            return false;
        }

        public bool CanShowItemDropInUI() => false;
        public string GetConditionDescription() => "A certain person must be present...";
    }


    public class CheesePlayerCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            // Loop through all players in the world
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && player.name == "lifenuggets")
                    return true;
            }
            return false;
        }

        public bool CanShowItemDropInUI() => false;
        public string GetConditionDescription() => "A certain person must be present...";
    }

    public class AkiraPlayerCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            // Loop through all players in the world
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && player.name == "Akira")
                    return true;
            }
            return false;
        }

        public bool CanShowItemDropInUI() => false;
        public string GetConditionDescription() => "A certain person must be present...";
    }
}
