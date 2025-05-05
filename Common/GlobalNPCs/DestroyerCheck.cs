using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;
using InfernalEclipseAPI.Core.World;
namespace InfernalEclipseAPI.Common.GlobalNPCs
{
    public class DestroyerCheck : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            InfernalWorld.dreadonDestroyerDialoguePlayed = false;
        }

        public override bool CheckDead(NPC npc)
        {
            InfernalWorld.dreadonDestroyerDialoguePlayed = false;

            return base.CheckDead(npc);
        }
        public override bool InstancePerEntity => true;
    }
}
