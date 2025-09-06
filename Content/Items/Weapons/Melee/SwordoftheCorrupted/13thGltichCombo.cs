using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Luminance.Core.Graphics;
using InfernalEclipseAPI.Core.Graphics.Automators;
using InfernalEclipseAPI.Common.Tools.Easings;
using ReLogic.Content;
using InfernalEclipseAPI.Core.Utils;
using Luminance.Common.Utilities;
using InfernalEclipseAPI.Content.Items.Weapons.BossRush.Swordofthe14thGlitch;
using InfernalEclipseAPI.Core.Graphics.Primitives;
using CalamityMod.Buffs.StatBuffs;


namespace InfernalEclipseAPI.Content.Items.Weapons.Melee.SwordoftheCorrupted
{
    public class _13thGltichCombo : ModProjectile, IDrawLocalDistortion
    {
        // ========= NEW Tuning (non-visual) =========
        // Per-swing i-frames: one hit per action
        public static int FramesPerAction => Swordofthe13thGlitch.UseTime / MaxUpdates;

        // RMB shotgun (wider & denser)
        const int ShotgunCount = 11;
        const float ShotgunSpreadDegrees = 62f;
        const float ShotgunSpeed = 30f;
        const float ShotgunDamageFactor = 0.50f; // buffed

        // LMB (Rage) spawn
        const int RageCloneCount = 3;
        const float RageCloneDamageFactor = 2.25f; // buffed
        const float BlackSlashDamageFactor = 1.75f; // buffed
        const int ExtraBlackSlashes = 2;
        const float RageFireAt = 0.20f; // at 20% through the spin

        // ========= Original fields (unchanged) =========
        private List<Vector2> trailPositions;
        private short[] trailIndices;
        private VertexPositionColorTexture[] trailVertices;
        private Matrix compositeVertexMatrix;
        public static readonly short[] QuadIndices = new short[6] { 0, 1, 2, 2, 3, 0 };
        private bool HideDuringRage => Mode == AttackMode.Dash && IsCalamityRageActive(Owner);

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

        private float SpeedMult
        {
            get
            {
                float m = 1f;
                if (Mode == AttackMode.Slashes) m *= 1.5f;                 // RMB 1.5x faster
                if (IsCalamityRageActive(Owner)) m *= 2.5f;                  // Rage 2x faster
                return m;
            }
        }

        // UseTime that respects speed scaling
        private float EffectiveUseTime => Swordofthe13thGlitch.UseTime / SpeedMult;

        // Keep "one hit per action" despite speed changes
        private int EffectiveFramesPerAction =>
            Math.Max(1, (int)Math.Round(EffectiveUseTime / MaxUpdates));

        public float AnimationCompletion => Utilities.Saturate(Time / EffectiveUseTime);
        public Player Owner => Main.player[Projectile.owner];
        public ref float Time => ref Projectile.ai[0];
        public ref float HorizontalDirection => ref Projectile.ai[1];
        public ref float StartingRotation => ref Projectile.ai[2];

        new public static int MaxUpdates => 5;
        public static int AnimeVisualsDuration => Utilities.SecondsToFrames(0.05f);
        public static float BaseScale => 1.2f;
        public static VertexPositionColorTexture[] SwordQuad { get; private set; }

        // IMPORTANT: keep enum values the same as your item expects.
        // Slashes == RMB loop, Dash == LMB spin-only (repurposed, no dash).
        public enum AttackMode { Slashes = 0, Dash = 1 }
        public AttackMode Mode { get; set; } = AttackMode.Slashes;

        // one-shot flag per action (swing or spin)
        bool DidFireThisAction
        {
            get => Projectile.localAI[0] != 0f;
            set => Projectile.localAI[0] = value ? 1f : 0f;
        }

