namespace InfernalEclipseAPI.Core.DamageClasses.LegendaryClass
{
    public class LegendarySummonMeleeSpeed : DamageClass
    {
        internal static LegendarySummonMeleeSpeed Instance;

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
            return damageClass == SummonMeleeSpeed;
        }

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            return damageClass == SummonMeleeSpeed || damageClass == Generic || damageClass == Summon ? StatInheritanceData.Full : new StatInheritanceData(0.5f, 0.5f, 0.5f, 0.5f, 0.5f);
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return damageClass == SummonMeleeSpeed ? true : false;
        }
    }
}
