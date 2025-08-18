using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using NoxusBoss.Content.NPCs.Bosses.NamelessDeity.Projectiles;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod;
using InfernalEclipseAPI.Content.Items.Weapons.Nameless.Genesis.RandomSong;

namespace InfernalEclipseAPI.Content.Items.Weapons.Nameless.Genesis
{
    [ExtendsFromMod("NoxusBoss", "ThoriumMod")]
    [JITWhenModsEnabled("NoxusBoss", "ThoriumMod")]
    public class GalaxyRainController : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public static float DifficultyFactor => 1f;
        public static int StartingReleaseRate => (int)MathHelper.Clamp(20f / DifficultyFactor, 6f, 1000f);
        public static int MinReleaseRate => (int)MathHelper.Clamp(6f / DifficultyFactor, 1f, 1000f);
        private ref float releaseTimer => ref Projectile.ai[1];
        private ref float releaseCounter => ref Projectile.localAI[0];

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Galaxy Rain Controller");
        }

        private bool _musicStopped; // guard against double-stop

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2; // we’ll keep resetting while held
            Projectile.hide = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            _musicStopped = false;
        }

        private void StopMusicIfLocal()
        {
            if (!_musicStopped && Main.myPlayer == Projectile.owner)
            {
                Main.player[Projectile.owner].GetModPlayer<RandomSongPlayer>().StopForcedSong();
                _musicStopped = true;
            }
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead)
            {
                StopMusicIfLocal();
                Projectile.Kill();      // <-- safe: we're in AI, not OnKill
                return;
            }

            // keep-alive while holding
            if (owner.channel && owner.HeldItem?.type == owner.inventory[owner.selectedItem].type)
            {
                Projectile.timeLeft = 2;
                if (Main.myPlayer == owner.whoAmI)
                    owner.GetModPlayer<RandomSongPlayer>().EnsureRandomSongPlaying();
            }
            else
            {
                StopMusicIfLocal();
                Projectile.Kill();      // <-- end when released
                return;
            }

            // Exact ramp formula from your boss snippet:
            int releaseRate = (int)MathHelper.Clamp(StartingReleaseRate - releaseCounter * 3f, MinReleaseRate, StartingReleaseRate);

            // Window is simply "while holding"; continuously spawn using the rate
            releaseTimer++;

            if (releaseTimer >= releaseRate)
            {
                SoundEngine.PlaySound(SoundID.Item117 with { Volume = 0.6f }, owner.Center);

                if (Main.myPlayer == owner.whoAmI)
                {
                    float horizontalOffset = Main.rand.NextFloatDirection() * 900f;
                    if (Main.rand.NextBool(8)) horizontalOffset = 0f;

                    Vector2 spawnPos = owner.Center
                                     + Vector2.UnitX * (horizontalOffset + owner.velocity.X * Main.rand.NextFloat(30f, 45f));

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        spawnPos,
                        Vector2.UnitY,
                        ModContent.ProjectileType<FallingGalaxy>(),   // your friendly projectile
                        Projectile.damage,
                        0f,
                        owner.whoAmI
                    );
                }

                releaseTimer = 0f;
                releaseCounter++;
                Projectile.netUpdate = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            // Only stop music here. DO NOT call Kill() from here.
            StopMusicIfLocal();
        }
    }
}
