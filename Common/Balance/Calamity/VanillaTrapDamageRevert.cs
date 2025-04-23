using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.Balance.Calamity
{
    public class VanillaTrapDamageRevert : ModPlayer
    {
        public override void ModifyHitByProjectile(Projectile proj, ref Terraria.Player.HurtModifiers modifiers)
        {
            ref StatModifier sourceDamage = ref modifiers.SourceDamage;

            if (proj.type == 108)
            {
                sourceDamage /= Main.expertMode ? 0.225f : 0.35f;
            }
            else if (proj.type == 727 || proj.type == 763)
            {
                sourceDamage /= Main.expertMode ? 0.3f : 0.5f;
            }

            if (!Main.expertMode)
                return;

            if (proj.type == 99 || proj.type == 1005)
            {
                sourceDamage /= 0.65f;
            }
            else if (proj.type == 185 || proj.type == 187 || proj.type == 184)
            {
                sourceDamage /= 0.625f;
            }
            else if (proj.type == 186)
            {
                sourceDamage /= 0.6f;
            }
        }

    }
}
