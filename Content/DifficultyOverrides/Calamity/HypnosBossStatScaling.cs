using Microsoft.Xna.Framework;
using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;

namespace InfernalEclipseAPI.Content.DifficultyOverrides.Calamity
{
    public class HypnosBossStatScaling : GlobalNPC
    {
        public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
        {
            return ((ModType)npc.ModNPC)?.Mod.Name == "HypnosMod";
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
                if ((modNPC14 != null ? modNPC14.Name.Contains("HypnosBoss") ? 1 : 0 : 0) != 0)
                {
                    npc.lifeMax *= 2;
                }
            }

            if (InfernumActive.InfernumActive && npc.boss)
            {
                npc.lifeMax += (int)((double).10 * npc.lifeMax);
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (InfernumActive.InfernumActive)
            {
                modifiers.SourceDamage *= 0.80f;
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.ModNPC != null)
            {
                if (npc.ModNPC.Name.Contains("HypnosBoss"))
                {
                    if (npc.ai[0] != 0)
                    {
                        Player target = Main.player[npc.target];

                        if (!target.active || target.dead || Vector2.Distance(target.Center, npc.Center) > 3000f)
                        {
                            npc.active = false;

                            foreach (NPC otherNPC in Main.ActiveNPCs)
                            {
                                if (otherNPC.type == npc.type || otherNPC.ModNPC == null)
                                    continue;

                                if (otherNPC.ModNPC.Name.Contains("Draedon") && otherNPC.ModNPC.Mod.Name == "HypnosMod")
                                {
                                    otherNPC.active = false;
                                }
                            }
                        }
                    }
                }
            }

            return base.PreAI(npc);
        }
    }
}