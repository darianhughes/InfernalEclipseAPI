using CalamityMod;
using InfernumMode.Core.GlobalInstances.Systems;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.DifficultyOverrides.Calamity
{
    public class WarMachineStatScaling : GlobalNPC
    {
        public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
        {
            return npc.boss && ((ModType)npc.ModNPC)?.Mod.Name == "CalamityAddon";
        }

        public override void SetDefaults(NPC entity)
        {
            if (entity.ModNPC.Name.Contains("WulfrumMothership"))
            {
                entity.Calamity().canBreakPlayerDefense = true;
            }
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
                if ((modNPC14 != null ? modNPC14.Name.Contains("WulfrumMothership") ? 1 : 0 : 0) != 0)
                {
                    npc.lifeMax *= 125;
                }
            }

            //he is actually kinda overtuned lol
            if (WorldSaveSystem.InfernumModeEnabled)
            {
                //npc.lifeMax += (int)((double).1 * npc.lifeMax);
            }
        }

        /*
        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (WorldSaveSystem.InfernumModeEnabled)
            {
                modifiers.SourceDamage *= 1.35f;
            }
        }

        public override void PostAI(NPC npc)
        {
            if (WorldSaveSystem.InfernumModeEnabled)
            {
                npc.position += npc.velocity * 0.35f;
            }
        }
        */
    }
}
