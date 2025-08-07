using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.Terrarium;
using ThoriumMod.Core.DataClasses;
using ThoriumMod.Utilities;
using System.Reflection;
using ThoriumMod.Items.Thorium;
using CalamityMod;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Content.RogueThrower
{
    [ExtendsFromMod("ThoriumMod")]
    public class TerrariumStealthFocus : ModSystem
    {
        private static Hook throwingEffectHook;

        public override void Load()
        {
            // Find the TerrariumHelmet.ThrowingEffect method via reflection
            MethodInfo orig = typeof(TerrariumHelmet)
                .GetMethod("ThrowingEffect", BindingFlags.Public | BindingFlags.Static);

            // Create the hook
            throwingEffectHook = new Hook(orig, (Action<Player> origMethod, Player player) =>
            {
                // Call original
                origMethod(player);

                // Inject your code
                var modPlayer = player.Calamity();
                modPlayer.rogueStealthMax += 1.10f;
                modPlayer.wearingRogueArmor = true;
            });
        }

        public override void Unload()
        {
            throwingEffectHook?.Dispose();
            throwingEffectHook = null;
        }
    }
}
