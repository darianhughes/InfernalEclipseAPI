using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.ModSceneEffects.CameraEffects
{
    public class DreamEaterCameraSystem : ModSystem
    {
        public static int FocusNpcId = -1;
        public static int FocusTimer = 0;

        private static Vector2 _smooth;
        private static float _lerpStrength = 0.12f;

        // New: allow choosing whether to start from current screen (pan) or snap to target
        public static void StartFocus(int npcId, int frames, bool smoothStart = false, float lerpOverride = -1f)
        {
            if (Main.netMode == NetmodeID.Server) return;

            FocusNpcId = npcId;
            FocusTimer = frames;

            if (lerpOverride > 0f)
                _lerpStrength = lerpOverride;

            if (smoothStart)
            {
                _smooth = Main.screenPosition; // start from where the camera currently is -> pan
            }
            else if (npcId >= 0 && npcId < Main.maxNPCs && Main.npc[npcId].active)
            {
                // original snap behavior
                _smooth = Main.npc[npcId].Center - new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
            }
        }

        public override void ModifyScreenPosition()
        {
            if (FocusTimer <= 0) return;
            if (FocusNpcId < 0 || FocusNpcId >= Main.maxNPCs) { FocusTimer = 0; return; }

            NPC n = Main.npc[FocusNpcId];
            if (!n.active) { FocusTimer = 0; return; }

            Vector2 desired = n.Center - new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;

            desired.X = MathHelper.Clamp(desired.X, 0, Main.maxTilesX * 16 - Main.screenWidth);
            desired.Y = MathHelper.Clamp(desired.Y, 0, Main.maxTilesY * 16 - Main.screenHeight);

            _smooth = Vector2.Lerp(_smooth, desired, _lerpStrength);
            Main.screenPosition = _smooth;
        }

        public override void PostUpdateEverything()
        {
            if (FocusTimer > 0) FocusTimer--;
        }
    }
}
