using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CalamityMod;
using InfernalEclipseAPI.Common.Tools.Easings;
using InfernalEclipseAPI.Core.Graphics.Automators;
using InfernalEclipseAPI.Core.Graphics.Primitives;
using InfernalEclipseAPI.Core.Utils;
using Luminance.Common.Utilities;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using YouBoss.Assets;

namespace InfernalEclipseAPI.Content.Items.Weapons.BossRush.Swordofthe14thGlitch
{
    public class GlitchSwordHoldout : ModProjectile, IDrawLocalDistortion
    {
        private List<Vector2> trailPositions;

        private short[] trailIndices;

        private VertexPositionColorTexture[] trailVertices;

        private Matrix compositeVertexMatrix;

        public static readonly short[] QuadIndices = new short[6] { 0, 1, 2, 2, 3, 0 };

        // Reuseable SFX with high concurrency so both slashes play.
        private static readonly SoundStyle SlashSfx = new("InfernalEclipseAPI/Assets/Sounds/Custom/TerraBlade/Slash")
        {
            MaxInstances = 12,
            SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest,
            PitchVariance = 0.06f
        };

        private static readonly SoundStyle DashSfx = new("InfernalEclipseAPI/Assets/Sounds/Custom/TerraBlade/Dash")
        {
            MaxInstances = 6,
            SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest,
            PitchVariance = 0.03f
        };

        private static float FacingRotation(Player p) => (p.direction == 1) ? 0f : MathF.PI;

        public Quaternion Rotation { get; set; }

        public int VanishTimer { get; set; }

        public int AnimeHitVisualsCountdown { get; set; }

        public int SwingCounter { get; set; }

        public bool OwnerIsDashing { get; set; }

        public bool DontChangeOwnerDirection { get; set; }

        public float OldStartingRotation { get; set; }

        public float ZRotation => MathF.Atan2((Rotation.W * Rotation.Z + Rotation.X * Rotation.Y) * 2f, 1f - (Rotation.Y.Squared() + Rotation.Z.Squared()) * 2f);

        public float AnimationCompletion => Utilities.Saturate(Time / Swordofthe14thGlitch.UseTime);

        public Player Owner => Main.player[base.Projectile.owner];

        public ref float Time => ref base.Projectile.ai[0];

        public ref float HorizontalDirection => ref base.Projectile.ai[1];

        public ref float StartingRotation => ref base.Projectile.ai[2];
        new public static int MaxUpdates => 5;
        public static int AnimeVisualsDuration => Utilities.SecondsToFrames(0.05f);
        public static float BaseScale => 1.2f;
        public static VertexPositionColorTexture[] SwordQuad { get; private set; }
        public enum AttackMode { Slashes = 0, Dash = 1 }
        public AttackMode Mode { get; set; } = AttackMode.Slashes;
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
                if (projectile.active && projectile.owner == self.whoAmI && projectile.ModProjectile is GlitchSwordHoldout zenithGlitchSwordpro)
                {
                    if (zenithGlitchSwordpro.OwnerIsDashing)
                        self.maxFallSpeed = Swordofthe14thGlitch.PlayerDashSpeed * 1.5f;
                }
            }
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 72;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>();
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
            writer.Write((byte)Mode); // << add
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
            Mode = (AttackMode)reader.ReadByte(); // << add
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

