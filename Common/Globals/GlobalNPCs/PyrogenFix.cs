using CalamityMod.Buffs.DamageOverTime;
using InfernalEclipseAPI.Core.Systems;
using Clamity.Content.Bosses.Pyrogen.NPCs;
using System.Reflection;
using Terraria.DataStructures;
using InfernumMode.Core.GlobalInstances.Systems;
using CalamityMod.Events;
using InfernalEclipseAPI.Content.Items.Materials;

namespace InfernalEclipseAPI.Common.GlobalNPCs
{
    [JITWhenModsEnabled(InfernalCrossmod.Clamity.Name)]
    [ExtendsFromMod(InfernalCrossmod.Clamity.Name)]
    public class PyrogenFix : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == ModContent.NPCType<PyrogenBoss>() || entity.type == ModContent.NPCType<PyrogenShield>();
        }

        public override void SetDefaults(NPC npc)
        {
            if (npc.type == ModContent.NPCType<PyrogenBoss>())
            {
                npc.damage = 45;

                float lifeBoost = 0f;

                if (IsWorldLegendary())
                {
                    lifeBoost += 0.1f;
                }
                if (WorldSaveSystem.InfernumModeEnabled)
                {
                    lifeBoost += 0.20f;
                }

                npc.lifeMax += (int)(npc.lifeMax * lifeBoost);
            }
            else
                npc.damage = 35;
        }

        public override void PostAI(NPC npc)
        {
            npc.position += npc.velocity * 0.1f;

            if (IsWorldLegendary())
                npc.position += npc.velocity * 0.05f;
            if (WorldSaveSystem.InfernumModeEnabled)
                npc.position += npc.velocity * 0.2f;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 180);
            if (hurtInfo.Damage > 400 && !BossRushEvent.BossRushActive)
                hurtInfo.Damage = 400;
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type != ModContent.NPCType<PyrogenBoss>())
                return;

            npcLoot.Add(Terraria.GameContent.ItemDropRules.ItemDropRule.Common(ModContent.ItemType<EssenceOfFlame>(), minimumDropped: 3, maximumDropped: 5));
        }

        private static bool IsWorldLegendary()
        {
            FieldInfo findInfo = typeof(Main).GetField("_currentGameModeInfo", BindingFlags.Static | BindingFlags.NonPublic);
            GameModeData data = (GameModeData)findInfo.GetValue(null);
            return (Main.getGoodWorld && data.IsMasterMode);
        }
    }
}
