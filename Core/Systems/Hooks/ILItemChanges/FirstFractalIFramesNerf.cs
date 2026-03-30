using System.Reflection;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges
{
    public class FirstFractalIFramesNerf : ModSystem
    {
        private Hook hook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("YouBoss", out Mod youBoss))
                return;

            Type firstFractalType = youBoss.Code.GetType("YouBoss.Content.Items.ItemReworks.FirstFractal");
            MethodInfo getter = firstFractalType?
                .GetProperty("PlayerPostHitIFrameGracePeriod", BindingFlags.Public | BindingFlags.Static)?
                .GetGetMethod();

            if (getter is not null)
                hook = new Hook(getter, OverridePlayerPostHitIFrames);
        }

        public override void Unload()
        {
            hook?.Dispose();
            hook = null;
        }

        private static int OverridePlayerPostHitIFrames(Func<int> orig)
        {
            return 12;
        }
    }
}
