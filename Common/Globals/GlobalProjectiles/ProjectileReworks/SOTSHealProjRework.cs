using InfernalEclipseAPI.Core.Players;
using Microsoft.Xna.Framework;
using SOTS;
using SOTS.Projectiles.Base;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Common.Globals.GlobalProjectiles.ProjectileReworks
{
    [ExtendsFromMod("SOTS")]
    [JITWhenModsEnabled("SOTS")]
    public class SOTSHealProjRework : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private int previousLife;
        private int previousMana;
        private float previousVoid;
        private bool initialized;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ProjectileType<HealProj>();
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Player player = Main.player[projectile.owner];

            // HealProj uses projectile.damage == 0 for life healing.
            if (projectile.damage == 0 && player.lifeSteal <= 0f)
            {
                projectile.active = false;
            }
            if ((projectile.damage == 1 || projectile.ai[1] == 8) && player.GetModPlayer<InfernalPlayer>().manaSteal <= 0f)
            {
                projectile.active = false;
            }
            if (projectile.damage == 2 && player.GetModPlayer<InfernalPlayer>().voidSteal <= 0f)
            {
                projectile.active = false;
            }
            if (projectile.ai[1] == 10 && player.GetModPlayer<InfernalPlayer>().inspirationSteal <= 0f)
            {
                projectile.active = false;
            }
        }

        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];

            if (!initialized)
            {
                previousLife = player.statLife;
                previousMana = player.statMana;
                previousVoid = player.VoidPlayer().voidMeter;
                initialized = true;
            }

            return base.PreAI(projectile);
        }

        public override void AI(Projectile projectile)
        {
            if (!initialized)
            {
                Player player = Main.player[projectile.owner];
                previousLife = player.statLife;
                previousMana = player.statMana;
                previousVoid = player.VoidPlayer().voidMeter;
                initialized = true;
            }
        }

        public override void PostAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];

            // HealProj uses damage == 0 for life healing.
            if (projectile.damage == 0)
            {
                previousMana = player.statMana;
                previousVoid = player.VoidPlayer().voidMeter;

                int healedAmount = player.statLife - previousLife;

                float cooldownMult = 6f - healedAmount;

                if (cooldownMult < 1)
                    cooldownMult = 1;

                if (healedAmount > 0)
                    player.lifeSteal -= healedAmount * cooldownMult;

                if (player.lifeSteal < -70f)
                    player.lifeSteal = -70f;

                previousLife = player.statLife;
            }

            if (projectile.damage == 1 || projectile.ai[1] == 8)
            {
                previousLife = player.statLife;
                previousVoid = player.VoidPlayer().voidMeter;

                int healedAmount = player.statMana - previousMana;

                float cooldownMult = 3.5f - healedAmount;

                if (cooldownMult < 1)
                    cooldownMult = 1;

                if (healedAmount > 0)
                    player.GetModPlayer<InfernalPlayer>().manaSteal -= healedAmount * cooldownMult;

                if (player.GetModPlayer<InfernalPlayer>().manaSteal < -40f)
                    player.GetModPlayer<InfernalPlayer>().manaSteal = -40f;

                previousMana = player.statMana;
            }


            if (projectile.damage == 2)
            {
                previousLife = player.statLife;
                previousMana = player.statMana;

                float healedAmount = player.VoidPlayer().voidMeter - previousVoid;

                float cooldownMult = 4f - healedAmount;

                if (cooldownMult < 1)
                    cooldownMult = 1;

                if (healedAmount > 0)
                    player.GetModPlayer<InfernalPlayer>().voidSteal -= healedAmount * cooldownMult;

                if (player.GetModPlayer<InfernalPlayer>().voidSteal < -45f)
                    player.GetModPlayer<InfernalPlayer>().voidSteal = -45f;

                previousVoid = player.VoidPlayer().voidMeter;
            }

            if ((int)projectile.ai[1] == 10)
            {
                // Mirror their conditions in SOTSBardHealer for inspiration heal:
                // if projectile.timeLeft < 720, distance < 20, and ai[0] > 0, they heal then kill.
                if (projectile.timeLeft < 720)
                {
                    Vector2 toPlayer = player.Center - projectile.Center;
                    bool reachedPlayer = toPlayer.Length() < 20f;
                    int healedAmount = (int)projectile.ai[0];

                    if (reachedPlayer && healedAmount > 0f)
                    {
                        float cooldownMult = 6f - healedAmount;
                        if (cooldownMult < 1f)
                            cooldownMult = 1f;

                        player.GetModPlayer<InfernalPlayer>().inspirationSteal -= healedAmount * cooldownMult;

                        if (player.GetModPlayer<InfernalPlayer>().inspirationSteal < -10f)
                            player.GetModPlayer<InfernalPlayer>().inspirationSteal = -10f;
                    }
                }
            }
        }
    }
}
