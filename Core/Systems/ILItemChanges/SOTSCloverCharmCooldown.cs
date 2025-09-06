using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Core.Players;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems.ILItemChanges
{
    [ExtendsFromMod("SOTS")]
    public class SOTSCloverCharmCooldown : ModSystem
    {
        private Hook _cloverHook;
        private static MethodInfo _getSotsPlayer;      // SOTS.SOTSPlayer.ModPlayer(Player)
        private static FieldInfo _critLifestealField; // int SOTS.SOTSPlayer.CritLifesteal

        public override bool IsLoadingEnabled(Mod mod)
        {
            return InfernalConfig.Instance.SOTSBalanceChanges;
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod("SOTS", out Mod sots) || sots.Code is null)
                return;

            // Reflect SOTS types/members we need
            var sotsPlayerType = sots.Code.GetType("SOTS.SOTSPlayer");
            var cloverType = sots.Code.GetType("SOTS.Items.CritBonus.CloverCharm");

            if (sotsPlayerType is null || cloverType is null)
                return;

            _getSotsPlayer = sotsPlayerType.GetMethod("ModPlayer", BindingFlags.Public | BindingFlags.Static, new[] { typeof(Player) });
            _critLifestealField = sotsPlayerType.GetField("CritLifesteal", BindingFlags.Public | BindingFlags.Instance);

            var updateAcc = cloverType.GetMethod("UpdateAccessory", BindingFlags.Public | BindingFlags.Instance, new[] { typeof(Player), typeof(bool) });
            if (updateAcc is null || _getSotsPlayer is null || _critLifestealField is null)
                return;

            // Hook CloverCharm.UpdateAccessory
            _cloverHook = new Hook(updateAcc, Clover_UpdateAccessory_Hook);
        }

        public override void Unload()
        {
            _cloverHook?.Dispose();
            _cloverHook = null;
            _getSotsPlayer = null;
            _critLifestealField = null;
        }

        // Original method signature
        private delegate void UpdateAccessoryOrig(object self, Player player, bool hideVisual);

        // Our detour: apply +1% crit always; only allow lifesteal roll when cooldown is ready
        private static void Clover_UpdateAccessory_Hook(UpdateAccessoryOrig orig, object self, Player player, bool hideVisual)
        {
            var cd = player.GetModPlayer<InfernalPlayer>();

            if (cd.CloverCharmCooldown > 0)
            {
                // Preserve the Clover Charm's +1% generic crit even while on cooldown
                player.GetCritChance(DamageClass.Generic) += 1f;
                return; // Skip the original to suppress its lifesteal roll this tick
            }

            // Snapshot CritLifesteal before calling original
            int before = GetCritLifesteal(player);

            // Run original CloverCharm.UpdateAccessory (may add +1 crit and roll lifesteal 3–5 on 50% chance)
            orig(self, player, hideVisual);

            int after = GetCritLifesteal(player);

            // If the Clover Charm actually granted lifesteal this tick, start the 15-tick cooldown
            if (after > before)
                cd.CloverCharmCooldown = 30;
        }

        private static int GetCritLifesteal(Player player)
        {
            if (_getSotsPlayer is null || _critLifestealField is null)
                return 0;

            object? sotsPlayer = _getSotsPlayer.Invoke(null, new object[] { player });
            return sotsPlayer is null ? 0 : (int)(_critLifestealField.GetValue(sotsPlayer) ?? 0);
        }
    }
}
