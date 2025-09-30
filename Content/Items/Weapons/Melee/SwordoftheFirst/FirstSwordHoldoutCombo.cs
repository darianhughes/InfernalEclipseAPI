using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Core.Graphics.Automators;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria;
using Terraria.ModLoader;
using Luminance.Common.Utilities;
using InfernalEclipseAPI.Core.Utils;
using Terraria.ID;
using CalamityMod;
using System.IO;
using InfernalEclipseAPI.Common.Tools.Easings;
using ReLogic.Content;
using Terraria.GameContent;
using Luminance.Core.Graphics;
using InfernalEclipseAPI.Content.Items.Weapons.BossRush.Swordofthe14thGlitch;
using InfernalEclipseAPI.Core.Graphics.Primitives;

namespace InfernalEclipseAPI.Content.Items.Weapons.Melee.SwordoftheFirst
{
    public class FirstSwordHoldoutCombo : ModProjectile, IDrawLocalDistortion
    {
        private List<Vector2> trailPositions;
        private short[] trailIndices;
        private VertexPositionColorTexture[] trailVertices;
        private Matrix compositeVertexMatrix;

        public static readonly short[] QuadIndices = new short[6] { 0, 1, 2, 2, 3, 0 };

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

        public static int FramesPerSwingTicks => Swordofthe1stGlitch.UseTime / MaxUpdates;

        public Quaternion Rotation { get; set; }
        public int VanishTimer { get; set; }
        public int AnimeHitVisualsCountdown { get; set; }
        public int SwingCounter { get; set; }
        public bool OwnerIsDashing { get; set; }
        public bool DontChangeOwnerDirection { get; set; }
        public float OldStartingRotation { get; set; }

        public float ZRotation => MathF.Atan2((Rotation.W * Rotation.Z + Rotation.X * Rotation.Y) * 2f, 1f - (Rotation.Y.Squared() + Rotation.Z.Squared()) * 2f);
        public float AnimationCompletion => InfernalUtilities.Saturate(Time / Swordofthe1stGlitch.UseTime);

        public Player Owner => Main.player[Projectile.owner];
        public ref float Time => ref Projectile.ai[0];
        public ref float HorizontalDirection => ref Projectile.ai[1];
        public ref float StartingRotation => ref Projectile.ai[2];

        public static int MaxUpdates => 5;
        public static int AnimeVisualsDuration => Utilities.SecondsToFrames(0.05f);
        public static float BaseScale => 1.2f;
        public static VertexPositionColorTexture[] SwordQuad { get; private set; }

        public enum AttackMode { Slashes = 0, Dash = 1 }
        public AttackMode Mode { get; set; } = AttackMode.Slashes;

        public override string Texture => "InfernalEclipseAPI/Content/Items/Weapons/Melee/SwordoftheFirst/Swordofthe1stGlitch";

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
                if (projectile.active && projectile.owner == self.whoAmI && projectile.ModProjectile is FirstSwordHoldoutCombo holdout)
                {
                    if (holdout.OwnerIsDashing)
                        self.maxFallSpeed = Swordofthe1stGlitch.PlayerDashSpeed * 1.5f;
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

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 50;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)Mode);
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
            Mode = (AttackMode)reader.ReadByte();
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
                HorizontalDirection = Owner.direction;          // face with the player
                StartingRotation = FacingRotation(Owner);       // 0 (right) or π (left)
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

            // only dash mode can push the player during post-hit visuals
            if (AnimeHitVisualsCountdown > 0 && Mode == AttackMode.Dash)
            {
                Owner.velocity = Vector2.UnitX * HorizontalDirection * Swordofthe1stGlitch.PlayerPostHitSpeed;
                Time = (int)(Swordofthe1stGlitch.UseTime * 0.94f);
                AnimeHitVisualsCountdown--;
            }

            HandleSlashes();

            Time++;

