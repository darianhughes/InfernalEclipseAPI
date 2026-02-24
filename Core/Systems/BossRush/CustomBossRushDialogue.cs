using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Events;

namespace InfernalEclipseAPI.Core.Systems.BossRush
{
    public enum IEoRBossRushDialoguePhase : short
    {
        None = 0,

        TierFiveComplete = 1
    }
    public class CustomBossRushDialogue : ModSystem
    {
        public static bool GottaGoFast = false;
        public static int GottaGoFastSpeed = 5;

        public static IEoRBossRushDialoguePhase Phase = IEoRBossRushDialoguePhase.None;
        private static BossRushDialogueEvent[] currentSequence = null;
        public static int currentSequenceIndex = 0;

        public static int CurrentDialogueDelay = 0;

        internal struct BossRushDialogueEvent
        {
            private const int DefaultFrameDelay = 180;

            internal int FrameDelay;
            internal string LocalizationKey;
            internal Func<bool> skipCondition;

            public BossRushDialogueEvent()
            {
                FrameDelay = DefaultFrameDelay;
                LocalizationKey = null;
                skipCondition = null;
            }
            public BossRushDialogueEvent(string key)
            {
                LocalizationKey = key;
                FrameDelay = DefaultFrameDelay;
                skipCondition = null;
            }
            public BossRushDialogueEvent(string key, int delay = DefaultFrameDelay, Func<bool> skipFunc = null)
            {
                LocalizationKey = key;
                FrameDelay = delay;
                skipCondition = skipFunc;
            }

            public readonly bool ShouldDisplay()
            {
                if (skipCondition is null)
                    return true;
                return !skipCondition.Invoke();
            }
        }

        internal static Dictionary<IEoRBossRushDialoguePhase, BossRushDialogueEvent[]> IEoRBossRushDialogue;

        public override void Load()
        {
            BossRushDialogueEvent[] tierFiveDialogues = new BossRushDialogueEvent[]
            {
                new ("Mods.InfernalEclipseAPI.Events.BossRushTierFiveEndText_1", LumUtils.SecondsToFrames(2.40f) - 30),
                new ("Mods.InfernalEclipseAPI.Events.BossRushTierFiveEndText_2", LumUtils.SecondsToFrames(5.45f) - 30),
                new ("Mods.InfernalEclipseAPI.Events.BossRushTierFiveEndText_3", LumUtils.SecondsToFrames(5.03f) - 30)
            };

            IEoRBossRushDialogue = new Dictionary<IEoRBossRushDialoguePhase, BossRushDialogueEvent[]>()
            {
                { IEoRBossRushDialoguePhase.TierFiveComplete, tierFiveDialogues }
            };
        }

        public override void Unload()
        {
            IEoRBossRushDialogue = null;
        }

        public static void StartDialogue(IEoRBossRushDialoguePhase phaseToRun)
        {
            Phase = phaseToRun;
            bool validDialogueFound = IEoRBossRushDialogue.TryGetValue(Phase, out var dialogueListToUse);
            if (validDialogueFound)
            {
                currentSequence = dialogueListToUse;
                currentSequenceIndex = 0;
            }

            CurrentDialogueDelay = 4;
        }

        internal static void Tick()
        {
            // If the phase isn't defined properly, don't do anything.
            if (Phase == IEoRBossRushDialoguePhase.None)
                return;

            if (currentSequenceIndex < currentSequence.Length)
            {
                // If it's time to display dialogue, do so.
                if (CurrentDialogueDelay == 0 && currentSequenceIndex < currentSequence.Length)
                {
                    // Skip over all lines that should be skipped to find the first one that should not be skipped.
                    bool hasMoreDialogue = GetNextUnskippedDialogue(currentSequence, currentSequenceIndex, out int currentIndex);
                    if (hasMoreDialogue)
                    {
                        BossRushDialogueEvent line = currentSequence[currentSequenceIndex];

                        // Display dialogue and set appropriate delay, if this dialogue shouldn't be skipped.
                        if (line.skipCondition is null || !line.skipCondition.Invoke())
                        {
                            CalamityUtils.BroadcastLocalizedText(line.LocalizationKey, BossRushEvent.XerocTextColor);
                            CurrentDialogueDelay = line.FrameDelay;
                        }

                        // Move onto the next dialogue line.
                        currentSequenceIndex = currentIndex + 1;
                    }
                }
                // Otherwise, decrement the existing delay.
                else
                    --CurrentDialogueDelay;

                // Ensure a boss does not attack the player while they are reading dialogue.
                // Indefinitely stall the countdown.
                if (BossRushEvent.BossRushSpawnCountdown < 180)
                    BossRushEvent.BossRushSpawnCountdown = CurrentDialogueDelay + 180;

                // Gotta Go Fast Mode
                if (GottaGoFast && CurrentDialogueDelay > GottaGoFastSpeed)
                    CurrentDialogueDelay = GottaGoFastSpeed;
            }
            else
            {
                CurrentDialogueDelay = 0;
            }

            // If the end of a sequence has been reached, stay in this state indefinitely.
            // Allow the boss spawn countdown to hit zero and the next boss to appear without showing any dialogue or causing any delays.

            // However, if Boss Rush is not occurring, reset all variables.
            if (!BossRushEvent.BossRushActive)
            {
                Phase = IEoRBossRushDialoguePhase.None;
                currentSequence = null;
                currentSequenceIndex = 0;
                CurrentDialogueDelay = 0;
            }
        }

        private static bool GetNextUnskippedDialogue(BossRushDialogueEvent[] sequence, int index, out int newIndex)
        {
            int tryIndex = index;
            while (tryIndex < sequence.Length)
            {
                BossRushDialogueEvent lineToTry = currentSequence[tryIndex];
                if (lineToTry.skipCondition is not null && lineToTry.skipCondition.Invoke())
                {
                    ++tryIndex;
                    continue;
                }

                newIndex = tryIndex;
                return true;
            }

            newIndex = -1;
            return false;
        }
    }
}
