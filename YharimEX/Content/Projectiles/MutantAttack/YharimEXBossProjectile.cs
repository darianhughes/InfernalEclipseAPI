using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using InfernalEclipseAPI.YharimEX.Content.NPCs.Bosses;
using InfernalEclipseAPI.YharimEX.Core.Systems;
using CalamityMod.World;
using InfernumMode.Core.GlobalInstances.Systems;
using Luminance.Common.Utilities;

namespace InfernalEclipseAPI.YharimEX.Content.Projectiles.MutantAttack
{
    public class YharimEXBossProjectile : ModProjectile
    {
        public override string Texture => "InfernalEclipseAPI/YharimEX/Assets/NPCs/YharimEXBoss";
        public static string trailTexture => "InfernalEclipseAPI/YharimEX/Assets/NPCs/YharimEXSoul";
        public static int npcType => ModContent.NPCType<YharimEXBoss>();
        public bool auraTrail;
        const int auraFrames = 19;
        public bool sansEye;
        public float SHADOWMUTANTREAL;
        public bool Cake;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 50;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.hide)
                behindProjectiles.Add(index);
        }

        public override void AI()
        {
            Cake = false;

            NPC npc = YharimEXUtils.NPCExists(Projectile.ai[1], npcType);
            if (npc != null)
            {
                Projectile.Center = npc.Center;
                Projectile.alpha = npc.alpha;
                Projectile.direction = Projectile.spriteDirection = npc.direction;
                Projectile.timeLeft = 30;
                auraTrail = npc.localAI[3] >= 3;

                // RETURN        Projectile.hide =
                //            Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<YharimEXSpearAim>()] > 0
                //            || Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<YharimEXSpearDash>()] > 0
                //            || Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<YharimEXSpearSpin>()] > 0
                //            || Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<YharimEXSlimeRain>()] > 0;

                sansEye =
                    npc.ai[0] == 10 && npc.ai[1] > 150
                    || npc.ai[0] == -5 && npc.ai[2] > 420 - 90 && npc.ai[2] < 420;

                if (npc.ai[0] == 10 && CalamityWorld.death)
                {
                    SHADOWMUTANTREAL += 0.03f;
                    if (SHADOWMUTANTREAL > 0.75f)
                        SHADOWMUTANTREAL = 0.75f;

                    if (npc.ai[1] > 150 && WorldSaveSystem.InfernumModeEnabled && Main.getGoodWorld)
                        Cake = true;
                }

                Projectile.localAI[1] = sansEye ? MathHelper.Lerp(Projectile.localAI[1], 1f, 0.05f) : 0; //for rotation of sans eye
                Projectile.ai[0] = sansEye ? Projectile.ai[0] + 1 : 0;

                if (WorldSaveSystem.InfernumModeEnabled && (npc.ai[0] >= 11 || npc.ai[0] < 0))
                {
                    sansEye = true;
                    Projectile.ai[0] = -1;
                }

                if (!Main.dedServ)
                    Projectile.frame = (int)(npc.frame.Y / (float)(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]));

                if (npc.frameCounter == 0)
                {
                    if (++Projectile.localAI[0] >= auraFrames)
                        Projectile.localAI[0] = 0;
                }

                if (WorldSaveSystem.InfernumModeEnabled && Main.getGoodWorld)
                {
                    if (!npc.HasValidTarget && npc.velocity.Y < 0)
                    {
                        Cake = true;
                    }

                    if (npc.ai[0] == -7) //death anim
                    {
                        Cake = true;
                    }
                }
            }
            else
            {
                sansEye = false;
                if (YharimEXUtils.HostCheck)
                    Projectile.Kill();
                return;
            }

            SHADOWMUTANTREAL -= 0.01f;
            if (SHADOWMUTANTREAL < 0)
                SHADOWMUTANTREAL = 0;
        }

        public override void OnKill(int timeLeft)
        {
            /*Main.NewText("i die now");
            if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("i die now aaaaaa"), Color.LimeGreen);*/
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture2D14 = ModContent.Request<Texture2D>(trailTexture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            if (Cake)
            {
                texture2D13 = texture2D14 = ModContent.Request<Texture2D>("FargowiltasSouls/Content/Bosses/MutantBoss/MutantCake", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                sansEye = false;
                auraTrail = true;
            }
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / (Cake ? 1 : Main.projFrames[Projectile.type]); //ypos of lower right corner of sprite to draw
            int y3 = num156 * (Cake ? 0 : Projectile.frame); //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Texture2D aura = ModContent.Request<Texture2D>("InfernalEclipseAPI/YharimEX/Assets/NPCs/YharimEXAura", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            int auraFrameHeight = aura.Height / auraFrames;
            int auraY = auraFrameHeight * (int)Projectile.localAI[0];
            Rectangle auraRectangle = new(0, auraY, aura.Width, auraFrameHeight);

            /*Texture2D lightning = ModContent.Request<Texture2D>("FargowiltasSouls/Content/Bosses/MutantBoss/MutantLightning", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            int lightningFrameHeight = lightning.Height / lightningFrames;
            int lightningY = lightningFrameHeight * (int)Projectile.localAI[0];
            Rectangle lightningRectangle = new Rectangle(0, lightningY, lightning.Width, lightningFrameHeight);*/

            Color color26 = Projectile.GetAlpha(Projectile.hide && Main.netMode == NetmodeID.MultiplayerClient ? Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16) : lightColor);

            SpriteEffects effects = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            float scale = (Main.mouseTextColor / 200f - 0.35f) * 0.4f + 0.9f;
            scale *= Projectile.scale;

            Color trailColor = new Color(255, 255, 255, 100);
            Color color25 = (Cake ? trailColor : new Color(255, 255, 255, 200)) * Projectile.Opacity;

            if (auraTrail || SHADOWMUTANTREAL > 0)
            {
                int max = Cake ? 5 : 1;
                for (int i = 0; i < max; i++)
                    Main.EntitySpriteDraw(texture2D14, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color25, Projectile.rotation, origin2, scale, effects, 0);
            }

            if (auraTrail)
            {
                for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.25f)
                {
                    Color color27 = color25 * 0.5f;
                    color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                    int max0 = (int)i - 1;//Math.Max((int)i - 1, 0);
                    if (max0 < 0)
                        max0 = 0;
                    float num165 = Projectile.oldRot[max0];
                    Vector2 center = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
                    center += Projectile.Size / 2;
                    Main.EntitySpriteDraw(texture2D14, center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, effects, 0);
                }

                Main.EntitySpriteDraw(aura, -16 * Vector2.UnitY + Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(auraRectangle), color25, Projectile.rotation, auraRectangle.Size() / 2f, scale, effects, 0);
            }
            else
            {
                for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
                {
                    Color color27 = color26;
                    color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                    Vector2 value4 = Projectile.oldPos[i];
                    float num165 = Projectile.oldRot[i];
                    Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, effects, 0);
                }
            }

            color26 = Color.Lerp(color26, Color.Black, SHADOWMUTANTREAL);

            //if (YharimEXWorldFlags.MasochistModeReal)
            //{
            //    Main.spriteBatch.End();
            //    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

            //    GameShaders.Misc["WCWingShader"].UseColor(Color.LimeGreen).UseSecondaryColor(Color.LightPink).UseImage0("Images/Misc/noise");
            //    GameShaders.Misc["WCWingShader"].Apply(new DrawData?());
            //}
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, Projectile.rotation, origin2, Projectile.scale, effects, 0);
            //if (auraTrail && YharimEXWorldFlags.MasochistModeReal)
            //{
            //    Main.spriteBatch.End();
            //    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            //}

            if (sansEye)
            {
                Color color = Color.Red;

                bool forcedMasoEye = WorldSaveSystem.InfernumModeEnabled && Projectile.ai[0] == -1;

                const int maxTime = 120;
                float effectiveTime = Projectile.ai[0];
                float rotation = MathHelper.TwoPi * Projectile.localAI[1];
                float modifier = Math.Min(1f, (float)Math.Sin(Math.PI * effectiveTime / maxTime) * 2f);
                float opacity =
                    forcedMasoEye
                    ? 1f
                    : Math.Min(1f, modifier * 2f);
                float sansScale =
                    forcedMasoEye
                    ? Projectile.scale * Main.cursorScale * 0.8f * Main.rand.NextFloat(0.75f, 1.25f)
                    : Projectile.scale * modifier * Main.cursorScale * 1.25f;

                Texture2D star = ModContent.Request<Texture2D>("InfernalEclipseAPI/YharimEX/Assets/ExtraTextures/LifeStar", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                Rectangle rect = new(0, 0, star.Width, star.Height);
                Vector2 origin = new(star.Width / 2 + sansScale, star.Height / 2 + sansScale);

                Vector2 drawPos = Projectile.Center;
                drawPos.X += 8 * Projectile.spriteDirection;
                drawPos.Y -= 11;

                Main.spriteBatch.UseBlendState(BlendState.Additive);

                Main.spriteBatch.Draw(star, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(rect), color * opacity, rotation, origin, sansScale, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(star, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(rect), Color.White * opacity * 0.75f, rotation, origin, sansScale, SpriteEffects.None, 0);
                /*DrawData starDraw = new DrawData(star, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(rect), Color.White * opacity, rotation, origin, sansScale, SpriteEffects.None, 0);
                GameShaders.Misc["LCWingShader"].UseColor(Color.LimeGreen * opacity).UseSecondaryColor(color * opacity);
                GameShaders.Misc["LCWingShader"].Apply(starDraw);
                starDraw.Draw(spriteBatch);*/

                Main.spriteBatch.ResetToDefault();
            }

            //if (auraTrail) Main.EntitySpriteDraw(lightning, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(lightningRectangle), Color.White * Projectile.Opacity, Projectile.rotation, lightningRectangle.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}