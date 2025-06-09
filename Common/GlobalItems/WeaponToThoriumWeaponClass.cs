using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.Cultist;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    [ExtendsFromMod("ThoriumMod")]
    public class WeaponToThoriumWeaponClass : GlobalItem
    {
        public override void SetDefaults(Item entity)
        {
            if (entity.type == ModContent.ItemType<AncientFlame>() && InfernalConfig.Instance.ChanageWeaponClasses)
            {
                entity.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
                entity.damage = 32;
            }
        }
    }
}
