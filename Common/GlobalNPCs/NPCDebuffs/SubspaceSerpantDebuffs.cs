using CalamityMod.Items.Mounts;
using InfernumMode;
using SOTS.NPCs.Boss;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Common.GlobalNPCs.NPCDebuffs
{
    [ExtendsFromMod("SOTS")]
    public class SubspaceSerpantDebuffs : GlobalNPC
    {
        private Mod clamity
        {
            get
            {
                ModLoader.TryGetMod("Clamity", out Mod clam);
                return clam;
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (!InfernalConfig.Instance.SOTSBalanceChanges || !npc.active || npc.type != ModContent.NPCType<SubspaceSerpentHead>()) return base.PreAI(npc);

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.dead || !player.active || !npc.WithinRange(player.Center, 10000f))
                    continue;

                if (player.mount?.Type == ModContent.MountType<DraedonGamerChairMount>())
                        player.mount.Dismount(player);
                if (clamity != null)
                {
                    if (player.mount?.Type == clamity.Find<ModMount>("PlagueChairMount").Type)
                        player.mount.Dismount(player);
                }

                player.DoInfiniteFlightCheck(Color.LimeGreen);
            }

            return base.PreAI(npc);
        }
    }
}
