using Terraria.ModLoader.IO;
using System.IO;
using CalamityMod.UI;
using InfernalEclipseAPI.Core.Systems;
using InfernumMode.Core.GlobalInstances.Systems;
using CalamityMod.Events;
using InfernalEclipseAPI.Core.Systems.BossRush;
using SubworldLibrary;
using InfernalEclipseAPI.Content.Items.Other;

namespace InfernalEclipseAPI.Core.World
{
    public class InfernalWorld : ModSystem
    {
        public static bool sulfurScourgeDialoguePlayed = false;
        public static bool brimstoneDialoguePlayed = false;
        public static bool namelessDeveloperDiagloguePlayed = false;
        public static bool craftedWorkshop = false;
        public static bool RagnarokModeEnabled;
        public static bool hasChosenDifficulty = false;
        public static bool tier5Downed = false;
        public static bool tier6Downed = false;

        public static int dreamEaterAttempts = 0;

        public static void ResetFlags()
        {
            sulfurScourgeDialoguePlayed =false;
            brimstoneDialoguePlayed =false;
            namelessDeveloperDiagloguePlayed = false;
            craftedWorkshop = false;
            RagnarokModeEnabled = false;
            hasChosenDifficulty = false;
            tier5Downed = false;
            tier6Downed = false;

            dreamEaterAttempts = 0;
        }

        public override void PreUpdateWorld()
        {
            if (InfernalConfig.Instance.ThereIsNoReasonDisableThis && (WorldSaveSystem.InfernumModeEnabled || RagnarokModeEnabled))
            {
                if (Main.getGoodWorld && (RagnarokModeEnabled || WorldSaveSystem.InfernumModeEnabled))
                {
                    RagnarokModeEnabled = false;
                    WorldSaveSystem.InfernumModeEnabled = false;
                }

                if (InfernalCrossmod.FargosSouls.Loaded)
                {
                    if (FargoWorldFlagAdjustments.IsEmodeOrMasoActive())
                        FargoWorldFlagAdjustments.UnsetEmode();
                }
            }

            if (RagnarokModeEnabled && !WorldSaveSystem.InfernumModeEnabled)
                WorldSaveSystem.InfernumModeEnabled = true;

            if (SubworldSystem.AnyActive())
            {
                if (InfernalConfig.Instance.ForceRagnarokInfernumModeInSubworlds && !WorldSaveSystem.InfernumModeEnabled)
                {
                    if (Main.masterMode)
                        RagnarokModeEnabled = true;
                    else if (Main.expertMode)
                        WorldSaveSystem.InfernumModeEnabled = true;
                }

                if (InfernalCrossmod.SOTS.Loaded)
                {
                    foreach (Projectile projectile in Main.projectile)
                    {
                        if (projectile.type == InfernalCrossmod.SOTS.Mod.Find<ModProjectile>("VoidAnomaly").Type)
                            projectile.active = false;
                    }
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.type == InfernalCrossmod.SOTS.Mod.Find<ModNPC>("Archaeologist").Type)
                            npc.active = false;
                    }
                }
            }

