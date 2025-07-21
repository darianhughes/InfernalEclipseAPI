using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfernumMode.Core.GlobalInstances;
using MonoMod.RuntimeDetour;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.Achievements
{
    //public class GlobalNPCOverridesFixes : ModSystem
    //{
    //    private delegate void OnKillDelegate(GlobalNPCOverrides self, NPC npc);

    //    private static Hook onKillHook;
    //    private static OnKillDelegate orig_OnKill;

    //    public override void Load()
    //    {
    //        MethodInfo target = typeof(GlobalNPCOverrides).GetMethod("OnKill", BindingFlags.Instance | BindingFlags.Public);
    //        orig_OnKill = (OnKillDelegate)Delegate.CreateDelegate(typeof(OnKillDelegate), target);
    //        onKillHook = new Hook(target, new OnKillDelegate(OnKillHook));
    //    }

    //    public override void Unload()
    //    {
    //        onKillHook?.Dispose();
    //        onKillHook = null;
    //        orig_OnKill = null;
    //    }

    //    private static void OnKillHook(GlobalNPCOverrides self, NPC npc)
    //    {
    //        // Call original logic
    //        orig_OnKill(self, npc);

    //        // Inject custom logic
    //        if (Main.netMode == NetmodeID.Server && npc.boss)
    //        {
    //            SendBossDeadAchievementPacket(npc);
    //        }
    //    }

    //    private static void SendBossDeadAchievementPacket(NPC npc)
    //    {
    //        if (Main.netMode != NetmodeID.Server)
    //            return;

    //        ModPacket packet = ModContent.GetInstance<InfernalEclipseAPI>().GetPacket();
    //        packet.Write(npc.whoAmI);
    //        packet.Send();
    //    }
    //}

    //public class GlobalNPCOverridesFixes : GlobalNPC
    //{
    //    public override void OnKill(NPC npc)
    //    {
    //        if (Main.netMode == NetmodeID.Server && npc.boss)
    //        {
    //            SendBossDeadAchievementPacket(npc);
    //        }
    //    }

    //    private static void SendBossDeadAchievementPacket(NPC npc)
    //    {
    //        if (Main.netMode != NetmodeID.Server)
    //            return;

    //        ModPacket packet = ModContent.GetInstance<InfernalEclipseAPI>().GetPacket();
    //        packet.Write(npc.whoAmI);
    //        packet.Send();
    //    }
    //}
}
