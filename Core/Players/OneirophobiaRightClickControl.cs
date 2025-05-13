using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Core.Players
{
    //Provided by Wardrobe Hummus
    public class OneirophobiaRightClickControl : ModPlayer
    {
        private int spawnedProjID = -1;

        public override void PostUpdate()
        {
            Mod mod;
            ModItem modItem;
            if (!ModLoader.TryGetMod("ThoriumRework", out mod) || !mod.TryFind<ModItem>("Oneirophobia", out modItem) || this.Player.HeldItem.type != modItem.Type || !InfernalConfig.Instance.ThoriumBalanceChangess || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;
            if ((!Main.mouseRight ? 0 : (!Main.mouseLeft ? 1 : 0)) != 0)
            {
                ModProjectile modProjectile;
                if (this.spawnedProjID != -1 && ((Entity)Main.projectile[this.spawnedProjID]).active || !mod.TryFind<ModProjectile>("OneirophobiaMinion", out modProjectile))
                    return;
                int damage = this.Player.HeldItem.damage;
                this.spawnedProjID = Projectile.NewProjectile(((Entity)this.Player).GetSource_Misc("RightClickSpawn"), ((Entity)this.Player).Center, Vector2.Zero, modProjectile.Type, damage, 0.0f, ((Entity)this.Player).whoAmI, 0.0f, 0.0f, 0.0f);
            }
            else
            {
                if (this.spawnedProjID == -1 || !((Entity)Main.projectile[this.spawnedProjID]).active)
                    return;
                Main.projectile[this.spawnedProjID].Kill();
                this.spawnedProjID = -1;
            }
        }
    }
}
