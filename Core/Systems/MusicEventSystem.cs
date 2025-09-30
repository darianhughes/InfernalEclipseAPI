using System.Collections.Generic;
using System.IO;
using System.Threading;
using CalamityMod;
using CalamityMod.Events;
using Terraria.ModLoader.IO;
using InfernalEclipseAPI.Common;

namespace InfernalEclipseAPI.Core.Systems
{
    //This system was created by Nycro for the main Calamity Mod
    //If your a Calamity Developer and don't approve of this refactoring, I will remove it.
    public record class MusicEventEntry(string Id, int Song, TimeSpan Length, TimeSpan IntroSilence, TimeSpan OutroSilence, Func<bool> ShouldPlay, Func<bool> Enabled);
    public class MusicEventSystem : ModSystem
    {
        #region Statics

        public static MusicEventEntry CurrentEvent { get; set; } = null;

        public static DateTime? TrackStart { get; set; } = null;

        public static DateTime? TrackEnd { get; set; } = null;

        public static int LastPlayedEvent { get; set; } = -1;

        public static TimeSpan? OutroSilence { get; set; } = null;

        public static bool NoFade { get; set; } = false;

        public static Thread EventTrackerThread { get; set; } = null;

        public static List<string> PlayedEvents { get; set; } = [];

        public static List<MusicEventEntry> EventCollection { get; set; } = [];

        private static bool oldWorld { get; set; } = true;

        #endregion

        #region Events List

        public override void OnModLoad()
        {
            static void AddEntry(string eventId, string songName, TimeSpan length, Func<bool> shouldPlay, Func<bool> enabled, TimeSpan? introSilence = null, TimeSpan? outroSilence = null)
            {
                MusicEventEntry entry = new(eventId, MusicLoader.GetMusicSlot(songName), length, introSilence ?? TimeSpan.Zero, outroSilence ?? TimeSpan.Zero, shouldPlay, enabled);
                EventCollection.Add(entry);
            }

            // Interlude 4
            AddEntry("Interlude4", "InfernalEclipseAPI/Assets/Music/Interlude04", TimeSpan.FromSeconds(300.434), () => DownedBossSystem.downedExoMechs && DownedBossSystem.downedCalamitas, () => ModContent.GetInstance<InfernalConfig>().EnableInterlude4);
        }

        public override void Unload() => EventCollection.Clear();

        #endregion

        #region Event Handling

        public override void PostUpdateTime()
        {
            // If the Boss Rush is active, any would-be music events should be cancelled out and marked as played
            if (BossRushEvent.BossRushActive)
            {
                foreach (MusicEventEntry entry in EventCollection)
                {
                    if (entry.ShouldPlay())
                        PlayedEvents.Add(entry.Id);
                }

                TrackStart = null;
                LastPlayedEvent = -1;
                OutroSilence = null;

                TrackEnd = null;
                CurrentEvent = null;

                return;
            }

            // If the player has already completed conditions to trigger certain music events, we don't
            // want to queue a bunch of tracks to play as soon as they enter the world, so instead just mark them as played
            if (oldWorld)
            {
                foreach (MusicEventEntry entry in EventCollection)
                {
                    if (entry.ShouldPlay())
                        PlayedEvents.Add(entry.Id);
                }

                oldWorld = false;
            }

            // If the event has just finished, we want a little silence before fading back to normal
            if (TrackEnd is not null)
            {
                // `silence` is the time after a track ends before music goes back to normal
                TimeSpan silence = OutroSilence.Value;
                TimeSpan postTrack = DateTime.Now - TrackEnd.Value;

                // Play silence for the time specified
                if (postTrack < silence)
                {
                    int silenceSlot = MusicLoader.GetMusicSlot("CalamityMod/Sounds/Music/Silence");
                    Main.musicBox2 = silenceSlot;
                }

                else
                {
                    LastPlayedEvent = -1;
                    TrackEnd = null;
                    OutroSilence = null;
                }

                return;
            }

            // Only check for new events to play if none is currently playing
            // This makes sure events always finish before a new one starts
            if (CurrentEvent is null && MusicEventSystem.CurrentEvent is null)
            {
                foreach (MusicEventEntry musicEvent in EventCollection)
                {
                    // Make sure the event hasn't already played and SHOULD play
                    if (!PlayedEvents.Contains(musicEvent.Id) && musicEvent.ShouldPlay())
                    {
                        // Even if an event isn't marked as enabled, it should be counted
                        // as "played" so it isn't played when the player doesn't expect it
                        PlayedEvents.Add(musicEvent.Id);

                        // Events are always enabled on the server
                        if (Main.dedServ || musicEvent.Enabled())
                        {
                            // Assign the current event and start time
                            CurrentEvent = musicEvent;
                            TrackStart = DateTime.Now + musicEvent.IntroSilence;

                            // On clients, use a background thread to make sure the track always plays for exactly
                            // the specified length, regardless of if the game gets minimized, lags, or time becomes
                            // detangled from a consistent 60fps in any other way
                            if (!Main.dedServ)
                            {
                                EventTrackerThread = new(WatchMusicEvent);
                                EventTrackerThread.Start();
                            }

                            break;
                        }
                    }
                }
            }

            if (TrackStart is not null)
            {
                if (TrackStart > DateTime.Now)
                {
                    int silenceSlot = MusicLoader.GetMusicSlot("CalamityMod/Sounds/Music/Silence");
                    Main.musicBox2 = silenceSlot;
                    NoFade = true;
                }

                else
                {
                    Main.musicBox2 = CurrentEvent.Song;

                    if (NoFade)
                    {
                        Main.musicFade[CurrentEvent.Song] = 1f;
                        NoFade = false;
                    }

                    // If the event has finished playing, mark the end as now and clear the current event
                    if (DateTime.Now - TrackStart >= CurrentEvent.Length)
                    {
                        int silenceSlot = MusicLoader.GetMusicSlot("CalamityMod/Sounds/Music/Silence");
                        Main.musicBox2 = silenceSlot;
                        Main.musicFade[CurrentEvent.Song] = 0f;

                        TrackEnd = DateTime.Now;
                        LastPlayedEvent = CurrentEvent.Song;
                        OutroSilence = CurrentEvent.OutroSilence;

                        TrackStart = null;
                        CurrentEvent = null;
                    }
                }
            }
        }

