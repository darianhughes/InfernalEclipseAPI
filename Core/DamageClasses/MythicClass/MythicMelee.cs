using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.DamageClasses.MythicClass
{
    public class MythicMelee : DamageClass
    {
        internal static MythicMelee Instance;

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
            return damageClass == Melee || damageClass == Generic ? StatInheritanceData.Full : new StatInheritanceData(0.5f, 0.5f, 0.5f, 0.5f, 0.5f);
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return damageClass == Melee ? true : false;
        }
    }
}
