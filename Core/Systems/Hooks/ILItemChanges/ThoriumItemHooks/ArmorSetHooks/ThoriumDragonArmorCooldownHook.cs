using System.Reflection;
using InfernalEclipseAPI.Content.RogueThrower;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using Terraria.Audio;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.ThoriumItemHooks.ArmorSetHooks
{
    public class ThoriumDragonArmorCooldownHook : ModSystem
    {
        private Hook onHitProjHook;
        private Hook onHitItemHook;

        private static Type thoriumPlayerType;
        private static FieldInfo dragonSetField;

        private static int dragonPulseType = -1;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            thoriumPlayerType = thorium.Code.GetType("ThoriumMod.ThoriumPlayer");
            if (thoriumPlayerType is null)
                return;

            dragonSetField = thoriumPlayerType.GetField("dragonSet", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            MethodInfo projMethod = thoriumPlayerType.GetMethod("OnHitNPCWithProj", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo itemMethod = thoriumPlayerType.GetMethod("OnHitNPCWithItem", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (ModContent.TryFind("ThoriumMod/DragonPulse", out ModProjectile dragonPulse))
                dragonPulseType = dragonPulse.Type;

            if (projMethod is not null)
                onHitProjHook = new Hook(projMethod, OnHitNPCWithProj_Hook);

            if (itemMethod is not null)
                onHitItemHook = new Hook(itemMethod, OnHitNPCWithItem_Hook);
        }

        public override void Unload()
        {
            onHitProjHook?.Dispose();
            onHitItemHook?.Dispose();

            onHitProjHook = null;
            onHitItemHook = null;

            thoriumPlayerType = null;
            dragonSetField = null;
            dragonPulseType = -1;
        }

        private delegate void Orig_OnHitNPCWithProj(object self, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone);
        private delegate void Orig_OnHitNPCWithItem(object self, Item item, NPC target, NPC.HitInfo hit, int damageDone);

        private static Player GetPlayer(object thoriumPlayer)
        {
            return thoriumPlayer is ModPlayer mp ? mp.Player : null;
        }

        private static bool GetDragonSet(object thoriumPlayer)
        {
            return dragonSetField?.GetValue(thoriumPlayer) is bool b && b;
        }

        private static void SetDragonSet(object thoriumPlayer, bool value)
        {
            dragonSetField?.SetValue(thoriumPlayer, value);
        }

        private static void SpawnDragonSetProc(Player player, IEntitySource source, Vector2 targetCenter, int baseDamage)
        {
            if (dragonPulseType < 0)
                return;

            SoundEngine.PlaySound(SoundID.Item74, player.position);

            for (int i = 0; i < 5; i++)
            {
                Vector2 spawnPos = targetCenter + new Vector2(Main.rand.Next(-45, 45), Main.rand.Next(-45, 45));

                Projectile.NewProjectile(
                    source,
                    spawnPos,
                    Vector2.Zero,
                    dragonPulseType,
                    (int)(baseDamage * 0.25f),
                    0f,
                    player.whoAmI);
            }
        }

        private void OnHitNPCWithProj_Hook(Orig_OnHitNPCWithProj orig, object self, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = GetPlayer(self);
            if (player is null || dragonSetField is null || dragonPulseType < 0)
            {
                orig(self, proj, target, hit, damageDone);
                return;
            }

            RogueThrowerPlayer cooldowns = player.GetModPlayer<RogueThrowerPlayer>();
            bool dragonSetEnabled = GetDragonSet(self);

            // Always suppress Thorium's native dragon proc so only our version can happen.
            if (dragonSetEnabled)
                SetDragonSet(self, false);

            orig(self, proj, target, hit, damageDone);

            if (dragonSetEnabled)
                SetDragonSet(self, true);

            if (!dragonSetEnabled)
                return;

            if (cooldowns.dragonSetCooldown > 0)
                return;

            // Match Thorium's original projectile restriction.
            if (proj.type == dragonPulseType)
                return;

            if (!Main.rand.NextBool(5))
                return;

            cooldowns.dragonSetCooldown = 60;
            SpawnDragonSetProc(player, proj.GetSource_OnHit(target), target.Center, proj.damage);
        }

        private void OnHitNPCWithItem_Hook(Orig_OnHitNPCWithItem orig, object self, Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = GetPlayer(self);
            if (player is null || dragonSetField is null || dragonPulseType < 0)
            {
                orig(self, item, target, hit, damageDone);
                return;
            }

            RogueThrowerPlayer cooldowns = player.GetModPlayer<RogueThrowerPlayer>();
            bool dragonSetEnabled = GetDragonSet(self);

            if (dragonSetEnabled)
                SetDragonSet(self, false);

            orig(self, item, target, hit, damageDone);

            if (dragonSetEnabled)
                SetDragonSet(self, true);

            if (!dragonSetEnabled)
                return;

            if (cooldowns.dragonSetCooldown > 0)
                return;

            if (!Main.rand.NextBool(5))
                return;

            cooldowns.dragonSetCooldown = 60;
            SpawnDragonSetProc(player, player.GetSource_OnHit(target), target.Center, hit.Damage);
        }
    }
}
