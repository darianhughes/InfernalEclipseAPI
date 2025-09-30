using CalamityMod.Projectiles.Typeless;
using InfernalEclipseAPI.Content.Items.Lore.InfernalEclipse;

namespace InfernalEclipseAPI.Common.GlobalProjectiles.ProjectileReworks
{
    public class BossRushProjectile : GlobalProjectile
    {
        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            if (projectile.type == ModContent.ProjectileType<BossRushEndEffectThing>())
            {
                for (int i = Main.maxPlayers - 1; i >= 0; i--)
                {
                    Player p = Main.player[i];
                    if (p is null || !p.active)
                        continue;

                    //int notRock = Item.NewItem(p.GetSource_Misc("CalamityMod_BossRushRock"), (int)p.position.X, (int)p.position.Y, p.width, p.height, ModContent.ItemType<DemonicChaliceOfInfernum>());
                    int alsonotRock = Item.NewItem(p.GetSource_Misc("CalamityMod_BossRushRock"), (int)p.position.X, (int)p.position.Y, p.width, p.height, ModContent.ItemType<MysteriousDiary>());
                    if (Main.netMode == NetmodeID.Server)
                    {
                        //Main.timeItemSlotCannotBeReusedFor[notRock] = 54000;
                        Main.timeItemSlotCannotBeReusedFor[alsonotRock] = 54000;
                        //NetMessage.SendData(MessageID.InstancedItem, i, -1, null, notRock);
                        NetMessage.SendData(MessageID.InstancedItem, i, -1, null, alsonotRock);
                    }
                }
            }
            return base.PreKill(projectile, timeLeft);
        }
    }
}
