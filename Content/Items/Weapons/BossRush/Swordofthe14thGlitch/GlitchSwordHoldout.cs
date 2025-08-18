using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using YouBoss.Assets;
using YouBoss.Common.Tools.Easings;
using YouBoss.Core.Graphics.Automators;
using YouBoss.Core.Graphics.Primitives;
using YouBoss.Core.Graphics.Shaders;
using YouBoss.Core.Graphics.SpecificEffectManagers;
using static YouBoss.Content.Items.ItemReworks.FirstFractal;
using static Microsoft.Xna.Framework.MathHelper;
using YouBoss.Content.Items.ItemReworks;

namespace InfernalEclipseAPI.Content.Items.Weapons.BossRush.Swordofthe14thGlitch
{
    public class GlitchSwordHoldout : FirstFractalHoldout
    {
        private List<Vector2> trailPositions;

        private short[] trailIndices;

        private VertexPositionColorTexture[] trailVertices;

        private Matrix compositeVertexMatrix;
        new public static int MaxUpdates => 5;
        public override string Texture => "InfernalEclipseAPI/Content/Items/Weapons/BossRush/Swordofthe14thGlitch/Swordofthe14thGlitch";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 120;

            On_Player.ResizeHitbox += DisableMaxFallSpeed;
        }

        private void DisableMaxFallSpeed(On_Player.orig_ResizeHitbox orig, Player self)
        {
            orig(self);

            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.active && projectile.owner == self.whoAmI && projectile.ModProjectile is GlitchSwordHoldout firstFractal)
                {
                    if (firstFractal.OwnerIsDashing)
                        self.maxFallSpeed = PlayerDashSpeed * 1.5f;
                }
            }
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 72;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 720000;
            Projectile.MaxUpdates = MaxUpdates;
            Projectile.noEnchantmentVisuals = true;

            // Use local i-frames.
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = MaxUpdates * 3;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(OwnerIsDashing);

            writer.Write(VanishTimer);
            writer.Write(SwingCounter);

            writer.Write(OldStartingRotation);
            writer.Write(Projectile.rotation);

            writer.Write(Rotation.X);
            writer.Write(Rotation.Y);
            writer.Write(Rotation.Z);
            writer.Write(Rotation.W);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            OwnerIsDashing = reader.ReadBoolean();

            VanishTimer = reader.ReadInt32();
            SwingCounter = reader.ReadInt32();

            OldStartingRotation = reader.ReadSingle();
            Projectile.rotation = reader.ReadSingle();

            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            float z = reader.ReadSingle();
            float w = reader.ReadSingle();
            Rotation = new(x, y, z, w);
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.mount.Active)
            {
                player.mount.Dismount(player);
            }

            return base.PreAI();
        }

        public override void AI()
        {
            base.AI();
        }

        private Quaternion EulerAnglesConversion(float angle2D, float angleSide = 0f)
        {
            float forwardRotationOffset = angle2D * HorizontalDirection + (HorizontalDirection == -1f ? PiOver2 : 0f);
            return ManualEulerZX(forwardRotationOffset, angleSide);
        }
        public static Quaternion ManualEulerZX(float angleZ, float angleX)
        {
            // Optionally wrap angleZ to [0, 2π)
            while (angleZ < 0f)
                angleZ += TwoPi;
            while (angleZ >= TwoPi)
                angleZ -= TwoPi;

            // Half angles for quaternion formula
            float halfZ = angleZ * 0.5f;
            float halfX = angleX * 0.5f;

            // Calculate sin and cos for each
            float cz = (float)Math.Cos(halfZ);
            float sz = (float)Math.Sin(halfZ);
            float cx = (float)Math.Cos(halfX);
            float sx = (float)Math.Sin(halfX);

            // Combine Z then X rotation manually (order matters!)
            return new Quaternion(
                sx * cz,  // X
                sx * sz,  // Y
                cx * sz,  // Z
                cx * cz   // W
            );
        }

        // This projectile should remain glued to the owner's hand, and not move.
        public override bool ShouldUpdatePosition() => false;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = 0f;
            Vector2 swordDirection = (Projectile.rotation - PiOver4).ToRotationVector2();
            Vector2 start = Projectile.Center;
            Vector2 end = start + swordDirection * Projectile.scale * 112f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.width * 0.5f, ref _);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            base.PreDraw(ref lightColor);

            return false;
        }
    }
}
