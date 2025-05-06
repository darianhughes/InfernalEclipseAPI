using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;

namespace InfernalEclipseAPI.Content.DifficultyOverrides
{
    public class SpookyBossStatScaling : GlobalNPC
    {
        public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
        {
            return ((ModType)npc.ModNPC)?.Mod.Name == "Spooky";
        }

        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            if (npc.boss)
            {
                if (InfernumActive.InfernumActive)
                {
                    npc.lifeMax += (int)(((double).35) * (double)npc.lifeMax);
                }
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (npc.boss)
            {
                if (InfernumActive.InfernumActive)
                {
                    modifiers.SourceDamage *= 1.35f;
                }
            }
        }

        public override void PostAI(NPC npc)
        {
            if (npc.boss)
            {
                if (InfernumActive.InfernumActive)
                {
                    npc.position += npc.velocity * 0.35f;
                }
            }
        }
    }
}