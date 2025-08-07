using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.DamageClasses.LegendaryClass
{
    public class LegendaryMelee : DamageClass
    {
        internal static LegendaryMelee Instance;

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
            return damageClass == Melee;
        }

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            return damageClass == Melee || damageClass == Generic ? StatInheritanceData.Full : new StatInheritanceData(0.25f, 0.25f, 0.25f, 0.25f, 0.25f);
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return damageClass == Melee ? true : false;
        }
    }
}
