using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;

namespace InfernalEclipseAPI.YharimEX.Core.Systems
{
    public class YharimEXModIntegrationSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            MusicDisplaySetup();
            AddInfernumCards();
            BossChecklistSetup();
        }

        private void MusicDisplaySetup()
        {
            ModLoader.TryGetMod("MusicDisplay", out Mod musicDisplay);
            if (musicDisplay is null) return;

            musicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, "YharimEX/Assets/Music/TheRealityoftheProphecy"), "The Reality of the Prophecy", "by theforge129", "Infernal Eclipse of Ragnarok");
            musicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, "YharimEX/Assets/Music/Storia"), "Storia", "by Xi Vs Sakuzyo", "Infernal Eclipse of Ragnarok");
            musicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, "YharimEX/Assets/Music/StoriaShort"), "Storia (Desperation)", "by Xi Vs Sakuzyo", "Infernal Eclipse of Ragnarok");
            musicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, "YharimEX/Assets/Music/LegendsAboutTheGodseeker"), "Legends About The Godseeker", "TheTrester", "Infernal Eclipse of Ragnarok");
        }

        private void BossChecklistSetup()
        {
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist) || bossChecklist.Version < new Version(1, 6))
                return;

            ChecklistAddBoss(Mod, "YharimEXBoss", 27.9f, () => YharimEXWorldFlags.downedYharimEX, ModContent.NPCType<YharimEXBoss>());
        }

        private void ChecklistAddBoss(Mod mod, string internalName, float weight, Func<bool> downed, int bossType)
        {
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
                return;

            bossChecklist.Call(new object[7]
            {
                "LogBoss",
                mod,
                internalName,
                weight,
                downed,
                bossType,
                SpawnDictionaryBuilderSystem.GetDictionary(internalName, mod)
            });
        }

        internal void AddInfernumCards()
        {
            MakeCard(ModContent.NPCType<YharimEXBoss>(), (horz, anim) => Color.Lerp(Color.Red, Color.Gold, anim), "YharimMutant", SoundID.DD2_BetsyFireballShot, new SoundStyle("CalamityMod/Sounds/Custom/Scare"));
        }

        internal void MakeCard(int type, Func<float, float, Color> color, string title, SoundStyle tickSound, SoundStyle endSound, int time = 300, float size = 1f)
        {
            MakeCard(() => NPC.AnyNPCs(type), color, title, tickSound, endSound, time, size);
        }
        internal void MakeCard(Func<bool> condition, Func<float, float, Color> color, string title, SoundStyle tickSound, SoundStyle endSound, int time = 300, float size = 1f)
        {
            Mod Infernum = ModLoader.GetMod("InfernumMode");
            // Initialize the base instance for the intro card. Alternative effects may be added separately.
            Func<float, float, Color> textColorSelectionDelegate = color;
            object instance = Infernum.Call("InitializeIntroScreen", Mod.GetLocalization("InfernumIntegration." + title), time, true, condition, textColorSelectionDelegate);
            Infernum.Call("IntroScreenSetupLetterDisplayCompletionRatio", instance, new Func<int, float>(animationTimer => MathHelper.Clamp(animationTimer / (float)time * 1.36f, 0f, 1f)));

            // dnc but needed or else it errors
            Action onCompletionDelegate = () => { };
            Infernum.Call("IntroScreenSetupCompletionEffects", instance, onCompletionDelegate);

            // Letter addition sound.
            Func<SoundStyle> chooseLetterSoundDelegate = () => tickSound;
            Infernum.Call("IntroScreenSetupLetterAdditionSound", instance, chooseLetterSoundDelegate);

            // Main sound.
            Func<SoundStyle> chooseMainSoundDelegate = () => endSound;
            Func<int, int, float, float, bool> why = (_, _2, _3, _4) => true;
            Infernum.Call("IntroScreenSetupMainSound", instance, why, chooseMainSoundDelegate);

            // Text scale.
            Infernum.Call("IntroScreenSetupTextScale", instance, size);

            // Register the intro card.
            Infernum.Call("RegisterIntroScreen", instance);
        }
    }

    public class SpawnDictionaryBuilderSystem : ModSystem
    {
        public static Dictionary<string, object> GetDictionary(string InternalName, Mod mod)
        {
            List<int> intList1 = new List<int>();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            List<int> intList2 = new List<int>();
            bool flag = false;

            if (mod.Name == "YharimEX")
            {
                if (InternalName != null)
                {
                    if (InternalName == "YharimEXBoss")
                    {
                        Action<SpriteBatch, Rectangle, Color> action = (sb, rect, color) =>
                        {
                            Texture2D texture2D = ModContent.Request<Texture2D>("YharimEX/Assets/NPCs/YharimEXBossChecklist", (AssetRequestMode)2).Value;
                            Vector2 vector2;
                            vector2 = new Vector2(
                                rect.X + rect.Width / 2f - texture2D.Width / 2f,
                                rect.Y + rect.Height / 2f - texture2D.Height / 2f
                            );
                            sb.Draw(texture2D, vector2, color);
                        };
                        dictionary.Add("customPortrait", action);
                        dictionary.Add("displayName", Language.GetText("Mods.YharimEX.NPCs.YharimEXBoss.DisplayName"));
                        dictionary.Add("overrideHeadTextures", "YharimEX/Assets/NPCs/YharimEXBoss_Head");
                    }
                }
            }

            if (intList2.Count > 0)
            {
                if (intList2.Count == 1)
                    dictionary.Add("spawnItems", intList2[0]);
                else
                    dictionary.Add("spawnItems", intList2);
            }
            dictionary.Add("collectibles", intList1);
            if (!flag)
                dictionary.Add("spawnInfo", Language.GetText("Mods.YharimEX.SpawnInfo." + InternalName));

            return dictionary;
        }
    }
}
