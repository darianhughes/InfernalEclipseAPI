using MonoMod.RuntimeDetour;
using System.Collections.Generic;
using System.Reflection;

namespace InfernalEclipseAPI.Core.Systems.Hooks
{
    public class SheathCompatibilitySystem : ModSystem
    {
        public enum CompatibilityState : byte
        {
            Default,
            ForceCompatible,
            ForceIncompatible
        }

        private static readonly Dictionary<int, CompatibilityState> OverridesByItemType = [];

        private static Hook validItemHook;
        private static Hook meleeButNotValidItemHook;

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("ThoriumMod");

        public override void Load()
        {
            Mod thorium = ModLoader.GetMod("ThoriumMod");
            if (thorium is null)
                return;

            Type sheathDataType = thorium.Code.GetType("ThoriumMod.Core.Sheaths.SheathData", throwOnError: false);
            if (sheathDataType is null)
                return;

            MethodInfo validItemMethod = sheathDataType.GetMethod(
                "ValidItem",
                BindingFlags.Public | BindingFlags.Static);

            MethodInfo meleeButNotValidItemMethod = sheathDataType.GetMethod(
                "MeleeButNotValidItem",
                BindingFlags.Public | BindingFlags.Static);

            if (validItemMethod is not null)
                validItemHook = new Hook(validItemMethod, ValidItemDetour);

            if (meleeButNotValidItemMethod is not null)
                meleeButNotValidItemHook = new Hook(meleeButNotValidItemMethod, MeleeButNotValidItemDetour);
        }

        public override void Unload()
        {
            validItemHook?.Dispose();
            meleeButNotValidItemHook?.Dispose();

            validItemHook = null;
            meleeButNotValidItemHook = null;

            OverridesByItemType.Clear();
        }

        private delegate bool Orig_ValidItem(Item item);
        private delegate bool Orig_MeleeButNotValidItem(Item item);

        private static bool ValidItemDetour(Orig_ValidItem orig, Item item)
        {
            if (item is null || item.IsAir)
                return orig(item);

            return GetState(item.type) switch
            {
                CompatibilityState.ForceCompatible => true,
                CompatibilityState.ForceIncompatible => false,
                _ => orig(item)
            };
        }

        private static bool MeleeButNotValidItemDetour(Orig_MeleeButNotValidItem orig, Item item)
        {
            if (item is null || item.IsAir)
                return orig(item);

            return GetState(item.type) switch
            {
                // If we explicitly whitelist it, it should never count as "invalid for sheaths".
                CompatibilityState.ForceCompatible => false,

                // If we explicitly blacklist it, treat it as a blocked melee item for tooltip/UI purposes.
                CompatibilityState.ForceIncompatible => item.damage > 0 && item.CountsAsClass(DamageClass.Melee),

                _ => orig(item)
            };
        }

        public static CompatibilityState GetState(int itemType)
        {
            return OverridesByItemType.TryGetValue(itemType, out CompatibilityState state)
                ? state
                : CompatibilityState.Default;
        }

        public static CompatibilityState GetState(Item item)
        {
            if (item is null || item.IsAir)
                return CompatibilityState.Default;

            return GetState(item.type);
        }

        public static void SetState(int itemType, CompatibilityState state)
        {
            if (itemType <= 0)
                return;

            if (state == CompatibilityState.Default)
                OverridesByItemType.Remove(itemType);
            else
                OverridesByItemType[itemType] = state;
        }

        public static void SetCompatible(int itemType) =>
            SetState(itemType, CompatibilityState.ForceCompatible);

        public static void SetIncompatible(int itemType) =>
            SetState(itemType, CompatibilityState.ForceIncompatible);

        public static void ClearOverride(int itemType) =>
            SetState(itemType, CompatibilityState.Default);

        public static bool IsExplicitlyCompatible(int itemType) =>
            GetState(itemType) == CompatibilityState.ForceCompatible;

        public static bool IsExplicitlyIncompatible(int itemType) =>
            GetState(itemType) == CompatibilityState.ForceIncompatible;
    }
}