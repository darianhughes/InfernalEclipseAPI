using System.Reflection;
using InfernalEclipseAPI.Content.Buffs;
using MonoMod.RuntimeDetour;
using SOTS.Items.Void;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.SOTSItemHooks
{
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    public class VoidSicknessDetour : ModSystem
    {
        private static Hook onConsumeHook;

        public override void Load()
        {
            if (!InfernalCrossmod.SOTS.Loaded)
                return;

            MethodInfo m = typeof(VoidConsumable).GetMethod("OnConsumeItem", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (m is null)
                throw new MissingMethodException("SOTS.Items.Void.VoidConsumable.OnConsumeItem(Player) not found.");

            onConsumeHook = new Hook(m, OnConsumeItem_Detour);
        }

        public override void Unload()
        {
            onConsumeHook?.Dispose();
            onConsumeHook = null;
        }

        private static void OnConsumeItem_Detour(Action<VoidConsumable, Player> orig, VoidConsumable self, Player player)
        {
            orig(self, player);

            if (player?.active == true)
                player.AddBuff(ModContent.BuffType<VoidSickness2>(), 300);
        }
    }
}
