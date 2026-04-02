using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ItemReworks.Weapons.Healer.Scythes
{
    [ExtendsFromMod("ThoriumMod")]
    public class KinetoScytheRework : GlobalItem
    {
        public override bool InstancePerEntity => true;

        // Cache Thorium weapon and projectile types
        private static int KinetoScytheID = -1;

        private static int KinetoScytheProID = -1;

        public override void Load()
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thor))
            {
                KinetoScytheID = thor.Find<ModItem>("Kinetoscythe").Type;

                KinetoScytheProID = thor.Find<ModProjectile>("KinetoscythePro2").Type;
            }
        }

        public override void HoldItem(Item item, Player player)
        {
            if (KinetoScytheID == -1 || item.type != KinetoScytheID)
                return;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (!proj.active)
                    continue;

                if (proj.owner != player.whoAmI)
                    continue;

                if (proj.type != KinetoScytheProID)
                    continue;

                if (player.controlUseItem) // left click being held
                {
                    if (proj.ai[2] != 1f)
                    {
                        proj.ai[2] = 1f;
                        proj.netUpdate = true;
                    }
                }
                else
                {
                    if (proj.ai[2] != 0f)
                    {
                        proj.ai[2] = 0f;
                        proj.netUpdate = true;
                    }
                }
            }
        }

        public class KinetoScytheGlobalProjectile : GlobalProjectile
        {
            public override bool InstancePerEntity => true;

            public override void AI(Projectile projectile)
            {
                if (projectile.type != KinetoScytheProID)
                    return;

                if (projectile.ai[2] != 1f)
                {
                    projectile.velocity = Vector2.Zero;
                    return;
                }

                Player player = Main.player[projectile.owner];

                NPC target = FindClosestTarget(projectile, 1800f);

                Vector2 targetPos;

                if (target != null)
                    targetPos = target.Center;
                else
                    targetPos = player.Center;

                float speed = 6f;
                float inertia = 10f;

                Vector2 desiredVelocity = projectile.DirectionTo(targetPos) * speed;

                // Smooth homing
                projectile.velocity = (projectile.velocity * (inertia - 1) + desiredVelocity) / inertia;

                // CRITICAL: actually apply movement
                projectile.Center += projectile.velocity;
            }

            private NPC FindClosestTarget(Projectile proj, float maxDistance)
            {
                NPC closest = null;
                float distance = maxDistance;

                foreach (NPC npc in Main.npc)
                {
                    if (!npc.CanBeChasedBy())
                        continue;

                    float d = Vector2.Distance(proj.Center, npc.Center);

                    if (d < distance)
                    {
                        distance = d;
                        closest = npc;
                    }
                }

                return closest;
            }
        }
    }
}
