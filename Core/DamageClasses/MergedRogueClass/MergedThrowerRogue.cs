using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.DamageClasses.MergedRogueClass
{
    public class MergedThrowerRogue : DamageClass
    {
        public static MergedThrowerRogue Instance { get; private set; }

        public override void Load()
        {
            Instance = this;
        }

        public override void Unload()
        {
            Instance = null;
        }

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Throwing || damageClass == Generic)
            {
                return StatInheritanceData.Full;
            }

            return StatInheritanceData.None;
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Throwing) return true;
            if (damageClass == ModContent.GetInstance<RogueDamageClass>()) return true;
            return false;
        }

        public override bool GetPrefixInheritance(DamageClass damageClass)
        {
            return damageClass == Ranged;
        }
    }
}
