using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using InfernalEclipseAPI.Core.Systems;
using ThoriumRework;
using CalamityMod.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod.Particles;
using Terraria.DataStructures;
using InfernalEclipseAPI.Content.Items.Weapons.Legendary.SplitFirebrand;
using CalamityMod.Systems.Collections;

namespace InfernalEclipseAPI.Content.Buffs.SoulBurn
{
    public class SoulBurn : ModBuff
    {
        public int damage;
        public int buffRevision;
        private int lastBuffHash;

        public static DebuffData debuffData = new DebuffData
        {
            EnemyLostRegen = 20f,
            HeatDebuffScaling = 1f,
            DrawAboveNPC = true
        };

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;

            CalamityBuffSets.DebuffDataset[Type] = debuffData;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<SoulBurnPlayer>().hasSoulBurn = true;

            player.Calamity().HeatDebuffMultiplier += 0.25f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<SoulBurnNPC>().hasSoulBurn = true;

            npc.Calamity().HeatDebuffMultiplier += 0.25f;

            DrawEffects(npc, ref npc.color);
        }

        internal static void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Utils.NextBool(Main.rand, 4))
            {
                Vector2 position = npc.Center +
                    new Vector2(
                        Utils.NextFloat(Main.rand, -npc.width / 2f, npc.width / 2f),
                        Utils.NextFloat(Main.rand, -npc.height / 2f, npc.height / 2f)
                    );

                Vector2 Vect = Utils.RotatedByRandom(
                    new Vector2(0f, Utils.NextBool(Main.rand, 4) ? -5f : -9f),
                    MathHelper.ToRadians(25f)
                ) * Utils.NextFloat(Main.rand, 0.1f, 1.9f);

                GeneralParticleHandler.SpawnParticle(
                    new CritSpark(position, Vect,
                        Utils.NextBool(Main.rand) ? Color.Red : Color.DarkRed,
                        Color.IndianRed,
                        0.8f, 15, 2f, 1.9f)
                );
            }

            if (Utils.NextBool(Main.rand, 4))
            {
                Vector2 val = npc.position - Vector2.One * 2f;
                Vector2 dustVel = npc.velocity + new Vector2(0f, Utils.NextFloat(Main.rand, -5f, -1f));

                Dust obj = Dust.NewDustDirect(val, npc.width + 4, npc.height + 4, DustID.GemTopaz,
                    dustVel.X, dustVel.Y, 0, default(Color), 1f);

                obj.noGravity = true;
                obj.scale = Utils.NextFloat(Main.rand, 0.7f, 1.2f);
                obj.alpha = 235;
            }

            Lighting.AddLight(npc.position, 0.25f, 0.25f, 0.1f);
        }
    }

    public class SoulBurnPlayer : ModPlayer
    {
        public bool hasSoulBurn;
        public int whipDamage;

        public override void ResetEffects()
        {
            // reset each frame automatically
            hasSoulBurn = false;
        }

        public override void PostUpdate()
        {
            if (hasSoulBurn)
            {
                Player.Calamity().HeatDebuffMultiplier -= 0.25f;

                if (NPC.downedMoonlord)
                {
                    Player.Calamity().ElectricDebuffMultiplier -= 0.25f;
                }
            }
        }

        public override void Kill( double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (!hasSoulBurn) return;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.downedBoss3 || Main.hardMode || NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 || NPC.downedPlantBoss || NPC.downedGolemBoss || NPC.downedMoonlord)
                {
                    Projectile.NewProjectile(Player.GetSource_Death(), Player.Center, Vector2.Zero, ModContent.ProjectileType<SplitFirebrandExplosion>(), whipDamage, 0f, Main.myPlayer);
                }
            }
        }
    }

    public class SoulBurnNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool hasSoulBurn;
        public int whipDamage;

        public override void ResetEffects(NPC npc)
        {
            hasSoulBurn = false;
        }

        public override void PostAI(NPC npc)
        {
            if (hasSoulBurn)
            {
                npc.Calamity().HeatDebuffMultiplier -= 0.25f;

                if (NPC.downedMoonlord)
                {
                    npc.Calamity().ElectricDebuffMultiplier -= 0.25f;
                }
            }
        }

        public override void OnKill(NPC npc)
        {
            if (!hasSoulBurn) return;

            if (NPC.downedBoss3 || Main.hardMode || NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 || NPC.downedPlantBoss || NPC.downedGolemBoss || NPC.downedMoonlord)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile( npc.GetSource_Death(), npc.Center, Vector2.Zero, ModContent.ProjectileType<SplitFirebrandExplosion>(), whipDamage, 0f, Main.myPlayer);
                }
            }
        }
    }
}