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

namespace InfernalEclipseAPI.Core
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
            }
        }
    }
}
