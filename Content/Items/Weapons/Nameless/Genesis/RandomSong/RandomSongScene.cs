using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Weapons.Nameless.Genesis.RandomSong
{
    public class RandomSongScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
         => player.GetModPlayer<RandomSongPlayer>().IsActive;

        public override SceneEffectPriority Priority => SceneEffectPriority.Event;

        public override int Music => Main.LocalPlayer.GetModPlayer<RandomSongPlayer>().forcedMusic;
    }
}
