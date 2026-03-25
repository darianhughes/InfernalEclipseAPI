using System.Reflection;
using MonoMod.RuntimeDetour;
using SOTS;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;
using SOTS.Items.ChestItems;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.SOTSItemHooks
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class ShatteredDreamsNerfSystem : ModSystem
    {
        private Hook castWishingStarHook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("SOTS", out Mod sots))
                return;

            Type sotsPlayerType = sots.Code.GetType("SOTS.SOTSPlayer");
            MethodInfo castWishingStarMethod = sotsPlayerType?.GetMethod("CastWishingStar", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (castWishingStarMethod != null)
                castWishingStarHook = new Hook(castWishingStarMethod, CastWishingStar_Hook);
        }

        public override void Unload()
        {
            castWishingStarHook?.Dispose();
            castWishingStarHook = null;
        }

        private delegate void Orig_CastWishingStar(Player player, Vector2 target, int damage);

        private static void CastWishingStar_Hook(Orig_CastWishingStar orig, Player player, Vector2 target, int damage)
        {
            SOTSPlayer sotsPlayer = SOTSPlayer.ModPlayer(player);

            if (!WishingStar.IsAlternate)
                orig(player, target, damage);

            if (player.ownedProjectileCounts[ModContent.ProjectileType<WishingStarProj>()] < 3)
                orig(player, target, damage);
        }
    }
}
