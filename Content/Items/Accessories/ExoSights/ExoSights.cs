using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SOTS;
using Terraria.Localization;
using SOTS.Items.CritBonus;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace InfernalEclipseAPI.Content.Items.Accessories.ExoSights
{
    [ExtendsFromMod("SOTS")]
    public class ExoSights : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Register vertical animation: 10 frames, 5 ticks per frame (adjust as desired)
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 10));
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 36;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.accessory = true;
            Item.defense = 3;
        }

        #region Sprite Drawing
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            int frameWidth = 44;
            int frameHeight = 36;
            int totalFrames = 10;
            int ticksPerFrame = 5;

            int currentFrame = (int)((Main.GameUpdateCount / ticksPerFrame) % totalFrames);

            Rectangle sourceRect = new Rectangle(0, frameHeight * currentFrame, frameWidth, frameHeight);

            Texture2D texture = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Accessories/ExoSights/ExoSights").Value;

            // Calculate offset: (frame - slot) / 2 for both x and y
            Vector2 slotOffset = new Vector2((frameWidth - 32) / 2f, (frameHeight - 32) / 2f);

            spriteBatch.Draw(
                texture,
                position - slotOffset,
                sourceRect,
                drawColor,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f
            );
            return false; // Don't draw default
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            int frameWidth = 44;
            int frameHeight = 36;
            int totalFrames = 10;
            int ticksPerFrame = 5;

            int currentFrame = (int)((Main.GameUpdateCount / ticksPerFrame) % totalFrames);

            Rectangle sourceRect = new Rectangle(0, frameHeight * currentFrame, frameWidth, frameHeight);

            Texture2D texture = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Accessories/ExoSights/ExoSights").Value;

            // Center of item's hitbox
            Vector2 itemCenter = Item.position + new Vector2(Item.width, Item.height) / 2f;
            Vector2 origin = new Vector2(frameWidth / 2f, frameHeight / 2f);
            Vector2 drawPos = itemCenter - Main.screenPosition;

            spriteBatch.Draw(
                texture,
                drawPos,
                sourceRect,
                lightColor,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                0f
            );

            return false; // Suppress default drawing
        }
        #endregion

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            SOTSPlayer sotsPlayer = SOTSPlayer.ModPlayer(player);
            player.GetCritChance(DamageClass.Generic) += 25f;
            sotsPlayer.CritBonusDamage += 40;
            player.buffImmune[30] = true;
            player.buffImmune[20] = true;
            player.nightVision = true;

            sotsPlayer.CritLifesteal += 1 + (Utils.NextBool(Main.rand, 3) ? 1 : 0);
            sotsPlayer.CritVoidsteal += 1.25f;
            sotsPlayer.CritManasteal += 5 + Main.rand.Next(4);
            sotsPlayer.CritCurseFire = true;

            //Inferno Lord is handled in a global item
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out _))
            {
                tooltips.Add(new(Mod, "ItemInfo", Language.GetTextValue("Mods.InfernalEclipseAPI.Items.ExoSights.ThoriumTooltip")));
            }
            else
            {
                tooltips.Add(new(Mod, "ItemInfo", Language.GetTextValue("Mods.InfernalEclipseAPI.Items.ExoSights.DefaultTooltip")));
            }

            tooltips.Add(new TooltipLine(Mod, "Lore", Language.GetTextValue("Mods.InfernalEclipseAPI.Items.ExoSights.Lore")) { OverrideColor = Color.MediumPurple });

            tooltips.Add(new TooltipLine(Mod, "DedItem", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Contributor"))
            {
                OverrideColor = new Microsoft.Xna.Framework.Color(50, 205, 50)
            });
        }

        public override void AddRecipes()
        {
            Recipe exoSights = Recipe.Create(ModContent.ItemType<ExoSights>());

            exoSights.AddIngredient<BagOfCharms>();
            exoSights.AddIngredient<FocusReticle>();

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                exoSights.AddIngredient(thorium.Find<ModItem>("InfernoLordsFocus"));

            exoSights.AddIngredient<ExoPrism>(8);
            exoSights.AddTile<DraedonsForge>();
            exoSights.Register();
        }
    }
}
