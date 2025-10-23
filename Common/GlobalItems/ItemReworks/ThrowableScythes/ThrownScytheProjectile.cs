using System.Collections.Generic;
using InfernalEclipseAPI.Common.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using ThoriumMod.Projectiles.Scythe;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks.ThrowableScythes
{
    [ExtendsFromMod("ThoriumMod")]
    public class ThrownScytheProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            List<ModItem> throwableScythes = new();

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                if (entity.type == thorium.Find<ModProjectile>("BatScythePro").Type) return true;
                //if (entity.type == thorium.Find<ModProjectile>("TitanScythePro").Type) return true;
                if (entity.type == thorium.Find<ModProjectile>("IceShaverPro").Type) return true;
                if (entity.type == thorium.Find<ModProjectile>("DemoniteScythePro").Type) return true;
                if (entity.type == thorium.Find<ModProjectile>("CrimtaneScythePro").Type) return true;
                if (entity.type == thorium.Find<ModProjectile>("FallingTwilightPro").Type) return true;
                if (entity.type == thorium.Find<ModProjectile>("BloodHarvestPro").Type) return true;

                if (entity.type == thorium.Find<ModProjectile>("BoneReaperPro").Type) return true;
                if (entity.type == thorium.Find<ModProjectile>("LustrousBatonPro").Type) return true;
                //if (entity.type == thorium.Find<ModProjectile>("TrueFallingTwilightPro").Type) return true;
                //if (entity.type == thorium.Find<ModProjectile>("TrueBloodHarvestPro").Type) return true;
                if (entity.type == thorium.Find<ModProjectile>("MorningDewPro").Type) return true;
                //if (entity.type == thorium.Find<ModProjectile>("TerraScythePro").Type) return true;
                if (entity.type == thorium.Find<ModProjectile>("ChristmasCheerPro").Type) return true;
                if (entity.type == thorium.Find<ModProjectile>("DreadTearerPro").Type) return true;
                if (entity.type == thorium.Find<ModProjectile>("TheBlackScythePro").Type) return true;
            }

            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
            {
                if (entity.type == ragnarok.Find<ModProjectile>("ProfanedScythePro").Type) return true;
                if (entity.type == ragnarok.Find<ModProjectile>("ScoriaDualscythePro").Type) return true;
            }

            if (ModLoader.TryGetMod("CalamityBardHealer", out Mod calbardhealer))
            {
                if (entity.type == calbardhealer.Find<ModProjectile>("HyphaeBaton").Type) return true;
            }

            foreach (var item in throwableScythes)
            {
                if (item != null && entity.type == item.Type)
                    return true;
            }
            return false;
        }

        public override bool InstancePerEntity => true;

        public float rotationSpeed; // Base rotation speed
        public int scytheCount;
        public int dustCount;
        public int dustType;
        public Vector2 dustOffset = Vector2.Zero;

        public Vector2 DustCenterBase;

        public Vector2 DustCenter => DustCenterBase + dustOffset;

        public override bool PreAI(Projectile projectile)
        {
            if (projectile.ModProjectile is ScythePro scythePro)
            {
                rotationSpeed = scythePro.rotationSpeed;
                scytheCount = scythePro.scytheCount;
                dustCount = scythePro.dustCount;
                dustType = scythePro.dustType;
                dustOffset = scythePro.dustOffset;
                DustCenterBase = new Vector2(projectile.width / 2f, projectile.height / 2f);
            }

            if (projectile.ai[0] != 0f) // Thrown mode
            {
                if (projectile.TryGetGlobalProjectile(out ProjectileBalanceChanges scaler))
                    scaler.EnsureScaled(projectile);

                Player player = Main.player[projectile.owner];
                player.heldProj = projectile.whoAmI;

                // Face the correct way based on aim
                player.ChangeDir(projectile.velocity.X < 0f ? 1 : -1);
                projectile.spriteDirection = player.direction;

                // Spin faster while thrown
                projectile.rotation += rotationSpeed * projectile.spriteDirection * 2.5f;

                // Keep projectile alive for full ai[1] duration
                if (projectile.ai[1] - projectile.ai[2] > projectile.timeLeft)
                    projectile.timeLeft++;

                float attackTime = (++projectile.ai[2]) / projectile.ai[1];

                float v = projectile.velocity.Length();

                // keep width the same, make it taller
                float widthScale = GetCustomWidth(projectile);  // horizontal radius
                float heightScale = GetCustomHeight(projectile); // vertical radius

                Vector2 orbit = Vector2.UnitX.RotatedBy(attackTime * MathHelper.TwoPi);

                // X = width, Y = height (respect ai[0] if you want vertical to scale with your throw factor)
                orbit *= new Vector2(
                    v * widthScale,
                    v * heightScale * Math.Max(projectile.ai[0], 0.001f) * projectile.spriteDirection
                );

                orbit = orbit.RotatedBy(projectile.velocity.ToRotation());
                projectile.Center = player.MountedCenter + orbit - projectile.velocity;

                // Play sound when rotation loops
                if (projectile.rotation > Math.PI)
                {
                    SoundEngine.PlaySound(SoundID.Item1, projectile.Center);
                    projectile.rotation -= MathHelper.TwoPi;
                }
                else if (projectile.rotation < -Math.PI)
                {
                    SoundEngine.PlaySound(SoundID.Item1, projectile.Center);
                    projectile.rotation += MathHelper.TwoPi;
                }

                SpawnDust(projectile);

                return false; // Skip normal scythe AI
            }

            return true; // Normal scythe AI for left-click swing
        }

        private float GetCustomWidth(Projectile projectile)
        {
            //thorium ones can be added here without checking for the mod being loaded
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                //if (projectile.type == thorium.Find<ModProjectile>("TitanScythePro").Type) return 1.2f;
                //if (projectile.type == thorium.Find<ModProjectile>("BatScythePro").Type) return 1.1f;
            }

            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
            {
                //add custom for ragnarok widths here
            }
            return 1.15f; //default
        }

        private float GetCustomHeight(Projectile projectile)
        {
            //thorium ones can be added here
            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
            {
                //add custom for ragnarok heights here
            }
            return 1.5f; //default
        }

        private void SpawnDust(Projectile projectile)
        {
            int num = dustCount;
            int num2 = scytheCount;
            int num3 = dustType;
            Vector2 dustCenter = DustCenter;
            if (num2 <= 0 || num <= 0 || num3 <= -1)
            {
                return;
            }

            for (int i = 0; i < num2; i++)
            {
                float num4 = (float)i * ((float)Math.PI * 2f / (float)num2);
                float rotation = projectile.rotation;
                Vector2 val = dustCenter;
                if (projectile.spriteDirection < 0)
                {
                    val.X = 0f - val.X;
                }

                val = Utils.RotatedBy(val, (double)(rotation + num4), default);
                Vector2 val2 = projectile.Center + new Vector2(0f, projectile.gfxOffY) + val;
                for (int j = 0; j < num; j++)
                {
                    Dust val3 = Dust.NewDustPerfect(val2, num3, (Vector2?)Vector2.Zero, 0, default(Color), 1f);
                    val3.noGravity = true;
                    val3.noLight = true;
                    ModifyDust(projectile, val3, val2, i);
                }
            }
        }
        public void ModifyDust(Projectile projectile, Dust dust, Vector2 position, int scytheIndex)
        {
            if (projectile.ModProjectile is ScythePro scythePro)
            {
                scythePro.ModifyDust(dust, position, scytheIndex);
            }
        }   
    }
}
