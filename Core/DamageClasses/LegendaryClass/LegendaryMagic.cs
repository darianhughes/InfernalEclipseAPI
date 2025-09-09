using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.DamageClasses.LegendaryClass
{
    public class LegendaryMagic : DamageClass
    {
        internal static LegendaryMagic Instance;

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
            return damageClass == Magic;
        }

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            return damageClass == Magic || damageClass == Generic ? StatInheritanceData.Full : new StatInheritanceData(0.25f, 0.25f, 0.25f, 0.25f, 0.25f);
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return damageClass == Magic ? true : false;
        }
    }
}
