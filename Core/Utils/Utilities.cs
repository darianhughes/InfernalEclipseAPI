using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using CalamityMod;
using System.Collections.Generic;
using System.Reflection;
using Terraria.DataStructures;
using InfernumMode.Core.GlobalInstances.Systems;

namespace InfernalEclipseAPI.Core.Utils
{
    public static partial class InfernalUtilities
    {
        private static BlendState subtractiveBlending;
        private static RasterizerState cullOnlyScreen;
        public static Vector2 DirectionToSafe(this Entity entity, Vector2 destination)
        {
            return (destination - entity.Center).SafeNormalize(Vector2.Zero);
        }

        public static bool HasAccessoryEquipped<T>(Player player, bool includeVanity = false)
            where T : ModItem
        {
            return HasAccessoryEquipped(player, ModContent.ItemType<T>(), includeVanity);
        }

        public static bool HasAccessoryEquipped(Player player, int accessoryType, bool includeVanity = false)
        {
            // Functional accessory slots
            for (int i = 3; i < 10 + player.extraAccessorySlots; i++)
            {
                Item item = player.armor[i];

                if (!item.IsAir && item.type == accessoryType && item.accessory)
                    return true;
            }

            if (includeVanity)
            {
                // Vanity accessory slots (13–20 normally)
                for (int i = 13; i < 20 + player.extraAccessorySlots; i++)
                {
                    Item item = player.armor[i];

                    if (!item.IsAir && item.type == accessoryType && item.accessory)
                        return true;
                }
            }

            return false;
        }

        #region Math Utilities
        public static int INonZeroSign(this float x) => x >= 0f ? 1 : -1;

        public static int AngleToXDirection(float angle) => MathF.Cos(angle).INonZeroSign();

        public static float Saturate(float x)
        {
            if (x > 1f)
                return 1f;
            if (x < 0f)
                return 0f;
            return x;
        }

        public static float InverseLerp(float from, float to, float x, bool clamped = true)
        {
            float inverse = (x - from) / (to - from);
            if (!clamped)
                return inverse;

            return Saturate(inverse);
        }

        public static float InverseLerpBump(float start1, float start2, float end1, float end2, float x)
        {
            return InverseLerp(start1, start2, x) * InverseLerp(end2, end1, x);
        }
        #endregion

        #region Drawing Utilities
        public static void DrawAfterimagesCentered(Projectile proj, int mode, Color lightColor, int typeOneIncrement = 1, int? afterimageCountOverride = null, float minScale = 1f, float positionClumpInterpolant = 0f, Texture2D texture = null, bool drawCentered = true)
        {
            // Use the projectile's default texture if nothing is explicitly supplied.
            texture ??= TextureAssets.Projectile[proj.type].Value;

            // Calculate frame information for the projectile.
            int frameHeight = texture.Height / Main.projFrames[proj.type];
            int frameY = frameHeight * proj.frame;
            Rectangle rectangle = new(0, frameY, texture.Width, frameHeight);

            // Calculate the projectile's origin, rotation, and scale.
            Vector2 origin = rectangle.Size() * 0.5f;
            float rotation = proj.rotation;

            // Calculate the direction of the projectile as a SpriteEffects instance.
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (proj.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            // If no afterimages are drawn due to an invalid mode being specified, ensure the projectile itself is drawn anyway at the end of this method.
            bool failedToDrawAfterimages = false;

            // Determine whether afterimages should be drawn at all.
            Vector2 centerOffset = drawCentered ? proj.Size * 0.5f : Vector2.Zero;
            switch (mode)
            {
                // Standard afterimages. No customizable features other than total afterimage count.
                // Type 0 afterimages linearly scale down from 100% to 0% opacity. Their color and lighting is equal to the main projectile's.
                case 0:
                    int afterimageCount = afterimageCountOverride ?? proj.oldPos.Length;
                    for (int i = afterimageCount - 1; i >= 0; i--)
                    {
                        float scale = proj.scale * MathHelper.Lerp(1f, minScale, 1f - (afterimageCount - i) / (float)afterimageCount);
                        Vector2 drawPos = Vector2.Lerp(proj.oldPos[i] + centerOffset, proj.Center, positionClumpInterpolant) - Main.screenPosition + Vector2.UnitY * proj.gfxOffY;
                        Color color = proj.GetAlpha(lightColor) * ((proj.oldPos.Length - i) / (float)proj.oldPos.Length);
                        Main.spriteBatch.Draw(texture, drawPos, new Rectangle?(rectangle), color, rotation, origin, scale, spriteEffects, 0f);
                    }
                    break;

                // Paladin's Hammer style afterimages. Can be optionally spaced out further by using the typeOneDistanceMultiplier variable.
                // Type 1 afterimages linearly scale down from 66% to 0% opacity. They otherwise do not differ from type 0.
                case 1:
                    // Safety check: the loop must increment
                    int increment = Math.Max(1, typeOneIncrement);
                    Color drawColor = proj.GetAlpha(lightColor);
                    afterimageCount = afterimageCountOverride ?? ProjectileID.Sets.TrailCacheLength[proj.type];
                    int i2 = afterimageCount - 1;
                    while (i2 >= 0)
                    {
                        float scale = proj.scale * MathHelper.Lerp(1f, minScale, 1f - (afterimageCount - i2) / (float)afterimageCount);
                        Vector2 drawPos = Vector2.Lerp(proj.oldPos[i2] + centerOffset, proj.Center, positionClumpInterpolant) - Main.screenPosition + Vector2.UnitY * proj.gfxOffY;
                        if (i2 > 0)
                        {
                            float opacity = afterimageCount - i2;
                            drawColor *= opacity / (afterimageCount * 1.5f);
                        }
                        Main.spriteBatch.Draw(texture, drawPos, new Rectangle?(rectangle), drawColor, rotation, origin, scale, spriteEffects, 0f);
                        i2 -= increment;
                    }
                    break;

                // Standard afterimages with rotation. No customizable features other than total afterimage count.
                // Type 2 afterimages linearly scale down from 100% to 0% opacity. Their color and lighting is equal to the main projectile's.
                case 2:
                    afterimageCount = afterimageCountOverride ?? proj.oldPos.Length;
                    for (int i = afterimageCount - 1; i >= 0; i--)
                    {
                        float afterimageRot = proj.oldRot[i];
                        float scale = proj.scale * MathHelper.Lerp(1f, minScale, 1f - (afterimageCount - i) / (float)afterimageCount);
                        SpriteEffects sfxForThisAfterimage = proj.oldSpriteDirection[i] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                        Vector2 drawPos = Vector2.Lerp(proj.oldPos[i] + centerOffset, proj.Center, positionClumpInterpolant) - Main.screenPosition + Vector2.UnitY * proj.gfxOffY;
                        Color color = proj.GetAlpha(lightColor) * ((afterimageCount - i) / (float)afterimageCount);
                        Main.spriteBatch.Draw(texture, drawPos, new Rectangle?(rectangle), color, afterimageRot, origin, scale, sfxForThisAfterimage, 0f);
                    }
                    break;

                default:
                    failedToDrawAfterimages = true;
                    break;
            }

            // Draw the projectile itself. Only do this if no afterimages are drawn because afterimage 0 is the projectile itself.
            if (ProjectileID.Sets.TrailCacheLength[proj.type] <= 0 || failedToDrawAfterimages)
            {
                Vector2 startPos = drawCentered ? proj.Center : proj.position;
                Main.spriteBatch.Draw(texture, startPos - Main.screenPosition + new Vector2(0f, proj.gfxOffY), rectangle, proj.GetAlpha(lightColor), rotation, origin, proj.scale, spriteEffects, 0f);
            }
        }

        public static BlendState SubtractiveBlending
        {
            get
            {
                return subtractiveBlending ??= new()
                {
                    ColorSourceBlend = Blend.SourceAlpha,
                    ColorDestinationBlend = Blend.One,
                    ColorBlendFunction = BlendFunction.ReverseSubtract,
                    AlphaSourceBlend = Blend.SourceAlpha,
                    AlphaDestinationBlend = Blend.One,
                    AlphaBlendFunction = BlendFunction.ReverseSubtract
                };
            }
        }

        public static RasterizerState CullOnlyScreen
        {
            get
            {
                if (cullOnlyScreen is null)
                {
                    cullOnlyScreen = RasterizerState.CullNone;
                    cullOnlyScreen.ScissorTestEnable = true;
                }

                return cullOnlyScreen;
            }
        }
        #endregion

        #region Colors
        public static readonly Color InfernalRed = Color.Lerp(Color.White, new Color(255, 80, 0), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));
        #endregion