        public override string Texture => "InfernalEclipseAPI/Content/Items/Weapons/Melee/SwordoftheCorrupted/Swordofthe13thGlitch";

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
                if (projectile.active && projectile.owner == self.whoAmI && projectile.ModProjectile is _13thGltichCombo h && h.OwnerIsDashing)
                    self.maxFallSpeed = Swordofthe13thGlitch.PlayerDashSpeed * 1.5f;
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

            // one hit per swing/spin
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Swordofthe13thGlitch.UseTime / MaxUpdates;
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
            writer.Write(DidFireThisAction);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Mode = (AttackMode)reader.ReadByte();
            OwnerIsDashing = reader.ReadBoolean();
            VanishTimer = reader.ReadInt32();
            SwingCounter = reader.ReadInt32();
            OldStartingRotation = reader.ReadSingle();
            Projectile.rotation = reader.ReadSingle();
            float x = reader.ReadSingle(), y = reader.ReadSingle(), z = reader.ReadSingle(), w = reader.ReadSingle();
            Rotation = new(x, y, z, w);
            DidFireThisAction = reader.ReadBoolean();
        }

        public override void AI()
        {
            Projectile.localNPCHitCooldown = EffectiveFramesPerAction;

            if (HorizontalDirection == 0f)
            {
                HorizontalDirection = Owner.direction;
                StartingRotation = FacingRotation(Owner);
                Time = 0f;
                DidFireThisAction = false;
                Projectile.netUpdate = true;
                Projectile.hide = HideDuringRage;
            }

            StickToOwner();
            DontChangeOwnerDirection = false;
            OwnerIsDashing = false;

            if (VanishTimer >= 1)
            {
                DoBehavior_Vanish();
                return;
            }

            // Only original dash visuals push player (we’re not using that here, but keep for compatibility)
            if (AnimeHitVisualsCountdown > 0 && Mode == AttackMode.Dash)
            {
                Owner.velocity = Vector2.UnitX * HorizontalDirection * Swordofthe13thGlitch.PlayerPostHitSpeed;
                Time = (int)(EffectiveUseTime * 0.94f);
                AnimeHitVisualsCountdown--;
            }

            HandleSlashes(); // we’ll reinterpret modes inside

            Time++;

            // Chain logic at end of one animation
            if (Main.myPlayer == Projectile.owner && AnimationCompletion >= 1f)
            {
                if (Mode == AttackMode.Slashes && Owner.channel) // RMB loop
                {
                    // toggle forward <-> upward
                    SwingCounter = (SwingCounter + 1) & 1;
                    // reseed facing at the start of each cycle
                    HorizontalDirection = Owner.direction;
                    StartingRotation = FacingRotation(Owner);
                    OldStartingRotation = StartingRotation;
                    Time = 0f;
                    DidFireThisAction = false;
                    Projectile.netUpdate = true;
                    return;
                }

                // spin-only or RMB released -> vanish
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

            // RE-INTERPRET MODES:
            // Dash  => spin-only (no dash, no beams); if Rage, spawn clones+black slashes at cursor
            // Slashes => original forward↔upward loop; also fire a shotgun once per swing
            if (Mode == AttackMode.Dash)
            {
                if (IsCalamityRageActive(Owner)) TryFireRageClonesOnce();
                else Behavior_SpinAround_NoBeams(); // keep animation, remove beam spawns
                return;
            }

            // RMB loop with shotgun
            if ((SwingCounter & 1) == 0)
                DoBehavior_SwingForward_WithShotgun(forwardStart, forwardAnticipation, forwardSlash, forwardEnd);
            else
                DoBehavior_SwingUpward_WithShotgun(forwardEnd, upwardSlash, upwardEnd);
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

            if (!HideDuringRage)
                CreateSlashParticles();
        }

        public void CreateSlashParticles()
        {
            float x = MathHelper.WrapAngle(Projectile.rotation - Projectile.oldRot[1]);
            float mag = MathF.Abs(x);
            if (mag > 0.1f && Projectile.scale > 0.5f)
            {
                Vector2 dir = (Projectile.rotation - MathF.PI / 4f).ToRotationVector2();
                Vector2 sweep = dir.RotatedBy((float)x.NonZeroSign() * (MathF.PI / 2f));
                Dust d = Dust.NewDustPerfect(Projectile.Center + dir * Main.rand.NextFloat(28f, 74f) * Projectile.scale, 264);
                d.velocity = sweep * 3f + Owner.velocity * 0.35f;
                d.color = Color.Lerp(new Color(255, 96, 96), new Color(255, 32, 32), MathF.Sqrt(Main.rand.NextFloat(0.95f)));
                d.scale = Main.rand.NextFloat(1f, 1.6f);
                d.fadeIn = Main.rand.NextFloat(0.9f);
                d.noGravity = true;
            }
        }

        public void DoBehavior_Vanish()
        {
            if (Projectile.IsFinalExtraUpdate())
                VanishTimer++;

            Projectile.scale = Utilities.InverseLerp(11f, 0f, VanishTimer).Squared() * BaseScale;
            if (Projectile.scale <= 0f)
                Projectile.Kill();
        }

        // ======= RMB: Forward swing with shotgun =======
        void DoBehavior_SwingForward_WithShotgun(Quaternion forwardStart, Quaternion forwardAnticipation, Quaternion forwardSlash, Quaternion forwardEnd)
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

            // shotgun trigger around mid swing
            int trig = (int)(EffectiveUseTime * 0.35f);
            if (!DidFireThisAction && Time >= trig)
            {
                FireShotgun();
                DidFireThisAction = true;
                SoundEngine.PlaySound(SlashSfx, Projectile.Center);
            }

            if (SwingCounter <= 0)
                Projectile.scale = Utilities.InverseLerp(0f, 0.18f, AnimationCompletion) * BaseScale;
        }

