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
using static CalamityMod.Projectiles.BaseProjectiles.BaseMaceFlailProjectile;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.SplitFirebrand
{
    public class SplitFirebrandExplosion : ModProjectile
    {
        public override string Texture => "InfernalEclipseAPI/Assets/Textures/Empty";

        public override void SetDefaults()
        {
            ((Entity)((ModProjectile)this).Projectile).width = 120;
            ((Entity)((ModProjectile)this).Projectile).height = 120;
            ((ModProjectile)this).Projectile.friendly = true;
            ((ModProjectile)this).Projectile.ignoreWater = false;
            ((ModProjectile)this).Projectile.tileCollide = false;
            ((ModProjectile)this).Projectile.alpha = 255;
            ((ModProjectile)this).Projectile.penetrate = -1;
            ((ModProjectile)this).Projectile.timeLeft = 60;
            ((ModProjectile)this).Projectile.usesLocalNPCImmunity = true;
            ((ModProjectile)this).Projectile.localNPCHitCooldown = 600;
            Projectile.DamageType = LegendarySummonMeleeSpeed.Instance;
        }

        public override void AI()
        {
            Lighting.AddLight(((Entity)((ModProjectile)this).Projectile).Center, 0.9f, 0.8f, 0.6f);
            ((ModProjectile)this).Projectile.ai[1] += 0.01f;
            ((ModProjectile)this).Projectile.ai[0] += 1f;
            if (((ModProjectile)this).Projectile.ai[0] >= (float)(3 * Main.projFrames[((ModProjectile)this).Type]))
            {
                ((ModProjectile)this).Projectile.Kill();
                return;
            }
            int incrementer = ((ModProjectile)this).Projectile.frameCounter + 1;
            ((ModProjectile)this).Projectile.frameCounter = incrementer;
            if (incrementer >= 3)
            {
                ((ModProjectile)this).Projectile.frameCounter = 0;
                incrementer = ((ModProjectile)this).Projectile.frame + 1;
                ((ModProjectile)this).Projectile.frame = incrementer;
                if (incrementer >= Main.projFrames[((ModProjectile)this).Type])
                {
                    ((ModProjectile)this).Projectile.hide = true;
                }
            }
            Projectile projectile = ((ModProjectile)this).Projectile;
            projectile.alpha -= 63;
            if (((ModProjectile)this).Projectile.alpha < 0)
            {
                ((ModProjectile)this).Projectile.alpha = 0;
            }
            if (((ModProjectile)this).Projectile.ai[0] == 1f)
            {
                Vector2 oldCenter = Projectile.Center;

                Projectile.width = 120;
                Projectile.height = 120;

                Projectile.Center = oldCenter;

                SoundEngine.PlaySound(SoundID.Item14);
                for (int dustIndexA = 0; dustIndexA < 4; dustIndexA++)
                {
                    int smoky = Dust.NewDust(((Entity)((ModProjectile)this).Projectile).position, ((Entity)((ModProjectile)this).Projectile).width, ((Entity)((ModProjectile)this).Projectile).height, 31, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[smoky].position = ((Entity)((ModProjectile)this).Projectile).Center + Utils.RotatedByRandom(Vector2.UnitY, Math.PI) * (float)Main.rand.NextDouble() * (float)((Entity)((ModProjectile)this).Projectile).width / 2f;
                }
                for (int dustIndexB = 0; dustIndexB < 10; dustIndexB++)
                {
                    int fireDust = Dust.NewDust(((Entity)((ModProjectile)this).Projectile).position, ((Entity)((ModProjectile)this).Projectile).width, ((Entity)((ModProjectile)this).Projectile).height, 6, 0f, 0f, 200, default(Color), 2.7f);
                    Dust obj = Main.dust[fireDust];
                    obj.position = ((Entity)((ModProjectile)this).Projectile).Center + Utils.RotatedByRandom(Vector2.UnitY, Math.PI) * (float)Main.rand.NextDouble() * (float)((Entity)((ModProjectile)this).Projectile).width / 2f;
                    obj.noGravity = true;
                    obj.velocity *= 3f;
                    fireDust = Dust.NewDust(((Entity)((ModProjectile)this).Projectile).position, ((Entity)((ModProjectile)this).Projectile).width, ((Entity)((ModProjectile)this).Projectile).height, 6, 0f, 0f, 100, default(Color), 1.5f);
                    obj.position = ((Entity)((ModProjectile)this).Projectile).Center + Utils.RotatedByRandom(Vector2.UnitY, Math.PI) * (float)Main.rand.NextDouble() * (float)((Entity)((ModProjectile)this).Projectile).width / 2f;
                    obj.velocity *= 2f;
                    obj.noGravity = true;
                    obj.fadeIn = 2.5f;
                }
                for (int dustIndexC = 0; dustIndexC < 5; dustIndexC++)
                {
                    int fireDust2 = Dust.NewDust(((Entity)((ModProjectile)this).Projectile).position, ((Entity)((ModProjectile)this).Projectile).width, ((Entity)((ModProjectile)this).Projectile).height, 6, 0f, 0f, 0, default(Color), 2.7f);
                    Dust obj2 = Main.dust[fireDust2];
                    obj2.position = ((Entity)((ModProjectile)this).Projectile).Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, Math.PI), (double)Utils.ToRotation(((Entity)((ModProjectile)this).Projectile).velocity), default(Vector2)) * (float)((Entity)((ModProjectile)this).Projectile).width / 2f;
                    obj2.noGravity = true;
                    obj2.velocity *= 3f;
                }
                for (int dustIndexD = 0; dustIndexD < 10; dustIndexD++)
                {
                    int smokier = Dust.NewDust(((Entity)((ModProjectile)this).Projectile).position, ((Entity)((ModProjectile)this).Projectile).width, ((Entity)((ModProjectile)this).Projectile).height, 31, 0f, 0f, 0, default(Color), 1.5f);
                    Dust obj3 = Main.dust[smokier];
                    obj3.position = ((Entity)((ModProjectile)this).Projectile).Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, Math.PI), (double)Utils.ToRotation(((Entity)((ModProjectile)this).Projectile).velocity), default(Vector2)) * (float)((Entity)((ModProjectile)this).Projectile).width / 2f;
                    obj3.noGravity = true;
                    obj3.velocity *= 3f;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return ((ModProjectile)this).Projectile.ai[0] > 1f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 127);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.boss)
            {
                target.GetGlobalNPC<SplitFirebrandBossCooldown>().ExplosionCooldown = 300;
            }
            GetSoulBurn(target);
            ((Entity)((ModProjectile)this).Projectile).direction = ((Entity)Main.player[((ModProjectile)this).Projectile.owner]).direction;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (!target.boss) return true;

            var cooldown = target.GetGlobalNPC<SplitFirebrandBossCooldown>();

            return cooldown.ExplosionCooldown <= 0;
        }

        public void GetSoulBurn(NPC target)
        {
            if (NPC.downedMoonlord)
                target.AddBuff(ModContent.BuffType<SoulBurn7>(), 240);
            else if (NPC.downedGolemBoss)
                target.AddBuff(ModContent.BuffType<SoulBurn6>(), 240);
            else if (NPC.downedPlantBoss)
                target.AddBuff(ModContent.BuffType<SoulBurn5>(), 240);
            else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                target.AddBuff(ModContent.BuffType<SoulBurn4>(), 240);
            else if (Main.hardMode)
                target.AddBuff(ModContent.BuffType<SoulBurn3>(), 240);
            else if (NPC.downedBoss3)
                target.AddBuff(ModContent.BuffType<SoulBurn2>(), 240);
            else
                target.AddBuff(ModContent.BuffType<SoulBurn>(), 240);
        }
    }

    public class SplitFirebrandBossCooldown : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int ExplosionCooldown;

        public override void ResetEffects(NPC npc)
        {
            if (ExplosionCooldown > 0)
                ExplosionCooldown--;
        }
    }
}
