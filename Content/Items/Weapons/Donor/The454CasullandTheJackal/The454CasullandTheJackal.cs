using System.Collections.Generic;
using System.IO;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Particles;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace InfernalEclipseAPI.Content.Items.Weapons.Donor.The454CasullandTheJackal
{
    public class The454CasullandTheJackal : ModItem
    {
        private int lastUsedMode = 1;
        private int justUsedMode = 1;

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 44;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 1375;
            Item.knockBack = 7f;
            Item.crit = 26;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.channel = true;

            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<CasullandJackalProj>();
            Item.shootSpeed = 1f;

            Item.Calamity().donorItem = true;

            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        #region Toggleable Ammo Swap
        bool toggleEnabled;
        public override bool CanRightClick() => Main.keyState.PressingShift();
        public override void RightClick(Player player)
        {
            toggleEnabled = !toggleEnabled;
            Item.NetStateChanged();
        }
        public override bool ConsumeItem(Player player) => false;
        public override void SaveData(TagCompound tag)
        {
            tag.Add("toggleEffect", toggleEnabled);
        }
        public override void LoadData(TagCompound tag)
        {
            toggleEnabled = tag.GetBool("toggleEffect");
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(toggleEnabled);
        }
        public override void NetReceive(BinaryReader reader)
        {
            toggleEnabled = reader.ReadBoolean();
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            CalamityUtils.DrawInventoryDot(spriteBatch, position, new Vector2(16, 16) * Main.inventoryScale, toggleEnabled);
        }
        public override void UpdateInventory(Player player)
        {
        }
        #endregion

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[Item.shoot] == 0)
            {
                justUsedMode = player.altFunctionUse == 2 ? 2 : 1;

                if (justUsedMode != lastUsedMode && toggleEnabled)
                {
                    int ammoType = Item.useAmmo;

                    int firstSlot = -1;
                    int secondSlot = -1;

                    // Find the first two valid ammo slots.
                    for (int i = 54; i <= 57; i++)
                    {
                        if (player.inventory[i].IsAir || player.inventory[i].ammo != ammoType)
                            continue;

                        if (firstSlot == -1)
                            firstSlot = i;
                        else
                        {
                            secondSlot = i;
                            break;
                        }
                    }

                    // Only swap if two valid ammo slots were found.
                    if (firstSlot != -1 && secondSlot != -1)
                    {
                        Item firstItem = player.inventory[firstSlot].Clone();
                        Item secondItem = player.inventory[secondSlot].Clone();

                        bool firstFavorited = firstItem.favorited;
                        bool secondFavorited = secondItem.favorited;

                        player.inventory[firstSlot] = secondItem;
                        player.inventory[secondSlot] = firstItem;

                        player.inventory[firstSlot].favorited = secondFavorited;
                        player.inventory[secondSlot].favorited = firstFavorited;

                    // Produce a visual effect showing the top ammo that you swapped to.
                        int visualType = player.inventory[firstSlot].type;
                        Texture2D ammoTex = TextureAssets.Item[visualType].Value;
                        int frameAmt = Main.itemAnimations[visualType] == null ? 1 : ammoTex.Height / Main.itemAnimations[visualType].GetFrame(ammoTex).Height;
                        CustomSprite ammoVisual = new(player.Center - Vector2.UnitY * 20f, -Vector2.UnitY * 7f, 30, ammoTex, 1f, Color.White, 0f, false, false, frameAmt);
                        GeneralParticleHandler.SpawnParticle(ammoVisual);
                    }
                }

                lastUsedMode = justUsedMode;

                return true;
            }
            return false; 
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(
                source,
                position,
                velocity,
                Item.shoot,
                damage,
                knockback,
                player.whoAmI,
                player.altFunctionUse == 2 ? 1f : 0f // 0 = left gun, 1 = right gun
            );

            return false;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            return player.altFunctionUse == 2 ? 0.25f : base.UseSpeedMultiplier(player);
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.altFunctionUse == 2)
                damage *= 0.2f;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            if (player.altFunctionUse == 2)
                crit = 11f;
        }

        public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback)
        {
           if (player.altFunctionUse == 2)
                knockback *= 0.4f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!toggleEnabled)
            {
                tooltips.RemoveAll(x => x.Name == "Tooltip0");
            }

            base.ModifyTooltips(tooltips);

            if (Main.keyState.IsKeyDown(Keys.LeftShift))
            {
                TooltipLine line5 = new(Mod, "DedicatedItem", $"{Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.DedTo", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Dedicated.rose"))}");
                line5.OverrideColor = new(196, 35, 44);
                tooltips.Add(line5);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Auralis>()
                .AddIngredient(ItemID.Handgun)
                .AddIngredient<CosmiliteBar>(8)
                .AddIngredient<BloodstoneCore>(3)
                .AddIngredient<RuinousSoul>(3)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}
