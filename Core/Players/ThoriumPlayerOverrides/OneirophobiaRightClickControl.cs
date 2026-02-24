using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Core.Players.ThoriumPlayerOverrides
{
    //Provided by Wardrobe Hummus
    public class OneirophobiaRightClickControl : ModPlayer
    {
        private int spawnedProjID = -1;

        public override void PostUpdate()
        {
            Mod mod;
            ModItem modItem;
            if (!ModLoader.TryGetMod("ThoriumRework", out mod) || !mod.TryFind("Oneirophobia", out modItem) || Player.HeldItem.type != modItem.Type || !InfernalConfig.Instance.ThoriumBalanceChangess || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;
            if ((!Main.mouseRight ? 0 : !Main.mouseLeft ? 1 : 0) != 0)
            {
                ModProjectile modProjectile;
                if (spawnedProjID != -1 && Main.projectile[spawnedProjID].active || !mod.TryFind("OneirophobiaMinion", out modProjectile))
                    return;
                int damage = Player.HeldItem.damage;
                spawnedProjID = Projectile.NewProjectile(Player.GetSource_Misc("RightClickSpawn"), Player.Center, Vector2.Zero, modProjectile.Type, damage, 0.0f, Player.whoAmI, 0.0f, 0.0f, 0.0f);
            }
            else
            {
                if (spawnedProjID == -1 || !Main.projectile[spawnedProjID].active)
                    return;
                Main.projectile[spawnedProjID].Kill();
                spawnedProjID = -1;
            }
        }
    }
}
