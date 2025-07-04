using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;

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
        }

        public override void OnWorldLoad()
        {
            ResetFlags();
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
        }
    }
}
