using InfernalEclipseAPI.Common.Globals.GlobalItems.ModSpecific;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Buffs.Tag
{
    public class SplitFirebrandTag : ModBuff
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }

        public class FirebrandTaggedNPC : GlobalNPC
        {
            public override bool InstancePerEntity => true;

            public override void ModifyHitByProjectile(
                NPC npc,
                Projectile projectile,
                ref NPC.HitModifiers modifiers)
            {
                if (npc.HasBuff<SplitFirebrandTag>() && projectile.IsMinionOrSentryRelated)
                {
                    modifiers.FlatBonusDamage += GetFirebrandTagDamage();
                }
            }
        }

        public static int GetFirebrandTagDamage()
        {
            if (NPC.downedMoonlord)
                return 15;
            if (NPC.downedGolemBoss)
                return 12;
            if (NPC.downedPlantBoss)
                return 9;
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                return 6;
            if (Main.hardMode)
                return 5;
            if (NPC.downedBoss3)
                return 4;
            return 3;
        }
    }
}