        public override void AI()
        {
            if (HorizontalDirection == 0f)
            {
                /* OLD
                StartingRotation = Projectile.velocity.ToRotation();
                HorizontalDirection = Projectile.velocity.X.NonZeroSign();
                Time = 0f;
                Projectile.netUpdate = true;
                */
                HorizontalDirection = Owner.direction;           // <- face with the player
                StartingRotation = FacingRotation(Owner);      // <- 0 (right) or π (left)
                Time = 0f;
                Projectile.netUpdate = true;
            }

            StickToOwner();
            DontChangeOwnerDirection = false;
            OwnerIsDashing = false;

            if (VanishTimer >= 1)
            {
                DoBehavior_Vanish();
                return;
            }

            // Only dash mode is allowed to push the player during hit visuals.
            if (AnimeHitVisualsCountdown > 0 && Mode == AttackMode.Dash)
            {
                Owner.velocity = Vector2.UnitX * HorizontalDirection * Swordofthe14thGlitch.PlayerPostHitSpeed;
                Time = (int)(Swordofthe14thGlitch.UseTime * 0.94f);
                AnimeHitVisualsCountdown--;
            }

            HandleSlashes();

            Time++;

            // Chain logic at end of one animation
            if (Main.myPlayer == Projectile.owner && AnimationCompletion >= 1f)
            {
                /*OLD
                if (Mode == AttackMode.Slashes && Main.mouseLeft)
                {
                    // Continue the two-step loop (forward ↔ upward).
                    Projectile.velocity = Projectile.DirectionToSafe(Main.MouseWorld);
                    SwingCounter = (SwingCounter + 1) & 1; // 0 <-> 1

                    if (SwingCounter == 0) // coming back to forward
                    {
                        HorizontalDirection = Projectile.velocity.X.NonZeroSign();
                        OldStartingRotation = StartingRotation;
                    }

                    Time = 0f;
                    Projectile.netUpdate = true;
                }
                */
                if (Mode == AttackMode.Slashes && Main.mouseLeft && AnimationCompletion >= 0.85f)
                {
                    // lock to current player facing
                    HorizontalDirection = Owner.direction;
                    StartingRotation = FacingRotation(Owner);

                    // ensure we have a non-zero forward vector (don’t aim at mouse)
                    Projectile.velocity = StartingRotation.ToRotationVector2();

                    // toggle forward <-> upward
                    SwingCounter = (SwingCounter + 1) & 1;

                    // when we come back to forward, reseed rotation/facing
                    if (SwingCounter == 0)
                    {
                        OldStartingRotation = StartingRotation;
                    }

                    Time = 0f;
                    Projectile.netUpdate = true;
                    return;
                }
                else if (Mode == AttackMode.Dash && SwingCounter == 0)
                {
                    // Right-click: after forward, do one spin.
                    SwingCounter = 1; // spin
                    Time = 0f;
                    Projectile.netUpdate = true;
                }
                else
                {
                    // Dash after spin, or left-click released.
                    VanishTimer = 1;
                    Projectile.netUpdate = true;
                }
            }

            Projectile.rotation = StartingRotation + ZRotation;
        }

