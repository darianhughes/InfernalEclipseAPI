using InfernalEclipseAPI.YharimEX.Content.NPCs.Bosses;
using InfernalEclipseAPI.YharimEX.Core.Globals;
using InfernalEclipseAPI.YharimEX.Core.Systems;
using InfernumMode.Core.GlobalInstances.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.YharimEX.Content.Sky
{
    public class YharimEXSky : CustomSky
    {
        private bool isActive = false;
        private float intensity = 0f;
        private float lifeIntensity = 0f;
        private float specialColorLerp = 0f;
        private Color? specialColor = null;
        private int delay = 0;
        private readonly int[] xPos = new int[50];
        private readonly int[] yPos = new int[50];

        public override void Update(GameTime gameTime)
        {
            const float increment = 0.01f;

            bool useSpecialColor = false;

            if (YharimEXUtils.BossIsAlive(ref YharimEXGlobalNPC.yharimEXBoss, ModContent.NPCType<YharimEXBoss>())
                && (Main.npc[YharimEXGlobalNPC.yharimEXBoss].ai[0] < 0 || Main.npc[YharimEXGlobalNPC.yharimEXBoss].ai[0] >= 10))
            {
                intensity += increment;
                lifeIntensity = Main.npc[YharimEXGlobalNPC.yharimEXBoss].ai[0] < 0 ? 1f : 1f - (float)Main.npc[YharimEXGlobalNPC.yharimEXBoss].life / Main.npc[YharimEXGlobalNPC.yharimEXBoss].lifeMax;

                void ChangeColorIfDefault(Color color) //waits for bg to return to default first
                {
                    if (specialColor == null)
                        specialColor = color;
                    if (specialColor != null && specialColor == color)
                        useSpecialColor = true;
                }

                switch ((int)Main.npc[YharimEXGlobalNPC.yharimEXBoss].ai[0])
                {
                    case -5:
                        if (Main.npc[YharimEXGlobalNPC.yharimEXBoss].ai[2] >= 420)
                            ChangeColorIfDefault(new Color(255, 180, 50));
                        break;

                    case 10: //p2 transition, smash to black
                        useSpecialColor = true;
                        specialColor = Color.Black;
                        specialColorLerp = 1f;
                        break;

                    case 27: //twins
                        ChangeColorIfDefault(Color.Red);
                        break;

                    case 36: //slime rain
                        if (WorldSaveSystem.InfernumModeEnabled && Main.npc[YharimEXGlobalNPC.yharimEXBoss].ai[2] > 180 * 3 - 60)
                            ChangeColorIfDefault(Color.Blue);
                        break;

                    case 44: //empress
                        ChangeColorIfDefault(Color.DeepPink);
                        break;

                    case 48: //queen slime
                        ChangeColorIfDefault(Color.Purple);
                        break;

                    default:
                        break;
                }

                if (intensity > 1f)
                    intensity = 1f;
            }
            else
            {
                lifeIntensity -= increment;
                if (lifeIntensity < 0f)
                    lifeIntensity = 0f;

                specialColorLerp -= increment * 2;
                if (specialColorLerp < 0)
                    specialColorLerp = 0;

                intensity -= increment;
                if (intensity < 0f)
                {
                    intensity = 0f;
                    lifeIntensity = 0f;
                    specialColorLerp = 0f;
                    specialColor = null;
                    delay = 0;
                    Deactivate();
                    return;
                }
            }

            if (useSpecialColor)
            {
                specialColorLerp += increment * 2;
                if (specialColorLerp > 1)
                    specialColorLerp = 1;
            }
            else
            {
                specialColorLerp -= increment * 2;
                if (specialColorLerp < 0)
                {
                    specialColorLerp = 0;
                    specialColor = null;
                }
            }
        }

        private Color ColorToUse(ref float opacity)
        {
            Color color = Color.OrangeRed;
            opacity = intensity * 0.5f + lifeIntensity * 0.5f;

            if (specialColorLerp > 0 && specialColor != null)
            {
                color = Color.Lerp(color, (Color)specialColor, specialColorLerp);
                if (specialColor == Color.Black)
                    opacity = Math.Min(1f, opacity + Math.Min(intensity, lifeIntensity) * 0.5f);
            }

            return color;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0 && minDepth < 0)
            {
                float opacity = 0f;
                Color color = ColorToUse(ref opacity);

                spriteBatch.Draw(ModContent.Request<Texture2D>("InfernalEclipseAPI/YharimEX/Assets/Sky/YharimEXBossSky", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                    new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color * opacity);

                if (--delay < 0)
                {
                    delay = Main.rand.Next(5 + (int)(85f * (1f - lifeIntensity)));
                    for (int i = 0; i < 50; i++) //update positions
                    {
                        xPos[i] = Main.rand.Next(Main.screenWidth);
                        yPos[i] = Main.rand.Next(Main.screenHeight);
                    }
                }

                for (int i = 0; i < 50; i++) //static on screen
                {
                    int width = Main.rand.Next(3, 251);
                    spriteBatch.Draw(ModContent.Request<Texture2D>("InfernalEclipseAPI/YharimEX/Assets/Sky/YharimEXBossStatic", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                    new Rectangle(xPos[i] - width / 2, yPos[i], width, 3),
                    color * lifeIntensity * 0.75f);
                }
            }
        }

        public override float GetCloudAlpha()
        {
            return 1f - intensity;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }

        public override void Reset()
        {
            isActive = false;
        }

        public override bool IsActive()
        {
            return isActive;
        }

        public override Color OnTileColor(Color inColor)
        {
            float dummy = 0f;
            Color skyColor = Color.Lerp(Color.White, ColorToUse(ref dummy), 0.5f);
            return Color.Lerp(skyColor, inColor, 1f - intensity);
        }
    }
}