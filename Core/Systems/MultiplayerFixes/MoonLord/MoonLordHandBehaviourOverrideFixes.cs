using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernumMode.Content.BehaviorOverrides.BossAIs.MoonLord;
using Terraria;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.MoonLord
{
    public class MoonLordHandBehaviourOverrideFixes : MoonLordHandBehaviorOverride
    {
        public override bool PreAI(NPC npc)
        {
            bool result = base.PreAI(npc);
            if (npc.ai[0] == -2)
            {
                npc.life = npc.lifeMax;
            }
            return result;
        }
    }
}
