using Terraria.Audio;
using Terraria.DataStructures;
using ThoriumMod;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Common.GlobalProjectiles.ProjectileReworks
{
    [ExtendsFromMod("RagnarokMod")]
    public class ExecutionerMark05ThrowRework : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            Mod ragnarok = ModLoader.GetMod("RagnarokMod");
            if (ragnarok == null)
                return false;

            int targetProjType = ragnarok.Find<ModProjectile>("ExecutionerMark05ThrowPro")?.Type ?? -1;
            int wapBamBoomType = ragnarok.Find<ModProjectile>("WapBamBoom")?.Type ?? -1;

            return entity.type == targetProjType || entity.type == wapBamBoomType;
        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            Mod ragnarok = ModLoader.GetMod("RagnarokMod");
            Mod calamity = ModLoader.GetMod("CalamityMod");

            if (ragnarok == null || calamity == null)
            {
                return;
            }

            int targetProjType = ragnarok.Find<ModProjectile>("ExecutionerMark05ThrowPro")?.Type ?? -1;
            int supernovaBoomType = calamity.Find<ModProjectile>("SupernovaBoom")?.Type ?? -1;

            if (projectile.type == targetProjType && supernovaBoomType != -1)
            {
                int damage = (int)(projectile.damage * 2.25);

                int index = Projectile.NewProjectile(
                    projectile.GetSource_Death(),
                    projectile.Center,
                    Vector2.Zero,
                    supernovaBoomType,
                    damage,
                    projectile.knockBack,
                    projectile.owner
                );

                if (index >= 0 && index < Main.maxProjectiles)
                {
                    Projectile newProj = Main.projectile[index];
                    newProj.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
                    newProj.localAI[0] = 1337f;

                    // --- SCALE THE PROJECTILE ---
                    float scaleFactor = 1.5f;
                    Vector2 originalSize = new Vector2(newProj.width, newProj.height);
                    Vector2 oldCenter = newProj.Center;

                    newProj.scale *= scaleFactor;
                    newProj.width = (int)(originalSize.X * scaleFactor);
                    newProj.height = (int)(originalSize.Y * scaleFactor);
                    newProj.Center = oldCenter;
                }
            }

            SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Item/SupernovaBoom", (SoundType)0));
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Mod ragnarok = ModLoader.GetMod("RagnarokMod");
            if (ragnarok == null)
                return;

            int wapBamBoomType = ragnarok.Find<ModProjectile>("WapBamBoom")?.Type ?? -1;

            if (projectile.type == wapBamBoomType)
            {
                projectile.Kill();
            }
        }
    }
}
