using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Weapons.Melee;
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
            if (InfernalConfig.Instance.ChanageWeaponClasses) 
            {
                if (entity.type == ModContent.ItemType<AncientFlame>())
                {
                    entity.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
                    entity.damage = 32;
                }

                if (entity.type == ModContent.ItemType<TheBurningSky>())
                {
                    entity.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
                    entity.mana = 35;
                }
            }
        }
    }
}
