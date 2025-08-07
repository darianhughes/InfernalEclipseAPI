using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.DamageClasses.LegendaryClass
{
    public class LegendaryRanged : DamageClass
    {
        internal static LegendaryRanged Instance;

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
            return damageClass == Ranged;
        }

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Ranged || damageClass == Generic) return StatInheritanceData.Full;
            else return new StatInheritanceData(0.25f, 0.25f, 0.25f, 0.25f, 0.25f);
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Ranged) return true;
            return false;
        }
    }
}
