using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Weapons.Melee;
using InfernumMode;
using InfernumMode.Content.Rarities.InfernumRarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using YouBoss.Content.Items.ItemReworks;
using static Microsoft.Xna.Framework.MathHelper;

namespace InfernalEclipseAPI.Content.Items.Weapons.Swordofthe14thGlitch
{
    public class Swordofthe14thGlitch : ModItem
    {
        public static int UseTime => GlitchSwordHoldout.MaxUpdates * (int)Math.Round(0.55f * 60f);

        public static int BaseDamage => 23000;

        public static float HomingBeamSearchRange => 1372f;

        public static float HomingBeamDamageFactor => 0.4f;

        public static float HomingSlashDamageFactor => 0.8f;

        public static float HomingBeamAcceleration => 1.38f;

        public static float HomingBeamFlySpeedInterpolant => 0.09f;

        public static float PlayerPostHitSpeed => 40f;

   
        public static int PlayerPostHitIFrameGracePeriod => (int)Math.Round(4.0f * 60f);

        public static float PlayerDashSpeed => 100f;

        public static float HomingBeamStartingSpeed => 250f;

        public static float HomingBeamDecelerationFactor => 0.6f;

        public override void SetDefaults()
        {
            Item.width = 88;
            Item.height = 88;
            Item.damage = BaseDamage;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.useTurn = true;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.UseSound = null;
            Item.knockBack = 8f;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<GlitchSwordHoldout>();
            Item.shootSpeed = 9f;
            Item.rare = ModContent.RarityType<InfernumRedSparkRarity>();
            if (ModLoader.TryGetMod("NoxusBoss", out Mod noxus))
            {
                ModRarity r;
                noxus.TryFind("NamelessDeityRarity", out r);
                Item.rare = r.Type;
            }

            Item.Infernum_Tooltips().DeveloperItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (Item.type != ItemID.FirstFractal)
                return true;

            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }

        public override void AddRecipes()
        {
            if (ModLoader.TryGetMod("CalamityHunt", out Mod calamityHunt) && calamityHunt.TryFind("ChromaticMass", out ModItem ChormaticMass))
            {
                CreateRecipe()
                    .AddIngredient<AshesofAnnihilation>(3)
                    .AddIngredient<MiracleMatter>(3)
                    .AddIngredient(ChormaticMass.Type, 3)
                    .AddIngredient<FirstFractal>(1)
                    .AddIngredient(ItemID.Zenith, 1)
                    .AddIngredient<ArkoftheCosmos>(1)
                    .AddIngredient<Rock>()
                    .AddTile(TileID.DemonAltar)
                    .Register();
            }
            else
            {
                CreateRecipe()
                    .AddIngredient<AshesofAnnihilation>(3)
                    .AddIngredient<MiracleMatter>(3)
                    .AddIngredient<ShadowspecBar>(3)
                    .AddIngredient<FirstFractal>(1)
                    .AddIngredient(ItemID.Zenith, 1)
                    .AddIngredient<ArkoftheCosmos>(1)
                    .AddIngredient<Rock>()
                    .AddTile(TileID.DemonAltar)
                    .Register();
            }
        }
    }
}
