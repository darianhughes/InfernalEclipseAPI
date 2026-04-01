using InfernalEclipseAPI.YharimEX.Core.Systems;
using InfernalEclipseAPI.YharimEX.Content.NPCs.Bosses;
using CalamityMod.Rarities;
using CalamityMod.Items.Materials;
using Terraria;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace InfernalEclipseAPI.YharimEX.Content.Items
{
    public class YharimsRage : ModItem
    {
        public override string Texture => "CalamityMod/Items/Weapons/Magic/YharimsCrystal";
        public override void SetStaticDefaults()
        {
            //Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 11));
            //ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = false;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 52;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.maxStack = 20;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.buyPrice(1);
        }

        public override bool CanUseItem(Player player) 
        {
            if (NPC.AnyNPCs(ModContent.NPCType<YharimEXBoss>()))
                return false;
            return player.Center.Y / 16 < Main.worldSurface;
        }

        public override bool? UseItem(Player player)
        {
            YharimEXUtils.SpawnBossNetcoded(player, ModContent.NPCType<YharimEXBoss>());
            return true;
        }

        //public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<AuricBar>(5);
            recipe.AddIngredient<AshesofAnnihilation>(3);
            recipe.AddIngredient<MiracleMatter>();
            if (ModLoader.TryGetMod("NoxusBoss", out Mod wotg)) recipe.AddIngredient(wotg.Find<ModItem>("MetallicChunk").Type);
            recipe.AddTile<SCalAltar>();
            recipe.Register();
        }
    }
}