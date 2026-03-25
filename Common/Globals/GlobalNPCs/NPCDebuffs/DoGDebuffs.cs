using CalamityMod.NPCs.DevourerofGods;
using InfernalEclipseAPI.Core.Systems;
using InfernumMode;
using InfernumMode.Content.BehaviorOverrides.BossAIs.DoG;
using Terraria;

namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs
{
    public class DoGDebuffs : GlobalNPC
    {
        private Mod clamity
        {
            get
            {
                ModLoader.TryGetMod("Clamity", out Mod clam);
                return clam;
            }
        }

        public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
        {
            return npc.type == ModContent.NPCType<DevourerofGodsHead>();
        }

        public override bool PreAI(NPC npc)
        {
            if (!InfernalConfig.Instance.CalamityBalanceChanges || !npc.active) return base.PreAI(npc);

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead)
                {
                    if (clamity != null)
                    {
                        if (player.mount?.Type == clamity.Find<ModMount>("PlagueChairMount").Type)
                            player.mount.Dismount(player);
                    }
                    if (InfernalCrossmod.Thorium.Loaded)
                    {
                        if (InfernalCrossmod.Thorium.Mod.TryFind("SuperAnvilMount", out ModMount supAnvil))
                            if (player.mount?.Type == supAnvil.Type)
                                player.mount.Dismount(player);
                    }
                }
            }

            return base.PreAI(npc);
        }

        public override void PostAI(NPC npc)
        {
            if (!npc.active)
                return;

            if (DoGPhase2HeadBehaviorOverride.InPhase2)
            {
                if (npc.damage == 650)
                    npc.damage = 885;
            }
            else
            {
                if (npc.damage == 600)
                    npc.damage = 800;
            }
        }
    }
}
