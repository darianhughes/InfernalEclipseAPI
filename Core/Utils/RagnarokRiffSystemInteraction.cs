using InfernalEclipseAPI.Core.Players.ThoriumPlayerOverrides.ThoriumMulticlassNerf;
using RagnarokMod.Utils;
using Terraria.Audio;

namespace InfernalEclipseAPI.Core.Utils
{
    [JITWhenModsEnabled("RagnarokMod")]
    [ExtendsFromMod("RagnarokMod")]
    public class RagnarokRiffSystemInteraction
    {
        public static void ClearRiffs(Player player)
        {
            RagnarokModPlayer mp = player.GetModPlayer<RagnarokModPlayer>();

            if (SoundEngine.TryGetActiveSound(mp.riffSlot, out var sound))
                sound.Stop();
            mp.riffPlaying = false;
            mp.activeRiffType = 0;
            mp.riffItemType = -1;

            ThoriumHelpers.ClearAllEmpowerments(player);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Mod mod = InfernalEclipseAPI.Instance;

                ModPacket p = mod.GetPacket();
                p.Write((byte)InfernalEclipseMessageType.ThoriumEmpowerment);
                p.Write((byte)ThoriumEmpowermentMsg.ClearEmpowerments);
                p.Write((byte)player.whoAmI);
                p.Send();
            }
        }
    }
}