            if (BossRushEvent.BossRushActive)
                CustomBossRushDialogue.Tick();
        }

        public override void OnWorldLoad()
        {
            ResetFlags();

            if (InfernalCrossmod.SOTS.Loaded)
            {
                int advisorType = InfernalCrossmod.SOTS.Mod.Find<ModNPC>("TheAdvisorHead").Type;
                if (!BossHealthBarManager.BossExclusionList.Contains(advisorType))
                    BossHealthBarManager.BossExclusionList.Add(advisorType);
            }
        }

        public override void OnWorldUnload()
        {
            ResetFlags();
        }

        public override void SaveWorldData(TagCompound tag)
        {
            var infernalDowned = new System.Collections.Generic.List<string>();

            InfernalRecipeUnlockHandler.Save(infernalDowned);

            tag["sulfurScourgeDialoguePlayed"] = sulfurScourgeDialoguePlayed;
            tag["brimstoneDialoguePlayed"] = brimstoneDialoguePlayed;
            tag["namelessDeveloperDiagloguePlayed"] = namelessDeveloperDiagloguePlayed;
            tag["craftedWorkshop"] = craftedWorkshop;
            tag["RagnarokModeEnabled"] = RagnarokModeEnabled;
            tag["hasChosenDifficulty"] = hasChosenDifficulty;
            tag["tier5Downed"] = tier5Downed;
            tag["tier6Downed"] = tier6Downed;
            tag["dreamEaterAttempts"] = dreamEaterAttempts;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            var infernalDowned = tag.GetList<string>("infernalDowned");

            InfernalRecipeUnlockHandler.Load(infernalDowned);

            GetData(ref sulfurScourgeDialoguePlayed, "sulfurScourgeDialoguePlayed", tag);
            GetData(ref brimstoneDialoguePlayed, "brimstoneDialoguePlayed", tag);
            GetData(ref namelessDeveloperDiagloguePlayed, "namelessDeveloperDiagloguePlayed", tag);
            GetData(ref craftedWorkshop, "craftedWorkshop", tag);
            GetData(ref hasChosenDifficulty, "hasChosenDifficulty", tag);
            GetData(ref tier5Downed, "tier5Downed", tag);
            GetData(ref tier6Downed, "tier6Downed", tag);

            if (tag.TryGet("RagnarokModeEnabled", out bool value))
                RagnarokModeEnabled = value;
            else
                RagnarokModeEnabled = false;

            if (tag.TryGet("dreamEaterAttempts", out int attempts))
                dreamEaterAttempts = attempts;
            else
                dreamEaterAttempts = 0;
        }

        public static void GetData(ref bool baseVar, string path, TagCompound tag)
        {
            if (tag.ContainsKey(path)) { baseVar = tag.Get<bool>(path); }
        }

        public override void NetSend(BinaryWriter writer)
        {
            InfernalRecipeUnlockHandler.SendData(writer);

            writer.Write(sulfurScourgeDialoguePlayed);
            writer.Write(brimstoneDialoguePlayed);
            writer.Write(namelessDeveloperDiagloguePlayed);
            writer.Write(craftedWorkshop);
            writer.Write(RagnarokModeEnabled);
            writer.Write(hasChosenDifficulty);
            writer.Write(tier5Downed);
            writer.Write(tier6Downed);
            writer.Write(dreamEaterAttempts);
        }

        public override void NetReceive(BinaryReader reader)
        {
            InfernalRecipeUnlockHandler.ReceiveData(reader);

            sulfurScourgeDialoguePlayed = reader.ReadBoolean();
            brimstoneDialoguePlayed = reader.ReadBoolean();
            namelessDeveloperDiagloguePlayed = reader.ReadBoolean();
            craftedWorkshop = reader.ReadBoolean();
            RagnarokModeEnabled = reader.ReadBoolean();
            hasChosenDifficulty = reader.ReadBoolean();
            tier5Downed = reader.ReadBoolean();
            tier6Downed = reader.ReadBoolean();
            dreamEaterAttempts = reader.ReadInt32();
        }
    }

    [JITWhenModsEnabled(InfernalCrossmod.FargosSouls.Name)]
    [ExtendsFromMod(InfernalCrossmod.FargosSouls.Name)]
    public static class FargoWorldFlagAdjustments
    {
        public static bool IsEmodeOrMasoActive() => FargowiltasSouls.Core.Systems.WorldSavingSystem.EternityMode || FargowiltasSouls.Core.Systems.WorldSavingSystem.MasochistModeReal;

        public static void UnsetEmode()
        {
            FargowiltasSouls.Core.Systems.WorldSavingSystem.EternityMode = false;
            FargowiltasSouls.Core.Systems.WorldSavingSystem.MasochistModeReal = false;
        }
    }
}
