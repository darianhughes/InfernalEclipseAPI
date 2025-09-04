using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ModLoader;
using YouBoss.Content.NPCs.Bosses.TerraBlade.SpecificEffectManagers;
using YouBoss.Content.NPCs.Bosses.TerraBlade;
using CalamityMod.Events;
using YouBoss.Common.Utilities;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes
{
    [ExtendsFromMod("YouBoss")]
    public class YouBossModSceneFix : ModSystem
    {
        private static Hook _isSceneEffectActiveHook;

        public override void Load()
        {
            MethodInfo targetMethod = typeof(TerraBladeSkyScene).GetMethod(
                nameof(TerraBladeSkyScene.IsSceneEffectActive),
                BindingFlags.Instance | BindingFlags.Public);

            _isSceneEffectActiveHook = new Hook(targetMethod, IsSceneEffectActive_Detour);
        }

        public override void Unload()
        {
            _isSceneEffectActiveHook?.Dispose();
            _isSceneEffectActiveHook = null;
        }

        // Completely overrides original logic
        private static bool IsSceneEffectActive_Detour(Func<TerraBladeSkyScene, Player, bool> orig, TerraBladeSkyScene self, Player player)
        {
            bool bossActive = false;
            if (TerraBladeBoss.Myself != null)
            {
                var tbBoss = TerraBladeBoss.Myself.As<TerraBladeBoss>();
                if (tbBoss != null && !tbBoss.PerformingStartAnimation)
                    bossActive = true;
            }
            return bossActive || BossRushEvent.BossRushActive;
        }
    }
}
