using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.SOTSItemHooks
{
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    public class HarmonyBlacklistHook : ModSystem
    {
        private static ILHook _hook;

        private static bool ShouldEarlyReturn(Player player)
        {
            if (InfernalCrossmod.Thorium.Loaded)
            {
                Mod thor = InfernalCrossmod.Thorium.Mod;
                
                int[] thorBlacklist =
                [
                    thor.Find<ModBuff>("ScytheofUndoingBuff").Type
                ];

                for (int i = 0; i < player.buffType.Length; i++)
                {
                    int buffType = player.buffType[i];
                    if (buffType <= 0)
                        continue;

                    for (int j = 0; j < thorBlacklist.Length; j++)
                    {
                        if (buffType == thorBlacklist[j])
                            return true;
                    }
                }
            }

            return false;
        }

        public override void Load()
        {
            if (!ModLoader.HasMod("SOTS"))
                return;

            try
            {
                Assembly sotsAsm = ModLoader.GetMod("SOTS").Code;

                Type sotsPlayerType = sotsAsm.GetType("SOTS.SOTSPlayer", throwOnError: false);
                if (sotsPlayerType is null)
                    return;

                MethodInfo target = sotsPlayerType.GetMethod(
                    "IncreaseBuffDurations",
                    BindingFlags.Public | BindingFlags.Static,
                    binder: null,
                    types: new[]
                    {
                        typeof(Terraria.Player),
                        typeof(int),
                        typeof(float),
                        typeof(int),
                        typeof(bool),
                        typeof(bool),
                        typeof(bool),
                    },
                    modifiers: null);

                if (target is null)
                    return;

                _hook = new ILHook(target, InjectEarlyReturn);
            }
            catch
            {
                _hook = null;
            }
        }

        public override void Unload()
        {
            _hook?.Dispose();
            _hook = null;
        }

        private static void InjectEarlyReturn(ILContext il)
        {
            var c = new ILCursor(il);
            c.Goto(0);

            Instruction continueOriginal = c.Next;

            c.Emit(OpCodes.Ldarg_0);

            c.Emit(OpCodes.Call, typeof(HarmonyBlacklistHook)
                .GetMethod(nameof(ShouldEarlyReturn), BindingFlags.NonPublic | BindingFlags.Static));

            c.Emit(OpCodes.Brfalse_S, continueOriginal);

            c.Emit(OpCodes.Ret);
        }
    }
}
