using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Weapons.Nameless.Genesis.RandomSong
{
    public class RandomSongPlayer : ModPlayer
    {
        public int forcedMusic = -1;
        public int forcedMusicTimer;

        private static readonly int[] Pool = new int[] {
        MusicID.OverworldDay, MusicID.Night, MusicID.DayRemix,
        MusicID.Underground, MusicID.AltUnderground,
        MusicID.Desert, MusicID.UndergroundDesert, MusicID.Snow,
        MusicID.WindyDay, MusicID.Rain, MusicID.Monsoon, MusicID.MorningRain,
        MusicID.Jungle, MusicID.JungleNight, MusicID.JungleUnderground,
        MusicID.Corruption, MusicID.Crimson, MusicID.TheHallow,
        MusicID.UndergroundCorruption, MusicID.UndergroundHallow,
        MusicID.Dungeon, MusicID.Mushrooms, MusicID.Ocean, MusicID.OceanNight,
        MusicID.Space, MusicID.SpaceDay, MusicID.Hell, MusicID.Temple,
        MusicID.Eclipse, MusicID.Sandstorm,
        MusicID.GoblinInvasion, MusicID.PirateInvasion, MusicID.MartianMadness, MusicID.SlimeRain,
        MusicID.TheTowers, MusicID.LunarBoss,
        MusicID.Boss1, MusicID.Boss2, MusicID.Boss3, MusicID.Boss4, MusicID.Boss5,
        MusicID.Plantera, MusicID.EmpressOfLight, MusicID.QueenSlime, MusicID.DukeFishron,
        MusicID.Graveyard
    };

        public void EnsureRandomSongPlaying()
        {
            if (forcedMusicTimer <= 0 || forcedMusic < 0)
                forcedMusic = Pool[Main.rand.Next(Pool.Length)];
            forcedMusicTimer = 2; // refresh every tick while holding
        }

        public void StopForcedSong() { forcedMusic = -1; forcedMusicTimer = 0; }

        public override void PostUpdate()
        {
            if (forcedMusicTimer > 0 && --forcedMusicTimer == 0)
                forcedMusic = -1;
        }

        public bool IsActive => forcedMusicTimer > 0 && forcedMusic >= 0;
    }
}
