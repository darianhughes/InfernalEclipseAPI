using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Items.Placeables;
using InfernalEclipseAPI.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems
{
    public class MusicBoxRegistry : ModSystem
    {
        public override void PostSetupContent()
        {
            if (!Main.dedServ)
            {
                Mod thisMod = InfernalEclipseAPI.Instance;
                if (ModLoader.TryGetMod("CalamityModMusic", out Mod musicMod))
                {
                    int boxId = musicMod.Find<ModItem>("BossRushTier5MusicBox").Type;
                    int boxTileID = musicMod.Find<ModTile>("BossRushTier5MusicBox").Type;
                    int musicID = MusicLoader.GetMusicSlot(thisMod, "Assets/Music/tier5");
                    MusicLoader.AddMusicBox(thisMod, musicID, boxId, boxTileID);
                }

                int t6boxID = ModContent.ItemType<BossRushTier6MusicBox>();
                int t6BoxTileID = ModContent.TileType<BossRushTier6MusicBoxTile>();
                int t6musicID = MusicLoader.GetMusicSlot(thisMod, "Assets/Music/tier6");
                MusicLoader.AddMusicBox(thisMod, t6musicID, t6boxID, t6BoxTileID);

                int t42boxID = ModContent.ItemType<BossRushTierNamelessMusicBox>();
                int t42BoxTileID = ModContent.TileType<BossRushTierNamelessMusicBoxTile>();
                int t42musicID = MusicLoader.GetMusicSlot(thisMod, "Assets/Music/TWISTEDGARDENRemix");
                MusicLoader.AddMusicBox(thisMod, t42musicID, t42boxID, t42BoxTileID);

                int encoreBoxID = ModContent.ItemType<BossRushEncoreMusicBox>();
                int encoreBoxTileID = ModContent.TileType<BossRushEncoreMusicBoxTile>();
                int encoreMusicID = MusicLoader.GetMusicSlot(thisMod, "Assets/Music/EnsembleofFools(EncoreMix)");
                MusicLoader.AddMusicBox(thisMod, encoreMusicID, encoreBoxID, encoreBoxTileID);
            }
        }
    }
}