        // ======= RMB: Upward swing with shotgun =======
        void DoBehavior_SwingUpward_WithShotgun(Quaternion forwardEnd, Quaternion upwardSlash, Quaternion upwardEnd)
        {
            IPiecewiseRotation piecewiseRotation = new IPiecewiseRotation()
                .Add(new PolynomialEasing(12f), Common.Tools.Easings.EasingType.InOut, upwardSlash, 0.9f, forwardEnd)
                .Add(PolynomialEasing.Quadratic, Common.Tools.Easings.EasingType.In, upwardEnd, 1f);

            Rotation = piecewiseRotation.Evaluate(AnimationCompletion, AnimationCompletion < 0.85f, -1);
            DontChangeOwnerDirection = true;

            int trig = (int)(EffectiveUseTime * 0.30f);
            if (!DidFireThisAction && Time >= trig)
            {
                FireShotgun();
                DidFireThisAction = true;
                SoundEngine.PlaySound(SlashSfx, Projectile.Center);
            }
        }

        // ======= LMB Spin (no beams), keep your animation feel =======
        void Behavior_SpinAround_NoBeams()
        {
            float angleSide = Utils.MultiLerp(AnimationCompletion.Squared(), 0.55f, 1.16f, 0f);
            float angle2D = MathF.PI * new PolynomialEasing(3.5f).Evaluate(EasingType.InOut, 1f - AnimationCompletion) * -4f;
            Rotation = EulerAnglesConversion(angle2D, angleSide);

            if (Time == (int)(Swordofthe13thGlitch.UseTime * 0.25f))
            {
                SoundEngine.PlaySound(new("InfernalEclipseAPI/Assets/Sounds/Custom/TerraBlade/Split"), Projectile.Center);
            }
        }

