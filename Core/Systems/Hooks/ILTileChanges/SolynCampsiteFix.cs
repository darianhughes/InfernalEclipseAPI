using System.Reflection;
using InfernalEclipseAPI.Core.Configs;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILTileChanges
{
    [JITWhenModsEnabled("NoxusBoss")]
    [ExtendsFromMod("NoxusBoss")]
    public class SolynCampsiteFix : ModSystem
    {
        internal static ILHook? SurveyHook;

        public override void Load() 
        {
            if (InfernalConfig.Instance.SolynCampsiteFixes || !ModLoader.HasMod("WOTGCampsiteFix"))
                TryApplyPatch();
        }   

        public override void Unload()
        {
            SurveyHook?.Dispose();
            SurveyHook = null;
        }

        public override void PostSetupContent()
        {
            if (InfernalConfig.Instance.SolynCampsiteFixes || !ModLoader.HasMod("WOTGCampsiteFix"))
                TryApplyPatch();
        }

        internal void TryApplyPatch()
        {
            if (!ModLoader.TryGetMod("NoxusBoss", out Mod noxusBoss))
            {
                InfernalEclipseAPI.Instance.Logger.Info("[IEoR]: NoxusBoss not found — patch not applied.");
                return;
            }

            if (SurveyHook != null)
            {
                try
                {
                    SurveyHook.Dispose();
                }
                catch
                {
                }

                SurveyHook = null;
            }

            MethodInfo patchTarget = FindPatchTarget(noxusBoss);

            if (patchTarget is null)
            {
                InfernalEclipseAPI.Instance.Logger.Error("[IEoR]: Could not find a patchable WOTG campsite method. Patch not applied.");
                return;
            }

            try
            {
                SurveyHook = new ILHook(patchTarget, PatchSolidTileCalls);

                InfernalEclipseAPI.Instance.Logger.Info($"[IEoR]: Patched {patchTarget.DeclaringType?.Name}.{patchTarget.Name} — flagpole crash protection active.");
            }
            catch (Exception ex)
            {
                InfernalEclipseAPI.Instance.Logger.Error("[IEoR]: ILHook failed: " + ex.Message);
            }
        }

        private static MethodInfo? FindPatchTarget(Mod noxusBoss)
        {
            Type? surveyType = noxusBoss.Code?.GetType("NoxusBoss.Core.World.WorldGeneration.SolynCampsiteSurvey");

            if (surveyType is not null)
            {
                foreach (MethodInfo method in surveyType.GetMethods(LumUtils.UniversalBindingFlags))
                {
                    if (method.Name == "TryToFindSpotInRange")
                        return method;
                }
            }

            return noxusBoss.Code?.GetType("NoxusBoss.Core.World.WorldGeneration.SolynCampsiteWorldGen")?.GetMethod("FlatTerrainExists", LumUtils.UniversalBindingFlags);
        }

        private static void PatchSolidTileCalls(ILContext il)
        {
            ILCursor cursor = new(il);

            MethodInfo? solidTile3 = typeof(WorldGen).GetMethod(nameof(WorldGen.SolidTile), LumUtils.UniversalBindingFlags, null,
                new[]
                {
                    typeof(int),
                    typeof(int),
                    typeof(bool)
                },
                null
            );

            MethodInfo? solidTile2 = typeof(WorldGen).GetMethod(nameof(WorldGen.SolidTile), LumUtils.UniversalBindingFlags,
                null,
                new[]
                {
                    typeof(int),
                    typeof(int)
                },
                null
            );

            if (solidTile3 is not null)
            {
                cursor.Index = 0;

                while (cursor.TryGotoNext(MoveType.Before, i => i.MatchCall(solidTile3)))
                {
                    cursor.Remove();
                    cursor.EmitDelegate<Func<int, int, bool, bool>>(SafeSolidTile3);
                }
            }

            if (solidTile2 is not null)
            {
                cursor.Index = 0;

                while (cursor.TryGotoNext(MoveType.Before, i => i.MatchCall(solidTile2)))
                {
                    cursor.Remove();
                    cursor.EmitDelegate<Func<int, int, bool>>(SafeSolidTile2);
                }
            }
        }

        private static bool SafeSolidTile3(int i, int j, bool noDoors)
        {
            return i >= 0 && j >= 0 && i < Main.maxTilesX && j < Main.maxTilesY && WorldGen.SolidTile(i, j, noDoors);
        }

        private static bool SafeSolidTile2(int i, int j)
        {
            return i >= 0 && j >= 0 && i < Main.maxTilesX && j < Main.maxTilesY && WorldGen.SolidTile(i, j, false);
        }

    }
}
