using CalamityMod.NPCs.SupremeCalamitas;
using InfernalEclipseAPI.Content.Buffs;
using InfernalEclipseAPI.Core.World;
using InfernumMode;
using InfernumMode.Content.BehaviorOverrides.BossAIs.SupremeCalamitas;

namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs
{
    public class SupremeCalamitasDebuffs : GlobalNPC
    {
        private Mod clamity
        {
            get
            {
                ModLoader.TryGetMod("Clamity", out Mod clam);
                return clam;
            }
        }

        public override bool InstancePerEntity => true;
        
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == ModContent.NPCType<SupremeCalamitas>();
        }

        public override bool PreAI(NPC npc)
        {
            if (!npc.active)
                return base.PreAI(npc);

            if (clamity != null)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player.active && !player.dead)
                    {
                        if (player.mount?.Type == clamity.Find<ModMount>("PlagueChairMount").Type)
                            player.mount.Dismount(player);
                    }
                }
            }

            if (InfernalWorld.RagnarokModeEnabled && IsInfernumSCalDesperationPhase(npc))
            {
                foreach (Player player in Main.ActivePlayers)
                {
                    if (!player.active || player.dead || player.ghost)
                        continue;

                    if (!npc.WithinRange(player.Center, 10000f))
                        continue;

                    player.AddBuff(ModContent.BuffType<BrimstoneDesperation>(), 2);
                }
            }

            return base.PreAI(npc);
        }

        public override void AI(NPC npc)
        {
        }

        public static bool IsInfernumSCalDesperationPhase(NPC npc)
        {
            if (npc is null || !npc.active || npc.type != ModContent.NPCType<SupremeCalamitas>())
                return false;

            return (SupremeCalamitasBehaviorOverride.SCalAttackType)(int)npc.ai[0] ==
                   SupremeCalamitasBehaviorOverride.SCalAttackType.DesperationPhase;
        }
    }
}