        /// <summary>
        /// Watches for the game minimizing at any point, and adjusts the amount of time to play the song for accordingly
        /// </summary>
        public static void WatchMusicEvent()
        {
            DateTime? minimized = null;

            while (CurrentEvent is not null)
            {
                bool musicPaused = !Main.instance.IsActive;

                if (musicPaused && !minimized.HasValue)
                    minimized = DateTime.Now;

                else if (!musicPaused && minimized.HasValue)
                {
                    TrackStart += DateTime.Now - minimized.Value;
                    minimized = null;
                }
            }

            EventTrackerThread = null;
        }

        #endregion

        #region Event Saving

        public override void SaveWorldData(TagCompound tag)
        {
            tag["calamityPlayedMusicEventCount"] = PlayedEvents.Count;
            for (int i = 0; i < PlayedEvents.Count; i++)
                tag[$"calamityPlayedMusicEvent{i}"] = PlayedEvents[i];
        }

        public override void LoadWorldData(TagCompound tag)
        {
            PlayedEvents.Clear();

            if (tag.TryGet("calamityPlayedMusicEventCount", out int playedMusicEventCount))
            {
                for (int i = 0; i < playedMusicEventCount; i++)
                {
                    if (tag.TryGet($"calamityPlayedMusicEvent{i}", out string playedEvent))
                        PlayedEvents.Add(playedEvent);
                }
            }

            oldWorld = false;
        }

        public override void OnWorldUnload()
        {
            oldWorld = true;
            TrackStart = null;
            TrackEnd = null;
            CurrentEvent = null;
            PlayedEvents.Clear();
            NoFade = false;
            LastPlayedEvent = -1;
        }

        #endregion

        #region Event Syncing

        public static void SendSyncRequest()
        {
            ModPacket packet = InfernalEclipseAPI.Instance.GetPacket();
            packet.Write((byte)MusicNetcode.InfernalEclipseMusicMessageType.MusicEventSyncRequest);
            packet.Send();
        }

        public static void FulfillSyncRequest(int requester)
        {
            // Only fulfill requests as the server host
            if (!Main.dedServ)
                return;

            ModPacket packet = InfernalEclipseAPI.Instance.GetPacket();
            packet.Write((byte)MusicNetcode.InfernalEclipseMusicMessageType.MusicEventSyncResponse);

            int trackCount = PlayedEvents.Count;
            packet.Write(trackCount);

            for (int i = 0; i < trackCount; i++)
                packet.Write(PlayedEvents[i]);

            packet.Send(toClient: requester);
        }

        public static void ReceiveSyncResponse(BinaryReader reader)
        {
            // Only receive info on clients
            if (Main.dedServ)
                return;

            PlayedEvents.Clear();
            int trackCount = reader.ReadInt32();

            for (int i = 0; i < trackCount; i++)
                PlayedEvents.Add(reader.ReadString());
        }
        #endregion
    }

    public class MusicEventsPlayer : ModPlayer
    {
        public override void OnEnterWorld()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient && Player.whoAmI != Main.myPlayer)
            {
                MusicEventSystem.SendSyncRequest();
            }
        }
    }
}
