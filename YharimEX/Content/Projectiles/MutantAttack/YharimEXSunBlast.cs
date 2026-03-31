using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using InfernalEclipseAPI.YharimEX.Content.NPCs.Bosses;
using InfernalEclipseAPI.YharimEX.Core.Systems;

namespace InfernalEclipseAPI.YharimEX.Content.Projectiles.MutantAttack
{
    public class YharimEXSunBlast : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_687";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = Main.projFrames[ProjectileID.LunarFlare];
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;
            Projectile.width = 70;
            Projectile.height = 70;
            CooldownSlot = 1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_Parent parent && parent.Entity is NPC npc
                && (npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight))
                Projectile.localAI[2] = 1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[2]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[2] = reader.ReadSingle();
        }

        public override bool? CanDamage()
        {
            return Projectile.frame == 3 || Projectile.frame == 4;
        }

        public override void AI()
        {
            if (Projectile.position.HasNaNs())
            {
                Projectile.Kill();
                return;
            }

            if (++Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame--;
                    Projectile.Kill();
                    return;
                }
            }

            if (Projectile.localAI[1] == 0)
            {
                SoundEngine.PlaySound(SoundID.Item88, Projectile.Center);
                Projectile.position = Projectile.Center;
                Projectile.scale = Projectile.localAI[2] == 0 ? Main.rand.NextFloat(1.5f, 4f)
                    : 3f;
                Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                Projectile.width = (int)(Projectile.width * Projectile.scale);
                Projectile.height = (int)(Projectile.height * Projectile.scale);
                Projectile.Center = Projectile.position;
            }

            if (++Projectile.localAI[1] == 6 && Projectile.ai[1] > 0 && YharimEXUtils.HostCheck)
            {
                Projectile.ai[1]--;

                Vector2 baseDirection = Projectile.ai[0].ToRotationVector2();
                float random = MathHelper.ToRadians(15);

                if (Projectile.localAI[0] != 2f)
                {
                    //spawn stationary blasts
                    float stationaryPersistence = Math.Min(5, Projectile.ai[1]);
                    int p = Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center + Main.rand.NextVector2Circular(20, 20), Vector2.Zero, Projectile.type,
                        Projectile.damage, 0f, Projectile.owner, Projectile.ai[0], stationaryPersistence);
                    if (p != Main.maxProjectiles)
                        Main.projectile[p].localAI[0] = 1f;
                }

                if (Projectile.localAI[0] != 1f)
                {
                    float length = Projectile.width / Projectile.scale * 10f / 7f;
                    Vector2 offset = length * baseDirection.RotatedBy(Main.rand.NextFloat(-random, random));
                    int p = Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center + offset, Vector2.Zero, Projectile.type,
                          Projectile.damage, 0f, Projectile.owner, Projectile.ai[0], Projectile.ai[1]);
                    if (p != Main.maxProjectiles)
                        Main.projectile[p].localAI[0] = Projectile.localAI[0];
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Burning, 120);
            target.AddBuff(BuffID.OnFire, 300);
        }
        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 100) * Projectile.Opacity;
    }
}

