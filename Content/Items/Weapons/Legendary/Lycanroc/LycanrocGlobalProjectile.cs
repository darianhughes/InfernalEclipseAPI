using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FargowiltasSouls.Content.Items.Accessories.Masomode;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.Lycanroc
{
    public class LycanrocGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool appliesCrumbling = false;
        public bool appliesArmorCrunch = false;
    }
}
