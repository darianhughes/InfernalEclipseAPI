using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using InfernalEclipseAPI.Core.Players;
using SOTS;
using SOTS.Void;

namespace InfernalEclipseAPI.Content.Items.PermanentBoosters
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class RuinousPlasmaInjection : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(5);
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 20;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;
            Item.useAnimation = Item.useTime = 12;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.maxStack = 9999;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.UseSound = SoundID.NPCDeath39;
            ItemID.Sets.ItemNoGravity[Type] = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<InfernalPlayer>().ruinousPlasmaInjection < 5;
        }

        public override bool? UseItem(Player player)
        {
            if (player.GetModPlayer<InfernalPlayer>().ruinousPlasmaInjection >= 5)
                return false;

            player.VoidPlayer().voidMeterMax += 10;
            VoidPlayer.VoidEffect(player, 10);
            player.GetModPlayer<InfernalPlayer>().ruinousPlasmaInjection++;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ArmoredShell>()
                .AddIngredient<DarkPlasma>()
                .AddIngredient<RuinousSoul>()
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
