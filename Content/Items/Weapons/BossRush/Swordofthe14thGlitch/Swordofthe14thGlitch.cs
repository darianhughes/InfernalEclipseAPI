using System;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using InfernumMode;
using InfernumMode.Content.Rarities.InfernumRarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Content.Items.Weapons.BossRush.Swordofthe14thGlitch
{
    public class Swordofthe14thGlitch : ModItem
    {
        public static int UseTime => GlitchSwordHoldout.MaxUpdates * (int)Math.Round(0.45f * 60f); // was 0.55f

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
            Item.DamageType = ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>();
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
            if (player.mount.Active && player.altFunctionUse == 2)
                player.mount.Dismount(player);

            if (Item.type != ItemID.FirstFractal)
                return true;

            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }

        public override bool? UseItem(Player player)
        {
            if (player.mount.Active && player.altFunctionUse == 2)
            {
                player.mount.Dismount(player);
            }
            return base.UseItem(player);
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo src, Vector2 pos, Vector2 vel, int type, int dmg, float kb)
        {
            if (player.ownedProjectileCounts[type] > 0) return false;

            Vector2 v = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            int p = Projectile.NewProjectile(src, player.Center, v, type, dmg, kb, player.whoAmI);

            if ((uint)p < Main.maxProjectiles)
            {
                var proj = Main.projectile[p];
                if (proj.ModProjectile is GlitchSwordHoldout h)
                    h.Mode = (player.altFunctionUse == 2) ? GlitchSwordHoldout.AttackMode.Dash : GlitchSwordHoldout.AttackMode.Slashes;

                proj.ai[1] = Math.Sign(v.X == 0f ? player.direction : v.X);
                proj.netUpdate = true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Zenith);
            if (ModLoader.TryGetMod("YouBoss", out Mod you)) recipe.AddIngredient(you.Find<ModItem>("FirstFractal").Type);
            recipe.AddIngredient<ArkoftheCosmos>();
            recipe.AddIngredient<AshesofAnnihilation>(3);
            recipe.AddIngredient<MiracleMatter>(3);
            if (ModLoader.TryGetMod("CalamityHunt", out Mod calamityHunt)) recipe.AddIngredient(calamityHunt.Find<ModItem>("ChromaticMass").Type, 3);
            if (ModLoader.TryGetMod("NoxusPort", out Mod noxus)) recipe.AddIngredient(noxus.Find<ModItem>("EntropicBar").Type, 3);
            if (ModLoader.TryGetMod("NoxusBoss", out Mod wotg)) recipe.AddIngredient(wotg.Find<ModItem>("MetallicChunk").Type);
            recipe.AddIngredient<Rock>();
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
