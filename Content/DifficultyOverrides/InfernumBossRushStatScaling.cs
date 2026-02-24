using CalamityMod.NPCs.GreatSandShark;

namespace InfernalEclipseAPI.Content.DifficultyOverrides
{
    public class InfernumBossRushStatScaling : GlobalNPC
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
                ModNPC modNPC14 = npc.ModNPC;
                if ((modNPC14 != null ? (modNPC14.Name.Contains("BereftVassal") ? 1 : 0) : 0) != 0)
                {
                    npc.lifeMax *= 5;
                }
                else
                {
                    ModNPC modNPC15 = npc.ModNPC;
                    if (npc.type == ModContent.NPCType<GreatSandShark>())
                    {
                        npc.lifeMax *= 75;
                    }
                }
            }
        }
    }
}