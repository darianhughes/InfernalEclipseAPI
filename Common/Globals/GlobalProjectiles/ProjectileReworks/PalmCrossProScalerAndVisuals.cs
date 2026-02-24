using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Common.GlobalProjectiles.ProjectileReworks
{
    [ExtendsFromMod("ThoriumMod")]
    public class PalmCrossProScalerAndVisuals : GlobalProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return !ModLoader.TryGetMod("WHummusMultiModBalancing", out _);
        }
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Projectile projectile, bool lateInstantiation)
        {
            return projectile.ModProjectile?.Name == "PalmCrossPro";
        }

        public override void PostAI(Projectile projectile)
        {
            if (projectile.ModProjectile?.Name != "PalmCrossPro")
                return;

            Player player = Main.player[projectile.owner];

            // --- Scaling logic ---
            float empower = 0f;
            var empowerField = projectile.ModProjectile.GetType().GetField("empower");
            if (empowerField != null)
                empower = Convert.ToSingle(empowerField.GetValue(projectile.ModProjectile));

            float baseSize = 45f + empower * 14f;
            float scale = 3f;

            // Determine the intended center (keep it relative to the player)
            Vector2 intendedCenter = player.Center + new Vector2(80 * player.direction, -3); // match original AI offset

            // Calculate new width/height
            int newWidth = (int)(baseSize * scale);
            int newHeight = (int)(baseSize * scale);

            // Apply size and recalc position so projectile stays centered
            projectile.width = newWidth;
            projectile.height = newHeight;
            projectile.position = intendedCenter - new Vector2(newWidth / 2f, newHeight / 2f);

            // --- Dust ring on channel end ---
            bool currentlyChanneling = player.channel && !player.noItems && !player.CCed;
            bool wasChanneling = projectile.localAI[1] == 1f;

            if (currentlyChanneling)
            {
                projectile.localAI[1] = 1f;
            }
            else if (wasChanneling)
            {
                int dustType = 87;
                if (player.GetModPlayer<ThoriumMod.ThoriumPlayer>().darkAura)
                    dustType = 86;

                int count = 50;
                float radius = 112;
                float dustVelocity = 1.5f + empower * 0.5f;

                for (int i = 0; i < count; i++)
                {
                    float angle = MathHelper.TwoPi * i / count;
                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                    Vector2 dustPosition = projectile.Center + offset;

                    int dustIndex = Dust.NewDust(dustPosition, 0, 0, dustType, 0f, 0f, 0, default, 0.5f + empower * 0.1f);
                    Dust dust = Main.dust[dustIndex];
                    dust.noGravity = true;
                    dust.velocity = Vector2.Normalize(offset) * dustVelocity * scale;
                }

                projectile.localAI[1] = 0f;
            }
        }
    }
}
