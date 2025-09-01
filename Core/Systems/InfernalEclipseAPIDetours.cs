using Luminance.Core.Hooking;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using InfernalEclipseAPI.Core.Players;

namespace InfernalEclipseAPI.Core.Systems
{
    //Credit: Fargo's Souls Team
    public class InfernalEclipseAPIDetours : ICustomDetourProvider
    {
        public void LoadDetours()
        {
            On_Main.DrawInterface_35_YouDied += DrawInterface_35_YouDied;
        }

        public void UnloadDetours()
        {
            On_Main.DrawInterface_35_YouDied -= DrawInterface_35_YouDied;
        }

        void ICustomDetourProvider.ModifyMethods()
        {
        }

        private static string GetDiffText()
        {
            Difficulty diff = InfernalConfig.Instance.MinimumDifficultyToPreventRespawns;
            string basePath = "Mods.InfernalEclipseAPI.UI.";
            switch (diff)
            {
                case Difficulty.Expert:
                    return Language.GetTextValue(basePath + "Expert");
                case Difficulty.Master:
                    return Language.GetTextValue(basePath + "Master");
                case Difficulty.Legendary:
                    return Language.GetTextValue(basePath + "Legendary");
                case Difficulty.Revengence:
                    return Language.GetTextValue(basePath + "Revengence");
                case Difficulty.MasterRevengence:
                    return Language.GetTextValue(basePath + "MRevengence");
                case Difficulty.LegendaryRevengence:
                    return Language.GetTextValue(basePath + "LRevengence");
                case Difficulty.Death:
                    return Language.GetTextValue(basePath + "Death");
                case Difficulty.MasterDeath:
                    return Language.GetTextValue(basePath + "MDeath");
                case Difficulty.LegendaryDeath:
                    return Language.GetTextValue(basePath + "LDeath");
                case Difficulty.Infernum:
                    return Language.GetTextValue(basePath + "Infernum");
                case Difficulty.MasterInfernum:
                    return Language.GetTextValue(basePath + "MInfernum");
                case Difficulty.LegendaryInfernum:
                    return Language.GetTextValue(basePath + "LInfernum");
                default:
                    return "the current difficulty in";
            }
        }

        public static void DrawInterface_35_YouDied(On_Main.orig_DrawInterface_35_YouDied orig)
        {
            orig();
            if (Main.LocalPlayer.dead && Main.LocalPlayer.GetModPlayer<RespawnPlayer>().PreventRespawn() && Main.netMode != NetmodeID.SinglePlayer)
            {
                float num = -60f;
                string value = Lang.inter[38].Value;
                if (Main.LocalPlayer.lostCoins > 0)
                {
                    num += 50f;
                }
                num += (float)((Main.LocalPlayer.lostCoins > 0) ? 24 : 50);
                num += 20f;
                float num2 = 0.7f;

                // draw you can't respawn text
                num += 60;
                num2 = 0.5f;

                string text = "";
                int respawnsAllowed = InfernalConfig.Instance.MultiplayerRespawnsAllowed;
                Difficulty diff = InfernalConfig.Instance.MinimumDifficultyToPreventRespawns;

                if (diff == Difficulty.AlwaysOn)
                {
                    if (respawnsAllowed == 0)
                    {
                        text = Language.GetTextValue("Mods.InfernalEclipseAPI.UI.NoRespawn4");
                    }
                    else if (respawnsAllowed == 1)
                    {
                        text = Language.GetTextValue("Mods.InfernalEclipseAPI.UI.NoRespawn5");
                    }
                    else
                    {
                        text = Language.GetTextValue("Mods.InfernalEclipseAPI.UI.NoRespawn6", respawnsAllowed);
                    }
                }
                else 
                {
                    if (respawnsAllowed == 0)
                    {
                        text = Language.GetTextValue("Mods.InfernalEclipseAPI.UI.NoRespawn3", GetDiffText());
                    }
                    else if (respawnsAllowed == 1)
                    {
                        text = Language.GetTextValue("Mods.InfernalEclipseAPI.UI.NoRespawn", GetDiffText());
                    }
                    else
                    {
                        text = Language.GetTextValue("Mods.InfernalEclipseAPI.UI.NoRespawn2", respawnsAllowed, GetDiffText());
                    }
                }

                DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.DeathText.Value, text,
                    new Vector2((float)(Main.screenWidth / 2) - FontAssets.DeathText.Value.MeasureString(text).X * num2 / 2, (float)(Main.screenHeight / 2) + num),
                    Main.LocalPlayer.GetDeathAlpha(Microsoft.Xna.Framework.Color.Transparent), 0f, default, num2, SpriteEffects.None, 0f);
            }
        }
    }
}