            // end-of-animation chaining
            if (Main.myPlayer == Projectile.owner && AnimationCompletion >= 1f)
            {
                if (Mode == AttackMode.Slashes && Main.mouseLeft && AnimationCompletion >= 0.85f)
                {
                    // lock to current player facing
                    HorizontalDirection = Owner.direction;
                    StartingRotation = FacingRotation(Owner);

                    // ensure non-zero forward vector
                    Projectile.velocity = StartingRotation.ToRotationVector2();

                    // toggle forward <-> upward
                    SwingCounter = (SwingCounter + 1) & 1;

                    if (SwingCounter == 0)
                        OldStartingRotation = StartingRotation;

                    Time = 0f;
                    Projectile.netUpdate = true;
                    return;
                }

                // Dash mode: no spin. Finish after single forward slash.
                VanishTimer = 1;
                Projectile.netUpdate = true;
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
                // Dash: single forward slash only (no spin)
                DoBehavior_SwingForward(forwardStart, forwardAnticipation, forwardSlash, forwardEnd, dashMode: true);
                return;
            }

            // Slashes: forward <-> upward loop
            if ((SwingCounter & 1) == 0)
                DoBehavior_SwingForward(forwardStart, forwardAnticipation, forwardSlash, forwardEnd, dashMode: false);
            else
                DoBehavior_SwingUpward(forwardEnd, upwardSlash, upwardEnd);
        }

        public void StickToOwner()
        {
            Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter);
            Owner.heldProj = Projectile.whoAmI;
            Owner.SetDummyItemTime(2);

            if (!DontChangeOwnerDirection && Time >= 2f && VanishTimer <= 0)
                Owner.ChangeDir((int)HorizontalDirection);

            float num = ZRotation - ((HorizontalDirection == 1f) ? (MathF.PI / 2f) : MathF.PI)
                        - HorizontalDirection * (MathF.PI / 4f) + StartingRotation;

            Owner.SetCompositeArmFront(Math.Abs(num) > 0.01f, Player.CompositeArmStretchAmount.Full, num);

            CreateSlashParticles();
        }

        public void CreateSlashParticles()
        {
            float x = MathHelper.WrapAngle(Projectile.rotation - Projectile.oldRot[1]);
            float mag = MathF.Abs(x);
            if (mag <= 0.1f || Projectile.scale <= 0.5f)
                return;

            Vector2 v = (Projectile.rotation - MathF.PI / 4f).ToRotationVector2();
            Vector2 sweep = v.RotatedBy((float)x.NonZeroSign() * (MathF.PI / 2f));
            Dust d = Dust.NewDustPerfect(Projectile.Center + v * Main.rand.NextFloat(28f, 74f) * Projectile.scale, 264);
            d.velocity = sweep * 3f + Owner.velocity * 0.35f;

            Color lightGray = new Color(210, 210, 210);
            Color darkGray = new Color(110, 110, 110);
            d.color = Color.Lerp(lightGray, darkGray, MathF.Sqrt(Main.rand.NextFloat(0.95f)));

            d.scale = Main.rand.NextFloat(1f, 1.6f);
            d.fadeIn = Main.rand.NextFloat(0.9f);
            d.noGravity = true;
        }

        public void DoBehavior_Vanish()
        {
            if (Projectile.IsFinalExtraUpdate())
                VanishTimer++;

            Projectile.scale = Utilities.InverseLerp(11f, 0f, VanishTimer).Squared() * BaseScale;
            if (Projectile.scale <= 0f)
                Projectile.Kill();
        }

        // Forward slash (shared by both modes). dashMode toggles dash push + timing tweaks.
        void DoBehavior_SwingForward(Quaternion forwardStart, Quaternion forwardAnticipation, Quaternion forwardSlash, Quaternion forwardEnd, bool dashMode)
        {
            if (Main.myPlayer == Projectile.owner && Time < (float)MaxUpdates * 10f && SwingCounter >= 1)
            {
                float targetAngle = Projectile.AngleTo(Main.MouseWorld);
                StartingRotation = StartingRotation.AngleLerp(targetAngle, Time / (float)MaxUpdates / 35f);
            }

            IPiecewiseRotation piecewiseRotation = new IPiecewiseRotation()
                .Add(SineEasing.Default, Common.Tools.Easings.EasingType.Out, forwardAnticipation, 0.5f, forwardStart)
                .Add(PolynomialEasing.Quartic, Common.Tools.Easings.EasingType.In, forwardSlash, 0.7f)
                .Add(PolynomialEasing.Quadratic, Common.Tools.Easings.EasingType.Out, forwardEnd, 1f);

            Rotation = piecewiseRotation.Evaluate(AnimationCompletion, HorizontalDirection == -1f && AnimationCompletion >= 0.7f, 1);
            DontChangeOwnerDirection = true;

            int trig = (int)(Swordofthe1stGlitch.UseTime * (dashMode ? 0.70f : 0.35f));
            if (Time == trig)
            {
                if (dashMode)
                {
                    Vector2 dashDir = Projectile.DirectionToSafe(Main.MouseWorld);
                    Owner.velocity = dashDir * Swordofthe1stGlitch.PlayerDashSpeed;

                    HorizontalDirection = dashDir.X.NonZeroSign();
                    StartingRotation = dashDir.ToRotation();

                    ScreenShakeSystem.StartShakeAtPoint(Projectile.Center, 2.8f);
                    SoundEngine.PlaySound(DashSfx, Projectile.Center);
                }

                SoundEngine.PlaySound(SlashSfx, Projectile.Center);
            }

            OwnerIsDashing = dashMode && AnimationCompletion >= 0.7f && AnimationCompletion < 0.95f;

            if (dashMode && Time == (int)(Swordofthe1stGlitch.UseTime * 0.95f) && AnimeHitVisualsCountdown <= 0)
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

            if (Time == (int)(Swordofthe1stGlitch.UseTime * 0.30f))
            {
                ScreenShakeSystem.StartShakeAtPoint(Projectile.Center, 2.8f);
                SoundEngine.PlaySound(SlashSfx, Projectile.Center);
            }
        }

        public override bool ShouldUpdatePosition() => false;

        // NOTE: clone/extra projectile spawns removed
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Mode == AttackMode.Dash && OwnerIsDashing && AnimeHitVisualsCountdown <= 0)
            {
                if (TerraBladeSilhouetteDrawSystem.SilhouetteOpacity <= 0f)
                    TerraBladeSilhouetteDrawSystem.Subject = target;

                AnimeHitVisualsCountdown = AnimeVisualsDuration;
                ScreenShakeSystem.StartShakeAtPoint(target.Center, 6.4f);
                Owner.SetImmuneTimeForAllTypes(Swordofthe1stGlitch.PlayerPostHitIFrameGracePeriod);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            Vector2 dir = (Projectile.rotation - MathF.PI / 4f).ToRotationVector2();
            Vector2 start = Projectile.Center;
            Vector2 end = start + dir * Projectile.scale * 112f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.width * 0.5f, ref collisionPoint);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 size = tex.Size();

            Matrix world = Matrix.CreateTranslation(new Vector3(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y, 0f));
            Matrix viewProj = Main.GameViewMatrix.TransformationMatrix * Matrix.CreateOrthographicOffCenter(0f, Main.screenWidth, Main.screenHeight, 0f, -150f, 150f);
            Matrix wvp = world * viewProj;
            Matrix rot = Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateRotationZ(StartingRotation);
            Matrix scale = Matrix.CreateScale(Projectile.scale);
            compositeVertexMatrix = rot * scale * wvp;

            Matrix drawMatrix = compositeVertexMatrix;
            if (HorizontalDirection == -1f)
                drawMatrix = Matrix.CreateReflection(new Plane(Vector3.UnitX, 1f)) * Matrix.CreateRotationZ(MathF.PI / 2f) * drawMatrix;

            if (SwordQuad == null)
            {
                var v0 = new VertexPositionColorTexture(new Vector3(0f, -size.Y, 0f), Color.White, Vector2.One * 0.01f);
                var v1 = new VertexPositionColorTexture(new Vector3(size.X, -size.Y, 0f), Color.White, Vector2.UnitX * 0.99f);
                var v2 = new VertexPositionColorTexture(new Vector3(size.X, 0f, 0f), Color.White, Vector2.One * 0.99f);
                var v3 = new VertexPositionColorTexture(new Vector3(0f, 0f, 0f), Color.White, Vector2.UnitY * 0.99f);
                SwordQuad = new[] { v0, v1, v2, v3 };
            }

            DefineAfterimageTrailCache();
            DrawAfterimageTrail();

            var shader = Assets.Effects.ShaderManager.GetShader("PrimitiveProjectionShader");
            shader.TrySetParameter("uWorldViewProjection", drawMatrix);
            shader.Apply();

            Main.instance.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            Main.instance.GraphicsDevice.Textures[1] = tex;
            Main.instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, SwordQuad, 0, SwordQuad.Length, QuadIndices, 0, QuadIndices.Length / 3);
            return false;
        }

        public void DefineAfterimageTrailCache()
        {
            int keyframes = 20;
            int subdiv = 5;
            float radius = 118f;

            trailPositions = new List<Vector2>();
            for (int i = 0; i < keyframes; i++)
            {
                float cur = Projectile.oldRot[i] - Projectile.rotation - MathF.PI / 4f;
                float next = Projectile.oldRot[i + 1] - Projectile.rotation - MathF.PI / 4f;
                for (int j = 0; j < subdiv; j++)
                {
                    float f = cur.AngleLerp(next, (float)j / subdiv);
                    trailPositions.Add(Projectile.Center + f.ToRotationVector2() * radius);
                }
            }

            if (trailPositions.Any())
            {
                float delta = MathHelper.WrapAngle(Projectile.rotation - Projectile.oldRot[1]);
                float sharp = Utilities.InverseLerp(0.056f, 0.1f, MathF.Abs(delta));

                trailVertices = new VertexPositionColorTexture[trailPositions.Count * 2];
                trailIndices = PrimitiveTrail.GetIndicesFromTrailPoints(trailPositions.Count);

                for (int k = 0; k < trailPositions.Count; k++)
                {
                    float t = (float)k / (trailPositions.Count - 1f);
                    Vector2 uv0 = new(t, 0f);
                    Vector2 uv1 = new(t, 1f);
                    Vector2 normal = (trailPositions[k] - Projectile.Center).SafeNormalize(Vector2.UnitY);
                    Vector3 p0 = new(trailPositions[k] - Projectile.Center, 0f);
                    Vector3 p1 = new(trailPositions[k] - Projectile.Center - normal * Projectile.scale * 90f, 0f);
                    Color c = Color.White * sharp;

                    trailVertices[k * 2] = new VertexPositionColorTexture(p0, c, uv0);
                    trailVertices[k * 2 + 1] = new VertexPositionColorTexture(p1, c, uv1);
                }
            }
        }

        public void DrawAfterimageTrail()
        {
            if (trailVertices is null || trailVertices.Length == 0) return;

            var shader = Assets.Effects.ShaderManager.GetShader("GlitchSwordTrailShader");
            shader.SetTexture(LoadDeferred("InfernalEclipseAPI/Assets/Textures/WavyBlotchNoise"), 1, SamplerState.LinearWrap);
            shader.TrySetParameter("colorA", new Vector3(0.85f, 0.85f, 0.85f));
            shader.TrySetParameter("colorB", new Vector3(0.35f, 0.35f, 0.35f));
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
            if (trailVertices == null) return;

            float delta = MathHelper.WrapAngle(Projectile.rotation - Projectile.oldRot[1]);
            float amt = Utilities.InverseLerp(0.04f, 0.09f, MathF.Abs(delta)) * 0.156f;

            VertexPositionColorTexture[] temp = new VertexPositionColorTexture[trailVertices.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                Vector2 tp = trailPositions[i / 2];
                Vector2 v = tp - Projectile.Center;
                float ang = v.ToRotation() - ZRotation;
                float r = Utilities.Cos01(ang);
                float g = Utilities.Sin01(ang);

                temp[i] = trailVertices[i];
                temp[i].Color = new Color(r, g, (1f - (i % 2)) * Utilities.InverseLerp(8f, 32f, i).Squared() * amt);
            }

            var shader = Assets.Effects.ShaderManager.GetShader("GlitchSwordTrailShader");
            shader.SetTexture(LoadDeferred("InfernalEclipseAPI/Assets/Textures/WavyBlotchNoise"), 1, SamplerState.LinearWrap);
            shader.TrySetParameter("colorA", new Vector3(0.85f, 0.85f, 0.85f));
            shader.TrySetParameter("colorB", new Vector3(0.35f, 0.35f, 0.35f));
            shader.TrySetParameter("blackAppearanceInterpolant", -3f);
            shader.TrySetParameter("trailAnimationSpeed", 0.6f);
            shader.TrySetParameter("flatOpacity", false);
            shader.TrySetParameter("uWorldViewProjection", compositeVertexMatrix);
            shader.Apply();

            Main.instance.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            Main.instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, temp, 0, temp.Length, trailIndices, 0, trailIndices.Length / 3);
        }

        public void DrawLocalDistortionExclusion(SpriteBatch spriteBatch) => DrawLocalDistortion(spriteBatch);

        private static Texture2D LoadDeferred(string path)
        {
            if (Main.netMode == NetmodeID.Server)
                return default;
            return ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad).Value;
        }
    }
}
