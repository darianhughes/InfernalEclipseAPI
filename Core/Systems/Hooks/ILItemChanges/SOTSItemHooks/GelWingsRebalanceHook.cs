using System.Reflection;
using MonoMod.RuntimeDetour;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.SOTSItemHooks
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class GelWingsRebalanceHook : ModSystem
    {
        private Hook verticalSpeedsHook;
        private Hook updateAccessoryHook;

        private static WingStats NewStats = new WingStats(70, 11.5f, 2f);

        public override void Load()
        {
            if (!ModLoader.TryGetMod("SOTS", out var sots) || !InfernalConfig.Instance.SOTSBalanceChanges)
                return;

            Type gelWingsType = sots.Code.GetType("SOTS.Items.Slime.GelWings", throwOnError: true);
            if (gelWingsType is null)
                return;

            MethodInfo vertical = gelWingsType.GetMethod("VerticalWingSpeeds",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            MethodInfo updateAcc = gelWingsType.GetMethod("UpdateAccessory",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (vertical != null)
                //verticalSpeedsHook = new Hook(vertical, (VerticalWingSpeedsDetour)Detour_VerticalWingSpeeds);

            if (updateAcc != null)
                updateAccessoryHook = new Hook(updateAcc, (UpdateAccessoryDetour)Detour_UpdateAccessory);
        }

        public override void Unload()
        {
            verticalSpeedsHook?.Dispose();
            verticalSpeedsHook = null;

            updateAccessoryHook?.Dispose();
            updateAccessoryHook = null;
        }

        public override void PostSetupContent()
        {
            if (!InfernalConfig.Instance.SOTSBalanceChanges)
                return;

            ModItem gelItem = ModContent.Find<ModItem>("SOTS", "GelWings");
            if (gelItem?.Item is null || gelItem.Item.wingSlot < 0)
                return;

            ArmorIDs.Wing.Sets.Stats[gelItem.Item.wingSlot] = NewStats;
        }

        // ---------------- HOOKS ----------------

        private delegate void Orig_VerticalWingSpeeds(
            ModItem self,
            Player player,
            ref float ascentWhenFalling,
            ref float ascentWhenRising,
            ref float maxCanAscendMultiplier,
            ref float maxAscentMultiplier,
            ref float constantAscend);

        private delegate void VerticalWingSpeedsDetour(
            Orig_VerticalWingSpeeds orig,
            ModItem self,
            Player player,
            ref float ascentWhenFalling,
            ref float ascentWhenRising,
            ref float maxCanAscendMultiplier,
            ref float maxAscentMultiplier,
            ref float constantAscend);

        private static void Detour_VerticalWingSpeeds(
            Orig_VerticalWingSpeeds orig,
            ModItem self,
            Player player,
            ref float ascentWhenFalling,
            ref float ascentWhenRising,
            ref float maxCanAscendMultiplier,
            ref float maxAscentMultiplier,
            ref float constantAscend)
        {
            // Call original (optional), then override.
            orig(self, player,
                ref ascentWhenFalling,
                ref ascentWhenRising,
                ref maxCanAscendMultiplier,
                ref maxAscentMultiplier,
                ref constantAscend);

            // Skyline defaults: 0.5 / 0.1 / 0.5 / 1.5 / 0.1
            ascentWhenFalling = 0.42f;
            ascentWhenRising = 0.08f;
            maxCanAscendMultiplier = 0.45f;
            maxAscentMultiplier = 1.40f;
            constantAscend = 0.085f;
        }

        private delegate void Orig_UpdateAccessory(ModItem self, Player player, bool hideVisual);
        private delegate void UpdateAccessoryDetour(Orig_UpdateAccessory orig, ModItem self, Player player, bool hideVisual);

        private static void Detour_UpdateAccessory(Orig_UpdateAccessory orig, ModItem self, Player player, bool hideVisual)
        {
            orig(self, player, hideVisual);

            if (self?.Item != null && self.Item.wingSlot >= 0)
                player.wingTimeMax = ArmorIDs.Wing.Sets.Stats[self.Item.wingSlot].FlyTime;
        }
    }
}
