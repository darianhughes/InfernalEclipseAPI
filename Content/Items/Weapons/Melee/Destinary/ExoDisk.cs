using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Achievements;
using Terraria.Graphics.Shaders;
using CalamityMod.Particles;
using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Buffs.DamageOverTime;
using System.Linq;

namespace InfernalEclipseAPI.Content.Items.Weapons.Melee.Destinary
{
    public class ExoDisk : ModProjectile
    {
        public const int Lifetime = 80;
        public Player Owner => Main.player[Projectile.owner];
        public ref float ShootReach => ref Projectile.ai[0];
        public ref float Time => ref Projectile.ai[1];
        public ref float CanBreakTrees => ref Projectile.ai[2];
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/RefractionRotor";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 3;
            ProjectileID.Sets.TrailCacheLength[Type] = 90;
        }

        public override void SetDefaults()
        {
            Projectile.width = 142;
            Projectile.height = 126;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 5;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 4;
            Projectile.timeLeft = Lifetime;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            if (Projectile.localAI[1] == 0f)
            {
                Projectile.localAI[1] = 1f;
                for (int i = 0; i < Projectile.oldPos.Length; ++i)
                    Projectile.oldPos[i] = Projectile.position;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi * Time / Lifetime;
            if (Main.rand.NextBool(6) && (Time < 35 || Time > 45))
            {
                Particle spark2 = new GlowOrbParticle(Projectile.Center + Main.rand.NextVector2Circular(12, 12), Projectile.velocity * Main.rand.NextFloat(0.6f, 0.9f), false, 22, Main.rand.NextFloat(0.3f, 0.9f), Main.hslToRgb((Time / 40f + Main.rand.NextFloat(-0.1f, 0.1f)) % 1f, 0.95f, 0.8f));
                GeneralParticleHandler.SpawnParticle(spark2);
            }

            Vector2 baseDirection = (MathHelper.TwoPi * Time / Lifetime - MathHelper.PiOver2).ToRotationVector2();
            baseDirection.X *= 0.25f;

            baseDirection.Y = baseDirection.Y * 0.5f + 0.5f;
            Vector2 positionOffset = baseDirection * ShootReach;

            if (Math.Abs(positionOffset.X) > 45f)
                positionOffset.X = Math.Sign(baseDirection.X) * 45f;

            positionOffset = positionOffset.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver2);

            Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + Projectile.velocity * 42f + positionOffset;
            Projectile.Opacity = Utils.GetLerpValue(0f, 12f, Time, true) * Utils.GetLerpValue(Lifetime, Lifetime - 12f, Lifetime - Projectile.timeLeft, true);

            if (CanBreakTrees == 1)
            {
                for (int i = 0; i < 20; i++)
                {
                    Point pointToCheck = (Projectile.oldPos[i] + Projectile.Size * 0.5f).ToTileCoordinates();
                    AbsolutelyFuckingAnnihilateTrees(pointToCheck.X, pointToCheck.Y);
                }
            }

            Lighting.AddLight(Projectile.Center, Vector3.One * 0.7f);

            Time++;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Projectile.RotatingHitboxCollision(targetHitbox);

        public void AbsolutelyFuckingAnnihilateTrees(int x, int y)
        {
            Tile tileAtPosition = CalamityUtils.ParanoidTileRetrieval(x, y);

            if (!tileAtPosition.HasTile || !Main.tileAxe[tileAtPosition.TileType])
                return;

            if (!WorldGen.CanKillTile(x, y))
                return;

            AchievementsHelper.CurrentlyMining = true;

            WorldGen.KillTile(x, y);
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y);

            AchievementsHelper.CurrentlyMining = false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        internal float WidthFunction(float completionRatio, Vector2 vertexPos) => (Projectile.scale * 24f * (1f - Utils.GetLerpValue(0.7f, 1f, completionRatio, true)) + 1f);

        internal Color ColorFunction(float completionRatio, Vector2 vertexPos)
        {
            float hue = (Projectile.identity % 9f / 9f + completionRatio * 0.7f) % 1f;
            return Color.Lerp(Color.White, Main.hslToRgb(hue, 0.95f, 0.55f), 0.35f) * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            /*
            if (Time <= 5f)
                return true;

            Vector2 generalOffset = Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.PiOver2) * 15f;
            generalOffset += Projectile.rotation.ToRotationVector2() * -5f * (float)Math.Sin(Projectile.rotation);
            generalOffset *= Vector2.Zero;
            Main.spriteBatch.EnterShaderRegion();
            GameShaders.Misc["CalamityMod:PrismaticStreak"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ScarletDevilStreak"));

            int numPointsRendered = 22;
            int numPointsProvided = 80;
            var positionsToUse = Projectile.oldPos.Take(numPointsProvided).ToArray();
            PrimitiveRenderer.RenderTrail(positionsToUse, new(WidthFunction, ColorFunction, (_, _) => Projectile.Size * 0.5f + generalOffset, shader: GameShaders.Misc["CalamityMod:PrismaticStreak"], smoothen: false), numPointsRendered);
            Main.spriteBatch.ExitShaderRegion();
            */
            return true;
        }

        public override bool ShouldUpdatePosition() => false;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
    }
}
