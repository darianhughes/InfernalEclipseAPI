using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using YharimEX.Content.Projectiles.FargoProjectile;
using YharimEX.Core.Globals;
using YharimEX.Core.Systems;
using YharimEX.Core.Players;
using InfernalEclipseAPI.YharimEX.Content.NPCs.Bosses;

namespace YharimEX.Content.Projectiles
{
    public class YharimEXSphereRing : ModProjectile
    {
        public override string Texture => "YharimEX/Assets/Projectiles/YharimEXSphere";

        protected bool DieOutsideArena;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 480;
            Projectile.alpha = 200;
            CooldownSlot = 1;

            if (Projectile.type == ModContent.ProjectileType<YharimEXSphereRing>())
            {
                DieOutsideArena = true;
                if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
                {
                    SetupFargoProjectile SetupFargoProjectile = Projectile.GetGlobalProjectile<SetupFargoProjectile>();
                    SetupFargoProjectile.TimeFreezeImmune = (YharimEXWorldFlags.MasochistModeReal || YharimEXWorldFlags.InfernumMode) && YharimEXGlobalUtilities.BossIsAlive(ref YharimEXGlobalNPC.yharimEXBoss, ModContent.NPCType<YharimEXBoss>()) && Main.npc[YharimEXGlobalNPC.yharimEXBoss].ai[0] == -5;
                }
            }
        }
        public override bool CanHitPlayer(Player target)
        {
            return target.hurtCooldowns[1] == 0 || YharimEXWorldFlags.MasochistModeReal;
        }

        private int ritualID = -1;

        float originalSpeed;
        bool spawned;

        public override void AI()
        {
            if (!spawned)
            {
                spawned = true;
                originalSpeed = Projectile.velocity.Length();
            }

            Projectile.velocity = originalSpeed * Vector2.Normalize(Projectile.velocity).RotatedBy(Projectile.ai[1] / (2 * Math.PI * Projectile.ai[0] * ++Projectile.localAI[0]));

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }
            Projectile.scale = 1f - Projectile.alpha / 255f;

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame > 1)
                    Projectile.frame = 0;
            }

            if (DieOutsideArena)
            {
                if (ritualID == -1) //identify the ritual CLIENT SIDE
                {
                    ritualID = -2; //if cant find it, give up and dont try every tick

                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<YharimEXRitual>())
                        {
                            ritualID = i;
                            break;
                        }
                    }
                }

                Projectile ritual = YharimEXGlobalUtilities.ProjectileExists(ritualID, ModContent.ProjectileType<YharimEXRitual>());
                if (ritual != null && Projectile.Distance(ritual.Center) > 1200f) //despawn faster
                    Projectile.timeLeft = 0;
            }

            TryTimeStop();
        }

        void TryTimeStop()
        {
            Mod FargoSouls = null;
            if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
            {
                FargoSouls = YharimEXCrossmodSystem.FargowiltasSouls.Mod;
                SetupFargoProjectile SetupFargoProjectile = Projectile.GetGlobalProjectile<SetupFargoProjectile>();
            }
            if (!Main.getGoodWorld)
                return;
            if (Projectile.hostile && !Projectile.friendly
                && Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost
                && YharimEXGlobalUtilities.BossIsAlive(ref YharimEXGlobalNPC.yharimEXBoss, ModContent.NPCType<YharimEXBoss>()))
            {
                //final spark spheres

                if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
                {
                    if ((YharimEXWorldFlags.MasochistModeReal || YharimEXWorldFlags.InfernumMode) && Main.npc[YharimEXGlobalNPC.yharimEXBoss].ai[0] == -5)
                {
                    if (!Main.LocalPlayer.HasBuff(FargoSouls.Find<ModBuff>("TimeFrozenBuff").Type))
                        SoundEngine.PlaySound(new SoundStyle("YharimEX/Assets/Sounds/Attacks/ZaWarudo"), Main.LocalPlayer.Center);
                    Main.LocalPlayer.AddBuff(FargoSouls.Find<ModBuff>("TimeFrozenBuff").Type, 300);
                }
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
            {
                if (YharimEXWorldFlags.DeathMode & !YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
                {
                    target.YharimPlayer().MaxLifeReduction += 100;
                }
                else if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
                {
                    EternityDebuffs.ManageOnHitDebuffs(target);
                }
                TryTimeStop();
            }
        }

        public override void OnKill(int timeleft)
        {
            if (Main.rand.NextBool(Main.player[Projectile.owner].ownedProjectileCounts[Projectile.type] / 10 + 1))
                SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.Center);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 208;
            Projectile.Center = Projectile.position;
            for (int index1 = 0; index1 < 2; ++index1)
            {
                int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0.0f, 0.0f, 100, new Color(), 1.5f);
                Main.dust[index2].position = new Vector2(Projectile.width / 2, 0.0f).RotatedBy(6.28318548202515 * Main.rand.NextDouble(), new Vector2()) * (float)Main.rand.NextDouble() + Projectile.Center;
            }
            for (int index1 = 0; index1 < 4; ++index1)
            {
                int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, 0.0f, 0.0f, 0, new Color(), 2.5f);
                Main.dust[index2].position = new Vector2(Projectile.width / 2, 0.0f).RotatedBy(6.28318548202515 * Main.rand.NextDouble(), new Vector2()) * (float)Main.rand.NextDouble() + Projectile.Center;
                Main.dust[index2].noGravity = true;
                Dust dust1 = Main.dust[index2];
                dust1.velocity *= 1f;
                int index3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, 0.0f, 0.0f, 100, new Color(), 1.5f);
                Main.dust[index3].position = new Vector2(Projectile.width / 2, 0.0f).RotatedBy(6.28318548202515 * Main.rand.NextDouble(), new Vector2()) * (float)Main.rand.NextDouble() + Projectile.Center;
                Dust dust2 = Main.dust[index3];
                dust2.velocity *= 1f;
                Main.dust[index3].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>("YharimEX/Assets/Projectiles/YharimEXSphereGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            int rect1 = glow.Height;
            int rect2 = 0;
            Rectangle glowrectangle = new(0, rect2, glow.Width, rect1);
            Vector2 gloworigin2 = glowrectangle.Size() / 2f;
            Color glowcolor = Color.Lerp(Color.Red, Color.Transparent, 0.9f);
            glowcolor *= Projectile.Opacity;
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++) //reused betsy fireball scaling trail thing
            {

                Color color27 = glowcolor;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                float scale = Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.oldPos[i] - Vector2.Normalize(Projectile.velocity) * i * 6;
                Main.EntitySpriteDraw(glow, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(glowrectangle), color27,
                    Projectile.velocity.ToRotation() + MathHelper.PiOver2, gloworigin2, scale * 1.5f, SpriteEffects.None, 0);
            }
            glowcolor = Color.Lerp(new Color(255, 255, 255, 0), Color.Transparent, 0.85f);
            Main.EntitySpriteDraw(glow, Projectile.position + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(glowrectangle), glowcolor,
                    Projectile.velocity.ToRotation() + MathHelper.PiOver2, gloworigin2, Projectile.scale * 1.5f, SpriteEffects.None, 0);

            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
        }
    }
}