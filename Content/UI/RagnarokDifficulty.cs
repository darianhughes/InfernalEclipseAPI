using System.Collections.Generic;
using CalamityMod.Systems;
using CalamityMod.World;
using InfernalEclipseAPI.Core.Systems;
using InfernalEclipseAPI.Core.World;
using InfernumMode.Content.UI;
using InfernumMode.Core.GlobalInstances.Systems;
using InfernumMode.Core.Netcode;
using InfernumMode.Core.Netcode.Packets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using static CalamityMod.Systems.DifficultyModeSystem;
using static Terraria.GameContent.Creative.CreativePowers;

namespace InfernalEclipseAPI.Content.UI
{
    public class RagnarokDifficulty : DifficultyMode
    {
        public override Asset<Texture2D> OutlineTexture
        {
            get
            {
                _outlineTexture ??= ModContent.Request<Texture2D>("InfernalEclipseAPI/Assets/ExtraTextures/UI/RagnarokIcon_Outline");

                return _outlineTexture;
            }
        }
        public override LocalizedText Name => Language.GetText("Mods.InfernalEclipseAPI.DifficultyUI.Name");
        public override Color ChatTextColor => new Color(196, 62, 0);
        public override SoundStyle ActivationSound => new("InfernalEclipseAPI/Assets/Sounds/NamelessDeityRageFail");
        public override int BackBoneGameModeID => GameModeID.Master;
        public override LocalizedText ShortDescription => Language.GetText("Mods.InfernalEclipseAPI.DifficultyUI.ShortDescription");
        public override Asset<Texture2D> TextureDisabled
        {
            get
            {
                _textureDisabled ??= ModContent.Request<Texture2D>("InfernalEclipseAPI/Assets/ExtraTextures/UI/RagnarokIcon_Off");

                return _textureDisabled;
            }
        }
        public override float DifficultyScale => 0.25f;

        public override bool Enabled
        {
            get => InfernalWorld.RagnarokModeEnabled;
            set
            {
                if (value)
                {
                    CalamityWorld.revenge = true;
                    CalamityWorld.death = true;
                    WorldSaveSystem.InfernumModeEnabled = true;
                }

                if (Main.getGoodWorld)
                {
                    if (!Main.GameModeInfo.IsJourneyMode)
                    {
                        Main.GameMode = value == true ? GameModeID.Expert : GameModeID.Normal;
                    }
                    else
                    {
                        AlignJourneyDifficultySlider();
                    }
                    InfernalWorld.RagnarokModeEnabled = value;
                }
                else
                {
                    InfernalWorld.RagnarokModeEnabled = value;
                    if (value && !Main.GameModeInfo.IsJourneyMode)
                        Main.GameMode = BackBoneGameModeID;
                }

                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    var netMessage = InfernalEclipseAPI.Instance.GetPacket();
                    netMessage.Write((byte)InfernalEclipseMessageType.SyncRagnarokState);
                    netMessage.Write(InfernalWorld.RagnarokModeEnabled);
                    netMessage.Send();

                    PacketManager.SendPacket<InfernumModeActivityPacket>();

                    NetMessage.SendData(MessageID.WorldData); //extra safety
                }

                /* old implementation stuff
                if (value)
                    {
                        WorldSaveSystem.InfernumModeEnabled = true;
                        CalamityWorld.revenge = true;

                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            if (Main.GameModeInfo.IsJourneyMode)
                            {
                                float journeyDiff = 1f;
                                var slider = CreativePowerManager.Instance.GetPower<DifficultySliderPower>();
                                typeof(DifficultySliderPower).GetMethod("SetValueKeyboardForced", LumUtils.UniversalBindingFlags).Invoke(slider, [journeyDiff]);
                            }
                            else if (Main.GameMode != GameModeID.Master)
                            {
                                Main.GameMode = GameModeID.Master;

                                Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.DifficultyUI.MasterToggle"), new Color(175, 75, 255));
                            }
                        }
                        else
                        {
                            var netMessage = InfernalEclipseAPI.Instance.GetPacket();
                            netMessage.Write((byte)InfernalEclipseMessageType.ToggleRagnarok);
                            netMessage.Write((byte)Main.LocalPlayer.whoAmI);
                            netMessage.Send();
                        }
                    }
                */
            }
        }

        public override Asset<Texture2D> Texture
        {
            get
            {
                _texture ??= ModContent.Request<Texture2D>("InfernalEclipseAPI/Assets/ExtraTextures/UI/RagnarokIcon");

                return _texture;
            }
        }

        public override LocalizedText ExpandedDescription => Language.GetText("Mods.InfernalEclipseAPI.DifficultyUI.ExpandedDescription");

        public override int[] FavoredDifficultyAtTier(int tier)
        {
            DifficultyMode[] difficultyTier = DifficultyTiers[tier];
            List<int> intList = new List<int>();
            for (int index = 0; index < difficultyTier.Length; ++index)
            {
                if (difficultyTier[index] is MasterDifficulty || difficultyTier[index] is DeathDifficulty)
                    intList.Add(index);
            }
            if (intList.Count <= 0)
                intList.Add(0);
            return intList.ToArray();
        }

        public override bool IsBasedOn(DifficultyMode mode)
        {
            return mode is InfernumDifficulty
                || mode is DeathDifficulty
                || mode is MasterDifficulty
                || mode is RevengeanceDifficulty;
        }
    }
}