        #region Tooltip Utilities
        public static void AddTooltip(List<TooltipLine> tooltips, string newTooltip, Color overrideColor = default)
        {
            int maxTooltipIndex = -1;
            int maxNumber = -1;

            // Find the TooltipLine with the highest TooltipX name
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Mod == "Terraria" && tooltips[i].Name.StartsWith("Tooltip"))
                {
                    if (int.TryParse(tooltips[i].Name.Substring(7), out int num) && num > maxNumber)
                    {
                        maxNumber = num;
                        maxTooltipIndex = i;
                    }
                }
            }

            // If found, insert a new TooltipLine right after it with the desired color
            if (maxTooltipIndex != -1)
            {
                int insertIndex = maxTooltipIndex + 1;
                TooltipLine customLine = new TooltipLine(InfernalEclipseAPI.Instance, "IEoRTooltip", newTooltip);
                if (overrideColor != default)
                    customLine.OverrideColor = overrideColor;

                tooltips.Insert(insertIndex, customLine);
            }
        }

        public static void FullTooltipOveride(List<TooltipLine> tooltips, string newTooltip)
        {
            for (int index = 0; index < tooltips.Count; ++index)
            {
                if (tooltips[index].Mod == "Terraria")
                {
                    if (tooltips[index].Name == "Tooltip0")
                    {
                        TooltipLine tooltip = tooltips[index];
                        tooltip.Text = $"{newTooltip}";
                    }
                    else if (tooltips[index].Name.Contains("Tooltip"))
                    {
                        tooltips[index].Hide();
                    }
                }
            }
        }
        #endregion

        #region Get Difficulty States
        public static bool GetCalDifficulty(string diff)
        {
            return ModLoader.TryGetMod("CalamityMod", out Mod calamity) &&
                   calamity.Call("GetDifficultyActive", diff) is bool b && b;
        }
        public static bool IsInfernumActive()
        {
            return WorldSaveSystem.InfernumModeEnabled;
        }
        public static bool GetFargoDifficullty(string diff)
        {
            if (!ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSouls))
            {
                return false;
            }

            return fargoSouls.Call(diff) is bool active && active;
        }
        public static bool IsWorldLegendary()
        {
            FieldInfo findInfo = typeof(Main).GetField("_currentGameModeInfo", BindingFlags.Static | BindingFlags.NonPublic);
            GameModeData data = (GameModeData)findInfo.GetValue(null);
            return (Main.getGoodWorld && data.IsMasterMode);
        }
        #endregion
    }
}
