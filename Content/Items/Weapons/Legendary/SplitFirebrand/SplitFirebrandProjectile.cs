using Microsoft.Xna.Framework;
using System.Collections.Generic;
using InfernumMode.Content.Rarities.InfernumRarities;
using CalamityMod.Items.Materials;
using Terraria.Localization;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;
using InfernalEclipseAPI.Content.Buffs.Tag;
using CalamityMod;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using System.Runtime.InteropServices;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.SplitFirebrand
{
    public class SplitFirebrandProjectile : ModProjectile
    {
        public Color fishingLineColor = Color.DarkRed;
        public Color lightingColor = Color.Transparent;
        public Color? drawColor;
        public int? swingDust = DustID.Torch;
        public int dustAmount = 1;
        public SoundStyle? whipCrackSound = new SoundStyle?(SoundID.Item153);
        private List<Vector2> whipPoints;
        public float multihitModifier = 0.8f;
        public float segmentRotation;
        private bool runOnce = true;

        private Texture2D handleTex;
        private Texture2D whipSegment;
        private Texture2D whipTip;

        public override void SetStaticDefaults() => ProjectileID.Sets.IsAWhip[Type] = true;

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = false;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = LegendarySummon.Instance;
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.WhipSettings.Segments = 5;
            Projectile.WhipSettings.RangeMultiplier = 0.8f;

            whipSegment = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrandSegment").Value;
            whipTip = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrandTip").Value;
        }

        public override bool PreAI()
        {
            if (Timer % 2 == 0)
            {
                List<Vector2> newPoints = new();
                Projectile.FillWhipControlPoints(Projectile, newPoints);
                whipPoints = newPoints;
            }

            return true;
        }

        public override void AI()
        {
            WhipAIMotion();
            WhipSFX(lightingColor, swingDust, dustAmount, whipCrackSound);
        }

        private void WhipAIMotion()
        {
            Player player = Main.player[Projectile.owner];
            float totalTime = player.itemAnimationMax * Projectile.MaxUpdates;

            if (runOnce)
            {
                Projectile.WhipSettings.Segments = (int)((player.whipRangeMultiplier + 1f) * Projectile.WhipSettings.Segments);
                runOnce = false;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.Center = Vector2.Lerp(Projectile.Center, whipPoints[whipPoints.Count - 1], 1f);
            Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;
            Timer++;

            if (Timer >= totalTime || player.itemAnimation <= 0)
                Projectile.Kill();
        }

        private void WhipSFX(Color lightCol, int? dustID, int dustNum, SoundStyle? sound)
        {
            Player player = Main.player[Projectile.owner];
            float totalTime = player.itemAnimationMax * Projectile.MaxUpdates;
            player.heldProj = Projectile.whoAmI;

            Vector2 tipPos = GetTipPosition();
            if (Timer == totalTime / 2f && sound.HasValue)
                SoundEngine.PlaySound(sound.Value, tipPos);

            if (Timer < totalTime * 0.5f)
                return;

            if (dustID.HasValue)
            {
                for (int i = 0; i < dustNum; i++)
                    Dust.NewDust(tipPos, 2, 2, dustID.Value, 0f, 0f, 0, default, 0.5f);
            }

            if (lightCol != Color.Transparent)
                Lighting.AddLight(tipPos, lightCol.ToVector3() / 255f);
        }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private Vector2 GetTipPosition() => whipPoints[whipPoints.Count - 2];

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * multihitModifier);
            if (Projectile.damage < 1)
                Projectile.damage = 1;

            target.AddBuff(ModContent.BuffType<SplitFirebrandTag1>(), 240);

            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;

            Vector2 tipPos = GetTipPosition();
            for (int j = 0; j < 8; j++)
            {
                Vector2 dustOffset = new Vector2(2f, 0f).RotatedBy(MathHelper.ToRadians(j * 45) + Main.rand.NextFloat(-0.1f, 0.1f));
                Dust dust = Dust.NewDustDirect(tipPos + dustOffset, 0, 0, DustID.Torch);
                dust.noGravity = true;
                dust.scale = 0.7f;
                dust.velocity *= 1.5f;
            }
        }

        private void UpdateWhipTextures()
        {
            switch ((int)Projectile.ai[2])
            {
                default:
                case 0:
                    handleTex = TextureAssets.Projectile[Type].Value;

                    whipSegment = ModContent.Request<Texture2D>(
                        "InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrandSegment"
                    ).Value;

                    whipTip = ModContent.Request<Texture2D>(
                        "InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrandTip"
                    ).Value;
                    break;

                case 1:
                    handleTex = ModContent.Request<Texture2D>(
                        "InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrandCrescendoProjectile"
                    ).Value;

                    whipSegment = ModContent.Request<Texture2D>(
                        "InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrandCrescendoSegment"
                    ).Value;

                    whipTip = ModContent.Request<Texture2D>(
                        "InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrandCrescendoTip"
                    ).Value;
                    break;

                case 2:
                    handleTex = ModContent.Request<Texture2D>(
                        "InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/FireHandle"
                    ).Value;

                    whipSegment = ModContent.Request<Texture2D>(
                        "InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/FireSegment"
                    ).Value;

                    whipTip = ModContent.Request<Texture2D>(
                        "InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/FireTip"
                    ).Value;
                    break;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (whipPoints == null || whipPoints.Count < 1)
                return false;

            UpdateWhipTextures();

            DrawFishingLineBetweenPoints(whipPoints, fishingLineColor);

            SpriteEffects effect = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 pos = whipPoints[1];

            //Handle
            Vector2 handleOrigin = handleTex.Size() / 2f;

            Vector2 handlePos = whipPoints[0];
            Vector2 nextPos = whipPoints[1];

            Vector2 dir = nextPos - handlePos;
            float handleRotation = dir.ToRotation() + MathHelper.PiOver2;

            handleRotation -= (MathHelper.Pi / 2);

            Main.EntitySpriteDraw(
                handleTex,
                handlePos - Main.screenPosition,
                null,
                Lighting.GetColor(handlePos.ToTileCoordinates()),
                handleRotation,
                handleOrigin,
                1f,
                SpriteEffects.None,
                0f
            );

            //Segments
            for (int i = 1; i < whipPoints.Count - 1; i++)
            {
                Texture2D tex = whipSegment;
                float scale = 1f;

                if (i == whipPoints.Count - 2)
                {
                    tex = whipTip;
                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out _, out _);
                    float t = Timer / timeToFlyOut;
                    scale = MathHelper.Lerp(0.35f, 1.1f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }

                Rectangle frame = new Rectangle(0, 0, tex.Width, tex.Height);
                Vector2 origin = frame.Size() / 2f;
                Vector2 diff = whipPoints[i + 1] - whipPoints[i];
                float rot = diff.ToRotation();

                SpriteEffects drawEffect;

                // Tip segment
                if (i == whipPoints.Count - 2)
                {
                    drawEffect = Projectile.spriteDirection < 0
                        ? SpriteEffects.FlipVertically
                        : SpriteEffects.None;
                }
                else
                {
                    // Body segments
                    drawEffect = Projectile.spriteDirection < 0
                        ? SpriteEffects.None
                        : SpriteEffects.None;
                }

                Main.EntitySpriteDraw(
                    tex,
                    pos - Main.screenPosition,
                    frame,
                    Lighting.GetColor(whipPoints[0].ToTileCoordinates()),
                    rot,
                    origin,
                    scale,
                    drawEffect,
                    0f
                );

                pos += diff;
            }

            return false;
        }

        private void DrawFishingLineBetweenPoints(List<Vector2> points, Color color)
        {
            Texture2D tex = TextureAssets.FishingLine.Value;
            Rectangle frame = tex.Frame();
            Vector2 origin = new Vector2(frame.Width / 2f, 2f);
            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 2; i++)
            {
                Vector2 diff = points[i + 1] - points[i];
                float rot = diff.ToRotation() - MathHelper.PiOver2;
                float length = diff.Length() + 2f;
                Vector2 scale = new Vector2(1f, length / frame.Height);
                Color lightCol = Lighting.GetColor(points[i].ToTileCoordinates(), color);
                Main.EntitySpriteDraw(tex, pos - Main.screenPosition, frame, lightCol, rot, origin, scale, SpriteEffects.None, 0f);
                pos += diff;
            }
        }
    }
}
