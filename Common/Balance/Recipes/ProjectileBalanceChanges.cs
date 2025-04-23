using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.Balance.Recipes
{
    public class ProjectileBalanceChanges : GlobalProjectile
    {
        public override void SetDefaults(Projectile entity)
        {
            if (ModLoader.TryGetMod("Clamity", out Mod clam))
            {
                if (entity.type == clam.Find<ModProjectile>("FireBarrage").Type)
                {
                    entity.damage = 135;
                }
                if (entity.type == clam.Find<ModProjectile>("FireBarrageHoming").Type)
                {
                    entity.damage = 130;
                }
                if (entity.type == clam.Find<ModProjectile>("Fireblast").Type)
                {
                    entity.damage = 140;
                }
                if (entity.type == clam.Find<ModProjectile>("FireBombExplosion").Type)
                {
                    entity.damage = 135;
                }
                if (entity.type == clam.Find<ModProjectile>("Firethrower").Type)
                {
                    entity.damage = 150;
                }
            }

            if (ModLoader.TryGetMod("Thorium", out Mod thorium) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                if (entity.type == thorium.Find<ModProjectile>("SeashellCastanettessPro1").Type)
                {
                    entity.penetrate = 2;
                }

                if (entity.type == thorium.Find<ModProjectile>("Cube").Type)
                {
                    entity.penetrate = 3;
                }
            } 
        }
    }
}
