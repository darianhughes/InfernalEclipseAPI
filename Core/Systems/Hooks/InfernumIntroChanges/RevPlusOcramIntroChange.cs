using System.Reflection;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks.InfernumIntroChanges
{
    public class RevPlusOcramIntroChange : ModSystem
    {
        private static Hook hook;

        public override void Load()
        {
            if (!ModLoader.HasMod("RevengeancePlus"))
                return;

            var asm = ModLoader.GetMod("RevengeancePlus").Code;
            var type = asm.GetType("RevengeancePlus.InfernumTitles");
            var method = type?.GetMethod(
                "AddInfernumTitle",
                BindingFlags.Static | BindingFlags.NonPublic
            );

            if (method != null)
                hook = new Hook(method, AddInfernumTitle_Detour);
        }

        public override void Unload()
        {
            hook?.Dispose();
            hook = null;
        }

        private static void AddInfernumTitle_Detour(Action<ModNPC, Color[], float[], Color, float> orig, ModNPC mnpc, Color[] titleColors, float[] healthGates, Color screenOverlay, float fontSize)
        {
            if (mnpc?.Name == "Ocram")
                screenOverlay = Color.Multiply(Color.BlueViolet, 0.1f);

            orig(mnpc, titleColors, healthGates, screenOverlay, fontSize);
        }
    }
}
