using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.SummonItems;
using Microsoft.Xna.Framework;
using ThoriumMod.Biomes.Depths;
using ThoriumMod.Items.Depths;
using ThoriumMod.NPCs.BossForgottenOne;

namespace InfernalEclipseAPI.Content.Items.SpawnItems
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class EldritchBattleHorn : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 11;
            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 38;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.consumable = false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(5, 12);

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }

        public override bool CanUseItem(Player player)
        {
            return player.InModBiome<DepthsBiome>() && !ForgottenOne.AnyForgottenOneAlive(false) && !BossRushEvent.BossRushActive;
        }


        public override bool? UseItem(Player player)
        {
            int posX = (int)player.position.X;
            int posY = (int)(player.position.Y - 700f);
            int bossToSpawn = ModContent.NPCType<ForgottenOne>();
            CalamityUtils.SpawnBossOnPosUsingItem(player, bossToSpawn, posX, posY, NaiadsWarhorn.HornSound);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BrackishClump>(20).
                AddIngredient<Lumenyl>(5).
                AddIngredient<DepthScale>(5).
                AddIngredient<AbyssalChitin>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
