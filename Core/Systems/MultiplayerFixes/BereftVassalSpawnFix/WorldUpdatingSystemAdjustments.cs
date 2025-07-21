using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernumMode.Core.GlobalInstances.Systems;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MonoMod.RuntimeDetour;
using System.Reflection;
using InfernumMode.Content.Subworlds;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.BereftVassalSpawnFix
{
    //public class WorldUpdatingSystemAdjustments : ModSystem
    //{
    //    private delegate void PostUpdateEverythingDelegate(WorldUpdatingSystem instance);
    //    private static Hook hook;
    //    private static PostUpdateEverythingDelegate orig;

    //    public override void Load()
    //    {
    //        MethodInfo target = typeof(WorldUpdatingSystem).GetMethod("PostUpdateEverything", BindingFlags.Instance | BindingFlags.Public);
    //        orig = (PostUpdateEverythingDelegate)Delegate.CreateDelegate(typeof(PostUpdateEverythingDelegate), target);
    //        hook = new Hook(target, new PostUpdateEverythingDelegate(Patched_PostUpdateEverything));
    //    }

    //    public override void Unload()
    //    {
    //        hook?.Dispose();
    //        hook = null;
    //    }

    //    private static void Patched_PostUpdateEverything(WorldUpdatingSystem instance)
    //    {
    //        if (Main.netMode == NetmodeID.Server)
    //        {
    //            bool wasHasBereftVassalAppeared = LostColosseum.HasBereftVassalAppeared;
    //            bool tempDisabled = false;

    //            if (!wasHasBereftVassalAppeared && Main.player.Any(p => p.active && p.dead))
    //            {
    //                LostColosseum.HasBereftVassalAppeared = true;
    //                tempDisabled = true;
    //            }

    //            orig(instance); // Call original

    //            if (tempDisabled)
    //            {
    //                LostColosseum.HasBereftVassalAppeared = wasHasBereftVassalAppeared;
    //            }

    //            return;
    //        }

    //        orig(instance);
    //    }
    //}
    
    public class WorldUpdatingSystemAdjustments : ModSystem
    {
        public override void PostUpdateEverything()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                bool wasHasBereftVassalAppeared = LostColosseum.HasBereftVassalAppeared;
                bool tempDisabled = false;

                if (!wasHasBereftVassalAppeared && Main.player.Any(p => p.active && p.dead))
                {
                    LostColosseum.HasBereftVassalAppeared = true;
                    tempDisabled = true;
                }

                base.PostUpdateEverything();

                if (tempDisabled)
                {
                    LostColosseum.HasBereftVassalAppeared = wasHasBereftVassalAppeared;
                }

                return;
            }

            base.PostUpdateEverything();
        }
    }
}