        private Quaternion EulerAnglesConversion(float angle2D, float angleSide = 0f)
        {
            float theta = angle2D * HorizontalDirection + ((HorizontalDirection == -1f) ? (MathF.PI / 2f) : 0f);
            return Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationZ(Utilities.WrapAngle360(theta)) * Matrix.CreateRotationX(angleSide));
        }

        public void HandleSlashes()
        {
            Quaternion forwardStart = EulerAnglesConversion(-0.06f, 0.12f);
            Quaternion forwardAnticipation = EulerAnglesConversion(-1.96f, -0.47f);
            Quaternion forwardSlash = EulerAnglesConversion(2.65f, -0.80f);
            Quaternion forwardEnd = EulerAnglesConversion(3.95f, 0.15f);
            Quaternion upwardSlash = EulerAnglesConversion(-0.06f, 0.45f);
            Quaternion upwardEnd = EulerAnglesConversion(-0.07f, 0.55f);

            if (Mode == AttackMode.Dash)
            {
                if ((SwingCounter & 1) == 0)
                    DoBehavior_SwingForward(forwardStart, forwardAnticipation, forwardSlash, forwardEnd);
                else
                    Behavior_SpinAround();
                return;
            }

            // Mode == Slashes
            if ((SwingCounter & 1) == 0)
                DoBehavior_SwingForward(forwardStart, forwardAnticipation, forwardSlash, forwardEnd);
            else
                DoBehavior_SwingUpward(forwardEnd, upwardSlash, upwardEnd);
        }

        public void StickToOwner()
        {
            /* OLD
            Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter);
            Owner.heldProj = base.Projectile.whoAmI;
            Owner.SetDummyItemTime(2);
            if (!DontChangeOwnerDirection && Time >= 2f && VanishTimer <= 0)
            {
                Owner.ChangeDir(InfernalUtilities.AngleToXDirection(ZRotation) * (int)HorizontalDirection);
            }

            float num = ZRotation - ((HorizontalDirection == 1f) ? (MathF.PI / 2f) : MathF.PI) - HorizontalDirection * (MathF.PI / 4f) + StartingRotation;
            Owner.SetCompositeArmFront(Math.Abs(num) > 0.01f, Player.CompositeArmStretchAmount.Full, num);
            CreateSlashParticles();
            */
            Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter);
            Owner.heldProj = Projectile.whoAmI;
            Owner.SetDummyItemTime(2);

            // keep the player’s sprite facing the swing direction we chose
            if (!DontChangeOwnerDirection && Time >= 2f && VanishTimer <= 0)
                Owner.ChangeDir((int)HorizontalDirection);

            float num = ZRotation - ((HorizontalDirection == 1f) ? (MathF.PI / 2f) : MathF.PI)
                        - HorizontalDirection * (MathF.PI / 4f) + StartingRotation;

            Owner.SetCompositeArmFront(Math.Abs(num) > 0.01f, Player.CompositeArmStretchAmount.Full, num);

            CreateSlashParticles();
        }

        public void CreateSlashParticles()
        {
            float x = MathHelper.WrapAngle(base.Projectile.rotation - base.Projectile.oldRot[1]);
            float num = MathF.Abs(x);
            if (!(num <= 0.1f) && !(base.Projectile.scale <= 0.5f))
            {
                Vector2 vector = (base.Projectile.rotation - MathF.PI / 4f).ToRotationVector2();
                Vector2 vector2 = vector.RotatedBy((float)x.NonZeroSign() * (MathF.PI / 2f));
                Dust dust = Dust.NewDustPerfect(base.Projectile.Center + vector * Main.rand.NextFloat(28f, 74f) * base.Projectile.scale, 264);
                dust.velocity = vector2 * 3f + Owner.velocity * 0.35f;
                dust.color = Color.Lerp(
                    new Color(255, 96, 96),   // light red
                    new Color(255, 32, 32),   // deep red
                    MathF.Sqrt(Main.rand.NextFloat(0.95f))
                );
                dust.scale = Main.rand.NextFloat(1f, 1.6f);
                dust.fadeIn = Main.rand.NextFloat(0.9f);
                dust.noGravity = true;
            }
        }

        public void DoBehavior_Vanish()
        {
            if (base.Projectile.IsFinalExtraUpdate())
            {
                VanishTimer++;
            }

            base.Projectile.scale = Utilities.InverseLerp(11f, 0f, VanishTimer).Squared() * BaseScale;
            if (base.Projectile.scale <= 0f)
            {
                base.Projectile.Kill();
            }
        }

        void DoBehavior_SwingForward(Quaternion forwardStart, Quaternion forwardAnticipation, Quaternion forwardSlash, Quaternion forwardEnd)
        {
            if (Main.myPlayer == Projectile.owner && Time < (float)MaxUpdates * 10f && SwingCounter >= 1)
            {
                float targetAngle = Projectile.AngleTo(Main.MouseWorld);
                StartingRotation = StartingRotation.AngleLerp(targetAngle, Time / (float)MaxUpdates / 35f);
            }

            bool first = SwingCounter == 0;
            IPiecewiseRotation piecewiseRotation = new IPiecewiseRotation()
                .Add(SineEasing.Default, Common.Tools.Easings.EasingType.Out, forwardAnticipation, 0.5f, forwardStart)
                .Add(PolynomialEasing.Quartic, Common.Tools.Easings.EasingType.In, forwardSlash, 0.7f)
                .Add(PolynomialEasing.Quadratic, Common.Tools.Easings.EasingType.Out, forwardEnd, 1f);

            Rotation = piecewiseRotation.Evaluate(AnimationCompletion, HorizontalDirection == -1f && AnimationCompletion >= 0.7f, 1);
            DontChangeOwnerDirection = true;

            bool dashMode = (Mode == AttackMode.Dash);
            int trig = (int)(Swordofthe14thGlitch.UseTime * (dashMode ? 0.70f : 0.35f));

            if (Time == trig)
            {
                if (dashMode)
                {
                    Vector2 dashDir = Projectile.DirectionToSafe(Main.MouseWorld); // <-- aim at cursor
                    Owner.velocity = dashDir * Swordofthe14thGlitch.PlayerDashSpeed;

                    // lock facing to dash direction so it never “only dashes right”
                    HorizontalDirection = dashDir.X.NonZeroSign();
                    StartingRotation = dashDir.ToRotation();

                    ScreenShakeSystem.StartShakeAtPoint(Projectile.Center, 2.8f);
                    SoundEngine.PlaySound(DashSfx, Projectile.Center);
                }

                SoundEngine.PlaySound(SlashSfx, Projectile.Center); // forward slash SFX
            }

            OwnerIsDashing = dashMode && AnimationCompletion >= 0.7f && AnimationCompletion < 0.95f;

            if (dashMode && Time == (int)(Swordofthe14thGlitch.UseTime * 0.95f) && AnimeHitVisualsCountdown <= 0)
                Owner.velocity *= 0.17f;

            if (SwingCounter <= 0)
                Projectile.scale = Utilities.InverseLerp(0f, 0.18f, AnimationCompletion) * BaseScale;
        }

        void DoBehavior_SwingUpward(Quaternion forwardEnd, Quaternion upwardSlash, Quaternion upwardEnd)
        {
            IPiecewiseRotation piecewiseRotation = new IPiecewiseRotation()
                .Add(new PolynomialEasing(12f), Common.Tools.Easings.EasingType.InOut, upwardSlash, 0.9f, forwardEnd)
                .Add(PolynomialEasing.Quadratic, Common.Tools.Easings.EasingType.In, upwardEnd, 1f);

            Rotation = piecewiseRotation.Evaluate(AnimationCompletion, AnimationCompletion < 0.85f, -1);
            DontChangeOwnerDirection = true;

            if (Time == (int)(Swordofthe14thGlitch.UseTime * 0.30f))
            {
                ScreenShakeSystem.StartShakeAtPoint(Projectile.Center, 2.8f);
                SoundEngine.PlaySound(SlashSfx, Projectile.Center); // slash on upward
            }
        }

        public void Behavior_SpinAround()
        {
            /* OLD CODE
            float angleSide = Utils.MultiLerp(AnimationCompletion.Squared(), 0.55f, 1.16f, 0f);
            float angle2D = MathF.PI * new PolynomialEasing(3.5f).Evaluate(Common.Tools.Easings.EasingType.InOut, 1f - AnimationCompletion) * -4f;
            Rotation = EulerAnglesConversion(angle2D, angleSide);
            if (Time == (float)(int)((float)Swordofthe14thGlitch.UseTime * 0.25f))
            {
                ScreenShakeSystem.StartShakeAtPoint(base.Projectile.Center, 5f);
                SoundEngine.PlaySound(new("InfernalEclipseAPI/Assets/Sounds/Custom/TerraBlade/Split"), base.Projectile.Center);
            }

            if (Main.myPlayer == base.Projectile.owner && AnimationCompletion >= 0.25f && Time % 3f == 0f)
            {
                float num = MathF.Cos(StartingRotation).NonZeroSign();
                Vector2 spinningpoint = (MathF.PI * 2f * Utilities.InverseLerp(0.25f, 1f, AnimationCompletion) * num + StartingRotation - MathF.PI / 4f).ToRotationVector2() * new Vector2(1f, 0.4f) * Swordofthe14thGlitch.HomingBeamStartingSpeed;
                Projectile.NewProjectile(base.Projectile.GetSource_FromThis(), base.Projectile.Center, spinningpoint.RotatedBy(StartingRotation), ModContent.ProjectileType<HomingTerraBeam>(), (int)((float)base.Projectile.damage * Swordofthe14thGlitch.HomingBeamDamageFactor), 0f, base.Projectile.owner);
            }
            */

            float angleSide = Utils.MultiLerp(AnimationCompletion.Squared(), 0.55f, 1.16f, 0f);
            float angle2D = MathF.PI * new PolynomialEasing(3.5f).Evaluate(EasingType.InOut, 1f - AnimationCompletion) * -4f;
            Rotation = EulerAnglesConversion(angle2D, angleSide);

            if (Time == (int)(Swordofthe14thGlitch.UseTime * 0.25f))
            {
                ScreenShakeSystem.StartShakeAtPoint(Projectile.Center, 5f);
                SoundEngine.PlaySound(new("InfernalEclipseAPI/Assets/Sounds/Custom/TerraBlade/Split"), Projectile.Center);
            }

            // Spawn beams during spin
            if (AnimationCompletion >= 0.20f && Time % 3 == 0)
            {
                // make them fan around the sword as it spins
                float swirl = MathHelper.TwoPi * Utilities.InverseLerp(0.20f, 1f, AnimationCompletion) * MathF.Sign(HorizontalDirection);
                float shootAngle = StartingRotation + swirl - MathF.PI / 4f;
                Vector2 dir = shootAngle.ToRotationVector2();

                Vector2 vel = dir * Swordofthe14thGlitch.HomingBeamStartingSpeed;
                int damage = (int)(Projectile.damage * Swordofthe14thGlitch.HomingBeamDamageFactor);

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    vel,
                    ModContent.ProjectileType<HomingTerraBeam>(),
                    damage, 0f, Projectile.owner
                );
            }
        }

        // This projectile should remain glued to the owner's hand, and not move.
        public override bool ShouldUpdatePosition() => false;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.myPlayer == base.Projectile.owner)
            {
                Vector2 vector = target.Center + Main.rand.NextVector2Circular(10f, 10f);
                Vector2 vector2 = base.Projectile.DirectionToSafe(target.Center).RotatedByRandom(2.0944998264312744);
                Projectile.NewProjectile(base.Projectile.GetSource_FromThis(), vector, vector2, ModContent.ProjectileType<TerraSlash>(), (int)((float)base.Projectile.damage * Swordofthe14thGlitch.HomingSlashDamageFactor), 0f, base.Projectile.owner);
                Vector2 vector3 = vector - vector2 * Main.rand.NextFloat(275f, 336f);
                Vector2 velocity = (target.Center - vector3).SafeNormalize(Vector2.UnitY) * 42f;
                Projectile.NewProjectile(base.Projectile.GetSource_FromThis(), vector3, velocity, ModContent.ProjectileType<PlayerShadowClone>(), (int)((float)base.Projectile.damage * Swordofthe14thGlitch.HomingSlashDamageFactor), 0f, base.Projectile.owner);
            }

            if (Mode == AttackMode.Dash && OwnerIsDashing && AnimeHitVisualsCountdown <= 0)
            {
                if (TerraBladeSilhouetteDrawSystem.SilhouetteOpacity <= 0f)
                    TerraBladeSilhouetteDrawSystem.Subject = target;

                AnimeHitVisualsCountdown = AnimeVisualsDuration;
                ScreenShakeSystem.StartShakeAtPoint(target.Center, 6.4f);
                Owner.SetImmuneTimeForAllTypes(Swordofthe14thGlitch.PlayerPostHitIFrameGracePeriod);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            Vector2 vector = (base.Projectile.rotation - MathF.PI / 4f).ToRotationVector2();
            Vector2 center = base.Projectile.Center;
            Vector2 lineEnd = center + vector * base.Projectile.scale * 112f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), center, lineEnd, (float)base.Projectile.width * 0.5f, ref collisionPoint);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value = TextureAssets.Projectile[base.Type].Value;
            Vector2 vector = value.Size();
            Matrix matrix = Matrix.CreateTranslation(new Vector3(base.Projectile.Center.X - Main.screenPosition.X, base.Projectile.Center.Y - Main.screenPosition.Y, 0f));
            Matrix matrix2 = Matrix.CreateOrthographicOffCenter(0f, Main.screenWidth, Main.screenHeight, 0f, -150f, 150f);
            Matrix matrix3 = matrix * Main.GameViewMatrix.TransformationMatrix * matrix2;
            Matrix matrix4 = Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateRotationZ(StartingRotation);
            Matrix matrix5 = Matrix.CreateScale(base.Projectile.scale);
            compositeVertexMatrix = matrix4 * matrix5 * matrix3;
            Matrix matrix6 = compositeVertexMatrix;
            if (HorizontalDirection == -1f)
            {
                matrix6 = Matrix.CreateReflection(new Plane(Vector3.UnitX, 1f)) * Matrix.CreateRotationZ(MathF.PI / 2f) * matrix6;
            }

            if (SwordQuad == null)
            {
                VertexPositionColorTexture vertexPositionColorTexture = new VertexPositionColorTexture(new Vector3(0f, 0f - vector.Y, 0f), Color.White, Vector2.One * 0.01f);
                VertexPositionColorTexture vertexPositionColorTexture2 = new VertexPositionColorTexture(new Vector3(vector.X, 0f - vector.Y, 0f), Color.White, Vector2.UnitX * 0.99f);
                VertexPositionColorTexture vertexPositionColorTexture3 = new VertexPositionColorTexture(new Vector3(vector.X, 0f, 0f), Color.White, Vector2.One * 0.99f);
                VertexPositionColorTexture vertexPositionColorTexture4 = new VertexPositionColorTexture(new Vector3(0f, 0f, 0f), Color.White, Vector2.UnitY * 0.99f);
                SwordQuad = new VertexPositionColorTexture[4] { vertexPositionColorTexture, vertexPositionColorTexture2, vertexPositionColorTexture3, vertexPositionColorTexture4 };
            }

            DefineAfterimageTrailCache();
            DrawAfterimageTrail();
            Assets.Effects.ManagedShader shader = Assets.Effects.ShaderManager.GetShader("PrimitiveProjectionShader");
            shader.TrySetParameter("uWorldViewProjection", matrix6);
            shader.Apply();
            Main.instance.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            Main.instance.GraphicsDevice.Textures[1] = value;
            Main.instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, SwordQuad, 0, SwordQuad.Length, QuadIndices, 0, QuadIndices.Length / 3);
            return false;
        }

        public void DefineAfterimageTrailCache()
        {
            int num = 20;
            int num2 = 5;
            float num3 = 118f;
            trailPositions = new List<Vector2>();
            for (int i = 0; i < num; i++)
            {
                float curAngle = base.Projectile.oldRot[i] - base.Projectile.rotation - MathF.PI / 4f;
                float targetAngle = base.Projectile.oldRot[i + 1] - base.Projectile.rotation - MathF.PI / 4f;
                for (int j = 0; j < num2; j++)
                {
                    float f = curAngle.AngleLerp(targetAngle, (float)j / (float)num2);
                    trailPositions.Add(base.Projectile.Center + f.ToRotationVector2() * num3);
                }
            }

            if (trailPositions.Any())
            {
                float x = MathHelper.WrapAngle(base.Projectile.rotation - base.Projectile.oldRot[1]);
                float x2 = MathF.Abs(x);
                float num4 = Utilities.InverseLerp(0.056f, 0.1f, x2);
                trailVertices = new VertexPositionColorTexture[trailPositions.Count * 2];
                trailIndices = PrimitiveTrail.GetIndicesFromTrailPoints(trailPositions.Count);
                for (int k = 0; k < trailPositions.Count; k++)
                {
                    float x3 = (float)k / ((float)trailPositions.Count - 1f);
                    Vector2 textureCoordinate = new Vector2(x3, 0f);
                    Vector2 textureCoordinate2 = new Vector2(x3, 1f);
                    Vector2 vector = (trailPositions[k] - base.Projectile.Center).SafeNormalize(Vector2.UnitY);
                    Vector3 position = new Vector3(trailPositions[k] - base.Projectile.Center, 0f);
                    Vector3 position2 = new Vector3(trailPositions[k] - base.Projectile.Center - vector * base.Projectile.scale * 90f, 0f);
                    Color color = Color.White * num4;
                    trailVertices[k * 2] = new VertexPositionColorTexture(position, color, textureCoordinate);
                    trailVertices[k * 2 + 1] = new VertexPositionColorTexture(position2, color, textureCoordinate2);
                }
            }
        }

        public void DrawAfterimageTrail()
        {
            Assets.Effects.ManagedShader shader = Assets.Effects.ShaderManager.GetShader("GlitchSwordTrailShader");
            shader.SetTexture(LoadDeferred("InfernalEclipseAPI/Assets/Textures/WavyBlotchNoise"), 1, SamplerState.LinearWrap);
            shader.TrySetParameter("colorA", new Vector3(1f, 0.25f, 0.25f)); // light red
            shader.TrySetParameter("colorB", new Vector3(1f, 0.0f, 0.0f));  // pure red
            shader.TrySetParameter("blackAppearanceInterpolant", 0.36f);
            shader.TrySetParameter("trailAnimationSpeed", 1.2f);
            shader.TrySetParameter("flatOpacity", false);
            shader.TrySetParameter("uWorldViewProjection", compositeVertexMatrix);
            shader.Apply();
            Main.instance.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            Main.instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, trailVertices, 0, trailVertices.Length, trailIndices, 0, trailIndices.Length / 3);
        }

        public void DrawLocalDistortion(SpriteBatch spriteBatch)
        {
            if (trailVertices != null)
            {
                float x = MathHelper.WrapAngle(base.Projectile.rotation - base.Projectile.oldRot[1]);
                float x2 = MathF.Abs(x);
                float num = Utilities.InverseLerp(0.04f, 0.09f, x2) * 0.156f;
                VertexPositionColorTexture[] array = new VertexPositionColorTexture[trailVertices.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    Vector2 vector = trailPositions[i / 2];
                    Vector2 v = vector - base.Projectile.Center;
                    float x3 = v.ToRotation() - ZRotation;
                    float r = Utilities.Cos01(x3);
                    float g = Utilities.Sin01(x3);
                    array[i] = trailVertices[i];
                    array[i].Color = new Color(r, g, (1f - (float)(i % 2)) * Utilities.InverseLerp(8f, 32f, i).Squared() * num);
                }

                Assets.Effects.ManagedShader shader = Assets.Effects.ShaderManager.GetShader("GlitchSwordTrailShader");
                shader.SetTexture(LoadDeferred("InfernalEclipseAPI/Assets/Textures/WavyBlotchNoise"), 1, SamplerState.LinearWrap);
                shader.TrySetParameter("colorA", new Vector3(1f, 0.25f, 0.25f));
                shader.TrySetParameter("colorB", new Vector3(1f, 0.00f, 0.00f));
                shader.TrySetParameter("blackAppearanceInterpolant", -3f);
                shader.TrySetParameter("trailAnimationSpeed", 0.6f);
                shader.TrySetParameter("flatOpacity", false);
                shader.TrySetParameter("uWorldViewProjection", compositeVertexMatrix);
                shader.Apply();
                Main.instance.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                Main.instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, array, 0, array.Length, trailIndices, 0, trailIndices.Length / 3);
            }
        }

        public void DrawLocalDistortionExclusion(SpriteBatch spriteBatch)
        {
            DrawLocalDistortion(spriteBatch);
        }

        private static Texture2D LoadDeferred(string path)
        {
            // Don't attempt to load anything server-side.
            if (Main.netMode == NetmodeID.Server)
                return default;

            return ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad).Value;
        }
    }
}
