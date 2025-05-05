using Terraria.ModLoader;
using MonoMod.Cil;
using System;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour;
using CalamityMod.UI.ModeIndicator;
using Terraria.Localization;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Core
{
    public class ForceModeSystemLock : ModSystem
    {
        public class ForceDifficultyMenuLockSystem : ModSystem
        {
            private static readonly FieldInfo menuOpenField = typeof(ModeIndicatorUI).GetField("menuOpen", BindingFlags.NonPublic | BindingFlags.Static);
            private static readonly FieldInfo menuOpenTransitionTimeField = typeof(ModeIndicatorUI).GetField("menuOpenTransitionTime", BindingFlags.NonPublic | BindingFlags.Static);
            private static readonly FieldInfo previousLockStatusField = typeof(ModeIndicatorUI).GetField("previousLockStatus", BindingFlags.NonPublic | BindingFlags.Static);
            private static readonly FieldInfo lockClickTimeField = typeof(ModeIndicatorUI).GetField("lockClickTime", BindingFlags.NonPublic | BindingFlags.Static);

            public override void PostUpdateInput()
            {
                if (!InfernalConfig.Instance.InfernumModeForced)
                    return;

                // Step 1: Always close the menu
                menuOpenField?.SetValue(null, false);
                menuOpenTransitionTimeField?.SetValue(null, 0);

                // Step 2: Force the lock icon to be visible
                previousLockStatusField?.SetValue(null, true);

                // Optional: animate the lock icon (shake effect)
                int currentClickTime = (int)(lockClickTimeField?.GetValue(null) ?? 0);
                if (currentClickTime <= 0)
                    lockClickTimeField?.SetValue(null, 15); // restart some shake
            }

            private static int customLockClickTime = 0;
            private const int LockAnimLength = 30;
            //Redraw the lock so we don't have to deal with IL calls
            public override void PostDrawInterface(SpriteBatch spriteBatch)
            {
                if (!InfernalConfig.Instance.InfernumModeForced || !Main.playerInventory)
                    return;

                // Check for hover & click
                Rectangle iconHitbox = new(Main.screenWidth - 400, 82, 30, 38); // Approximate
                if (iconHitbox.Contains(Main.mouseX, Main.mouseY) && Main.mouseLeft && Main.mouseLeftRelease)
                {
                    customLockClickTime = LockAnimLength;
                }

                // Calculate shake scale
                float shakeProgress = 1f;
                if (customLockClickTime > 0)
                {
                    float t = 1f - customLockClickTime / (float)LockAnimLength;
                    shakeProgress = 1f + (float)System.Math.Sin(t * MathHelper.TwoPi * 2f) * 0.5f * (1f - t);
                    customLockClickTime--;
                }

                Texture2D lockTexture = ModContent.Request<Texture2D>("CalamityMod/UI/ModeIndicator/ModeIndicatorLock").Value;
                Vector2 drawCenter = new Vector2(Main.screenWidth - 400f, 82f) + new Vector2(15f, 19f);
                Vector2 offset = Vector2.UnitY * 12f;
                float scale = 1f * shakeProgress;

                spriteBatch.Draw(
                    lockTexture,
                    drawCenter + offset,
                    null,
                    Color.White,
                    0f,
                    lockTexture.Size() * 0.5f,
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
