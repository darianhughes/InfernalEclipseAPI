using Terraria.ModLoader;
using SOTS.Void;
using CalamityMod;

namespace InfernalEclipseAPI.Core.DamageClasses
{
    [ExtendsFromMod("SOTS")]
    public class VoidRogue : DamageClass
    {
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            return damageClass == DamageClass.Generic || damageClass == ModContent.GetInstance<VoidGeneric>() || damageClass == ModContent.GetInstance<RogueDamageClass>() ? StatInheritanceData.Full : new StatInheritanceData(0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
        }
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return damageClass == ModContent.GetInstance<RogueDamageClass>() || damageClass != ModContent.GetInstance<VoidGeneric>();
        }

        public override bool UseStandardCritCalcs => true;
    }
}