        // ======= Rage: clones + black slashes at cursor (once) =======
        void TryFireRageClonesOnce()
        {
            if (!IsCalamityRageActive(Owner))
                return;

            int fireTick = (int)(EffectiveUseTime * RageFireAt);
            if (DidFireThisAction || Time < fireTick)
                return;

            Vector2 spawn = Main.MouseWorld;
            int cloneDmg = Scale(Projectile.damage, RageCloneDamageFactor);
            int slashDmg = Scale(Projectile.damage, BlackSlashDamageFactor);

            for (int i = 0; i < RageCloneCount; i++)
            {
                Vector2 pos = spawn + Main.rand.NextVector2Circular(36f, 36f);
                Vector2 vel = Main.rand.NextFloat(0f, MathHelper.TwoPi).ToRotationVector2() * Main.rand.NextFloat(24f, 34f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos, vel, ModContent.ProjectileType<PlayerShadowClone>(), cloneDmg, 0f, Projectile.owner, ai2: 1);
            }
            for (int i = 0; i < ExtraBlackSlashes; i++)
            {
                float ang = (spawn - Owner.Center).ToRotation() + Main.rand.NextFloat(-0.7f, 0.7f);
                Vector2 vel = ang.ToRotationVector2() * Main.rand.NextFloat(26f, 34f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawn + Main.rand.NextVector2Circular(18f, 18f), vel, ModContent.ProjectileType<TerraSlash>(), slashDmg, 0f, Projectile.owner);
            }

            DidFireThisAction = true;
        }

