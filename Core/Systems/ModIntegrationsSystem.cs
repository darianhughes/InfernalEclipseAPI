using CalamityMod.Items.Placeables.FurnitureAuric;
using CalamityMod.NPCs.CalClone;
using InfernalEclipseAPI.Content.Items.Placeables;
using InfernumMode.Content.Items.SummonItems;
using Microsoft.Build.Exceptions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems
{
    public class ModIntegrationsSystem : ModSystem
    {
        internal static Mod Infernum;
        internal static Mod SOTS;
        public override void Load()
        {
            ModLoader.TryGetMod("InfernumMode", out Infernum);
            ModLoader.TryGetMod("SOTS", out SOTS);
        }
        public override void Unload()
        {
            Infernum = null;
        }

        public override void PostSetupContent()
        {
            MusicDisplaySetup();
            BossChecklistSetup();
            AddInfernumCards();
        }
        private void MusicDisplaySetup()
        {
            ModLoader.TryGetMod("MusicDisplay", out Mod musicDisplay);
            if (musicDisplay is null)
            {
                return;
            }

            musicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot("InfernalEclipseAPI/Assets/Music/tier6"), "Descent Of Divinities", "psykomatic", "Infernal Eclipse of Ragnarok");
            musicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot("InfernalEclipseAPI/Assets/Music/tier5"), "Omiscience Of Gods", "TheTrester", "Infernal Eclipse of Ragnarok");
            musicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot("InfernalEclipseAPI/Assets/Music/TWISTEDGARDENRemix"), "TWISTED GARDEN [Remix]", "Kuudray", "Infernal Eclipse of Ragnarok");
            musicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot("InfernalEclipseAPI/Assets/Music/EnsembleofFools(EncoreMix)"), "Ensemble of Fools (Encore Mix)", "CDMusic", "Infernal Eclipse of Ragnarok");
            musicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot("InfernalEclipseAPI/Assets/Music/CatastrophicFabrications"), "Catastrophic Fabrications", "by PinpinNeon", "Infernum Mode Music");
        }

        private void BossChecklistSetup()
        {
            //if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
            //    return;

            //// Create the defeat condition delegate
            //Func<bool> downedDreadnautilus = () => InfernalDownedBossSystem.downedDreadNautilus;

            //bossChecklist.Call(
            //    "AddBoss",
            //    Mod, // your mod instance
            //    "InfernalDreadnautilus", // unique internal name
            //    new List<int> { NPCID.BloodNautilus }, // NPCs representing the boss
            //    ModContent.Request<Texture2D>("InfernalEclipseAPI/Assets/Textures/BossChecklist/DreadnautilusIcon").Value, // icon path
            //    7.9f, // progression position
            //    downedDreadnautilus, // defeat condition
            //    //null, 1
            //    ModContent.ItemType<RedBait>(), // summon item (null for vanilla spawn methods)
            //    "Fish in the Ocean at night during a Blood Moon", // spawn description
            //    "The Dreadnautilus flees beneath the crimson tides...", // despawn text
            //    null // optional loot list
            //);

            Mod mod1;
            if (!ModLoader.TryGetMod("BossChecklist", out mod1) || mod1.Version < new Version(1, 6))
                return;
            this.ChecklistAddPseudoMiniboss(((ModType)this).Mod, "Dreadnautilus", 7.9f, (Func<bool>)(() => InfernalDownedBossSystem.downedDreadNautilus), 618);

            mod1.Call(new object[3]
            {
            (object) "AddToBossCollection",
            (object) "CalamityMod Exo Mechs",
            (object) new List<int>()
                {
                    ModContent.ItemType<CatastrophicFabricationsMusicBox>()
                }
            });
        }

        public void ChecklistAddPseudoMiniboss(Mod mod, string internalName, float weight, Func<bool> downed, int bossType)
        {
            Mod mod1;
            if (!ModLoader.TryGetMod("BossChecklist", out mod1))
                return;
            mod1.Call(new object[7]
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
            if (Infernum is null) return;

            if (SOTS is null) return;
            MakeCard(SOTS.Find<ModNPC>("Polaris").Type, (horz, anim) => Color.Lerp(Color.Aquamarine, Color.Red, anim), "Polaris", SoundID.NPCHit4, new SoundStyle("InfernumMode/Assets/Sounds/Custom/ExoMechs/ThanatosTransition"));
            MakeCard(SOTS.Find<ModNPC>("NewPolaris").Type, (horz, anim) => Color.Lerp(Color.Aquamarine, Color.Red, anim), "NewPolaris", SoundID.NPCHit4, new SoundStyle("InfernumMode/Assets/Sounds/Custom/ExoMechs/ThanatosTransition"));
        }
        internal void MakeCard(int type, Func<float, float, Color> color, string title, SoundStyle tickSound, SoundStyle endSound, int time = 300, float size = 1f)
        {
            MakeCard(() => NPC.AnyNPCs(type), color, title, tickSound, endSound, time, size);
        }
        internal void MakeCard(Func<bool> condition, Func<float, float, Color> color, string title, SoundStyle tickSound, SoundStyle endSound, int time = 300, float size = 1f)
        {
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
            if (mod.Name == "InfernalEclipseAPI")
            {
                if (InternalName != null)
                { 
                    if (InternalName == "Dreadnautilus")
                    {
                        Action<SpriteBatch, Rectangle, Color> action = (Action<SpriteBatch, Rectangle, Color>)((sb, rect, color) =>
                        {
                            Texture2D texture2D = ModContent.Request<Texture2D>("InfernalEclipseAPI/Assets/Textures/BossChecklist/Dreadnautilus", (AssetRequestMode)2).Value;
                            Vector2 vector2;
                            // ISSUE: explicit constructor call
                            vector2 = new Vector2(
                                rect.X + rect.Width / 2f - texture2D.Width / 2f,
                                rect.Y + rect.Height / 2f - texture2D.Height / 2f
                            );
                            sb.Draw(texture2D, vector2, color);
                        });
                        dictionary.Add("customPortrait", action);
                        dictionary.Add("displayName", Language.GetText("NPCName.BloodNautilus"));
                        dictionary.Add("overrideHeadTextures", "InfernalEclipseAPI/Assets/Textures/BossChecklist/DreadnautilusIcon");       
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
                dictionary.Add("spawnInfo", Language.GetText("Mods.InfernalEclipseAPI.SpawnInfo." + InternalName));
            return dictionary;
        }
    }
}
