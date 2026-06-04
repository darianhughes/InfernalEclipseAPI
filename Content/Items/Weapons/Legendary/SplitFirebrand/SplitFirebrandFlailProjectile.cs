using Microsoft.Xna.Framework;
using System.Collections.Generic;
using InfernumMode.Content.Rarities.InfernumRarities;
using CalamityMod.Items.Materials;
using Terraria.Localization;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;
using InfernalEclipseAPI.Content.Buffs.Tag;
using InfernalEclipseAPI.Content.Buffs;
using CalamityMod;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using System.Runtime.InteropServices;
using Terraria;
using static CalamityMod.Projectiles.BaseProjectiles.BaseMaceFlailProjectile;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.SplitFirebrand
{
    public class SplitFirebrandFlailProjectile : ModProjectile
    {
        public override string Texture => "InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrandProjectile";

        public Color fishingLineColor = Color.DarkRed;
        public Color lightingColor = Color.Transparent;
        public Color? drawColor;
        public int? swingDust = DustID.Torch;
        public int dustAmount = 1;
        public SoundStyle? whipCrackSound = new SoundStyle?(SoundID.Item153);
        private List<Vector2> ropePoints;
        public float multihitModifier = 0.8f;
        public float segmentRotation;
        private bool runOnce = true;

        private Texture2D handleTex;
        private Texture2D whipSegment;
        private Texture2D whipTip;

        enum FlailState
        {
            Extending,
            StuckOrFree,
            Returning
        }

        private Dictionary<int, int> localHitCooldown = new();
        private float extendProgress = 0f;
        public List<Vector2> RopePoints => ropePoints;

        private float arcOffsetStart;
        private float arcOffsetCurrent;
        private float arcOffsetSign;

        private Vector2 aimDir;

        public override void SetStaticDefaults() => ProjectileID.Sets.IsAWhip[Type] = true;

        public override void SetDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = false;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = LegendarySummon.Instance;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.WhipSettings.Segments = 5;
            Projectile.WhipSettings.RangeMultiplier = GetFirebrandFlailRange();

            whipSegment = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrandSegment").Value;
            whipTip = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrandTip").Value;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];

            aimDir = (Main.MouseWorld - player.MountedCenter)
                .SafeNormalize(Vector2.UnitX);

            arcOffsetStart = MathHelper.ToRadians(Main.rand.NextFloat(0f, 12f));
            arcOffsetSign = Main.rand.NextBool() ? 1f : -1f;
            arcOffsetCurrent = arcOffsetStart * arcOffsetSign;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // init state
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                Projectile.ai[0] = (float)FlailState.Extending;
            }

            // 0 → 1 over swing
            float lifeProgress = 1f - (player.itemAnimation / (float)player.itemAnimationMax);
            lifeProgress = MathHelper.Clamp(lifeProgress, 0f, 1f);

            // base aim direction
            Vector2 baseDir = aimDir;

            // arc: start positive, end negative
            float arc = MathHelper.Lerp(
                arcOffsetStart * arcOffsetSign,
                -arcOffsetStart * arcOffsetSign,
                lifeProgress
            );

            Vector2 finalDir = baseDir.RotatedBy(arc);

            // state machine
            switch ((FlailState)(int)Projectile.ai[0])
            {
                case FlailState.Extending:
                    DoExtend(player, finalDir);
                    break;

                case FlailState.StuckOrFree:
                    DoFlailPhysics(player);
                    break;

                case FlailState.Returning:
                    DoReturn(player);
                    break;
            }

            // visuals + logic
            BuildFlailSegments();
            CheckChainHitboxes();

            // cooldown cleanup
            List<int> keys = new(localHitCooldown.Keys);
            foreach (int k in keys)
            {
                localHitCooldown[k]--;
                if (localHitCooldown[k] <= 0)
                    localHitCooldown.Remove(k);
            }
        }

        private void DoExtend(Player player, Vector2 dir)
        {
            Projectile.tileCollide = false;

            float rawProgress = 1f - (player.itemAnimation / (float)player.itemAnimationMax);
            rawProgress = MathHelper.Clamp(rawProgress, 0f, 1f);

            extendProgress = MathHelper.Lerp(extendProgress, rawProgress, 0.25f);

            float eased = 1f - MathF.Pow(1f - extendProgress, 3f);

            float range = 16f * GetFirebrandFlailRange();

            Projectile.Center = player.MountedCenter + dir * range * eased;
            Projectile.velocity = dir * (GetFirebrandFlailRange() * 1.5f);

            float halfTime = player.itemAnimationMax * 0.5f;

            if (player.itemAnimation <= halfTime)
            {
                Projectile.ai[0] = (float)FlailState.Returning;
            }
        }

        private void DoFlailPhysics(Player player)
        {
            Projectile.tileCollide = true;

            Vector2 toPlayer = player.Center - Projectile.Center;
            float distance = toPlayer.Length();

            if (distance > 3000f)
            {
                Projectile.Kill();
                return;
            }

            toPlayer.Normalize();

            float pullStrength = 1f; // tweak this for "weight"

            // slowly bend velocity toward player (flail rope tension feel)
            Projectile.velocity = Vector2.Lerp(
                Projectile.velocity,
                toPlayer * 10f,
                pullStrength * 0.05f
            );

            // collision with player = start return
            if (Projectile.Hitbox.Intersects(player.Hitbox))
            {
                Projectile.ai[0] = (float)FlailState.Returning;
                Projectile.ai[1] = 0f;
            }
        }

        private void DoReturn(Player player)
        {
            Projectile.tileCollide = false;

            Vector2 toPlayer = player.Center - Projectile.Center;
            float distance = toPlayer.Length();

            if (distance < 40f)
            {
                Projectile.Kill();
                return;
            }

            toPlayer.Normalize();

            // smooth acceleration instead of constant speed
            float returnSpeed = MathHelper.Clamp(distance * 0.12f, 10f, 56f);

            Projectile.velocity = Vector2.Lerp(Projectile.velocity, toPlayer * returnSpeed, 0.3f);
        }

        private void BuildFlailSegments()
        {
            ropePoints ??= new List<Vector2>();
            ropePoints.Clear();

            Player player = Main.player[Projectile.owner];

            Vector2 start = player.MountedCenter;
            Vector2 end = Projectile.Center;

            Vector2 dir = (end - start).SafeNormalize(Vector2.UnitX);

            int segments = 12;

            for (int i = 0; i <= segments; i++)
            {
                float t = i / (float)segments;

                // smooth whip curve instead of straight line jitter
                Vector2 point = Vector2.Lerp(start, end, t);

                ropePoints.Add(point);
            }
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
            if (ropePoints == null || ropePoints.Count < 2)
                return Projectile.Center;

            return ropePoints[ropePoints.Count - 2];
        }

        private void CheckChainHitboxes()
        {
            if (ropePoints == null || ropePoints.Count < 2) return;
            
            Player player = Main.player[Projectile.owner];
            
            for (int i = 0; i < ropePoints.Count - 1; i++)
            {
                Vector2 start = ropePoints[i]; Vector2 end = ropePoints[i + 1];
                
                // Treat each segment as a line hitbox with thickness
        
                float collisionWidth = 18f;

        
                Rectangle npcHitbox = new Rectangle(
            
                    (int)Projectile.position.X - 200,
            
                    (int)Projectile.position.Y - 200,
            
                    400,
            
                    400
                );

        foreach (NPC npc in Main.npc)
        {
            if (!npc.active || npc.dontTakeDamage)
                continue;

                    float collisionPoint = 0f;

                    if (Collision.CheckAABBvLineCollision(
                        npc.Hitbox.TopLeft(),
                        npc.Hitbox.Size(),
                        start,
                        end,
                        18f,
                        ref collisionPoint))
                    {
                        HitNPCFromChain(npc);
                    }
                }
            }
        }

        private void HitNPCFromChain(NPC npc)
        {
            if (localHitCooldown.TryGetValue(npc.whoAmI, out int cd) && cd > 0)
                return;

            localHitCooldown[npc.whoAmI] = 20;

            int damage = Projectile.damage;

            npc.SimpleStrikeNPC(damage, 0);

            npc.AddBuff(ModContent.BuffType<SplitFirebrandTag>(), 240);
            npc.AddBuff(ModContent.BuffType<SoulBurn>(), 240);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * multihitModifier);
            if (Projectile.damage < 1)
                Projectile.damage = 1;

            var modNPC = target.GetGlobalNPC<SoulBurnNPC>();

            target.AddBuff(ModContent.BuffType<SplitFirebrandTag>(), 240);
            target.AddBuff(ModContent.BuffType<SoulBurn>(), 240);

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

        public class SplitFirebrandHandleLayer : PlayerDrawLayer
        {
            public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.ArmOverItem);

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                Player player = drawInfo.drawPlayer;

                Projectile projInstance = null;

                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (!p.active)
                        continue;

                    if (p.owner == player.whoAmI && p.ModProjectile is SplitFirebrandFlailProjectile sf)
                    {
                        projInstance = p;
                        break;
                    }
                }

                if (projInstance?.ModProjectile is not SplitFirebrandFlailProjectile proj)
                    return;

                if (proj.RopePoints == null || proj.RopePoints.Count < 2)
                    return;

                Vector2 handlePos = proj.RopePoints[0];
                Vector2 nextPos = proj.RopePoints[1];

                Texture2D tex = proj.handleTex;
                if (tex == null)
                    return;

                float rot = (nextPos - handlePos).ToRotation();

                drawInfo.DrawDataCache.Add(new DrawData(
                    tex,
                    handlePos - Main.screenPosition,
                    null,
                    Lighting.GetColor(handlePos.ToTileCoordinates()),
                    rot,
                    tex.Size() * 0.5f,
                    1f,
                    SpriteEffects.None,
                    0
                ));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (ropePoints == null || ropePoints.Count < 2)
                return false;

            UpdateWhipTextures();

            DrawFishingLineBetweenPoints(ropePoints, fishingLineColor);

            for (int i = 1; i < ropePoints.Count; i++)
            {
                Vector2 start = ropePoints[i - 1];
                Vector2 end = ropePoints[i];

                Vector2 diff = end - start;
                float rot = diff.ToRotation();

                bool isTip = (i == ropePoints.Count - 1);

                Texture2D tex = isTip ? whipTip : whipSegment;

                Vector2 origin = new(tex.Width / 2f, tex.Height / 2f);

                SpriteEffects effects = SpriteEffects.None;
                if (diff.X < 0f)
                    effects = SpriteEffects.FlipVertically;

                Main.EntitySpriteDraw(
                    tex,
                    start - Main.screenPosition,
                    null,
                    Lighting.GetColor(start.ToTileCoordinates()),
                    rot,
                    origin,
                    1f,
                    effects,
                    0f
                );
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


        public static float GetFirebrandFlailRange()
        {
            if (NPC.downedMoonlord)
                return 58f;
            if (NPC.downedGolemBoss)
                return 42f;
            if (NPC.downedPlantBoss)
                return 36f;
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                return 30f;
            if (Main.hardMode)
                return 24f;
            if (NPC.downedBoss3)
                return 18f;
            return 13f;
        }
    }
}
