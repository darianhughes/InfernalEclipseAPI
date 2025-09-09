using System;
using CalamityMod;
using CalamityMod.Items;
using InfernumMode.Content.Rarities.InfernumRarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;

namespace InfernalEclipseAPI.Content.Items.Weapons.Melee.SwordoftheFirst
{
    public class Swordofthe1stGlitch : ModItem
    {
        public static int UseTime => FirstSwordHoldoutCombo.MaxUpdates * (int)Math.Round(0.65f * 60f);

        public static int BaseDamage => 230;

        public static float PlayerPostHitSpeed => 30f;
        public static int PlayerPostHitIFrameGracePeriod => (int)Math.Round(1.0f * 60f);
        public static float PlayerDashSpeed => 75f;

        private static readonly SoundStyle SlashSfx = new("InfernalEclipseAPI/Assets/Sounds/Custom/TerraBlade/Slash")
        {
            MaxInstances = 12,
            SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest,
            PitchVariance = 0.06f
        };

        public override void SetDefaults()
        {
            Item.width = 78;
            Item.height = 88;
            Item.damage = BaseDamage;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SlashSfx;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useTurn = true;
            Item.DamageType = ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>();
            Item.UseSound = null;
            Item.knockBack = 8f;
            Item.autoReuse = true;
            Item.noUseGraphic = false;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<FirstSwordHoldoutCombo>();
            Item.shootSpeed = 9f;
            Item.rare = ModContent.RarityType<InfernumRedSparkRarity>();
            if (ModLoader.TryGetMod("NoxusBoss", out Mod noxus))
            {
                if (noxus.TryFind("NamelessDeityRarity", out ModRarity r))
                    Item.rare = r.Type;
            }
            Item.value = CalamityGlobalItem.RarityLightPurpleBuyPrice;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.mount.Active)
                    player.mount.Dismount(player);
                Item.noUseGraphic = true;
                Item.UseSound = null;
            }
            else
            {
                Item.noUseGraphic = false;
                Item.UseSound = SlashSfx;
            }
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }

        public override bool? UseItem(Player player)
        {
            if (player.mount.Active && player.altFunctionUse == 2)
                player.mount.Dismount(player);
            return base.UseItem(player);
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo src, Vector2 pos, Vector2 vel, int type, int dmg, float kb)
        {
            if (player.altFunctionUse != 2) return false;
            if (player.ownedProjectileCounts[type] > 0) return false;

            Vector2 v = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            int p = Projectile.NewProjectile(src, player.Center, v, type, dmg / 100, kb, player.whoAmI);

            if ((uint)p < Main.maxProjectiles)
            {
                var proj = Main.projectile[p];
                if (proj.ModProjectile is FirstSwordHoldoutCombo h)
                    h.Mode = (player.altFunctionUse == 2) ? FirstSwordHoldoutCombo.AttackMode.Dash : FirstSwordHoldoutCombo.AttackMode.Slashes;

                // ensure facing matches intended direction
                proj.ai[1] = Math.Sign(v.X == 0f ? player.direction : v.X);
                proj.netUpdate = true;
            }
            return false;
        }

        public override bool PreDrawInInventory(
            SpriteBatch spriteBatch,
            Vector2 position,
            Rectangle frame,
            Color drawColor,
            Color itemColor,
            Vector2 origin,
            float scale)
        {
            const float factor = 1.3f; // your 1.5x
            var tex = TextureAssets.Item[Type].Value;

            // Keep the same position & origin; only change the scale.
            Main.EntitySpriteDraw(
                tex,
                position,
                frame,
                drawColor,
                0f,
                origin,
                scale * factor,
                SpriteEffects.None,
                0
            );
            return false; // skip vanilla draw
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AdamantiteBar, ModLoader.TryGetMod("ThoriumMod", out Mod thorium) ? 9 : 12);
            if (thorium != null) recipe.AddIngredient(thorium.Find<ModItem>("TitanicBar"), 3);
            if (ModLoader.TryGetMod("SOTS", out Mod sots)) recipe.AddIngredient(sots.Find<ModItem>("RubyKeystone").Type);
            else recipe.AddIngredient(ItemID.LargeRuby);
            if (thorium != null) recipe.AddIngredient(thorium.Find<ModItem>("CursedCloth"), 3);
            else recipe.AddIngredient(ItemID.RedBanner);
            recipe.AddIngredient(ItemID.DarkShard);
            recipe.AddIngredient(ItemID.LightShard);
            recipe.AddTile(thorium != null ? thorium.Find<ModTile>("SoulForge").Type : TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
