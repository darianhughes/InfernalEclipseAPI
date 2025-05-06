using InfernumMode.Content.Items.SummonItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoxusBoss.Core.CrossCompatibility.Inbound.BossChecklist;
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
        public override void PostSetupContent()
        {
            MusicDisplaySetup();
            BossChecklistSetup();
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
                        dictionary.Add("customPortrait", (object)action);
                        dictionary.Add("displayName", (object)Language.GetText("NPCName.BloodNautilus"));
                        dictionary.Add("overrideHeadTextures", (object)"InfernalEclipseAPI/Assets/Textures/BossChecklist/Dreadnautilus");       
                    }
                }
            }
            if (intList2.Count > 0)
            {
                if (intList2.Count == 1)
                    dictionary.Add("spawnItems", (object)intList2[0]);
                else
                    dictionary.Add("spawnItems", (object)intList2);
            }
            dictionary.Add("collectibles", (object)intList1);
            if (!flag)
                dictionary.Add("spawnInfo", (object)Language.GetText("Mods.InfernalEclipseAPI.SpawnInfo." + InternalName));
            return dictionary;
        }
    }
}
