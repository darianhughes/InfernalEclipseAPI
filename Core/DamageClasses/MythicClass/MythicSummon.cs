namespace InfernalEclipseAPI.Core.DamageClasses.MythicClass
{
    public class MythicSummon : DamageClass
    {
        internal static MythicSummon Instance;

        public override void Load()
        {
            Instance = this;
        }

        public override void Unload()
        {
            Instance = null;
        }

        public override bool GetPrefixInheritance(DamageClass damageClass)
        {
            return damageClass == Summon;
        }

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            return damageClass == Summon || damageClass == Generic ? StatInheritanceData.Full : new StatInheritanceData(0.75f, 0.75f, 0.75f, 0.75f, 0.75f);
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return damageClass == Summon ? true : false;
        }
    }
}
