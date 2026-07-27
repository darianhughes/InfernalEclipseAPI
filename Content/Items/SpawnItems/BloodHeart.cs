using System.Collections.Generic;
using CalamityMod.Items.Materials;
using InfernalEclipseAPI.Common.Balance.Recipes;
using ThoriumMod;
using ThoriumMod.Core.Handlers.HoverItemHandler;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.NPCs.BossViscount;
using ThoriumMod.Tiles;
using ThoriumMod.Utilities;

namespace InfernalEclipseAPI.Content.Items.SpawnItems
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class BloodHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.BloodySpine;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 42;
            Item.rare = ItemRarityID.Green;
            Item.consumable = false;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<BloodOrb>()
                .AddIngredient<UnholyShards>(5)
                .AddRecipeGroup(InfernalRecipeSystem.EvilSkinRecipeGroup, 5)
                .AddTile<ThoriumAnvil>()
                .Register();
        }
    }

    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class BloodAlterAdjustments : GlobalTile
    {
        public override void RightClick(int i, int j, int type)
        {
            if (type != ModContent.TileType<BloodAltar>()) return;

            Player localPlayer = Main.LocalPlayer;
            if (!CanSpawnViscount(localPlayer))
                base.RightClick(i, j, type);
            Dictionary<int, int> indexToConsumedStack = localPlayer.CountInventoryItemIdxWithStack(ModContent.ItemType<BloodHeart>(), 1);
            if (indexToConsumedStack.Count > 0 && !IsViscountActive())
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(localPlayer.whoAmI, ModContent.NPCType<Viscount>());
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, localPlayer.whoAmI, ModContent.NPCType<Viscount>(), 0.0f, 0.0f, 0, 0, 0);
                return;
            }
        }

        public override void MouseOver(int i, int j, int type)
        {
            if (type != ModContent.TileType<BloodAltar>()) return;

            Player localPlayer = Main.LocalPlayer;

            Dictionary<int, int> indexToConsumedStack = localPlayer.CountInventoryItemIdxWithStack(ModContent.ItemType<BloodHeart>(), 1);
            if (indexToConsumedStack.Count > 0 && CanSpawnViscount(localPlayer))
            {
                localPlayer.noThrow = 2;
                HoverItemSystem.QueueHoverItem(ModContent.ItemType<BloodHeart>(), 1);
            }
        }

        private static bool IsViscountActive() => NPC.AnyNPCs(ModContent.NPCType<Viscount>());

        private static bool CanSpawnViscount(Player player)
        {
            return (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || ThoriumWorld.downedViscount) && !IsViscountActive();
        }
    }
}
