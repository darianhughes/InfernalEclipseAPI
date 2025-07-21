using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfernumMode.Content.BehaviorOverrides.BossAIs.AquaticScourge;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.AquaticScourge
{
    //public class AquaticScourgeHeadBehaviourOverrideFixes : ModSystem
    //{
    //    private static Hook preAIHook;
    //    private static MethodInfo orig_PreAI;

    //    public override void Load()
    //    {
    //        Type type = typeof(AquaticScourgeHeadBehaviorOverride);
    //        orig_PreAI = type.GetMethod("PreAI", BindingFlags.Instance | BindingFlags.Public);
    //        preAIHook = new Hook(orig_PreAI, new Func<object, NPC, bool>(PreAI_Hook));
    //    }

    //    public override void Unload()
    //    {
    //        preAIHook?.Dispose();
    //        preAIHook = null;
    //        orig_PreAI = null;
    //    }

    //    private static bool PreAI_Hook(object self, NPC npc)
    //    {
    //        // Call original method
    //        bool result = (bool)orig_PreAI.Invoke(self, new object[] { npc });

    //        // Injected logic
    //        for (int i = 0; i < Main.maxPlayers; i++)
    //        {
    //            Player player = Main.player[i];
    //            if (!player.active || player.dead || !npc.WithinRange(player.Center, 10000f))
    //                continue;

    //            player.breath = player.breathMax;
    //            player.ignoreWater = true;
    //        }

    //        return result;
    //    }
    //}

    public class AquaticScourgeHeadBehaviourOverrideFixes : AquaticScourgeHeadBehaviorOverride
    {
        public override bool PreAI(NPC npc)
        {
            bool result = base.PreAI(npc);
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (!player.active || player.dead || !npc.WithinRange(player.Center, 10000f))
                    continue;

                player.breath = player.breathMax;
                player.ignoreWater = true;
            }

            return result;
        }
    }
}
