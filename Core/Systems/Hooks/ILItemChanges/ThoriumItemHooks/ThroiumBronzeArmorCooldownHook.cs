using System.Reflection;
using InfernalEclipseAPI.Content.RogueThrower;
using MonoMod.RuntimeDetour;
using RagnarokMod.Utils;
using Terraria.Audio;
using Terraria.DataStructures;
using ThoriumMod.Projectiles;
using ThoriumMod.Sounds;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.ThoriumItemHooks
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class ThroiumBronzeArmorCooldownHook : ModSystem
    {
        private Hook onHitNPCWithProjHook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            Type thoriumPlayerType = thorium.Code.GetType("ThoriumMod.ThoriumPlayer");
            MethodInfo target = thoriumPlayerType?.GetMethod(
                "OnHitNPCWithProj",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (target is null)
                return;

            onHitNPCWithProjHook = new Hook(target, OnHitNPCWithProj_Hook);
        }

        public override void Unload()
        {
            onHitNPCWithProjHook?.Dispose();
            onHitNPCWithProjHook = null;
        }

        private delegate void Orig_OnHitNPCWithProj(object self, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone);

        private void OnHitNPCWithProj_Hook(Orig_OnHitNPCWithProj orig, object self, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (self is not ModPlayer mp || mp.Player is not Player player)
            {
                orig(self, proj, target, hit, damageDone);
                return;
            }

            // Read ThoriumPlayer.setBronze through reflection.
            Type thoriumPlayerType = self.GetType();
            FieldInfo setBronzeField = thoriumPlayerType.GetField("setBronze", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            bool setBronze = setBronzeField is not null && setBronzeField.GetValue(self) is bool b && b;

            // Temporarily disable Thorium's native bronze proc if our cooldown is active,
            // so the original method runs without spawning LightStrike.
            RogueThrowerPlayer cooldownPlayer = player.GetModPlayer<RogueThrowerPlayer>();

            bool suppressOriginalBronzeProc = false;
            if (setBronze && cooldownPlayer.bronzeSetCooldown > 0 && setBronzeField is not null)
            {
                setBronzeField.SetValue(self, false);
                suppressOriginalBronzeProc = true;
            }

            orig(self, proj, target, hit, damageDone);

            if (suppressOriginalBronzeProc)
                setBronzeField.SetValue(self, true);

            // Recreate the bronze proc ourselves, but only if off cooldown.
            if (!setBronze)
                return;

            if (cooldownPlayer.bronzeSetCooldown > 0)
                return;

            int lightStrikeType = ModContent.ProjectileType<LightStrike>();
            int throwingGuideFollowupType = ModContent.ProjectileType<ThoriumMod.Projectiles.Thrower.ThrowingGuideFollowup>();

            if (proj.type == lightStrikeType || proj.type == throwingGuideFollowupType)
                return;

            if (!proj.IsThrown())
                return;

            if (!Main.rand.NextBool(5))
                return;

            SoundEngine.PlaySound(ThoriumSounds.ParalyzeSound, target.Center);

            IEntitySource source = proj.GetSource_OnHit(target);

            int damage = (int)player.GetTotalDamage(DamageClass.Throwing).ApplyTo(30f);

            Projectile.NewProjectile(
                source,
                target.Center.X,
                target.Center.Y - 600f,
                0f,
                15f,
                lightStrikeType,
                damage,
                1f,
                proj.owner);

            cooldownPlayer.bronzeSetCooldown = 60; // 1 second
        }
    }
}
