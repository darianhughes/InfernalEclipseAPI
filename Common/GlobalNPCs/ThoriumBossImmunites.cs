using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.SlimeGod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Buffs;

namespace InfernalEclipseAPI.Common.GlobalNPCs
{
    [ExtendsFromMod("ThoriumMod")]
    public class ThoriumBossImmunites : GlobalNPC
    {
        public override void SetDefaults(NPC npc)
        {
            int oozed = ModContent.BuffType<Oozed>();

            // Perforator worms
            if (npc.type == ModContent.NPCType<PerforatorHeadSmall>() ||
                npc.type == ModContent.NPCType<PerforatorBodySmall>() ||
                npc.type == ModContent.NPCType<PerforatorTailSmall>() ||

                npc.type == ModContent.NPCType<PerforatorHeadMedium>() ||
                npc.type == ModContent.NPCType<PerforatorBodyMedium>() ||
                npc.type == ModContent.NPCType<PerforatorTailMedium>() ||

                npc.type == ModContent.NPCType<PerforatorHeadLarge>() ||
                npc.type == ModContent.NPCType<PerforatorBodyLarge>() ||
                npc.type == ModContent.NPCType<PerforatorTailLarge>() ||

                // Slime God’s summoned slimes
                npc.type == ModContent.NPCType<CrimulanPaladin>() ||
                npc.type == ModContent.NPCType<EbonianPaladin>() ||

                npc.type == ModContent.NPCType<SplitCrimulanPaladin>() ||
                npc.type == ModContent.NPCType<SplitEbonianPaladin>()
                )
            {
                npc.buffImmune[oozed] = true;
            }
        }

        private static readonly HashSet<string> ThoriumBossNames = new()
{
            "TheGrandThunderBird",
            "QueenJellyfish",
            "Viscount",
            "GraniteEnergyStorm",
            "BuriedChampion",
            "StarScouter",
            "BoreanStrider",
            "FallenBeholder",
            "Lich",
            "LichHeadless",
            "ForgottenOne",
            "ForgottenOneCracked",
            "ForgottenOneReleased",
            "DreamEater",
            "Omnicide",
            "SlagFury",
            "Aquaius",
            "PatchWerk",
            "CorpseBloom",
            "Illusionist",
        };

        public override void PostAI(NPC npc)
        {
            if (!npc.active || !npc.boss || npc.ModNPC == null)
                return;

            if (npc.ModNPC.Mod.Name == "ThoriumMod" && ThoriumBossNames.Contains(npc.ModNPC.Name))
            {
                // TEMP: ignore BossZen to test
                if (CalamityConfig.Instance.BossZen)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player player = Main.player[i];
                        if (!player.active || player.dead)
                            continue;

                        if (Vector2.Distance(player.Center, npc.Center) < 6400f)
                        {
                            // give at least 1 second to confirm it’s being applied
                            player.AddBuff(ModContent.BuffType<BossEffects>(), 60, true, false);
                        }
                    }
                }
            }
        }
    }
}
