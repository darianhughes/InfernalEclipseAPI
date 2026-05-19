using System.Collections.Generic;
using System.Linq;
using InfernumMode.Content.Rarities.InfernumRarities;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Content.Items.Other
{
    public class SoulDrivenHeadphonesEclipse : ModItem
    {
        public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 76;
            Item.useTime = Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0f;

            Item.value = 0;
            Item.rare = ModContent.RarityType<InfernumSoulDrivenHeadphonesRarity>();

            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<EclipseHeadphonesProj>();
            Item.channel = true;
            Item.shootSpeed = 0f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Name == "Tooltip1");
            TooltipLine line2 = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Name == "Tooltip2");

            if (line is not null && line2 is not null)
                line.OverrideColor = line2.OverrideColor = Color.Lerp(Color.Orchid, new Color(255, 80, 0), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));
        }


        public override bool CanUseItem(Player player)
        {
            bool infernumHeadphonesOn = false;
            if (ModLoader.HasMod("InfernumModeMusic"))
            {
                infernumHeadphonesOn = player.ownedProjectileCounts[ModLoader.GetMod("InfernumModeMusic").Find<ModProjectile>("SoulDrivenHeadphonesProj").Type] > 0;
            }

            return !infernumHeadphonesOn && player.ownedProjectileCounts[Item.shoot] <= 0;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            if (ModLoader.TryGetMod("InfernumModeMusic", out Mod infMusic))
            {
                recipe.AddIngredient(infMusic.Find<ModItem>("SoulDrivenHeadphones").Type);
            }
            else
            {
                recipe.AddIngredient(ItemID.Glass, 10);
                recipe.AddIngredient(ItemID.Silk, 10);
                recipe.AddIngredient(ItemID.Stinger);
            }
            recipe.AddIngredient(ItemID.OrangeandBlackDye, 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }

    public class InfernumHeadphonesGlobal : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("InfernumModeMusic");

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ModLoader.GetMod("InfernumModeMusic").Find<ModItem>("SoulDrivenHeadphones").Type;

        public override bool CanUseItem(Item item, Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<EclipseHeadphonesProj>()] > 0)
                return false;

            return base.CanUseItem(item, player);
        }
    }
}
