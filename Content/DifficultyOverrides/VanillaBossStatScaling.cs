using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace InfernalEclipseAPI.Content.DifficultyOverrides
{
    public class VanillaBossStatScaling : GlobalNPC
    {
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
                NPC modNPC14 = npc;
                if (modNPC14.type == Terraria.ID.NPCID.BloodNautilus)
                {
                    npc.lifeMax *= 65;
                }
                else
                {
                    NPC modNPC15 = npc;
                    if (modNPC14.type == Terraria.ID.NPCID.DD2Betsy)
                    {
                        npc.lifeMax *= 15;
                    }
                }
            }

            NPC npc1 = npc;
            if (npc1.type == NPCID.CultistBoss && InfernalConfig.Instance.AdditonalVanillaBossAdjustments)
            {
                if (NPC.downedGolemBoss)
                    npc.lifeMax *= 3;
                else if (NPC.downedPlantBoss)
                    npc.lifeMax += (int)(1.5 * npc.lifeMax);
                else if (NPC.downedMechBossAny)
                {
                    if (NPC.downedMechBoss1)
                        npc.lifeMax += (int)(0.42 * npc.lifeMax);
                    if (NPC.downedMechBoss2)
                        npc.lifeMax += (int)(0.42 * npc.lifeMax);
                    if (NPC.downedMechBoss3)
                        npc.lifeMax += (int)(0.41 * npc.lifeMax);
                }
                else
                    npc.lifeMax *= 2;
            }
        }
    }
}