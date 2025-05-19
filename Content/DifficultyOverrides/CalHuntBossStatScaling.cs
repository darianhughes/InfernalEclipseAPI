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
    public class CalHuntBossStatScaling : GlobalNPC
    {
        public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
        {
            return npc.boss && ((ModType)npc.ModNPC)?.Mod.Name == "CalamityHunt";
        }

        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            Mod mod;
            bool flag = false;
            int num1 = 0, num2 = 0;

            if (ModLoader.TryGetMod("CalamityMod", out mod))
            {
                object result = mod.Call("GetDifficultyActive", "BossRush");
                if (result is bool b)
                {
                    flag = b;
                    num1 = 1;
                }
            }
            num2 = flag ? 1 : 0;
            if ((num1 & num2) != 0)
            {
                ModNPC modNPC14 = npc.ModNPC;
                if ((modNPC14 != null ? (((ModType)modNPC14).Name.Contains("Goozma") ? 1 : 0) : 0) != 0)
                {
                    npc.lifeMax *= 10;
                }
            }

            if (InfernumActive.InfernumActive)
            {
                npc.lifeMax += (int)(((double).35) * (double)npc.lifeMax);
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (InfernumActive.InfernumActive)
            {
                modifiers.SourceDamage *= 1.35f;
            }
        }

        public override void PostAI(NPC npc)
        {
            if (InfernumActive.InfernumActive)
            {
                npc.position += npc.velocity * 0.35f;
            }
        }
    }
}