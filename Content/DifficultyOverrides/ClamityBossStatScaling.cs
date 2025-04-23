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
    public class ClamityBossStatScaling : GlobalNPC
    {
        public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
        {
            return ((ModType)npc.ModNPC)?.Mod.Name == "Clamity";
        }

        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            if (npc.boss)
            {
                npc.lifeMax += npc.lifeMax;

                if (InfernumActive.InfernumActive)
                {
                    npc.lifeMax += (int)(((double).35) * (double)npc.lifeMax);
                }
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SourceDamage *= 2.0f;

            if (InfernumActive.InfernumActive)
            {
                modifiers.SourceDamage *= 1.35f;
            }
        }

        public override void PostAI(NPC npc)
        {
            ModNPC modNPC14 = npc.ModNPC;
            if (!((modNPC14 != null ? (((ModType)modNPC14).Name.Contains("") ? 1 : 0) : 0) != 0))
            {
                npc.position += npc.velocity * 0.1f;
            }

            if (InfernumActive.InfernumActive)
            {
                npc.position += npc.velocity * 0.35f;
            }
        }
    }
}