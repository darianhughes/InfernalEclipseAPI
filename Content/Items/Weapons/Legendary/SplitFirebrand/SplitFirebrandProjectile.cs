using Microsoft.Xna.Framework;
using System.Collections.Generic;
using InfernumMode.Content.Rarities.InfernumRarities;
using CalamityMod.Items.Materials;
using Terraria.Localization;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;
using InfernalEclipseAPI.Content.Buffs.Tag;
using InfernalEclipseAPI.Content.Buffs;
using InfernalEclipseAPI.Content.Buffs.SoulBurn;
using CalamityMod;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.DataStructures;

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
        public List<Vector2> whipPoints;
        public float multihitModifier = 0.8f;
        public float segmentRotation;
        private bool runOnce = true;

        public Texture2D handleTex;
        private Texture2D whipSegment;
        private Texture2D whipTip;

        public bool ReverseSwing => Projectile.ai[1] == 1f;
        private bool initialized;

        public override void SetStaticDefaults() => ProjectileID.Sets.IsAWhip[Type] = true;

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();

            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = false;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = LegendarySummonMeleeSpeed.Instance;
            Projectile.width =12;
            Projectile.height = 12;
            Projectile.WhipSettings.Segments = 10;
            Projectile.WhipSettings.RangeMultiplier = GetFirebrandRange();

            whipSegment = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrandSegment").Value;
            whipTip = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrandTip").Value;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            var modPlayer = player.GetModPlayer<SplitFirebrandPlayer>();

            if (modPlayer.comboCounter == 1)
            {
                if (Timer % 2 == 0)
                {
                    List<Vector2> newPoints = new();
                    Projectile.FillWhipControlPoints(Projectile, newPoints);
                    whipPoints = newPoints;
                }

                return true;
            }
            else if (modPlayer.comboCounter == 2)
            {
                if (Projectile.ai[1] == 1f)
                    return true;

                if (Projectile.ai[0] == 0f && Projectile.localAI[0] == 0f)
                {
                    Projectile.localAI[0] = 1f;
                    Projectile.ai[0] = Main.player[Projectile.owner].itemAnimationMax * Projectile.MaxUpdates - 1f;

                    return false;
                }

                float swingTime = Projectile.ai[0] / (Main.player[Projectile.owner].itemAnimationMax * Projectile.MaxUpdates);

                swingTime = MathHelper.Lerp(
                    0f,
                    2f,
                    MathHelper.Lerp(1f, 0f, swingTime * swingTime * swingTime)
                );

                if ((Projectile.ai[0] -= 1f) <= 0f)
                    Projectile.Kill();
                else
                    Projectile.ai[0] -= swingTime;

                return true;
            }

            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            var modPlayer = player.GetModPlayer<SplitFirebrandPlayer>();

            // lock combo at spawn moment only
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                Projectile.localAI[1] = modPlayer.comboCounter;
            }

            WhipAIMotion();
            WhipSFX(lightingColor, swingDust, dustAmount, whipCrackSound);
        }

        private void WhipAIMotion()
        {
            Player player = Main.player[Projectile.owner];
            float totalTime = player.itemAnimationMax * Projectile.MaxUpdates;

            whipPoints ??= new List<Vector2>();
            whipPoints.Clear();
            Projectile.FillWhipControlPoints(Projectile, whipPoints);

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
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        private Vector2 GetTipPosition()
        {
            if (whipPoints == null || whipPoints.Count < 2)
                return Projectile.Center;

            return whipPoints[whipPoints.Count - 2];
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * multihitModifier);
            if (Projectile.damage < 1)
                Projectile.damage = 1;

            target.AddBuff(ModContent.BuffType<SplitFirebrandTag>(), 240);
            GetSoulBurn(target);

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
            }
        }

        public class SplitFirebrandWhipHandleLayer : PlayerDrawLayer
        {
            public override Position GetDefaultPosition()
                => new BeforeParent(PlayerDrawLayers.ArmOverItem);

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                Player player = drawInfo.drawPlayer;

                Projectile projInstance = null;

                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];

                    if (!p.active)
                        continue;

                    if (p.owner == player.whoAmI &&
                        p.ModProjectile is SplitFirebrandProjectile)
                    {
                        projInstance = p;
                        break;
                    }
                }

                if (projInstance?.ModProjectile is not SplitFirebrandProjectile proj)
                    return;

                if (proj.whipPoints == null || proj.whipPoints.Count < 2)
                    return;

                proj.UpdateWhipTextures();

                Vector2 handlePos = proj.whipPoints[0];
                Vector2 nextPos = proj.whipPoints[1];

                Vector2 dir = Vector2.Normalize(nextPos - handlePos);

                float handleForwardOffset = 12f;

                if (NPC.downedMoonlord)
                {
                    handleForwardOffset = 16f;
                }
                else
                {
                    handleForwardOffset = 6f;
                }

                handlePos += dir * handleForwardOffset;

                float rot = dir.ToRotation() + MathHelper.Pi;

                drawInfo.DrawDataCache.Add(new DrawData(
                    proj.handleTex,
                    handlePos - Main.screenPosition,
                    null,
                    Lighting.GetColor(handlePos.ToTileCoordinates()),
                    rot,
                    proj.handleTex.Size() * 0.5f,
                    1f,
                    SpriteEffects.FlipHorizontally,
                    0
                ));
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

                SpriteEffects drawEffect = SpriteEffects.None;

                Player player = Main.player[Projectile.owner];
                var modPlayer = player.GetModPlayer<SplitFirebrandPlayer>();

                if (modPlayer.comboCounter == 1)
                {
                    if (i == whipPoints.Count - 2)
                    {
                        // Tip segment
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
                }
                else if (modPlayer.comboCounter == 2)
                {
                    if (i == whipPoints.Count - 2)
                    {
                        // Tip segment
                        drawEffect = Projectile.spriteDirection < 0
                            ? SpriteEffects.None
                            : SpriteEffects.FlipVertically;
                    }
                    else
                    {
                        // Body segments
                        drawEffect = Projectile.spriteDirection < 0
                            ? SpriteEffects.FlipVertically
                            : SpriteEffects.None;
                    }
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

        public static float GetFirebrandRange()
        {
            if (NPC.downedMoonlord)
                return 0.8f;
            if (NPC.downedGolemBoss)
                return 0.675f;
            if (NPC.downedPlantBoss)
                return 0.6f;
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                return 0.55f;
            if (Main.hardMode)
                return 0.45f;
            if (NPC.downedBoss3)
                return 0.35f;
            return 0.3f;
        }

        public void GetSoulBurn(NPC target)
        {
            if (NPC.downedMoonlord)
                target.AddBuff(ModContent.BuffType<SoulBurn2>(), 240);
            else if (NPC.downedAncientCultist)
                target.AddBuff(ModContent.BuffType<SoulBurn2>(), 240);
            else if (NPC.downedGolemBoss)
                target.AddBuff(ModContent.BuffType<SoulBurn2>(), 240);
            else if (NPC.downedPlantBoss)
                target.AddBuff(ModContent.BuffType<SoulBurn2>(), 240);
            else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                target.AddBuff(ModContent.BuffType<SoulBurn2>(), 240);
            else if (Main.hardMode)
                target.AddBuff(ModContent.BuffType<SoulBurn2>(), 240);
            else if (NPC.downedBoss3)
                target.AddBuff(ModContent.BuffType<SoulBurn2>(), 240);
            else
                target.AddBuff(ModContent.BuffType<SoulBurn>(), 240);
        }
    }
}
