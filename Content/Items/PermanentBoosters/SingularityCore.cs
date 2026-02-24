using CalamityMod.Items;
using InfernalEclipseAPI.Core.Players;
using SOTS;
using SOTS.Items.Celestial;
using SOTS.Items.Void;
using SOTS.Void;

namespace InfernalEclipseAPI.Content.Items.PermanentBoosters
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class SingularityCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(5);
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 20;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 36;
            Item.useAnimation = Item.useTime = 12;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;
            Item.rare = ItemRarityID.Purple;
            Item.maxStack = 9999;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.UseSound = SoundID.NPCDeath39;
            ItemID.Sets.ItemNoGravity[Type] = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.GetModPlayer<InfernalPlayer>().singularityCore;
        }

        public override bool? UseItem(Player player)
        {
            if (player.GetModPlayer<InfernalPlayer>().singularityCore == true)
                return false;

            player.VoidPlayer().voidMeterMax += 100;
            VoidPlayer.VoidEffect(player, 100);
            player.GetModPlayer<InfernalPlayer>().singularityCore = true;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<SoulHeart>(2)
                .AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient<SanguiteBar>(10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
