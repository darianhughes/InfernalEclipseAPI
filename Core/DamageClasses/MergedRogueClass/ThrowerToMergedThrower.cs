using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace InfernalEclipseAPI.Core.DamageClasses.MergedRogueClass
{
    public class ThrowerToMergedThrower : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return InfernalConfig.Instance.MergeThrowerIntoRogue;
        }
        public override void SetDefaults(Item item)
        {
            if (item.DamageType == DamageClass.Throwing)
            {
                item.DamageType = ModContent.GetInstance<MergedThrowerRogue>();
            }
        }
    }

    public class ThrowerToMergedThrowerProj : GlobalProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return InfernalConfig.Instance.MergeThrowerIntoRogue;
        }
        public override void SetDefaults(Projectile entity)
        {
            if (entity.DamageType == DamageClass.Throwing)
            {
                entity.DamageType = ModContent.GetInstance<MergedThrowerRogue>();
            }
        }
    }
}
