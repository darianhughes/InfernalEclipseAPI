using Terraria.ModLoader.IO;
using System.IO;
using CalamityMod.UI;
using InfernalEclipseAPI.Core.Systems;
using InfernumMode.Core.GlobalInstances.Systems;
using CalamityMod.Events;
using InfernalEclipseAPI.Core.Systems.BossRush;
using Terraria.GameContent.Events;

namespace InfernalEclipseAPI.Core.World
{
    public class InfernalWorld : ModSystem
    {
        public static bool dreadonDestroyerDialoguePlayed = false;
        public static bool dreadonDestroyer2DialoguePlayed = false;
        public static bool jungleSubshockPlanteraDialoguePlayed = false;
        public static bool jungleSlagspitterPlateraDiaglougePlayer = false;
        public static bool sulfurScourgeDialoguePlayed = false;
        public static bool brimstoneDialoguePlayed = false;
        public static bool yharonDischarge = false;
        public static bool yharonSmasher = false;
        public static bool namelessDeveloperDiagloguePlayed = false;
        public static bool craftedWorkshop = false;
        public static bool RagnarokModeEnabled;
        public static bool hasChosenDifficulty = false;

        public static void ResetFlags()
        {
            dreadonDestroyerDialoguePlayed = false;
            dreadonDestroyer2DialoguePlayed = false;
            jungleSubshockPlanteraDialoguePlayed = false;
            jungleSlagspitterPlateraDiaglougePlayer=false;
            sulfurScourgeDialoguePlayed =false;
            brimstoneDialoguePlayed =false;
            yharonDischarge =false;
            yharonSmasher=false;
            namelessDeveloperDiagloguePlayed = false;
            craftedWorkshop = false;
            RagnarokModeEnabled = false;
            hasChosenDifficulty = false;
        }

        public override void PreUpdateWorld()
        {
            BirthdayParty.GenuineParty = true;
            if (!BirthdayParty.ManualParty)
            {
                BirthdayParty.ToggleManualParty();
            }

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

            if (SubworldLibrary.SubworldSystem.AnyActive())
            {
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
            tag["dreadonDestroyerDialoguePlayed"] = dreadonDestroyerDialoguePlayed;
            tag["dreadonDestroyer2DialoguePlayed"] = dreadonDestroyer2DialoguePlayed;
            tag["jungleSubshockPlanteraDialoguePlayed"] = jungleSubshockPlanteraDialoguePlayed;
            tag["jungleSlagspitterPlateraDiaglougePlayer"] = jungleSlagspitterPlateraDiaglougePlayer;
            tag["sulfurScourgeDialoguePlayed"] = sulfurScourgeDialoguePlayed;
            tag["brimstoneDialoguePlayed"] = brimstoneDialoguePlayed;
            tag["yharonDischarge"] = yharonDischarge;
            tag["yharonSmasher"] = yharonSmasher;
            tag["namelessDeveloperDiagloguePlayed"] = namelessDeveloperDiagloguePlayed;
            tag["craftedWorkshop"] = craftedWorkshop;
            tag["RagnarokModeEnabled"] = RagnarokModeEnabled;
            tag["hasChosenDifficulty"] = hasChosenDifficulty;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            GetData(ref dreadonDestroyerDialoguePlayed, "dreadonDestroyerDialoguePlayed", tag);
            GetData(ref dreadonDestroyer2DialoguePlayed, "dreadonDestroyerDialoguePlayed", tag);
            GetData(ref jungleSubshockPlanteraDialoguePlayed, "junglePlanteraDialoguePlayed", tag);
            GetData(ref jungleSlagspitterPlateraDiaglougePlayer, "jungleSlagspitterPlateraDiaglougePlayer", tag);
            GetData(ref sulfurScourgeDialoguePlayed, "sulfurScourgeDialoguePlayed", tag);
            GetData(ref brimstoneDialoguePlayed, "brimstoneDialoguePlayed", tag);
            GetData(ref yharonDischarge, "yharonDischarge", tag);
            GetData(ref yharonSmasher, "yharonSmasher", tag);
            GetData(ref namelessDeveloperDiagloguePlayed, "namelessDeveloperDiagloguePlayed", tag);
            GetData(ref craftedWorkshop, "craftedWorkshop", tag);
            GetData(ref hasChosenDifficulty, "hasChosenDifficulty", tag);

            if (tag.TryGet("RagnarokModeEnabled", out bool value))
                RagnarokModeEnabled = value;
            else
                RagnarokModeEnabled = false;
        }

        public static void GetData(ref bool baseVar, string path, TagCompound tag)
        {
            if (tag.ContainsKey(path)) { baseVar = tag.Get<bool>(path); }
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(dreadonDestroyerDialoguePlayed);
            writer.Write(dreadonDestroyer2DialoguePlayed);
            writer.Write(jungleSubshockPlanteraDialoguePlayed);
            writer.Write(jungleSlagspitterPlateraDiaglougePlayer);
            writer.Write(sulfurScourgeDialoguePlayed);
            writer.Write(brimstoneDialoguePlayed);
            writer.Write(yharonDischarge);
            writer.Write(yharonSmasher);
            writer.Write(namelessDeveloperDiagloguePlayed);
            writer.Write(craftedWorkshop);
            writer.Write(RagnarokModeEnabled);
            writer.Write(hasChosenDifficulty);
        }

        public override void NetReceive(BinaryReader reader)
        {
            dreadonDestroyerDialoguePlayed = reader.ReadBoolean();
            dreadonDestroyer2DialoguePlayed = reader.ReadBoolean();
            jungleSubshockPlanteraDialoguePlayed =reader.ReadBoolean();
            jungleSlagspitterPlateraDiaglougePlayer = reader.ReadBoolean();
            sulfurScourgeDialoguePlayed = reader.ReadBoolean();
            brimstoneDialoguePlayed = reader.ReadBoolean();
            yharonSmasher = reader.ReadBoolean();
            yharonDischarge = reader.ReadBoolean();
            namelessDeveloperDiagloguePlayed = reader.ReadBoolean();
            craftedWorkshop = reader.ReadBoolean();
            RagnarokModeEnabled = reader.ReadBoolean();
            hasChosenDifficulty = reader.ReadBoolean();
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
