using Terraria.ModLoader;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Terraria;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using CalamityMod;
using InfernalEclipseAPI;
using System.Collections.Generic;
using System;
using Terraria.Audio;
using Terraria.ID;

public class PauldronNerfSystem : ModSystem
{
    private static Hook postUpdateMiscEffectsHook;
    private static Dictionary<int, int> pauldronCooldowns = new(); // whoAmI -> ticks left
    private static Dictionary<int, int> pauldronDashTicks = new(); // whoAmI -> ticks in dash

    public override void Load()
    {
        var calamityPlayerType = typeof(CalamityPlayer);
        var postUpdateMethod = calamityPlayerType.GetMethod("PostUpdateMiscEffects", BindingFlags.Instance | BindingFlags.Public);
        postUpdateMiscEffectsHook = new Hook(postUpdateMethod, MyPostUpdateMiscEffects);
    }

    public override void Unload()
    {
        postUpdateMiscEffectsHook?.Dispose();
        pauldronCooldowns.Clear();
        pauldronDashTicks.Clear();
    }

    private void MyPostUpdateMiscEffects(Action<CalamityPlayer> orig, CalamityPlayer self)
    {
        Player player = self.Player;
        int whoAmI = player.whoAmI;

        // Ensure dictionaries are initialized
        if (!pauldronCooldowns.ContainsKey(whoAmI))
            pauldronCooldowns[whoAmI] = 0;
        if (!pauldronDashTicks.ContainsKey(whoAmI))
            pauldronDashTicks[whoAmI] = 0;

        // Decrement cooldown
        if (pauldronCooldowns[whoAmI] > 0)
            pauldronCooldowns[whoAmI]--;

        // Track dash ticks
        bool dashing = self.Pauldron && player.dashDelay == -1;
        if (dashing)
            pauldronDashTicks[whoAmI]++;
        else
            pauldronDashTicks[whoAmI] = 0;

        // --- Pauldron Dash Override ---
        if (self.Pauldron)
        {
            // Only run effect if not on cooldown, dash active, and first frame of dash
            if (player.dashDelay == -1 && pauldronCooldowns[whoAmI] <= 0 && pauldronDashTicks[whoAmI] == 1)
            {
                pauldronDashTicks[whoAmI] = 1; // Start at 1 for the first tick
                pauldronCooldowns[whoAmI] = 90; // Cooldown

                // Opening blast
                SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact with { Volume = 0.4f, PitchVariance = 0.4f }, player.Center);
                int openDamage = player.ApplyArmorAccDamageBonusesTo(player.GetBestClassDamage().ApplyTo(45));
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center + player.velocity * 1.5f, Vector2.Zero,
                    ModContent.ProjectileType<CalamityMod.Projectiles.Typeless.PauldronDash>(), openDamage, 16f, player.whoAmI);

                self.HasReducedDashFirstFrame = true;
            }
            else if (player.dashDelay != -1)
            {
                self.HasReducedDashFirstFrame = false;
                pauldronDashTicks[whoAmI] = 0;
            }

            // Grant endurance ONLY for the first 15 ticks of the dash after proccing
            if (pauldronDashTicks[whoAmI] > 0 && pauldronDashTicks[whoAmI] <= 15 
                && pauldronCooldowns[whoAmI] >= 75
                )
            {
                player.endurance += 0.05f;

                // --- Pulsing effect: every 7 ticks, 80 damage ---
                if (pauldronDashTicks[whoAmI] % 7 == 0)
                {
                    int pulseDamage = player.ApplyArmorAccDamageBonusesTo(player.GetBestClassDamage().ApplyTo(80));
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center + player.velocity * 1.5f, Vector2.Zero,
                        ModContent.ProjectileType<CalamityMod.Projectiles.Typeless.PauldronDash>(), pulseDamage, 10f, player.whoAmI);
                }
            }

            // Visuals (dust and sparks), only if in dash
            if (dashing && pauldronDashTicks[whoAmI] <= 15 && pauldronCooldowns[whoAmI] >= 75)
            {
                float numberOfDusts = 10f;
                float rotFactor = 180f / numberOfDusts;
                for (int i = 0; i < numberOfDusts; i++)
                {
                    float rot = MathHelper.ToRadians(i * rotFactor);
                    Vector2 offset = new Vector2(MathF.Min(player.velocity.X * player.direction * 0.7f + 8f, 20f), 0)
                        .RotatedBy(rot * Main.rand.NextFloat(4f, 5f));
                    Vector2 velOffset = Vector2.Zero;
                    int dustType = Main.rand.NextBool() ? 35 : 127;
                    Dust dust = Dust.NewDustPerfect(player.Center + offset + player.velocity, dustType, velOffset);
                    dust.noGravity = true;
                    dust.velocity = velOffset;
                    dust.alpha = 100;
                    dust.scale = MathF.Min(player.velocity.X * player.direction * 0.08f, 1.2f);
                }
                float sparkscale = MathF.Min(player.velocity.X * player.direction * 0.08f, 1.2f);
                Vector2 SparkVelocity1 = player.velocity.RotatedBy(player.direction * -3, default) * 0.1f - player.velocity / 2f;
                var spark = new CalamityMod.Particles.SparkParticle(player.Center + player.velocity.RotatedBy(2f * player.direction) * 1.5f, SparkVelocity1, false, Main.rand.Next(11, 13), sparkscale, Main.rand.NextBool() ? Microsoft.Xna.Framework.Color.DarkOrange : Microsoft.Xna.Framework.Color.OrangeRed);
                CalamityMod.Particles.GeneralParticleHandler.SpawnParticle(spark);
                Vector2 SparkVelocity2 = player.velocity.RotatedBy(player.direction * 3, default) * 0.1f - player.velocity / 2f;
                var spark2 = new CalamityMod.Particles.SparkParticle(player.Center + player.velocity.RotatedBy(-2f * player.direction) * 1.5f, SparkVelocity2, false, Main.rand.Next(11, 13), sparkscale, Main.rand.NextBool() ? Microsoft.Xna.Framework.Color.DarkOrange : Microsoft.Xna.Framework.Color.OrangeRed);
                CalamityMod.Particles.GeneralParticleHandler.SpawnParticle(spark2);
            }
        }

        // Prevent original Pauldron dash block from firing:
        bool originalPauldron = self.Pauldron;
        if (self.Pauldron) self.Pauldron = false; // Suppress original code block

        orig(self);

        self.Pauldron = originalPauldron; // Restore for other logic
    }
}