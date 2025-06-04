using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Buffs.DamageOverTime;
using Terraria.ID;
using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;

namespace InfernalEclipseAPI.Common.GlobalNPCs
{
    public class PyrogenFix : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(NPC npc)
        {
            if (!ModLoader.TryGetMod("Clamity", out Mod clam) || !InfernumActive.InfernumActive)
                return;

            int pyrogenType = clam.Find<ModNPC>("PyrogenBoss")?.Type ?? -1;
            int shieldType = clam.Find<ModNPC>("PyrogenShield")?.Type ?? -1;

            if (npc.type == pyrogenType || npc.type == shieldType)
            {
                npc.lifeMax = (int)(npc.lifeMax * 1.35f);
                // Optionally sync current life with new max
                npc.life = npc.lifeMax;
            }
        }

        public override void PostAI(NPC npc)
        {
            // Try to get Clamity mod and types
            if (!ModLoader.TryGetMod("Clamity", out Mod clam))
                return;
            int pyrogenType = clam.Find<ModNPC>("PyrogenBoss")?.Type ?? -1;
            int shieldType = clam.Find<ModNPC>("PyrogenShield")?.Type ?? -1;

            if (npc.type != pyrogenType && npc.type != shieldType)
                return;

            // Only on server or singleplayer
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            // Check all players
            foreach (Player player in Main.player)
            {
                if (player is null || !player.active || player.dead)
                    continue;

                // Check intersection
                if (npc.Hitbox.Intersects(player.Hitbox))
                {
                    // Optionally check opacity/radius for shield, or always apply
                    int intendedDamage = npc.type == pyrogenType ? 140 : 110;

                    // Find Brimstone Flames
                    int brimstoneFlamesBuff = ModContent.BuffType<BrimstoneFlames>();

                    // Only apply if the player is not on damage cooldown (immune)
                    if (player.immuneTime <= 0)
                    {
                        player.Hurt(PlayerDeathReason.ByNPC(npc.whoAmI), intendedDamage, 0);
                        if (brimstoneFlamesBuff != -1)
                            player.AddBuff(brimstoneFlamesBuff, 300);
                    }
                }
            }
        }
    }
}
