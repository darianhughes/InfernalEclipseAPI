using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using InfernumMode.Core.GlobalInstances.Systems;
using MonoMod.RuntimeDetour;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Events;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.BossRush
{
    public class BossRushChangesSystemFixes : ModSystem
    {
        private static Hook handleTeleportsHook;

        public override void Load()
        {
            MethodInfo targetMethod = typeof(BossRushChangesSystem).GetMethod("HandleTeleports", BindingFlags.Public | BindingFlags.Static);
            handleTeleportsHook = new Hook(targetMethod, HandleTeleportsDetour);
        }

        public override void Unload()
        {
            handleTeleportsHook?.Dispose();
            handleTeleportsHook = null;
        }

        private static void HandleTeleportsDetour(Action orig)
        {
            if (BossRushEvent.BossRushStage >= BossRushEvent.Bosses.Count)
                return;

            int ceaselessID = ModContent.NPCType<CalamityMod.NPCs.CeaselessVoid.CeaselessVoid>();

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (BossRushEvent.CurrentlyFoughtBoss == ceaselessID && WorldSaveSystem.ForbiddenArchiveCenter == Point.Zero)
                {
                    int stage = BossRushEvent.BossRushStage;
                    orig();
                    BossRushEvent.BossRushStage = stage;
                }
                else orig();
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                if (BossRushEvent.CurrentlyFoughtBoss == ceaselessID && WorldSaveSystem.ForbiddenArchiveCenter == Point.Zero)
                {
                    orig();
                    var netMessage = ModContent.GetInstance<CalamityMod.CalamityMod>().GetPacket();
                    netMessage.Write((byte)CalamityModMessageType.BossRushStage);
                    netMessage.Write(BossRushEvent.BossRushStage);
                    netMessage.Send();
                }
                else orig();
            }
            else
            {
                orig();
            }
        }
    }
}
