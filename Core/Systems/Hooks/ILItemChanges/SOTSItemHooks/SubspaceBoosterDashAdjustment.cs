using System.Reflection;
using InfernalEclipseAPI.Core.Players;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.SOTSItemHooks
{
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    public class SubspaceBoosterDashAdjustment : ModSystem
    {
        private static Hook _hook;

        private static Type _subspaceBoostersType;
        private static FieldInfo _hasActivateField;
        private static FieldInfo _doAccelField;
        private static MethodInfo _fireBoostPlayerMethod;

        private static int _subspaceBoosterProjType = -1;

        private delegate void Orig_UpdateAccessory(object self, Player player, bool hideVisual);

        public override void Load()
        {
            if (!ModLoader.HasMod("SOTS"))
                return;

            var sots = ModLoader.GetMod("SOTS");
            _subspaceBoostersType = sots?.Code?.GetType("SOTS.Items.SubspaceBoosters");
            if (_subspaceBoostersType is null)
                return;

            var updateAccessory = _subspaceBoostersType.GetMethod("UpdateAccessory",
                BindingFlags.Instance | BindingFlags.Public);

            if (updateAccessory is null)
                return;

            _hasActivateField = _subspaceBoostersType.GetField("hasActivate",
                BindingFlags.Instance | BindingFlags.NonPublic);
            _doAccelField = _subspaceBoostersType.GetField("doAccel",
                BindingFlags.Instance | BindingFlags.NonPublic);

            _fireBoostPlayerMethod = _subspaceBoostersType.GetMethod("FireBoostPlayer",
                BindingFlags.Instance | BindingFlags.Public);

            if (_hasActivateField is null || _doAccelField is null || _fireBoostPlayerMethod is null)
                return;

            if (ModContent.TryFind<ModProjectile>("SOTS", "SubspaceBoosterProj", out var proj))
                _subspaceBoosterProjType = proj.Type;

            _hook = new Hook(updateAccessory, Detour_UpdateAccessory);
        }

        public override void Unload()
        {
            _hook?.Dispose();
            _hook = null;

            _subspaceBoostersType = null;
            _hasActivateField = null;
            _doAccelField = null;
            _fireBoostPlayerMethod = null;

            _subspaceBoosterProjType = -1;
        }

        private static void Detour_UpdateAccessory(Orig_UpdateAccessory orig, object self, Player player, bool hideVisual)
        {
            player.buffImmune[67] = true;
            player.waterWalk = true;
            player.fireWalk = true;
            player.lavaMax += 600;
            player.rocketBoots = player.vanityRocketBoots = 4;
            player.iceSkate = true;
            player.moveSpeed += 0.2f;
            player.accRunSpeed = 7f;

            int num1 = 0;
            try
            {
                bool fireBoost = (bool)_fireBoostPlayerMethod.Invoke(self, new object[] { player });
                num1 = fireBoost ? 1 : 0;
            }
            catch
            {
                num1 = 0;
            }

            bool flag = false;
            int num2 = 0;

            var mp = player.GetModPlayer<InfernalPlayer>();
            if (mp is not null && mp.BoostPressTimer > 0)
            {
                num2 = mp.BoostDirection;
                if (num2 == 0)
                    num2 = player.direction;

                // clamp to -1/1
                num2 = num2 > 0 ? 1 : -1;

                flag = true;
            }

            if (Main.myPlayer == player.whoAmI)
            {
                int hasActivate = GetHasActivate(self);

                if (hasActivate == -1 && flag && player.velocity.Y == 0f)
                {
                    if (_subspaceBoosterProjType != -1)
                    {
                        Projectile.NewProjectile(
                            player.GetSource_Misc("SOTS:SubspaceBoosterMultiplayerSync"),
                            player.Center,
                            Vector2.Zero,
                            _subspaceBoosterProjType,
                            0,
                            0f,
                            Main.myPlayer,
                            num2,
                            0f,
                            0f);
                    }

                    SetHasActivate(self, 60);
                    hasActivate = 60;
                }

                if (hasActivate > -1)
                    SetHasActivate(self, hasActivate - 1);
            }

            bool doAccel = GetDoAccel(self);

            if (num1 == 0 || !doAccel)
                return;

            player.accRunSpeed = 0f;
            player.velocity *= 1.04166675f;
        }

        private static int GetHasActivate(object self)
        {
            try
            {
                return (int)_hasActivateField.GetValue(self);
            }
            catch
            {
                return -1;
            }
        }

        private static void SetHasActivate(object self, int value)
        {
            try
            {
                _hasActivateField.SetValue(self, value);
            }
            catch
            {
            }
        }

        private static bool GetDoAccel(object self)
        {
            try
            {
                return (bool)_doAccelField.GetValue(self);
            }
            catch
            {
                return false;
            }
        }
    }
}