        // ======= Shared actions =======
        void FireShotgun()
        {
            float baseAngle = Projectile.AngleTo(Main.MouseWorld);
            float spread = MathHelper.ToRadians(ShotgunSpreadDegrees);
            int half = ShotgunCount / 2;
            int dmg = Scale(Projectile.damage, ShotgunDamageFactor);

            for (int i = -half; i <= half; i++)
            {
                float t = (ShotgunCount == 1) ? 0f : i / (float)half;
                float angle = baseAngle + (t * 0.5f * spread) + Main.rand.NextFloat(-0.09f, 0.09f);
                Vector2 vel = angle.ToRotationVector2() * ShotgunSpeed;

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, ModContent.ProjectileType<HomingTerraBeam>(), (int)(dmg * 2.5), 0f, Projectile.owner);
            }
        }

        static int Scale(int baseDmg, float factor) =>
            Math.Max(1, (int)MathF.Round(baseDmg * factor));

        // ======= (unchanged) stick, collision, draw, trails, distortion =======
        public override bool ShouldUpdatePosition() => false;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
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
            Vector2 dir = (Projectile.rotation - MathF.PI / 4f).ToRotationVector2();
            Vector2 start = Projectile.Center;
            Vector2 end = start + dir * Projectile.scale * 112f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.width * 0.5f, ref collisionPoint);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (HideDuringRage)
                return false;

            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 texSize = tex.Size();
            Matrix mWorld = Matrix.CreateTranslation(new Vector3(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y, 0f));
            Matrix mProj = Matrix.CreateOrthographicOffCenter(0f, Main.screenWidth, Main.screenHeight, 0f, -150f, 150f);
            Matrix vp = mWorld * Main.GameViewMatrix.TransformationMatrix * mProj;
            Matrix rot = Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateRotationZ(StartingRotation);
            Matrix scl = Matrix.CreateScale(Projectile.scale);
            compositeVertexMatrix = rot * scl * vp;
            Matrix drawMatrix = compositeVertexMatrix;
            if (HorizontalDirection == -1f)
                drawMatrix = Matrix.CreateReflection(new Plane(Vector3.UnitX, 1f)) * Matrix.CreateRotationZ(MathF.PI / 2f) * drawMatrix;

            if (SwordQuad == null)
            {
                var v0 = new VertexPositionColorTexture(new Vector3(0f, -texSize.Y, 0f), Color.White, Vector2.One * 0.01f);
                var v1 = new VertexPositionColorTexture(new Vector3(texSize.X, -texSize.Y, 0f), Color.White, Vector2.UnitX * 0.99f);
                var v2 = new VertexPositionColorTexture(new Vector3(texSize.X, 0f, 0f), Color.White, Vector2.One * 0.99f);
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
            int keyframes = 20, subdiv = 5;
            float radius = 118f;
            trailPositions = new List<Vector2>();
            for (int i = 0; i < keyframes; i++)
            {
                float a = Projectile.oldRot[i] - Projectile.rotation - MathF.PI / 4f;
                float b = Projectile.oldRot[i + 1] - Projectile.rotation - MathF.PI / 4f;
                for (int j = 0; j < subdiv; j++)
                {
                    float f = a.AngleLerp(b, (float)j / subdiv);
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
                    float t = k / ((float)trailPositions.Count - 1f);
                    Vector2 uv0 = new(t, 0f), uv1 = new(t, 1f);
                    Vector2 n = (trailPositions[k] - Projectile.Center).SafeNormalize(Vector2.UnitY);
                    Vector3 p0 = new(trailPositions[k] - Projectile.Center, 0f);
                    Vector3 p1 = new(trailPositions[k] - Projectile.Center - n * Projectile.scale * 90f, 0f);
                    Color c = Color.White * sharp;
                    trailVertices[k * 2] = new VertexPositionColorTexture(p0, c, uv0);
                    trailVertices[k * 2 + 1] = new VertexPositionColorTexture(p1, c, uv1);
                }
            }
        }

        public void DrawAfterimageTrail()
        {
            var shader = Assets.Effects.ShaderManager.GetShader("GlitchSwordTrailShader");
            shader.SetTexture(LoadDeferred("InfernalEclipseAPI/Assets/Textures/WavyBlotchNoise"), 1, SamplerState.LinearWrap);
            shader.TrySetParameter("colorA", new Vector3(1f, 0.25f, 0.25f));
            shader.TrySetParameter("colorB", new Vector3(1f, 0.0f, 0.0f));
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
            if (HideDuringRage) return;
            if (trailVertices == null) return;
            float delta = MathHelper.WrapAngle(Projectile.rotation - Projectile.oldRot[1]);
            float amt = Utilities.InverseLerp(0.04f, 0.09f, MathF.Abs(delta)) * 0.156f;
            var arr = new VertexPositionColorTexture[trailVertices.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                Vector2 tp = trailPositions[i / 2];
                Vector2 v = tp - Projectile.Center;
                float a = v.ToRotation() - ZRotation;
                float r = Utilities.Cos01(a);
                float g = Utilities.Sin01(a);
                arr[i] = trailVertices[i];
                arr[i].Color = new Color(r, g, (1f - (i % 2)) * Utilities.InverseLerp(8f, 32f, i).Squared() * amt);
            }

            var shader = Assets.Effects.ShaderManager.GetShader("GlitchSwordTrailShader");
            shader.SetTexture(LoadDeferred("InfernalEclipseAPI/Assets/Textures/WavyBlotchNoise"), 1, SamplerState.LinearWrap);
            shader.TrySetParameter("colorA", new Vector3(1f, 0.25f, 0.25f));
            shader.TrySetParameter("colorB", new Vector3(1f, 0.00f, 0.00f));
            shader.TrySetParameter("blackAppearanceInterpolant", -3f);
            shader.TrySetParameter("trailAnimationSpeed", 0.6f);
            shader.TrySetParameter("flatOpacity", false);
            shader.TrySetParameter("uWorldViewProjection", compositeVertexMatrix);
            shader.Apply();
            Main.instance.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            Main.instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, arr, 0, arr.Length, trailIndices, 0, trailIndices.Length / 3);
        }

        public void DrawLocalDistortionExclusion(SpriteBatch spriteBatch)
        {
            if (HideDuringRage) return;
            DrawLocalDistortion(spriteBatch);
        }

        private static Texture2D LoadDeferred(string path)
        {
            if (Main.netMode == NetmodeID.Server)
                return default;
            return ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad).Value;
        }

        // ==== Calamity Rage check (same helper used previously) ====
        bool IsCalamityRageActive(Player player)
        {
            return player.HasBuff(ModContent.BuffType<RageMode>());
        }
    }
}
