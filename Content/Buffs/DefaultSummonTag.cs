namespace InfernalEclipseAPI.Content.Buffs
{
    public class DefaultSummonTag : ModBuff
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class TaggedNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        // Store percentage as whole number: 20 = 20%
        public float summonTagDamage;

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff<DefaultSummonTag>() && projectile.IsMinionOrSentryRelated)
            {
                float percent = summonTagDamage / 100f;

                modifiers.FinalDamage *= 1f + percent;
            }
        }
    }
}
