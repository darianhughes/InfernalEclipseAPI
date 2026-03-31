using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Cooldowns;
using CalamityMod.Dusts;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using InfernalEclipseAPI.Core.DamageClasses.MythicClass;
using InfernalEclipseAPI.Core.Systems;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Items.Weapons.Melee.TyrantYharimsUltisword
{
    public class TyrantYharimsUltisword : ModItem
    {
        public int throwCount = 0;
        public override void SetDefaults()
        {
            Item.width = 140;
            Item.height = 140;

            Item.damage = 750;
            Item.crit = 41;
            Item.useAnimation = Item.useTime = 45;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<TyrantYharimsUltiswordThrownBlade>();
            Item.useTurn = true;
            Item.knockBack = 7.5f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = ModContent.GetInstance<MythicMelee>();
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.channel = true;

            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<TyrantYharimsUltiswordHoldout>()] <= 0 && !player.Calamity().mouseRight;
        }
        public override void HoldItem(Player player)
        {
            player.Calamity().mouseWorldListener = true;
            if (player.whoAmI != Main.myPlayer)
                return;
            if (player.Calamity().mouseRight && !player.mouseInterface && player.Calamity().killModeCooldown == 0 && !Main.mapFullscreen && !Main.blockMouse)
            {
                SoundStyle buff = new("CalamityMod/Sounds/Item/DemonSwordKillMode");
                SoundEngine.PlaySound(buff with { Volume = 0.95f }, player.Center);

                for (int i = 0; i < 10; i++)
                {
                    Vector2 vel = (MathHelper.TwoPi * i / 10f).ToRotationVector2() * 6.5f;
                    Particle spark2 = new CustomSpark(player.Center + vel * 14, -vel * 0.1f, "CalamityMod/Particles/DemonSigilParticle", false, 22, 0.6f, (i % 2 == 0 ? Color.Firebrick : Color.OrangeRed) * 0.7f, new Vector2(1, 1), true, false, 0, false, false, -0.23f);
                    GeneralParticleHandler.SpawnParticle(spark2);

                    Dust c = Dust.NewDustPerfect(player.Center, ModContent.DustType<LightDust>());
                    c.velocity = vel;
                    c.scale = 1.7f;
                    c.noGravity = true;
                    c.color = (i % 2 != 0 ? Color.Firebrick : Color.OrangeRed);
                    c.noLightEmittence = true;
                }

                player.Calamity().demonSwordKillMode = true;

                int cooldownTime = KillMode.cooldownMax + KillMode.buffMax;
                player.Calamity().killModeCooldown = cooldownTime / 2;
                player.AddCooldown(KillMode.ID, cooldownTime / 2);
            }
            if (player.Calamity().demonSwordKillMode && player.ownedProjectileCounts[ModContent.ProjectileType<TyrantYharimsUltiswordHoldout>()] <= 0 && player.Calamity().killModeCooldown == (KillMode.cooldownMax + KillMode.buffMax) / 2)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<TyrantYharimsUltiswordHoldout>(), Item.damage * 30, Item.knockBack, player.whoAmI, 0, throwCount);
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            throwCount++;
            int useSpeed = (int)MathHelper.Clamp((Item.useTime / 2.8f), 1, 100);
            Projectile blade = Projectile.NewProjectileDirect(source, player.MountedCenter, velocity, type, damage, knockback, player.whoAmI, 0, throwCount);
            blade.localAI[2] = useSpeed;
            blade.timeLeft += useSpeed;
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("ItemName"));
            if (index != -1)
            {
                tooltips.Insert(index + 1, new TooltipLine(Mod, "SignatureWeapon", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MythicTooltips.Base", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MythicTooltips.Yharim")))
                {
                    OverrideColor = Color.Cyan
                });
            }
        }

        public override void AddRecipes()
        {
            return;

            Recipe ultisword = CreateRecipe();
            ultisword.AddIngredient<DevilsDevastation>();
            ultisword.AddIngredient<ShadowspecBar>(10);
            ultisword.AddIngredient<AuricBar>(InfernalCrossmod.Thorium.Loaded ? 7 : 10);
            if (InfernalCrossmod.Thorium.Loaded)
                ultisword.AddIngredient(InfernalCrossmod.Thorium.Mod.Find<ModItem>("InfernoEssence"), 3);
            ultisword.AddTile<DraedonsForge>();
            ultisword.Register();
        }
    }
}
